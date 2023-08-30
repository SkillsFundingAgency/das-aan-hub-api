using MediatR;
using SFA.DAS.AANHub.Application.Common.Validators.MemberId;
using SFA.DAS.AANHub.Application.Common.Validators.RequestedByMemberId;
using SFA.DAS.AANHub.Application.Mediatr.Responses;

namespace SFA.DAS.AANHub.Application.Notifications.Commands;
public class CreateNotificationCommand : IRequest<ValidatedResponse<CreateNotificationCommandResponse>>, IRequestedByMemberId, IMemberId
{
    public Guid RequestedByMemberId { get; set; }
    public Guid MemberId { get; set; }
    public long NotificationTemplateId { get; set; }

}