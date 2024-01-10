using MediatR;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Application.Common.Validators.MemberId;
using SFA.DAS.AANHub.Application.Mediatr.Responses;

namespace SFA.DAS.AANHub.Application.Members.Commands.PostMemberLeaving;

public class PostMemberLeavingCommand : IRequest<ValidatedResponse<SuccessCommandResult>>, IMemberId
{
    public Guid MemberId { get; set; }
    public List<int> LeavingReasons { get; set; } = null!;
}