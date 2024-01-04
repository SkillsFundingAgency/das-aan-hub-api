using MediatR;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Application.Common.Validators.AdminMemberId;
using SFA.DAS.AANHub.Application.Common.Validators.MemberId;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Common;

namespace SFA.DAS.AANHub.Application.Members.Commands.PostMemberRemove;

public class PostMemberRemoveCommand : IRequest<ValidatedResponse<SuccessCommandResult>>, IMemberId, IAdminMemberId
{
    public Guid MemberId { get; set; }
    public MembershipStatusType Status { get; set; }
    public Guid AdminMemberId { get; set; }
}
