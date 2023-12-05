using System.Data;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Members.Queries.GetMembers;
using SFA.DAS.AANHub.Domain.Common;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using SFA.DAS.AANHub.Domain.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Application.UnitTests.Members.Queries.GetMembers;

public class GetMembersQueryHandlerTests
{
    [Test, RecursiveMoqAutoData]
    public async Task Handle_MembersNotFound_ReturnsEmptyList(
        List<int> regionIds,
        string keyword,
        List<UserType> userTypes,
        CancellationToken cancellationToken
     )
    {

        var membersReadRepositoryMock = new Mock<IMembersReadRepository>();
        membersReadRepositoryMock.Setup(c => c.GetMembers(It.IsAny<GetMembersOptions>(), cancellationToken))
        .ReturnsAsync(() => new List<MemberSummary>());

        var sut = new GetMembersQueryHandler(membersReadRepositoryMock.Object);

        var query = new GetMembersQuery
        {
            Keyword = keyword,
            UserTypes = userTypes,
            IsRegionalChair = null,
            RegionIds = regionIds,
            Page = 1,
            PageSize = 5
        };

        var result = await sut.Handle(query, cancellationToken);

        result.Members.Count().Should().Be(0);
        result.TotalCount.Should().Be(0);
    }

    [Test, RecursiveMoqAutoData]
    public async Task Handle_MembersFound_ReturnsMembers(
        CancellationToken cancellationToken
    )
    {
        var membersReadRepositoryMock = new Mock<IMembersReadRepository>();
        var regionIds = new List<int> { 1 };
        var keyword = "test";
        var userType = new List<UserType> { UserType.Employer };
        var status = new List<MembershipStatusType> { MembershipStatusType.Live };
        var membersSummary = new MemberSummary();
        var isRegionalChair = false;
        membersReadRepositoryMock.Setup(c => c.GetMembers(It.IsAny<GetMembersOptions>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<MemberSummary> { membersSummary });

        var sut = new GetMembersQueryHandler(membersReadRepositoryMock.Object);
        var query = new GetMembersQuery
        {
            Keyword = keyword,
            UserTypes = userType,
            IsRegionalChair = isRegionalChair,
            RegionIds = regionIds,
            Page = 1,
            PageSize = 5
        };

        var result = await sut.Handle(query, cancellationToken);

        using (new AssertionScope())
        {
            result.Members.Count().Should().Be(1);
            result.TotalCount.Should().Be(0);
            var members = result.Members;
            members.First().Should().BeEquivalentTo(membersSummary, options => options.Excluding(c => c.MemberId).Excluding(c => c.RegionId).Excluding(c => c.TotalCount));
        }
    }

    [TestCase(null, null)]
    [TestCase("", null)]
    [TestCase("event", "event")]
    [TestCase("west event", "west event")]
    [TestCase("north-west event", "north west event")]
    [TestCase("1 event", "1 event")]
    [TestCase("'--;<>/**/_1 event 2 3", "1 event 2 3")]
    public async Task Handle_Keyword_CheckUsedKeywordExpected(string? keyword, string? expectedKeywordUsed)
    {
        var membersReadRepositoryMock = new Mock<IMembersReadRepository>();
        var cancellationToken = new CancellationToken();
        var membersSummary = new MemberSummary();
        var regionIds = new List<int>();
        var userTypes = new List<UserType>();
        var status = new List<MembershipStatusType>();
        var isRegionalChair = false;

        membersReadRepositoryMock.Setup(c => c.GetMembers(It.IsAny<GetMembersOptions>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<MemberSummary> { membersSummary });

        var sut = new GetMembersQueryHandler(membersReadRepositoryMock.Object);
        var query = new GetMembersQuery
        {
            Keyword = keyword,
            UserTypes = userTypes,
            IsRegionalChair = isRegionalChair,
            RegionIds = regionIds,
            Page = 1,
            PageSize = 5
        };

        await sut.Handle(query, cancellationToken);
        membersReadRepositoryMock.Verify(x => x.GetMembers(
            It.Is<GetMembersOptions>(c => c.Keyword == expectedKeywordUsed), cancellationToken), Times.Once);
    }

    [TestCase(null)]
    [TestCase(true)]
    [TestCase(false)]
    public async Task Handle_IsRegionalChair_CheckIsRegionalChairExpected(bool? isRegionalChair)
    {
        var membersReadRepositoryMock = new Mock<IMembersReadRepository>();
        var cancellationToken = new CancellationToken();
        var membersSummary = new MemberSummary();
        var regionIds = new List<int>();
        var userTypes = new List<UserType>();
        var status = new List<MembershipStatusType>();
        var keyword = "test";

        membersReadRepositoryMock.Setup(c => c.GetMembers(It.IsAny<GetMembersOptions>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<MemberSummary> { membersSummary });

        var sut = new GetMembersQueryHandler(membersReadRepositoryMock.Object);
        var query = new GetMembersQuery
        {
            Keyword = keyword,
            UserTypes = userTypes,
            IsRegionalChair = isRegionalChair,
            RegionIds = regionIds,
            Page = 1,
            PageSize = 5
        };

        await sut.Handle(query, cancellationToken);
        membersReadRepositoryMock.Verify(x => x.GetMembers(
            It.Is<GetMembersOptions>(c => c.IsRegionalChair == isRegionalChair), cancellationToken), Times.Once);
    }

    [Test, TestCaseSource(nameof(_Data))]
    public async Task Handle_UserType_CheckUserTypeExpected(List<UserType> userTypes)
    {
        var membersReadRepositoryMock = new Mock<IMembersReadRepository>();
        var cancellationToken = new CancellationToken();
        var membersSummary = new MemberSummary();
        var regionIds = new List<int>();
        var keyword = "test";
        var isRegionalChair = false;

        membersReadRepositoryMock.Setup(c => c.GetMembers(It.IsAny<GetMembersOptions>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<MemberSummary> { membersSummary });

        var sut = new GetMembersQueryHandler(membersReadRepositoryMock.Object);
        var query = new GetMembersQuery
        {
            Keyword = keyword,
            UserTypes = userTypes,
            IsRegionalChair = isRegionalChair,
            RegionIds = regionIds,
            Page = 1,
            PageSize = 5
        };

        await sut.Handle(query, cancellationToken);
        membersReadRepositoryMock.Verify(x => x.GetMembers(
            It.Is<GetMembersOptions>(c => c.UserTypes == userTypes), cancellationToken), Times.Once);
    }

    [Test, RecursiveMoqAutoData]
    public async Task Handle_UserType_ShouldNotReturnMemberWithAnyUserTypeExceptEmployerAndApprentice(
        CancellationToken cancellationToken
    )
    {
        // arrange
        var membersReadRepositoryMock = new Mock<IMembersReadRepository>();
        var membersSummary = new MemberSummary
        {
            UserType = UserType.Employer
        };
        membersReadRepositoryMock.Setup(c => c.GetMembers(It.IsAny<GetMembersOptions>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<MemberSummary> { membersSummary });
        var sut = new GetMembersQueryHandler(membersReadRepositoryMock.Object);
        var query = new GetMembersQuery
        {
            Keyword = string.Empty,
            UserTypes = new List<UserType>(),
            IsRegionalChair = null,
            RegionIds = new List<int>(),
            Page = 1,
            PageSize = 5
        };

        List<UserType> allowableUserType = new() { UserType.Apprentice, UserType.Employer };

        // act
        var result = await sut.Handle(query, cancellationToken);
        var membersWithOtherUserRole = result.Members.Where(obj => !allowableUserType.Contains(obj.UserType)).ToList();

        // assert
        using (new AssertionScope())
        {
            result.Members.Count().Should().BeGreaterThanOrEqualTo(1);
            Assert.IsEmpty(membersWithOtherUserRole);
        }
    }

    private static readonly object?[] _Data =
    {
      null,
      new object[] {new List<UserType> { UserType.Employer} }
    };
}