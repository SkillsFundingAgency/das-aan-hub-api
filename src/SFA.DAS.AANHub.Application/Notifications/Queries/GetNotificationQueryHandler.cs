using MediatR;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.Notifications.Queries;
public class GetNotificationQueryHandler : IRequestHandler<GetNotificationQuery, ValidatedResponse<GetNotificationQueryResult>>
{
    private readonly INotificationsReadRepository _notificationsReadRepository;
    private readonly IMembersReadRepository _membersReadRepository;
    private readonly IEmployersReadRepository _employersReadRepository;

    public GetNotificationQueryHandler(INotificationsReadRepository notificationsReadRepository, IMembersReadRepository membersReadRepository,
        IEmployersReadRepository employersReadRepository)
    {
        _notificationsReadRepository = notificationsReadRepository;
        _membersReadRepository = membersReadRepository;
        _employersReadRepository = employersReadRepository;
    }

    public async Task<ValidatedResponse<GetNotificationQueryResult>> Handle(GetNotificationQuery request, CancellationToken cancellationToken)
    {
        var notification = await _notificationsReadRepository.GetNotificationById(request.NotificationId, cancellationToken);
        if ((notification == null) || (notification.MemberId != request.RequestedByMemberId)) { return new ValidatedResponse<GetNotificationQueryResult>((GetNotificationQueryResult)null!); }
        var employerAccountId = await PopulateEmployerAccountId(request.RequestedByMemberId, cancellationToken);

        var result = new GetNotificationQueryResult() { MemberId = request.RequestedByMemberId, TemplateName = notification.TemplateName, SentTime = DateTime.UtcNow, ReferenceId = notification.ReferenceId, EmployerAccountId = employerAccountId };
        return new ValidatedResponse<GetNotificationQueryResult>(result);
    }

    private async Task<long?> PopulateEmployerAccountId(Guid requestedByMemberId, CancellationToken cancellationToken)
    {
        var member = await _membersReadRepository.GetMember(requestedByMemberId);
        long? employerAccountId = null;
        if (member!.UserType == "Employer") { employerAccountId = await GetEmployerAccountId(requestedByMemberId, cancellationToken); }
        return employerAccountId;
    }

    private async Task<long> GetEmployerAccountId(Guid memberId, CancellationToken cancellationToken)
    {
        var employer = await _employersReadRepository.GetEmployerByMemberId(memberId, cancellationToken);
        return employer!.AccountId;
    }
}
