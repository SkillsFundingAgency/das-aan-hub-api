using MediatR;
using SFA.DAS.AANHub.Application.Commands.CreateMember;
using SFA.DAS.AANHub.Data;
using SFA.DAS.AANHub.Domain.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.AANHub.Api.AppStart
{
    [ExcludeFromCodeCoverage]
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
        }
    }
}
