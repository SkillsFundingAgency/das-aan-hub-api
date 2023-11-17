using FluentValidation;
using SFA.DAS.AANHub.Domain.Common;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.CalendarEvents.Commands.CalendarEventCommandBase;

public class CalendarEventCommandBaseValidator : AbstractValidator<CalendarEventCommandBase>
{
    public const string CalendarTypeIdMustNotBeEmpty = "calendarId must have a value";
    public const string CalendarTypeIdMustBeValid = "calendarId must have a valid value";
    public const string EventFormatMustNotBeEmpty = "eventFormat must have a valid value";
    public const string StartDateMustNotBeEmpty = "startDate must have a valid value";
    public const string StartDateMustBeInFuture = "startDate must be a future date";
    public const string StartDateMustBeLessThanEndDate = "startDate must be less than or equal to endDate";
    public const string EndDateMustNotBeEmpty = "endDate must have a valid value";
    public const string EndDateMustBeInFuture = "endDate must be a future date";
    public const string EndDateMustBeLessThanStartDate = "endDate must be greater than or equal to startDate";
    public const string TitleMustNotBeEmpty = "title must have a value";
    public const string TitleMustNotExceedLength = "title must not be greater than 200 characters long";
    public const string TitleMustExcludeSpecialCharacters = "title must not include any special characters: @, #, $, ^, =, +, \\, /, <, >, %";
    public const string SummaryMustNotBeEmpty = "summary must have a value";
    public const string SummaryMustNotExceedLength = "summary must not be greater than 200 characters long";
    public const string DescriptionMustNotBeEmpty = "description must have a value";
    public const string DescriptionMustNotExceedLength = "description must not be greater than 2000 characters long";
    public const string LocationMustNotBeEmpty = "location must have a value when event format is InPerson or Hybrid";
    public const string LocationMustBeEmpty = "location must be empty when event format is Online";
    public const string LocationMustNotExceedLength = "location must not be greater than 2000 characters long";
    public const string PostcodeMustNotBeEmpty = "postcode must have a value when event format is InPerson or Hybrid";
    public const string PostcodeMustBeEmpty = "postcode must be empty when event format is Online";
    public const string PostcodeMustBeValid = "postcode must be a valid in postcode format";
    public const string LatitudeMustNotBeEmpty = "latitude must have a value when event format is InPerson or Hybrid";
    public const string LatitudeMustBeEmpty = "latitude must be empty when event format is Online";
    public const string LatitudeMustBeValid = "latitude must be between -90 and 90";
    public const string LongitudeMustNotBeEmpty = "longitude must have a value when event format is InPerson or Hybrid";
    public const string LongitudeMustBeEmpty = "longitude must be empty when event format is Online";
    public const string LongitudeMustBeValid = "longitude must be between -180 and 180";
    public const string EventLinkMustBeValid = "eventLink must be a valid url";
    public const string EventLinkMustBeEmpty = "eventLink must be empty when event format is InPerson";
    public const string EventLinkMustNotExceedLength = "eventLink must not be greater than 2000 characters long";
    public const string ContactNameMustNotBeEmpty = "contactName must have a value";
    public const string ContactNameMustNotExceedLength = "contactName must not be greater than 200 characters long";
    public const string ContactEmailMustNotBeEmpty = "contactEmail must have a value";
    public const string ContactEmailMustNotExceedLength = "contactEmail must not be greater than 256 characters long";
    public const string ContactEmailMustBeValid = "contactEmail must be a valid email format";
    public const string PlannedAttendeesMustNotBeEmpty = "plannedAttendees must have a value";
    public const string PlannedAttendeesMustBeValid = "plannedAttendees must be  between 1 to 1000000";
    public const string RequestedByMemberIdMustNotBeEmpty = "requestedByMemberId must have a value";
    public const string RequestedByMemberIdMustBeAdmin = "requestedByMemberId must be an active, admin member or regional chair";

    public CalendarEventCommandBaseValidator(
        ICalendarsReadRepository calendarsReadRepository,
        IRegionsReadRepository regionsReadRepository,
        IMembersReadRepository membersReadRepository)
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(c => c.AdminMemberId)
            .NotEmpty()
            .WithMessage(RequestedByMemberIdMustNotBeEmpty)
            .MustAsync(async (memberId, cancellationToken) =>
            {
                var member = await membersReadRepository.GetMember(memberId);
                return
                    member != null &&
                    member!.Status == MembershipStatusType.Live.ToString() &&
                    (member.UserType == UserType.Admin || member.IsRegionalChair.GetValueOrDefault());
            })
            .WithMessage(RequestedByMemberIdMustBeAdmin)
            .DependentRules(() =>
            {
                RuleFor(c => c.CalendarId)
                    .GreaterThan(0)
                    .WithMessage(CalendarTypeIdMustNotBeEmpty)
                    .MustAsync(async (id, cancellationToken) =>
                    {
                        var calendarTypes = await calendarsReadRepository.GetAllCalendars(cancellationToken);
                        return calendarTypes.Exists(c => c.Id == id);
                    })
                    .WithMessage(CalendarTypeIdMustBeValid);

                RuleFor(c => c.EventFormat)
                    .NotEmpty()
                    .WithMessage(EventFormatMustNotBeEmpty);

                RuleFor(c => c.StartDate)
                    .Cascade(CascadeMode.Stop)
                    .NotEmpty()
                    .WithMessage(StartDateMustNotBeEmpty)
                    .GreaterThan(DateTime.UtcNow)
                    .WithMessage(StartDateMustBeInFuture)
                    .LessThanOrEqualTo(c => c.EndDate)
                    .WithMessage(StartDateMustBeLessThanEndDate);

                RuleFor(c => c.EndDate)
                    .NotEmpty()
                    .WithMessage(EndDateMustNotBeEmpty)
                    .GreaterThan(DateTime.UtcNow)
                    .WithMessage(EndDateMustBeInFuture)
                    .GreaterThanOrEqualTo(c => c.StartDate)
                    .WithMessage(EndDateMustBeLessThanStartDate);

                RuleFor(c => c.Title)
                    .NotEmpty()
                    .WithMessage(TitleMustNotBeEmpty)
                    .MaximumLength(200)
                    .WithMessage(TitleMustNotExceedLength)
                    .Matches(Constants.RegularExpressions.ExcludedCharactersRegex)
                    .WithMessage(TitleMustExcludeSpecialCharacters);

                RuleFor(c => c.Summary)
                    .NotEmpty()
                    .WithMessage(SummaryMustNotBeEmpty)
                    .MaximumLength(200)
                    .WithMessage(SummaryMustNotExceedLength);

                RuleFor(c => c.Description)
                    .NotEmpty()
                    .WithMessage(DescriptionMustNotBeEmpty)
                    .MaximumLength(2000)
                    .WithMessage(DescriptionMustNotExceedLength);

                RuleFor(c => c.RegionId)
                    .MustAsync(async (regionId, cancellationToken) =>
                    {
                        var regions = await regionsReadRepository.GetAllRegions(cancellationToken);
                        return regions.Exists(r => r.Id == regionId);
                    })
                    .When(c => c.RegionId.HasValue);

                When(c => c.EventFormat == EventFormat.InPerson, () =>
                {
                    RuleFor(c => c.Location)
                        .NotEmpty()
                        .WithMessage(LocationMustNotBeEmpty)
                        .MaximumLength(200)
                        .WithMessage(LocationMustNotExceedLength);

                    RuleFor(c => c.Postcode)
                        .NotEmpty()
                        .WithMessage(PostcodeMustNotBeEmpty)
                        .Matches(Constants.RegularExpressions.PostcodeRegex)
                        .WithMessage(PostcodeMustBeValid);

                    RuleFor(c => c.Latitude)
                        .NotEmpty()
                        .WithMessage(LatitudeMustNotBeEmpty)
                        .InclusiveBetween(-90, 90)
                        .WithMessage(LatitudeMustBeValid);

                    RuleFor(c => c.Longitude)
                        .NotEmpty()
                        .WithMessage(LongitudeMustNotBeEmpty)
                        .InclusiveBetween(-180, 180)
                        .WithMessage(LongitudeMustBeValid);

                    RuleFor(c => c.EventLink)
                        .Empty()
                        .WithMessage(EventLinkMustBeEmpty);
                });

                When(c => c.EventFormat == EventFormat.Online, () =>
                {
                    RuleFor(c => c.Location)
                        .Empty()
                        .WithMessage(LocationMustBeEmpty);

                    RuleFor(c => c.Postcode)
                        .Empty()
                        .WithMessage(PostcodeMustBeEmpty);

                    RuleFor(c => c.Latitude)
                        .Empty()
                        .WithMessage(LatitudeMustBeEmpty);

                    RuleFor(c => c.Longitude)
                        .Empty()
                        .WithMessage(LongitudeMustBeEmpty);

                    RuleFor(c => c.EventLink)
                        .MaximumLength(2000)
                        .WithMessage(EventLinkMustNotExceedLength)
                        .Matches(Constants.RegularExpressions.UrlRegex)
                        .WithMessage(EventLinkMustBeValid);
                });

                When(c => c.EventFormat == EventFormat.Hybrid, () =>
                {
                    RuleFor(c => c.Location)
                        .NotEmpty()
                        .WithMessage(LocationMustNotBeEmpty)
                        .MaximumLength(200)
                        .WithMessage(LocationMustNotExceedLength);

                    RuleFor(c => c.Postcode)
                        .NotEmpty()
                        .WithMessage(PostcodeMustNotBeEmpty)
                        .Matches(Constants.RegularExpressions.PostcodeRegex)
                        .WithMessage(PostcodeMustBeValid);

                    RuleFor(c => c.Latitude)
                        .NotEmpty()
                        .WithMessage(LatitudeMustNotBeEmpty)
                        .InclusiveBetween(-90, 90)
                        .WithMessage(LatitudeMustBeValid);

                    RuleFor(c => c.Longitude)
                        .NotEmpty()
                        .WithMessage(LongitudeMustNotBeEmpty)
                        .InclusiveBetween(-180, 180)
                        .WithMessage(LongitudeMustBeValid);

                    RuleFor(c => c.EventLink)
                        .MaximumLength(2000)
                        .WithMessage(EventLinkMustNotExceedLength)
                        .Matches(Constants.RegularExpressions.UrlRegex)
                        .WithMessage(EventLinkMustBeValid);
                });

                RuleFor(c => c.ContactName)
                    .NotEmpty()
                    .WithMessage(ContactNameMustNotBeEmpty)
                    .MaximumLength(200)
                    .WithMessage(ContactNameMustNotExceedLength);

                RuleFor(c => c.ContactEmail)
                    .NotEmpty()
                    .WithMessage(ContactEmailMustNotBeEmpty)
                    .MaximumLength(256)
                    .WithMessage(ContactEmailMustNotExceedLength)
                    .Matches(Constants.RegularExpressions.EmailRegex)
                    .WithMessage(ContactEmailMustBeValid);

                RuleFor(c => c.PlannedAttendees)
                    .NotEmpty()
                    .WithMessage(PlannedAttendeesMustNotBeEmpty)
                    .InclusiveBetween(1, 1000000)
                    .WithMessage(PlannedAttendeesMustBeValid);
            });
    }
}
