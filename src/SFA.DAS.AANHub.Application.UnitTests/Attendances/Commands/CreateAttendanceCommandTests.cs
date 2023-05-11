using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Attendances.Commands.CreateAttendance;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Application.UnitTests.Attendances.Commands;

public class CreateAttendanceCommandTests
{
    [Test]
    [MoqAutoData]
    public void Operator_ConvertsToAttendance(CreateAttendanceCommand sut)
    {
        Attendance attendance = sut;

        attendance.Id.Should().Be(sut.Id);
        attendance.CalendarEventId.Should().Be(sut.CalendarEventId);
        attendance.MemberId.Should().Be(sut.MemberId);
        attendance.AddedDate.Should().Be(sut.AddedDate);
        attendance.IsActive.Should().Be(sut.IsActive);
        attendance.Attended.Should().Be(sut.Attended);
        attendance.OverallRating.Should().Be(sut.OverallRating);
        attendance.FeedbackCompletedDate.Should().Be(sut.FeedbackCompletedDate);
    }
}
