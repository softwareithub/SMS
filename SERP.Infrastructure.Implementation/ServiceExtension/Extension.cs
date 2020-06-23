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
            serviceCollection.AddTransient<ISubjectMasterRepo, SubjectImplementation>();
            serviceCollection.AddTransient<ITimeSheetRepo, TimeSheetImplementation>();
            serviceCollection.AddTransient<IFeeDetailRepo, FeeDetailImplementation>();
            serviceCollection.AddTransient<IDashBoardGraphRepo, DashBoardRepo>();
        }
    }
}
