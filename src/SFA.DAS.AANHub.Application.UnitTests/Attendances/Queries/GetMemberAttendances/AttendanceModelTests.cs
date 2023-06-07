using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Attendances.Queries.GetMemberAttendances;
using SFA.DAS.AANHub.Domain.Common;

namespace SFA.DAS.AANHub.Application.UnitTests.Attendances.Queries.GetMemberAttendances;

public class AttendanceModelTests
{
    [Test]
    public void Operator_CreateAttendanceModelFromAttendance()
    {
        var attendance = AttendanceTestDataHelper.GetAttendances().First();

        AttendanceModel sut = attendance;

        sut.CalendarEventId.Should().Be(attendance.CalendarEventId);
        sut.EventFormat.Should().Be(EventFormat.InPerson);
        sut.EventStartDate.Should().Be(attendance.CalendarEvent.StartDate);
        sut.EventDescription.Should().Be(attendance.CalendarEvent.Description);
    }
}
