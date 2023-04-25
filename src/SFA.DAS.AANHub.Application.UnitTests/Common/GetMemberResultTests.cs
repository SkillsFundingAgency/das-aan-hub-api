using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Application.UnitTests.Common;

public class GetMemberResultTests
{
    [Test]
    [RecursiveMoqAutoData]
    public void Operator_ConvertsFromMember(Member member)
    {
        GetMemberResult result = member!;
        result.MemberId.Should().Be(member.Id);
        result.Email.Should().Be(member.Email);
        result.FirstName.Should().Be(member.FirstName);
        result.LastName.Should().Be(member.LastName);
        result.Status.Should().Be(member.Status);
        result.OrganisationName.Should().Be(member.OrganisationName);
        result.RegionId.Should().Be(member.RegionId);
    }

    [Test]
    public void Operator_NullArgument_ReturnsNull()
    {
        Member? member = null;

        GetMemberResult actual = member!;

        actual.Should().BeNull();
    }
}
