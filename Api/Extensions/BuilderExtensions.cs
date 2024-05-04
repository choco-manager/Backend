using System.Reflection;
using Api.Common;

namespace Api.Extensions;

public static class BuilderExtensions
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        var types = Assembly.GetCallingAssembly().GetTypes()
            .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IUseCase<,>)));

        foreach (var type in types)
        {
            services.AddTransient(typeof(IUseCase<,>), type);
        }

        return services;
    }
}