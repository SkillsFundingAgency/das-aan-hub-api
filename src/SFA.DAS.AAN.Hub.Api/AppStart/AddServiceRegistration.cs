
using MediatR;
using SFA.DAS.AAN.Application.Commands.CreateMember;
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
        }
    }
}
