using FluentValidation;
using SFA.DAS.AANHub.Application.Common.Validators.MemberId;
using SFA.DAS.AANHub.Application.Common.Validators.RequestedByMemberId;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.Notifications.Commands;
public class CreateNotificationCommandValidator : AbstractValidator<CreateNotificationCommand>
{
    public const string NotificationTemplateIdRequired = "NotificationTemplateId must have a value";
    public const string NotificationTemplateNotFound = "Could not find a valid notification template ID matching the notification template ID in the request";

    public CreateNotificationCommandValidator(IMembersReadRepository membersReadRepository, INotificationTemplateReadRepository notificationTemplateReadRepository)
    {
        Include(new RequestedByMemberIdValidator(membersReadRepository));
        Include(new MemberIdValidator(membersReadRepository));
        RuleFor(nt => nt.NotificationTemplateId)
            .NotEmpty()
            .WithMessage(NotificationTemplateIdRequired)
            .MustAsync(async (notificationTemplateId, cancellationToken) =>
            {
                var notificationTemplate = await notificationTemplateReadRepository.Get(notificationTemplateId, cancellationToken);
                return notificationTemplate is not null;
            })
            .WithMessage(NotificationTemplateNotFound);
    }
}