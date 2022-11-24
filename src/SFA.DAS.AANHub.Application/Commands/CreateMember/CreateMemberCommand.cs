using MediatR;
using SFA.DAS.AANHub.Domain.Enums;

namespace SFA.DAS.AANHub.Application.Commands.CreateMember
{
    public class CreateMemberCommand : IRequest<CreateMemberResponse>
    {
        public string? Id { get; set; }
        public MembershipUserTypes? UserType { get; set; }
        public DateTime Joined { get; set; }
        public int? Region { get; set; }
        public string? Information { get; set; }
        public string? Organisation { get; set; }
    }
}
