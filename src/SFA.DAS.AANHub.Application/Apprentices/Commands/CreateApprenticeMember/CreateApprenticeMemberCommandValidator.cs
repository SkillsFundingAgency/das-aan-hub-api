using FluentValidation;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Domain.Common;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.Apprentices.Commands.CreateApprenticeMember;

public class CreateApprenticeMemberCommandValidator : AbstractValidator<CreateApprenticeMemberCommand>
{
    public const string ApprenticeAlreadyExistsErrorMessage = "ApprenticeId already exists";
    public const string InvalidProfileIdsErrorMessage = "Some of the profile ids are invalid for Apprentice user type";
    public const string ProfileValuesMustNotBeEmptyErrorMessage = "ProfileValues cannot be empty";
    public const string EventFormatCannotBeEmptyIfReceiveNotificationsErrorMessage = "EventFormat cannot be empty if ReceiveNotifications is true.";
    public const string LocationCannotBeEmptyForNonOnlineEventErrorMessage = "Location cannot be empty if the event format is selected and is not 'Online'.";

    public CreateApprenticeMemberCommandValidator(IApprenticesReadRepository apprenticesReadRepository, IProfilesReadRepository profilesReadRepository, IMembersReadRepository membersReadRepository, IRegionsReadRepository regionsReadRepository)
    {
        Include(new CreateMemberCommandBaseValidator(membersReadRepository, regionsReadRepository));

        RuleFor(c => c.ApprenticeId)
            .NotEmpty()
            .MustAsync(async (apprenticeId, cancellation) =>
            {
                var apprentice = await apprenticesReadRepository.GetApprentice(apprenticeId);
                return apprentice == null;
            })
            .WithMessage(ApprenticeAlreadyExistsErrorMessage);

        RuleFor(c => c.ProfileValues)
            .NotEmpty()
            .WithMessage(ProfileValuesMustNotBeEmptyErrorMessage)
            .ForEach(x => x.SetValidator(new ProfileValueValidator()))
            .MustAsync(async (profileValues, _) =>
            {
                var profiles = await profilesReadRepository.GetProfilesByUserType(UserType.Apprentice);
                var profileIds = profiles.Select(profile => profile.Id);
                var b = profileValues.Select(v => v.Id).All(i => profileIds.Contains(i));
                return b;
            })
            .WithMessage(InvalidProfileIdsErrorMessage);

        RuleFor(x => x.MemberNotificationEventFormatValues)
            .NotEmpty()
            .When(x => x.ReceiveNotifications)
            .WithMessage(EventFormatCannotBeEmptyIfReceiveNotificationsErrorMessage);

        RuleFor(x => x.MemberNotificationLocationValues)
            .NotEmpty()
            .When(x => x.MemberNotificationEventFormatValues?.Any(eventFormat =>
                eventFormat.ReceiveNotifications &&
                !string.Equals(eventFormat.EventFormat, EventFormat.Online.ToString(), StringComparison.OrdinalIgnoreCase)) == true)
            .WithMessage(LocationCannotBeEmptyForNonOnlineEventErrorMessage);

        RuleFor(c => c.MemberNotificationEventFormatValues)
            .ForEach(x => x.SetValidator(new MemberNotificationEventFormatValuesValidator()));

        RuleFor(c => c.MemberNotificationLocationValues)
            .ForEach(x => x.SetValidator(new MemberNotificationLocationValuesValidator()));
    }
}
