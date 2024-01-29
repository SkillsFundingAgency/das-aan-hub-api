using MediatR;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Application.Mediatr.Responses;

namespace SFA.DAS.AANHub.Application.Members.Commands.PostMemberReinstate;

public class PostMemberReinstateCommand : IRequest<ValidatedResponse<SuccessCommandResult>>
{
    public Guid MemberId { get; set; }
}

