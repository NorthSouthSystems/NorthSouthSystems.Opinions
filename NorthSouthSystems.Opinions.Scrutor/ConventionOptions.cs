using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace NorthSouthSystems.Scrutor;

[AttributeUsage(AttributeTargets.Class)]
public sealed class ConventionOptionsAttribute : Attribute;

public static class ConventionOptionsExtensions
{
    public static OptionsBuilder<T> AddConventionOptions<T>(this IServiceCollection services,
        string? name = null, Action<T>? configure = null)
        where T : class
    {
        string sectionName = (typeof(T).FullName ?? typeof(T).Name).Replace('.', ':');

        if (sectionName.EndsWith(nameof(Options), StringComparison.OrdinalIgnoreCase))
            sectionName = sectionName[..^nameof(Options).Length];

        if (!string.IsNullOrEmpty(name) && name != Options.DefaultName)
            sectionName = string.Create(InvariantCulture, $"{sectionName}:{name}");

        return Throw.IfNull(services)
            .AddOptions<T>(name ?? Options.DefaultName)
            .BindConfiguration(sectionName)
            .Configure(configure ?? ConfigureNoop)
            .Validate<IValidator<T>>(
                (t, validator) => validator.Validate(t).IsValid,
                typeof(T).Name);

        static void ConfigureNoop(T _) { }
    }
}

internal class ConventionOptionsRegistrationStrategy : RegistrationStrategy
{
    public override void Apply(IServiceCollection services, ServiceDescriptor descriptor) =>
        AddConventionOptionsMethod.MakeGenericMethod(descriptor.ImplementationType ?? descriptor.KeyedImplementationType!)
            .Invoke(null, [services, null, null]);

    private static readonly MethodInfo AddConventionOptionsMethod = typeof(ConventionOptionsExtensions)
        .GetMethod(nameof(ConventionOptionsExtensions.AddConventionOptions), BindingFlags.Static | BindingFlags.Public)!;
}
