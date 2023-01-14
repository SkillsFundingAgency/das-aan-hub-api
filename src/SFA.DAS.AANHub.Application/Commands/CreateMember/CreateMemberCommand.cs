using MediatR;
using SFA.DAS.AANHub.Application.Common.Commands;

namespace SFA.DAS.AANHub.Application.Commands.CreateMember
{
    public class CreateMemberCommand : CreateMemberCommandBase, IRequest<CreateMemberResponse>
    {
        public string? UserType { get; set; }
    }
}
