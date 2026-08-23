using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace NorthSouthSystems.Scrutor;

public sealed class ConventionTransientAttribute() : ConventionLifetimeAttribute(ServiceLifetime.Transient);
public sealed class ConventionScopedAttribute() : ConventionLifetimeAttribute(ServiceLifetime.Scoped);
public sealed class ConventionSingletonAttribute() : ConventionLifetimeAttribute(ServiceLifetime.Singleton);

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public abstract class ConventionLifetimeAttribute(ServiceLifetime lifetime) : Attribute
{
    public ServiceLifetime Lifetime { get; } = lifetime;
}

internal class ConventionLifetimeRegistrationStrategy : RegistrationStrategy
{
    public override void Apply(IServiceCollection services, ServiceDescriptor descriptor)
    {
        // ServiceDescription.ImplementationType and KeyImplementationType are mutually exclusive.
        var implementationType = descriptor.ImplementationType ?? descriptor.KeyedImplementationType!;

        var lifetimeAttribute = implementationType.GetCustomAttribute<ConventionLifetimeAttribute>();

        var lifetime = lifetimeAttribute?.Lifetime
            ?? throw new InvalidOperationException(
                string.Create(InvariantCulture, $"Type '{implementationType}' has no {nameof(ConventionLifetimeAttribute)}."));

        var publicConstructors = implementationType.GetConstructors(BindingFlags.Instance | BindingFlags.Public);

        // Use default DI behavior.
        if (publicConstructors.Length > 0)
        {
            services.Add(new(descriptor.ServiceType, descriptor.ServiceKey, implementationType, lifetime));
            return;
        }

        var internalConstructors = implementationType
            .GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)
            .Where(c => c.IsAssembly)
            .ToImmutableArray();

        switch (internalConstructors.Length)
        {
            case 0:
                throw new InvalidOperationException(
                    string.Create(InvariantCulture, $"Type '{implementationType}' has no public or internal constructor."));
            case > 1:
                throw new InvalidOperationException(
                    string.Create(InvariantCulture, $"Type '{implementationType}' has multiple internal constructors."));
            default:
                services.Add(new(descriptor.ServiceType, descriptor.ServiceKey,
                    (serviceProvider, serviceKey) => Construct(internalConstructors[0], serviceProvider, serviceKey), lifetime));
                break;
        }

    }

    private static object Construct(ConstructorInfo constructor, IServiceProvider serviceProvider, object? _)
    {
        var arguments = constructor.GetParameters().Select(p => serviceProvider.GetRequiredService(p.ParameterType));

        return constructor.Invoke([.. arguments]);
    }
}
