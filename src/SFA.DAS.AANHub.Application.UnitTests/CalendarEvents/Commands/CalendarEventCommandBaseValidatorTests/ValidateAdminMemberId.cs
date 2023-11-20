using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.CalendarEvents.Commands.CreateCalendarEvent;
using ErrorConstants = SFA.DAS.AANHub.Application.CalendarEvents.Commands.CalendarEventCommandBase.CalendarEventCommandBaseValidator;

namespace SFA.DAS.AANHub.Application.UnitTests.CalendarEvents.Commands.CalendarEventCommandBaseValidatorTests;

public class ValidateAdminMemberId
{
    [TestCase("00000000-0000-0000-0000-000000000000", false, ErrorConstants.RequestedByMemberIdMustNotBeEmpty)]
    [TestCase("dc5079c5-ed1a-4e43-8b31-2335845cd437", false, ErrorConstants.RequestedByMemberIdMustBeAdmin)]
    [TestCase(CalendarEventCommandBaseValidatorBuilder.AdminInactiveMemberId, false, ErrorConstants.RequestedByMemberIdMustBeAdmin)]
    [TestCase(CalendarEventCommandBaseValidatorBuilder.EmployerRegionalChairInactiveMemberId, false, ErrorConstants.RequestedByMemberIdMustBeAdmin)]
    [TestCase(CalendarEventCommandBaseValidatorBuilder.ApprenticeRegionalChairInactiveMemberId, false, ErrorConstants.RequestedByMemberIdMustBeAdmin)]
    [TestCase(CalendarEventCommandBaseValidatorBuilder.EmployerActiveMemberId, false, ErrorConstants.RequestedByMemberIdMustBeAdmin)]
    [TestCase(CalendarEventCommandBaseValidatorBuilder.ApprenticeActiveMemberId, false, ErrorConstants.RequestedByMemberIdMustBeAdmin)]
    [TestCase(CalendarEventCommandBaseValidatorBuilder.AdminActiveMemberId, true, null)]
    [TestCase(CalendarEventCommandBaseValidatorBuilder.EmployerRegionalChairActiveMemberId, true, null)]
    [TestCase(CalendarEventCommandBaseValidatorBuilder.ApprenticeRegionalChairActiveMemberId, true, null)]
    public async Task Validate_AdminMemberId_ShouldBeValidValue(string adminMemberId, bool isValid, string? errorMessage)
    {
        var sut = CalendarEventCommandBaseValidatorBuilder.Create();

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
