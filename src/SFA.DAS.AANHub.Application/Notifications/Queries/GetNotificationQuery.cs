using MediatR;
using SFA.DAS.AANHub.Application.Common.Validators.RequestedByMemberId;
using SFA.DAS.AANHub.Application.Mediatr.Responses;

namespace SFA.DAS.AANHub.Application.Notifications.Queries;
public record GetNotificationQuery(Guid NotificationId, Guid RequestedByMemberId) 
    : IRequest<ValidatedResponse<GetNotificationQueryResult>>, IRequestedByMemberId;
