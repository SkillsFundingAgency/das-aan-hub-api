using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.CalendarEvents.Queries.GetCalendarEvent;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using SFA.DAS.Testing.AutoFixture;
using static SFA.DAS.AANHub.Domain.Common.Constants;

namespace SFA.DAS.AANHub.Application.UnitTests.CalendarEvents.Queries;

public class GetCalendarEventByIdQueryValidatorTests
{
    [Test]
    [RecursiveMoqAutoData]
    public async Task ValidateCalendarId_Missing_FailsValidation(Member member)
    {
        var membersReadRepositoryMock = new Mock<IMembersReadRepository>();
        membersReadRepositoryMock.Setup(m => m.GetMember(member.Id))
                                 .ReturnsAsync(member);

        var query = new GetCalendarEventByIdQuery(Guid.Empty, member.Id);

        var sut = new GetCalendarEventByIdQueryValidator(membersReadRepositoryMock.Object);

        var result = await sut.TestValidateAsync(query);

        result.ShouldHaveValidationErrorFor(c => c.CalendarEventId)
              .WithErrorMessage(GetCalendarEventByIdQueryValidator.CalendarEventIdMissingMessage);
    }

    [TestCase(MembershipStatus.Pending)]
    [TestCase(MembershipStatus.Deleted)]
    [TestCase(MembershipStatus.Withdrawn)]
    [TestCase(MembershipStatus.Cancelled)]
    public async Task ValidateCalendarId_Inactive_FailsValidation(string inactiveMembershipStatus)
    {
        var inactiveGuid = Guid.NewGuid();

        var membersReadRepositoryMock = new Mock<IMembersReadRepository>();
        membersReadRepositoryMock.Setup(m => m.GetMember(inactiveGuid))
                                 .ReturnsAsync(new Member() { Status = inactiveMembershipStatus });

        var query = new GetCalendarEventByIdQuery(Guid.NewGuid(), inactiveGuid);

        var sut = new GetCalendarEventByIdQueryValidator(membersReadRepositoryMock.Object);

        var result = await sut.TestValidateAsync(query);

        result.ShouldHaveValidationErrorFor(x => x.RequestedByMemberId);
    }
}