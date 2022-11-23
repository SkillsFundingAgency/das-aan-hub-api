using MediatR;
using SFA.DAS.AAN.Domain.Enums;

namespace SFA.DAS.AAN.Application.Commands.ModifyMember
{
    public class ModifyMemberCommand : IRequest<ModifyMemberResponse>
    {
        public MembershipStatuses Status { get; set; }
        public Regions[]? Regions { get; set; }
        public string? UserId { get; set; }
    }
}
