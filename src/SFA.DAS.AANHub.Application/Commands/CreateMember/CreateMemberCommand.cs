using MediatR;
using SFA.DAS.AANHub.Application.Common.Commands;
using SFA.DAS.AANHub.Domain.Enums;

namespace SFA.DAS.AANHub.Application.Commands.CreateMember
{
    public class CreateMemberCommand : CreateMemberCommandBase, IRequest<CreateMemberResponse>
    {
        public MembershipUserType? UserType { get; set; }
    }
}
