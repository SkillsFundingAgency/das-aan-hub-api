using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.AANHub.Data.Repositories;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.AANHub.Data.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAanDataContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<AanDataContext>((serviceProvider, options) =>
            {
                var connection = new SqlConnection(connectionString);

                options.UseSqlServer(
                    connection,
                    o => o.CommandTimeout((int)TimeSpan.FromMinutes(5).TotalSeconds));
            });
            RegisterServices(services);
            return services;
        }

        private static void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<IRegionsReadRepository, RegionsReadRepository>();
            services.AddTransient<IAdminsWriteRepository, AdminsWriteRepository>();
            services.AddTransient<IEmployersWriteRepository, EmployersWriteRepository>();
            services.AddTransient<IApprenticesWriteRepository, ApprenticesWriteRepository>();
            services.AddTransient<IPartnersWriteRepository, PartnersWriteRepository>();
            services.AddTransient<IMembersWriteRepository, MembersWriteRepository>();
            services.AddTransient<ICalendarsReadRepository, CalendarsReadRepository>();
            services.AddTransient<ICalendarsPermissionsReadRepository, CalendarsPermissionsReadRepository>();
            services.AddTransient<IMembersPermissionsReadRepository, MembersPermissionsReadRepository>();

        }
    }
}
