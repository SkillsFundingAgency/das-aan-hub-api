using Microsoft.EntityFrameworkCore;
using SFA.DAS.AANHub.Data;
using SFA.DAS.AANHub.Domain.Configuration;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.AANHub.Api.AppStart
{
    [ExcludeFromCodeCoverage]
    public static class AddDatabaseRegistrations
    {
        public static void AddDatabaseRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            var appSettings = configuration.GetSection("ApplicationSettings").Get<ApplicationSettings>();

            if (appSettings.DbConnectionString == null)
            {
                throw new ArgumentException("Null connection string in appsettings");
            }

            services.AddDbContext<AanDataContext>(
                options => options.UseSqlServer(appSettings.DbConnectionString).EnableSensitiveDataLogging(),
                ServiceLifetime.Transient);


        }
    }
}
