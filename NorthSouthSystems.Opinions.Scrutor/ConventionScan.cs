using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace NorthSouthSystems.Scrutor;

public static class ConventionScanExtensions
{
    public static IServiceCollection ConventionScanAssembly(this IServiceCollection services, Assembly assembly)
    {
        Throw.IfNull(assembly);

        return Throw.IfNull(services)
            .Scan(scan =>
                scan.FromAssemblies(assembly)
                    .Add<ConventionLifetimeAttribute>(new ConventionLifetimeRegistrationStrategy())
                    .Add<ConventionOptionsAttribute>(new ConventionOptionsRegistrationStrategy()));
    }

    private static IServiceTypeSelector Add<T>(this IImplementationTypeSelector selector, RegistrationStrategy strategy)
        where T : Attribute =>
        selector.AddClasses(filter => filter.WithAttribute<T>(), false)
            .AsSelfWithInterfaces()
            .UsingRegistrationStrategy(strategy);
}
