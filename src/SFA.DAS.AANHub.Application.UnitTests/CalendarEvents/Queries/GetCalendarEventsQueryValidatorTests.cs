using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.CalendarEvents.Queries;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using SFA.DAS.AANHub.Domain.Models;

namespace SFA.DAS.AANHub.Application.UnitTests.CalendarEvents.Queries;
public class GetCalendarEventsQueryValidatorTests
{
    [TestCase("00000000-0000-0000-0000-000000000000", false)]
    [TestCase("B46C2A2A-E35C-4788-B4B7-1F7E84081846", true)]
    public async Task Validates_MemberId_NotNull(string guid, bool isValid)
    {
        var memberId = Guid.Parse(guid);
        var query = new GetCalendarEventsQuery(memberId, 1);
        var calendarEvents = isValid ? new List<CalendarEventModel>() : null;
        var calendarEventsReadRepositoryMock = new Mock<ICalendarEventsReadRepository>();

        calendarEventsReadRepositoryMock.Setup(a => a.GetCalendarEvents(memberId))!.ReturnsAsync(calendarEvents);
        var sut = new GetCalendarEventsQueryValidator();
        var result = await sut.TestValidateAsync(query);

        if (isValid)
            result.ShouldNotHaveAnyValidationErrors();
        else
            result.ShouldHaveValidationErrorFor(c => c.RequestedByMemberId);
    }
}
