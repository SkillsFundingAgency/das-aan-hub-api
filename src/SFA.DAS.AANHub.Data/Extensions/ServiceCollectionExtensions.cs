using System.Diagnostics.CodeAnalysis;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.AANHub.Data.Repositories;
using SFA.DAS.AANHub.Domain.Interfaces;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Data.Extensions;

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
        services.AddTransient<IStagedApprenticesReadRepository, StagedApprenticesReadRepository>();
        services.AddTransient<IEmployersWriteRepository, EmployersWriteRepository>();
        services.AddTransient<IEmployersReadRepository, EmployersReadRepository>();
        services.AddTransient<IApprenticesWriteRepository, ApprenticesWriteRepository>();
        services.AddTransient<IApprenticesReadRepository, ApprenticesReadRepository>();
        services.AddTransient<IPartnersWriteRepository, PartnersWriteRepository>();
        services.AddTransient<IPartnersReadRepository, PartnersReadRepository>();
        services.AddTransient<IMembersWriteRepository, MembersWriteRepository>();
        services.AddTransient<IMembersReadRepository, MembersReadRepository>();
        services.AddTransient<IProfilesReadRepository, ProfilesReadRepository>();
        services.AddTransient<ICalendarEventsReadRepository, CalendarEventsReadRepository>();
        services.AddTransient<ICalendarsReadRepository, CalendarsReadRepository>();
        services.AddTransient<IAttendancesWriteRepository, AttendancesWriteRepository>();
        services.AddTransient<IAttendancesReadRepository, AttendancesReadRepository>();
        services.AddTransient<INotificationsWriteRepository, NotificationsWriteRepository>();
        services.AddTransient<INotificationsReadRepository, NotificationsReadRepository>();
        services.AddTransient<INotificationTemplateReadRepository, NotificationTemplateReadRepository>();
    }

    public static async Task<string> GenerateTokenAsync()
    {
        const string AzureResource = "https://database.windows.net/";
        var azureServiceTokenProvider = new AzureServiceTokenProvider();
        var accessToken = await azureServiceTokenProvider.GetAccessTokenAsync(AzureResource);

        return accessToken;
    }
}