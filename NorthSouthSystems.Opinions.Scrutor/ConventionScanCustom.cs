using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.Reflection;

namespace NorthSouthSystems.Scrutor;

public interface IConventionScanCustom
{
    static abstract void Register(IServiceCollection services);
}

internal class ConventionScanCustomStrategy : RegistrationStrategy
{
    public override void Apply(IServiceCollection services, ServiceDescriptor descriptor)
    {
        if (descriptor?.ImplementationType is null)
            throw new UnreachableException(nameof(descriptor));

        RegisterMethod.MakeGenericMethod(descriptor.ImplementationType).Invoke(null, [services]);
    }

    private static readonly MethodInfo RegisterMethod = typeof(ConventionScanCustomStrategy)
        .GetMethod(nameof(Register), BindingFlags.Static | BindingFlags.NonPublic)!;

    private static void Register<T>(IServiceCollection services) where T : IConventionScanCustom =>
        T.Register(services);
}
