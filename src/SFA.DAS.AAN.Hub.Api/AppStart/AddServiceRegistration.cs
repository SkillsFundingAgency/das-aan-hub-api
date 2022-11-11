
using MediatR;
using SFA.DAS.AAN.Application.Commands.CreateMember;
using SFA.DAS.AAN.Application.Interfaces;
using SFA.DAS.AAN.Application.Services;
using SFA.DAS.AAN.Data;
using SFA.DAS.AAN.Domain.Interfaces;


namespace SFA.DAS.AAN.Hub.Api.AppStart
{
    public static class AddServiceRegistration
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddMediatR(typeof(CreateMemberCommand).Assembly);
            services.AddScoped<IRegionsContext>(s => s.GetRequiredService<AanDataContext>());
            services.AddScoped<IMembersContext>(s => s.GetRequiredService<AanDataContext>());
            services.AddScoped<IApprenticesContext>(s => s.GetRequiredService<AanDataContext>());
            services.AddScoped<IEmployersContext>(s => s.GetRequiredService<AanDataContext>());
            services.AddScoped<IPartnersContext>(s => s.GetRequiredService<AanDataContext>());
            services.AddScoped<IAdminsContext>(s => s.GetRequiredService<AanDataContext>());
            services.AddScoped<ICalendarsContext>(s => s.GetRequiredService<AanDataContext>());
            services.AddScoped<ICalendarPermissionsContext>(s => s.GetRequiredService<AanDataContext>());
            services.AddScoped<IMemberPermissionsContext>(s => s.GetRequiredService<AanDataContext>());
            services.AddScoped<IAuditContext>(s => s.GetRequiredService<AanDataContext>());
            services.AddScoped(typeof(IAuditLogService<>), typeof(AuditLogService<>));
        }
    }
}
