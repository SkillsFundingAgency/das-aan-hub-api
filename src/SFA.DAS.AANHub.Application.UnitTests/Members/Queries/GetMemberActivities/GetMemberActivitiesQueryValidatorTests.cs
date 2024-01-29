using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Members.Queries.GetMemberActivities;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.UnitTests.Members.Queries.GetMemberActivities;
public class GetMemberActivitiesQueryValidatorTests
{
    [TestCase("00000000-0000-0000-0000-000000000000", false)]
    [TestCase("B46C2A2A-E35C-4788-B4B7-1F7E84081846", true)]
    public async Task Validates_MemberId_NotNull(string memberGuid, bool isValid)
    {
        // Arrange
        var memberId = Guid.Parse(memberGuid);
        var query = new GetMemberActivitiesQuery(memberId);
        var attendances = new List<Attendance>();
        var audit = new Audit();
        var attendancesReadRepositoryMock = new Mock<IAttendancesReadRepository>();
        var auditReadRepositoryMock = new Mock<IAuditReadRepository>();
        attendancesReadRepositoryMock.Setup(a => a.GetAttendances(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>())).ReturnsAsync(attendances);
        auditReadRepositoryMock.Setup(a => a.GetLastAttendanceAuditByMemberId(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(audit);
        var sut = new GetMemberActivitiesQueryValidator();

        // Act
        var result = await sut.TestValidateAsync(query);

        // Assert
        if (isValid)
            result.ShouldNotHaveValidationErrorFor(c => c.MemberId);
        else
            result.ShouldHaveValidationErrorFor(c => c.MemberId)
              .WithErrorMessage(GetMemberActivitiesQueryValidator.MemberIdMissingMessage);
    }
}