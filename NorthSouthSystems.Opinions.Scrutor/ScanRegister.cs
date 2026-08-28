using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace NorthSouthSystems.Scrutor;

public static class ScanRegisterExtensions
{
    public static IServiceCollection ConventionScanAssembly(this IServiceCollection services, Assembly assembly)
    {
        Throw.IfNull(assembly);

        return Throw.IfNull(services)
            .Scan(scan =>
                scan.FromAssemblies(assembly)
                    .Add<ScanRegisterTransientAttribute>(new ScanRegisterLifetimeStrategy(ServiceLifetime.Transient))
                    .Add<ScanRegisterScopedAttribute>(new ScanRegisterLifetimeStrategy(ServiceLifetime.Scoped))
                    .Add<ScanRegisterSingletonAttribute>(new ScanRegisterLifetimeStrategy(ServiceLifetime.Singleton))
                    .Add<ScanRegisterConventionOptionsAttribute>(new ScanRegisterConventionOptionsStrategy())
                    .AddClasses(filter => filter.AssignableTo(typeof(IScanRegisterCustom)))
                    .AsSelf()
                    .UsingRegistrationStrategy(new ScanRegisterCustomStrategy()));
    }

    private static IServiceTypeSelector Add<T>(this IImplementationTypeSelector selector, RegistrationStrategy strategy)
        where T : Attribute =>
        selector.AddClasses(filter => filter.WithAttribute<T>(), false)
            .AsSelfWithInterfaces()
            .UsingRegistrationStrategy(strategy);
}
