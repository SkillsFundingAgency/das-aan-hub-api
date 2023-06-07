using AutoFixture;
using SFA.DAS.AANHub.Domain.Common;
using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Application.UnitTests.Attendances.Queries.GetMemberAttendances;
public static class AttendanceTestDataHelper
{
    public static List<Attendance> GetAttendances()
    {
        Fixture fixture = new();
        return fixture
           .Build<Attendance>()
           .Without(a => a.Member)
           .With(a => a.CalendarEvent,
               fixture.Build<CalendarEvent>()
                   .With(c => c.EventFormat, EventFormat.InPerson.ToString())
                   .Without(c => c.Attendees)
                   .Without(c => c.Calendar)
                   .Without(c => c.EventGuests)
                   .Create())
           .CreateMany()
           .ToList();
    }
}
