using FluentValidation;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Domain.Common;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.Employers.Commands.CreateEmployerMember;

public class CreateEmployerMemberCommandValidator : AbstractValidator<CreateEmployerMemberCommand>
{
    public const string UserRefAlreadyCreatedErrorMessage = "UserRef already exists";
    public const string ProfileValuesMustNotBeEmptyErrorMessage = "ProfileValues cannot be empty";
    public const string InvalidProfileIdsErrorMessage = "Some of the profile ids are invalid for Employer user type";
    public const string EventFormatCannotBeEmptyIfReceiveNotificationsErrorMessage = "EventFormat cannot be empty if ReceiveNotifications is true.";
    public const string LocationCannotBeEmptyForNonOnlineEventErrorMessage = "Location cannot be empty if the event format is selected and is not 'Online'.";

    private readonly IEmployersReadRepository _employersReadRepository;
    private readonly IProfilesReadRepository _profilesReadRepository;

    public CreateEmployerMemberCommandValidator(IEmployersReadRepository employersReadRepository, IProfilesReadRepository profilesReadRepository, IMembersReadRepository membersReadRepository, IRegionsReadRepository regionsReadRepository)
    {
        _employersReadRepository = employersReadRepository;
        _profilesReadRepository = profilesReadRepository;

        Include(new CreateMemberCommandBaseValidator(membersReadRepository, regionsReadRepository));

        RuleFor(c => c.UserRef)
            .NotEmpty()
            .MustAsync(async (command, _, _) => await IsNewUser(command.UserRef))
            .WithMessage(UserRefAlreadyCreatedErrorMessage);

        RuleFor(c => c.AccountId)
            .NotEmpty();

        RuleFor(c => c.ProfileValues)
            .NotEmpty()
            .WithMessage(ProfileValuesMustNotBeEmptyErrorMessage)
            .ForEach(x => x.SetValidator(new ProfileValueValidator()))
            .MustAsync(async (profileValues, _) =>
            {
                var profiles = await _profilesReadRepository.GetProfilesByUserType(UserType.Employer);
                var profileIds = profiles.Select(profile => profile.Id);
                var b = profileValues.All(p => profileIds.Contains(p.Id));
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
                !string.Equals(eventFormat.EventFormat, EventFormat.Online.ToString(), StringComparison.OrdinalIgnoreCase)) == true)
            .WithMessage(LocationCannotBeEmptyForNonOnlineEventErrorMessage);

        RuleFor(c => c.MemberNotificationEventFormatValues)
            .ForEach(x => x.SetValidator(new MemberNotificationEventFormatValuesValidator()));

        RuleFor(c => c.MemberNotificationLocationValues)
            .ForEach(x => x.SetValidator(new MemberNotificationLocationValuesValidator()));
    }

    private async Task<bool> IsNewUser(Guid userRef)
    {
        var result = await _employersReadRepository.GetEmployerByUserRef(userRef);
        return result == null;
    }
}