﻿using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Members.Queries.GetMembers;
using SFA.DAS.AANHub.Domain.Common;
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
        List<MemberUserType> userType,
        List<MembershipStatusType> status,
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
            Status = status,
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
        var userType = new List<MemberUserType> { MemberUserType.Employer };
        var status = new List<MembershipStatusType> { MembershipStatusType.Live };
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
            Status = status,
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
        var userType = new List<MemberUserType>();
        var status = new List<MembershipStatusType>();
        var isRegionalChair = false;

        membersReadRepositoryMock.Setup(c => c.GetMembers(It.IsAny<GetMembersOptions>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<MembersSummary> { membersSummary });

        var sut = new GetMembersQueryHandler(membersReadRepositoryMock.Object);
        var query = new GetMembersQuery
        {
            RequestedByMemberId = memberId,
            Keyword = keyword,
            UserType = userType,
            Status = status,
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
        var userType = new List<MemberUserType>();
        var status = new List<MembershipStatusType>();
        var keyword = "test";

        membersReadRepositoryMock.Setup(c => c.GetMembers(It.IsAny<GetMembersOptions>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<MembersSummary> { membersSummary });

        var sut = new GetMembersQueryHandler(membersReadRepositoryMock.Object);
        var query = new GetMembersQuery
        {
            RequestedByMemberId = memberId,
            Keyword = keyword,
            UserType = userType,
            Status = status,
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
    public async Task Handle_UserType_CheckUserTypeExpected(List<MemberUserType> userType)
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

    private static readonly object?[] _Data =
    {
      null,
      new object[] {new List<MemberUserType> { MemberUserType.Employer} }
    };

    [Test, TestCaseSource(nameof(_StatusData))]
    public async Task Handle_Status_CheckStatusExpected(List<MembershipStatusType> status)
    {
        var membersReadRepositoryMock = new Mock<IMembersReadRepository>();
        var cancellationToken = new CancellationToken();
        var membersSummary = new MembersSummary();
        var memberId = Guid.NewGuid();
        var regionIds = new List<int>();
        var userType = new List<MemberUserType>();
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
            Status = status,
            IsRegionalChair = isRegionalChair,
            RegionIds = regionIds,
            Page = 1,
            PageSize = 5
        };

        await sut.Handle(query, cancellationToken);
        membersReadRepositoryMock.Verify(x => x.GetMembers(
            It.Is<GetMembersOptions>(c => c.Status == status), cancellationToken), Times.Once);
    }

    private static readonly object?[] _StatusData =
    {
      null,
      new object[] {new List<MembershipStatusType> { MembershipStatusType.Live} }
    };
}