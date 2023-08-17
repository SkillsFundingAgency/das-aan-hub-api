using FluentValidation;
using SFA.DAS.AANHub.Application.Common.Validators.RequestedByMemberId;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.Notifications.Queries;
public class GetNotificationQueryValidator : AbstractValidator<GetNotificationQuery>
{
    public const string NotificationIdIsRequired = "NotificationId must have a value";

    public GetNotificationQueryValidator(IMembersReadRepository membersReadRepository)
    {
        Include(new RequestedByMemberIdValidator(membersReadRepository));

        RuleFor(n => n.NotificationId)
            .NotEmpty()
            .WithMessage(NotificationIdIsRequired);
    }
}
