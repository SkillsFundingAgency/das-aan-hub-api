using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;
using SFA.DAS.AANHub.Api.Models;
using SFA.DAS.AANHub.Application.Members.Queries.GetMembers;

namespace SFA.DAS.AANHub.Api.UnitTests.Models;

public class GetMembersModelTests
{
    [Test, AutoData]
    public void Operator_ConvertsToGetMembersQuery(GetMembersModel sut)
    {
        //Action
        GetMembersQuery actual = sut;

        //Assert
        using (new AssertionScope())
        {
            actual.Keyword.Should().Be(sut.Keyword);
            actual.RegionIds.Should().BeEquivalentTo(sut.RegionId);
            actual.UserTypes.Should().BeEquivalentTo(sut.UserType);
            actual.IsNew.Should().Be(sut.IsNew);
            actual.Page.Should().Be(sut.Page);
            actual.PageSize.Should().Be(sut.PageSize);
        }
    }
}
