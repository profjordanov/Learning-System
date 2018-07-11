using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Reflection;
using LearningSystem.Services;

namespace LearningSystem.Web.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDomainServices(this IServiceCollection serviceCollection)
        {
            Assembly
                .GetAssembly(typeof(IService))
                .GetTypes()
                .Where(t => t.IsClass &&
                            t.GetInterfaces().Any(i => i.Name == $"I{t.Name}"))
                .Select(t => new
                {
                    Interface = t.GetInterface($"I{t.Name}"),
                    Implementation = t
                })
                .ToList()
                .ForEach(s => serviceCollection.AddTransient(s.Interface,s.Implementation));

            return serviceCollection;
        }
    }
}
