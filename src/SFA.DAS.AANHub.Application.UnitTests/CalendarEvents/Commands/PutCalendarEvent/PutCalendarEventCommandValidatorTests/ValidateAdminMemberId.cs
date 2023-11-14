using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.CalendarEvents.Commands.PutCalendarEvent;
using ErrorConstants = SFA.DAS.AANHub.Application.CalendarEvents.Commands.CalendarEventCommandBase.CalendarEventCommandBaseValidator;

namespace SFA.DAS.AANHub.Application.UnitTests.CalendarEvents.Commands.PutCalendarEvent.PutCalendarEventCommandValidatorTests;

public class ValidateAdminMemberId
{
    [TestCase("00000000-0000-0000-0000-000000000000", false, ErrorConstants.RequestedByMemberIdMustNotBeEmpty)]
    [TestCase("dc5079c5-ed1a-4e43-8b31-2335845cd437", false, ErrorConstants.RequestedByMemberIdMustBeAdmin)]
    [TestCase(PutCalendarEventCommandValidatorBuilder.AdminInactiveMemberId, false, ErrorConstants.RequestedByMemberIdMustBeAdmin)]
    [TestCase(PutCalendarEventCommandValidatorBuilder.EmployerRegionalChairInactiveMemberId, false, ErrorConstants.RequestedByMemberIdMustBeAdmin)]
    [TestCase(PutCalendarEventCommandValidatorBuilder.ApprenticeRegionalChairInactiveMemberId, false, ErrorConstants.RequestedByMemberIdMustBeAdmin)]
    [TestCase(PutCalendarEventCommandValidatorBuilder.EmployerActiveMemberId, false, ErrorConstants.RequestedByMemberIdMustBeAdmin)]
    [TestCase(PutCalendarEventCommandValidatorBuilder.ApprenticeActiveMemberId, false, ErrorConstants.RequestedByMemberIdMustBeAdmin)]
    [TestCase(PutCalendarEventCommandValidatorBuilder.AdminActiveMemberId, true, null)]
    [TestCase(PutCalendarEventCommandValidatorBuilder.EmployerRegionalChairActiveMemberId, true, null)]
    [TestCase(PutCalendarEventCommandValidatorBuilder.ApprenticeRegionalChairActiveMemberId, true, null)]
    public async Task Validate_AdminMemberId_ShouldBeValidValue(string adminMemberId, bool isValid, string? errorMessage)
    {
        var sut = PutCalendarEventCommandValidatorBuilder.Create();

        PutCalendarEventCommand command = new()
        {
            AdminMemberId = PutCalendarEventCommandValidatorBuilderExtensions.ToGuid(adminMemberId)
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
