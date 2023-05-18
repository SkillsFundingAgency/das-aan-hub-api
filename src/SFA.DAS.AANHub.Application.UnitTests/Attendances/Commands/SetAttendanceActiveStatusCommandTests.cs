using FluentAssertions;
using FluentAssertions.Extensions;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Attendances.Commands.PutAttendance;
using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Application.UnitTests.Attendances.Commands
{
    public class SetAttendanceActiveStatusCommandTests
    {
        [TestCase(true)]
        [TestCase(false)]
        public void Operator_CreatesAttendance(bool requestedActiveStatus)
        {
            var sut = new PutAttendanceCommand(Guid.NewGuid(), Guid.NewGuid(), requestedActiveStatus);
            Attendance attendance = sut;

            attendance.CalendarEventId.Should().Be(sut.CalendarEventId);

            attendance.MemberId.Should().Be(sut.RequestedByMemberId);

            attendance.AddedDate.Should().BeWithin(2.Minutes()).Before(DateTime.UtcNow);

            attendance.IsActive.Should().Be(requestedActiveStatus);
        }
    }
}
