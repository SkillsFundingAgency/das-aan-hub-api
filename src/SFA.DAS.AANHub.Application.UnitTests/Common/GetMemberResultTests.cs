using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Application.UnitTests.Common;

public class GetMemberResultTests
{
    [Test, RecursiveMoqAutoData]
    public void Operator_ConvertsFromMember(Member member)
    {
        GetMemberResult result = member!;
        result.MemberId.Should().Be(member.Id);
        result.UserType.Should().Be(member.UserType);
        result.FirstName.Should().Be(member.FirstName);
        result.LastName.Should().Be(member.LastName);
        result.Email.Should().Be(member.Email);
        result.Status.Should().Be(member.Status);
        result.JoinedDate.Should().Be(member.JoinedDate);
        result.EndDate.Should().Be(member.EndDate);
        result.RegionId.Should().Be(member.RegionId);
        result.OrganisationName.Should().Be(member.OrganisationName);
        result.LastUpdatedDate.Should().Be(member.LastUpdatedDate);
        result.IsRegionalChair.Should().Be(member.IsRegionalChair);
        result.FullName.Should().Be(member.FullName);
        result.ApprenticeId.Should().Be(member.Apprentice!.ApprenticeId);
        result.UserRef.Should().Be(member.Employer!.UserRef);
    }

    [Test]
    public void Operator_NullArgument_ReturnsNull()
    {
        Member? member = null;

        GetMemberResult actual = member!;

        actual.Should().BeNull();
    }
}
