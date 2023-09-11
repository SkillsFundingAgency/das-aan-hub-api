using System.Text.Json;
using MediatR;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Common;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using SFA.DAS.AANHub.Domain.Models;

namespace SFA.DAS.AANHub.Application.Notifications.Commands;
public class CreateNotificationCommandHandler : IRequestHandler<CreateNotificationCommand, ValidatedResponse<CreateNotificationCommandResponse>>
{
    private readonly INotificationsWriteRepository _notificationsWriteRepository;
    private readonly IMembersReadRepository _membersReadRepository;
    private readonly INotificationTemplateReadRepository _notificationsTemplateReadRepository;
    private readonly IAanDataContext _aanDataContext;

    public CreateNotificationCommandHandler(INotificationsWriteRepository notificationsWriteRepository, IMembersReadRepository membersReadRepository,
        INotificationTemplateReadRepository notificationTemplateReadRepository, IAanDataContext aanDataContext)
    {
        _notificationsWriteRepository = notificationsWriteRepository;
        _membersReadRepository = membersReadRepository;
        _notificationsTemplateReadRepository = notificationTemplateReadRepository;
        _aanDataContext = aanDataContext;
    }
    public async Task<ValidatedResponse<CreateNotificationCommandResponse>> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
    {
        var requestedByMember = await _membersReadRepository.GetMember(request.RequestedByMemberId);
        var contactMember = await _membersReadRepository.GetMember(request.MemberId);
        var notificationTemplate = await _notificationsTemplateReadRepository.Get(request.NotificationTemplateId, cancellationToken);

        var tokens = CreateTokens(requestedByMember!, contactMember!);
        var notificationId = CreateNotification(tokens, contactMember!, requestedByMember!, notificationTemplate!, _notificationsWriteRepository);
        await _aanDataContext.SaveChangesAsync(cancellationToken);

        return new ValidatedResponse<CreateNotificationCommandResponse>(new CreateNotificationCommandResponse(notificationId));
    }

    private static string CreateTokens(Member requestedByMember, Member contactMember)
    {
        var emailTemplate = new MemberToMemberContactEmailTemplate(contactMember.FullName, requestedByMember.FullName, requestedByMember.FirstName, requestedByMember.Email);
        return JsonSerializer.Serialize(emailTemplate);
    }

    private static Guid CreateNotification(string tokens, Member contactMember, Member requestedByMember, NotificationTemplate notificationTemplate, INotificationsWriteRepository _notificationsWriteRepository)
    {
        var notification = NotificationHelper.CreateNotification(Guid.NewGuid(), contactMember.Id, notificationTemplate.TemplateName, tokens, requestedByMember.Id, false, null);
        _notificationsWriteRepository.Create(notification);
        return notification.Id;
    }
}
