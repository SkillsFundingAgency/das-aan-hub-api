using System.Diagnostics.CodeAnalysis;
using Azure.Core;
using Azure.Identity;
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
    private static readonly string AzureResource = "https://database.windows.net/";

    private static readonly ChainedTokenCredential AzureTokenProvider = new ChainedTokenCredential(
        new ManagedIdentityCredential(),
        new AzureCliCredential(),
        new VisualStudioCodeCredential(),
        new VisualStudioCredential()
    );

    public static IServiceCollection AddAanDataContext(this IServiceCollection services, string connectionString, string environmentName)
    {
        services.AddDbContext<AanDataContext>((serviceProvider, options) =>
        {
            SqlConnection connection = null!;

            if (!environmentName.Equals("LOCAL", StringComparison.CurrentCultureIgnoreCase))
            {
                connection = new SqlConnection
                {
                    ConnectionString = connectionString,
                    AccessToken = AzureTokenProvider.GetToken(new TokenRequestContext(scopes: new string[] { AzureResource })).Token
                };
            }
            else
            {
                connection = new SqlConnection(connectionString);
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
}
