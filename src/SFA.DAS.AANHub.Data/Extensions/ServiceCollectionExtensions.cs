using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.AANHub.Data.Repositories;
using SFA.DAS.AANHub.Domain.Interfaces;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.AANHub.Data.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAanDataContext(this IServiceCollection services, string connectionString, string environmentName)
        {
            services.AddDbContext<AanDataContext>((serviceProvider, options) =>
            {
                var connection = new SqlConnection(connectionString);

                if (!environmentName.Equals("LOCAL", StringComparison.CurrentCultureIgnoreCase))
                {
                    var generateTokenTask = GenerateTokenAsync();
                    connection.AccessToken = generateTokenTask.GetAwaiter().GetResult();
                }
                options.UseSqlServer(
                    connection,
                    o => o.CommandTimeout((int)TimeSpan.FromMinutes(5).TotalSeconds));
            });
            services.AddTransient<IAanDataContext, AanDataContext>(provider => provider.GetService<AanDataContext>()!);
            RegisterServices(services);
            return services;
        }

        private static void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<IAuditWriteRepository, AuditWriteRepository>();
            services.AddTransient<IRegionsReadRepository, RegionsReadRepository>();
            services.AddTransient<IAdminsWriteRepository, AdminsWriteRepository>();
            services.AddTransient<IEmployersWriteRepository, EmployersWriteRepository>();
            services.AddTransient<IApprenticesWriteRepository, ApprenticesWriteRepository>();
            services.AddTransient<IApprenticesReadRepository, ApprenticesReadRepository>();
            services.AddTransient<IPartnersWriteRepository, PartnersWriteRepository>();
            services.AddTransient<IMembersWriteRepository, MembersWriteRepository>();
            services.AddTransient<IMembersReadRepository, MembersReadRepository>();
            services.AddTransient<ICalendarsReadRepository, CalendarsReadRepository>();
            services.AddTransient<ICalendarsPermissionsReadRepository, CalendarsPermissionsReadRepository>();
            services.AddTransient<IMembersPermissionsReadRepository, MembersPermissionsReadRepository>();

        }
        public static async Task<string> GenerateTokenAsync()
        {
            const string AzureResource = "https://database.windows.net/";
            var azureServiceTokenProvider = new AzureServiceTokenProvider();
            var accessToken = await azureServiceTokenProvider.GetAccessTokenAsync(AzureResource);

            return accessToken;
        }
    }
}
