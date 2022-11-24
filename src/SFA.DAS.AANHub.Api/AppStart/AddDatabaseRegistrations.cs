using Microsoft.EntityFrameworkCore;
using SFA.DAS.AANHub.Data;
using SFA.DAS.AANHub.Domain.Configuration;

namespace SFA.DAS.AANHub.Api.AppStart
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
