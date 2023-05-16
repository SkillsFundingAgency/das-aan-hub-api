using NUnit.Framework;
using SFA.DAS.AANHub.Application.Models;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Application.UnitTests.Models;
public class AttendeeModelTests
{
    [Test]
    [RecursiveMoqAutoData]
    public void Attendance_IsConvertedTo_AttendeeModel_WithPropertiesFromAttendance(Attendance source)
    {
        AttendeeModel sut = source;

        Assert.Multiple(() =>
        {
            Assert.That(sut.AttendanceId, Is.EqualTo(source.Id));
            Assert.That(sut.MemberId, Is.EqualTo(source.MemberId));
            Assert.That(sut.MemberName, Is.EqualTo(source.Member.FullName));
        });
    }
}
