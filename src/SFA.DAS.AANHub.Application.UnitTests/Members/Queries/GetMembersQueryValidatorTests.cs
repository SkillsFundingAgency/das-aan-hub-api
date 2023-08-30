using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Members.Queries.GetMembers;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using SFA.DAS.AANHub.Domain.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Application.UnitTests.Members.Queries;
public class GetMembersQueryValidatorTests
{
    [Test, RecursiveMoqAutoData]
    public async Task ValidateMemberId_NotActiveMemberId_FailsValidation(Member member, CancellationToken cancellationToken)
    {

        var membersReadRepositoryMock = new Mock<IMembersReadRepository>();
        membersReadRepositoryMock.Setup(m => m.GetMember(member.Id))
            .ReturnsAsync(member);

        var query = new GetMembersQuery
        {
            RequestedByMemberId = member.Id,
            UserType = "employer",
            IsRegionalChair = false,
            Keyword = "test",
            RegionIds = new List<int>(),
            Page = 1,
            PageSize = 5
        };

        var membersSummary = (List<MembersSummary>)null!;

        membersReadRepositoryMock
            .Setup(c => c.GetMembers(new GetMembersOptions
            {
                MemberId = member.Id,
                UserType = "employer",
                IsRegionalChair = false,
                Keyword = "test",
                RegionIds = new List<int>(),
                Page = 1,
                PageSize = 5
            }, It.IsAny<CancellationToken>()))
            .ReturnsAsync(membersSummary);
        var sut = new GetMembersQueryValidator(membersReadRepositoryMock.Object);

        //Action
        var result = await sut.TestValidateAsync(query);

        result.ShouldHaveValidationErrorFor(c => c.RequestedByMemberId);
    }

    [TestCase(AANHub.Domain.Common.Constants.MembershipStatus.Pending)]
    [TestCase(AANHub.Domain.Common.Constants.MembershipStatus.Deleted)]
    [TestCase(AANHub.Domain.Common.Constants.MembershipStatus.Withdrawn)]
    [TestCase(AANHub.Domain.Common.Constants.MembershipStatus.Cancelled)]
    public async Task ValidateMemberId_MembershipStatusNotActive_FailsValidation(string inactiveMembershipStatus)
    {
        var inactiveGuid = Guid.NewGuid();

        var membersReadRepositoryMock = new Mock<IMembersReadRepository>();
        membersReadRepositoryMock.Setup(m => m.GetMember(inactiveGuid))
            .ReturnsAsync(new Member() { Status = inactiveMembershipStatus });

        var query = new GetMembersQuery
        {
            RequestedByMemberId = inactiveGuid,
            UserType = "employer",
            IsRegionalChair = false,
            Keyword = "test",
            RegionIds = new List<int>(),
            Page = 1,
            PageSize = 5
        };

        var sut = new GetMembersQueryValidator(membersReadRepositoryMock.Object);

        var result = await sut.TestValidateAsync(query);

        result.ShouldHaveValidationErrorFor(x => x.RequestedByMemberId);
    }
}
