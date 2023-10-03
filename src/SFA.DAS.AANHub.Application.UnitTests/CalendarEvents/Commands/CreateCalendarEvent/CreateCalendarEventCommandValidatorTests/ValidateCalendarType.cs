using AutoFixture;
using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.CalendarEvents.Commands.CreateCalendarEvent;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.UnitTests.CalendarEvents.Commands.CreateCalendarEvent.CreateCalendarEventCommandValidatorTests;

public class ValidateCalendarType
{
    [TestCase(1, true, null)]
    [TestCase(0, false, CreateCalendarEventCommandValidator.CalendarTypeIdMustNotBeEmpty)]
    [TestCase(-1, false, CreateCalendarEventCommandValidator.CalendarTypeIdMustNotBeEmpty)]
    [TestCase(6, false, CreateCalendarEventCommandValidator.CalendarTypeIdMustBeValid)]
    public async Task ShouldBeValidValue(int calendarId, bool isValid, string? error)
    {
        Fixture fixture = new();
        var i = 1;
        var calendars = fixture
            .Build<Calendar>()
            .With(c => c.Id, () => i++)
            .CreateMany()
            .ToList();
        CancellationToken cancellationToken = new();
        Mock<ICalendarsReadRepository> calendarRepoMock = new();
        calendarRepoMock.Setup(r => r.GetAllCalendars(cancellationToken)).ReturnsAsync(calendars);

        CreateCalendarEventCommand command = new();
        command.CalendarId = calendarId;

        CreateCalendarEventCommandValidator sut = new(calendarRepoMock.Object);

        var result = await sut.TestValidateAsync(command);
        if (isValid)
        {
            result.ShouldNotHaveValidationErrorFor(s => s.CalendarId);
        }
        else
        {
            result
                .ShouldHaveValidationErrorFor(s => s.CalendarId)
                .WithErrorMessage(error);
        }
    }
}
