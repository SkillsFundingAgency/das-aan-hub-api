using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.CalendarEvents.Commands.CreateCalendarEvent;
using ErrorConstants = SFA.DAS.AANHub.Application.CalendarEvents.Commands.CreateCalendarEvent.CreateCalendarEventCommandValidator;

namespace SFA.DAS.AANHub.Application.UnitTests.CalendarEvents.Commands.CreateCalendarEvent.CreateCalendarEventCommandValidatorTests;

public class ValidateAdminMemberId
{
    [TestCase("00000000-0000-0000-0000-000000000000", false, ErrorConstants.RequestedByMemberIdMustNotBeEmpty)]
    [TestCase("dc5079c5-ed1a-4e43-8b31-2335845cd437", false, ErrorConstants.RequestedByMemberIdMustBeAdmin)]
    [TestCase(CreateCalendarEventCommandValidatorBuilder.AdminInactiveMemberId, false, ErrorConstants.RequestedByMemberIdMustBeAdmin)]
    [TestCase(CreateCalendarEventCommandValidatorBuilder.EmployerRegionalChairInactiveMemberId, false, ErrorConstants.RequestedByMemberIdMustBeAdmin)]
    [TestCase(CreateCalendarEventCommandValidatorBuilder.ApprenticeRegionalChairInactiveMemberId, false, ErrorConstants.RequestedByMemberIdMustBeAdmin)]
    [TestCase(CreateCalendarEventCommandValidatorBuilder.EmployerActiveMemberId, false, ErrorConstants.RequestedByMemberIdMustBeAdmin)]
    [TestCase(CreateCalendarEventCommandValidatorBuilder.ApprenticeActiveMemberId, false, ErrorConstants.RequestedByMemberIdMustBeAdmin)]
    [TestCase(CreateCalendarEventCommandValidatorBuilder.AdminActiveMemberId, true, null)]
    [TestCase(CreateCalendarEventCommandValidatorBuilder.EmployerRegionalChairActiveMemberId, true, null)]
    [TestCase(CreateCalendarEventCommandValidatorBuilder.ApprenticeRegionalChairActiveMemberId, true, null)]
    public async Task Validate_AdminMemberId_ShouldBeValidValue(string adminMemberId, bool isValid, string? errorMessage)
    {
        var sut = CreateCalendarEventCommandValidatorBuilder.Create();

        CreateCalendarEventCommand command = new()
        {
            AdminMemberId = adminMemberId.ToGuid()
        };

        var result = await sut.TestValidateAsync(command);

        if (isValid)
        {
            result.ShouldNotHaveValidationErrorFor(c => c.AdminMemberId);
            result.ShouldHaveAnyValidationError();
        }
        else
        {
            result
                .ShouldHaveValidationErrorFor(c => c.AdminMemberId)
                .WithErrorMessage(errorMessage);
            result.ShouldNotHaveValidationErrorFor(c => c.CalendarId);
        }
    }
}
