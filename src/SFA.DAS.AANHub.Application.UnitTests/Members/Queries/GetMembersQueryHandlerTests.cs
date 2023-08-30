using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Members.Queries.GetMembers;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using SFA.DAS.AANHub.Domain.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Application.UnitTests.Members.Queries;
public class GetMembersQueryHandlerTests
{
    [Test, RecursiveMoqAutoData]
    public async Task Handle_MembersNotFound_ReturnsEmptyList(
        Member member,
        List<int> regionIds,
        string keyword,
        string? userType,
        CancellationToken cancellationToken
     )
    {

        var membersReadRepositoryMock = new Mock<IMembersReadRepository>();
        membersReadRepositoryMock.Setup(c => c.GetMembers(It.IsAny<GetMembersOptions>(), cancellationToken))
        .ReturnsAsync(() => new List<MembersSummary>());

        var sut = new GetMembersQueryHandler(membersReadRepositoryMock.Object);

        var query = new GetMembersQuery
        {
            RequestedByMemberId = member.Id,
            Keyword = keyword,
            UserType = userType,
            IsRegionalChair = null,
            RegionIds = regionIds,
            Page = 1,
            PageSize = 5
        };

        var result = await sut.Handle(query, cancellationToken);

        result.Result.Members.Count.Should().Be(0);
        result.Result.TotalCount.Should().Be(0);
    }

    [Test, RecursiveMoqAutoData]
    public async Task Handle_MembersFound_ReturnsMembers(
        Member member,
        CancellationToken cancellationToken
    )
    {
        var membersReadRepositoryMock = new Mock<IMembersReadRepository>();
        var regionIds = new List<int> { 1 };
        var keyword = "test";
        var userType = "employer";
        var membersSummary = new MembersSummary();
        var isRegionalChair = false;
        membersReadRepositoryMock.Setup(c => c.GetMembers(It.IsAny<GetMembersOptions>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<MembersSummary> { membersSummary });

        var sut = new GetMembersQueryHandler(membersReadRepositoryMock.Object);
        var query = new GetMembersQuery
        {
            RequestedByMemberId = member.Id,
            Keyword = keyword,
            UserType = userType,
            IsRegionalChair = isRegionalChair,
            RegionIds = regionIds,
            Page = 1,
            PageSize = 5
        };

        var result = await sut.Handle(query, cancellationToken);

        using (new AssertionScope())
        {
            result.Result.Members.Count.Should().Be(1);
            result.Result.TotalCount.Should().Be(0);
            var members = result.Result.Members;
            members.First().Should().BeEquivalentTo(membersSummary, options => options.Excluding(c => c.MemberId).Excluding(c => c.RegionId).Excluding(c => c.TotalCount));
        }
    }

    [TestCase(null, null, 0)]
    [TestCase("", null, 0)]
    [TestCase("event", "event", 1)]
    [TestCase("west event", "west event", 2)]
    [TestCase("north-west event", "north west event", 3)]
    [TestCase("1 event", "1 event", 2)]
    [TestCase("'--;<>/**/_1 event 2 3", "1 event 2 3", 4)]
    public async Task Handle_Keyword_CheckUsedKeywordExpected(string? keyword, string? expectedKeywordUsed, int keywordCount)
    {
        var membersReadRepositoryMock = new Mock<IMembersReadRepository>();
        var cancellationToken = new CancellationToken();
        var membersSummary = new MembersSummary();
        var memberId = Guid.NewGuid();
        var regionIds = new List<int>();
        var userType = "employer";
        var isRegionalChair = false;

        membersReadRepositoryMock.Setup(c => c.GetMembers(It.IsAny<GetMembersOptions>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<MembersSummary> { membersSummary });

        var sut = new GetMembersQueryHandler(membersReadRepositoryMock.Object);
        var query = new GetMembersQuery
        {
            RequestedByMemberId = memberId,
            Keyword = keyword,
            UserType = userType,
            IsRegionalChair = isRegionalChair,
            RegionIds = regionIds,
            Page = 1,
            PageSize = 5
        };

        await sut.Handle(query, cancellationToken);
        membersReadRepositoryMock.Verify(x => x.GetMembers(
            It.Is<GetMembersOptions>(c => c.Keyword == expectedKeywordUsed && c.KeywordCount == keywordCount), cancellationToken), Times.Once);
    }

    [TestCase(null)]
    [TestCase(true)]
    [TestCase(false)]
    public async Task Handle_IsRegionalChair_CheckIsRegionalChairExpected(bool? isRegionalChair)
    {
        var membersReadRepositoryMock = new Mock<IMembersReadRepository>();
        var cancellationToken = new CancellationToken();
        var membersSummary = new MembersSummary();
        var memberId = Guid.NewGuid();
        var regionIds = new List<int>();
        var userType = "employer";
        var keyword = "test";

        membersReadRepositoryMock.Setup(c => c.GetMembers(It.IsAny<GetMembersOptions>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<MembersSummary> { membersSummary });

        var sut = new GetMembersQueryHandler(membersReadRepositoryMock.Object);
        var query = new GetMembersQuery
        {
            RequestedByMemberId = memberId,
            Keyword = keyword,
            UserType = userType,
            IsRegionalChair = isRegionalChair,
            RegionIds = regionIds,
            Page = 1,
            PageSize = 5
        };

        await sut.Handle(query, cancellationToken);
        membersReadRepositoryMock.Verify(x => x.GetMembers(
            It.Is<GetMembersOptions>(c => c.IsRegionalChair == isRegionalChair), cancellationToken), Times.Once);
    }

    [TestCase(null)]
    [TestCase("employer")]
    public async Task Handle_UserType_CheckUserTypeExpected(string? userType)
    {
        var membersReadRepositoryMock = new Mock<IMembersReadRepository>();
        var cancellationToken = new CancellationToken();
        var membersSummary = new MembersSummary();
        var memberId = Guid.NewGuid();
        var regionIds = new List<int>();
        var keyword = "test";
        var isRegionalChair = false;

        membersReadRepositoryMock.Setup(c => c.GetMembers(It.IsAny<GetMembersOptions>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<MembersSummary> { membersSummary });

        var sut = new GetMembersQueryHandler(membersReadRepositoryMock.Object);
        var query = new GetMembersQuery
        {
            RequestedByMemberId = memberId,
            Keyword = keyword,
            UserType = userType,
            IsRegionalChair = isRegionalChair,
            RegionIds = regionIds,
            Page = 1,
            PageSize = 5
        };

        await sut.Handle(query, cancellationToken);
        membersReadRepositoryMock.Verify(x => x.GetMembers(
            It.Is<GetMembersOptions>(c => c.UserType == userType), cancellationToken), Times.Once);
    }
}
