using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.CalendarEvents.Commands.CreateCalendarEvent;
using ErrorConstants = SFA.DAS.AANHub.Application.CalendarEvents.Commands.CalendarEventCommandBase.CalendarEventCommandBaseValidator;

namespace SFA.DAS.AANHub.Application.UnitTests.CalendarEvents.Commands.CreateCalendarEvent.CreateCalendarEventCommandValidatorTests;

public class ValidateEventTitle
{
    [TestCase(0, null, false, ErrorConstants.TitleMustNotBeEmpty)]
    [TestCase(0, "", false, ErrorConstants.TitleMustNotBeEmpty)]
    [TestCase(0, " ", false, ErrorConstants.TitleMustNotBeEmpty)]
    [TestCase(0, "@ event", false, ErrorConstants.TitleMustExcludeSpecialCharacters)]
    [TestCase(0, "# event", false, ErrorConstants.TitleMustExcludeSpecialCharacters)]
    [TestCase(0, "$ event", false, ErrorConstants.TitleMustExcludeSpecialCharacters)]
    [TestCase(0, "^ event", false, ErrorConstants.TitleMustExcludeSpecialCharacters)]
    [TestCase(0, "= event", false, ErrorConstants.TitleMustExcludeSpecialCharacters)]
    [TestCase(0, "+ event", false, ErrorConstants.TitleMustExcludeSpecialCharacters)]
    [TestCase(0, "\\ event", false, ErrorConstants.TitleMustExcludeSpecialCharacters)]
    [TestCase(0, "/ event", false, ErrorConstants.TitleMustExcludeSpecialCharacters)]
    [TestCase(0, "< event", false, ErrorConstants.TitleMustExcludeSpecialCharacters)]
    [TestCase(0, "> event", false, ErrorConstants.TitleMustExcludeSpecialCharacters)]
    [TestCase(0, "% event", false, ErrorConstants.TitleMustExcludeSpecialCharacters)]
    [TestCase(0, "some valid title ", true, null)]
    [TestCase(201, null, false, ErrorConstants.TitleMustNotExceedLength)]
    public async Task Validate_EventTitle_ShouldHaveValidValue(int length, string? title, bool isValid, string? errorMessage)
    {
        var sut = CreateCalendarEventCommandValidatorBuilder.Create();

        CreateCalendarEventCommand command = new()
        {
            AdminMemberId = CreateCalendarEventCommandValidatorBuilder.AdminActiveMemberId.ToGuid(),
            Title = length == 0 ? title : new string('a', length)
        };

        var result = await sut.TestValidateAsync(command);

        if (isValid)
        {
            result.ShouldNotHaveValidationErrorFor(c => c.Title);
        }
        else
        {
            result.ShouldHaveValidationErrorFor(c => c.Title).WithErrorMessage(errorMessage);
        }
    }
}
