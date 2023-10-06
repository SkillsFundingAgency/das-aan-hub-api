using FluentValidation;
using SFA.DAS.AANHub.Domain.Common;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.EventGuests.PutEventGuests;

public class PutEventGuestsCommandValidator : AbstractValidator<PutEventGuestsCommand>
{
    public const string RequestedByMemberIdMustNotBeEmpty = "requestedByMemberId must have a value";
    public const string RequestedByMemberIdMustBeAdmin = "requestedByMemberId must be an active, admin member or regional chair";

    public PutEventGuestsCommandValidator(IMembersReadRepository membersReadRepository)
    {
        RuleFor(c => c.AdminMemberId)
            .NotEmpty()
            .WithMessage(RequestedByMemberIdMustNotBeEmpty)
            .MustAsync(async (memberId, cancellationToken) =>
            {
                var member = await membersReadRepository.GetMember(memberId);
                return
                    member != null &&
                    member!.Status == MembershipStatusType.Live.ToString() &&
                    (member.UserType == UserType.Admin.ToString() || member.IsRegionalChair.GetValueOrDefault());
            })
            .WithMessage(RequestedByMemberIdMustBeAdmin);
        RuleFor(c => c.CalendarEvent)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("does not exist")
            .Must(calendarEvent => calendarEvent!.IsActive)
            .WithMessage("it is not active")
            .Must(calendarEvent => calendarEvent!.StartDate > DateTime.UtcNow)
            .WithMessage("event is in past");
    }
}
