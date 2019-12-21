using Microsoft.Extensions.DependencyInjection;
using SERP.Infrastructure.Implementation.Infratructure.Implementation;
using SERP.Infrastructure.Repository.Infrastructure.Repo;

namespace SERP.Infrastructure.Implementation.ServiceExtension
{
    public static class Extension
    {
        public static void Exentension(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient(typeof(IGenericRepository<,>), typeof(GenericImplementation<,>));
        }
    }
}
