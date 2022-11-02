using Microsoft.EntityFrameworkCore;
using SFA.DAS.AAN.Data;
using SFA.DAS.AAN.Domain.Configuration;

namespace SFA.DAS.AAN.Hub.Api.AppStart
{
    public static class AddDatabaseRegistrations
    {
        public static void AddDatabaseRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            var appSettings = configuration.GetSection("ApplicationSettings").Get<ApplicationSettings>();

            services.AddDbContext<AanDataContext>(
                options => options.UseSqlServer(appSettings.DbConnectionString).EnableSensitiveDataLogging(),
                ServiceLifetime.Transient);


        }
    }
}
