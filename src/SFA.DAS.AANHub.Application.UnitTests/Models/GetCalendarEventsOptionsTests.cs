using NUnit.Framework;
using SFA.DAS.AANHub.Domain.Common;
using SFA.DAS.AANHub.Domain.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Application.UnitTests.Models;
public class GetCalendarEventsOptionsTests
{
    [Test, MoqAutoData]
    public void GetCalendarEventsOptions_ConstructorMapsAsExpected(Guid requestedByMemberId, DateTime? fromDate, DateTime? toDate, List<EventFormat> eventFormats, List<int> calendarIds, List<int> regionIds, int? page)
    {
        var sut = new GetCalendarEventsOptions(requestedByMemberId, fromDate, toDate, eventFormats, calendarIds, regionIds, page);

        Assert.Multiple(() =>
        {
            Assert.That(sut.MemberId, Is.EqualTo(requestedByMemberId));
            Assert.That(sut.FromDate, Is.EqualTo(fromDate));
            Assert.That(sut.ToDate, Is.EqualTo(toDate));
            Assert.That(sut.EventFormats, Is.EqualTo(eventFormats));
            Assert.That(sut.CalendarIds, Is.EqualTo(calendarIds));
            Assert.That(sut.RegionIds, Is.EqualTo(regionIds));
            Assert.That(sut.Page, Is.EqualTo(page));
        });
    }
}