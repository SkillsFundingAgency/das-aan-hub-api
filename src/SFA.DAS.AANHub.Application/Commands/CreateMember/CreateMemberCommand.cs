using MediatR;
using SFA.DAS.AANHub.Application.Common.Commands;
using SFA.DAS.AANHub.Domain.Enums;

namespace SFA.DAS.AANHub.Application.Commands.CreateMember
{
    public class CreateMemberCommand : BaseMemberCommand, IRequest<CreateMemberResponse>
    {
        public string? Id { get; set; }
        public MembershipUserTypes? UserType { get; set; }
        public string? Organisation { get; set; }
    }
}
