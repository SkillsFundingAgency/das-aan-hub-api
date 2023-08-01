using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Admins.Commands.CreateAdminMember;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.Testing.AutoFixture;
using static SFA.DAS.AANHub.Domain.Common.Constants;

namespace SFA.DAS.AANHub.Application.UnitTests.Admins.Commands.CreateAdminMember;

public class CreateAdminMemberCommandTests
{
    [Test]
    [MoqAutoData]
    public void Operator_ConvertsToMember(CreateAdminMemberCommand sut)
    {
        Member member = sut;

        member.Id.Should().Be(sut.MemberId);
        member.UserType.Should().Be(MembershipUserType.Admin);
        member.Status.Should().Be(MembershipStatus.Live);
        member.Email.Should().Be(sut.Email);
        member.FirstName.Should().Be(sut.FirstName);
        member.LastName.Should().Be(sut.LastName);
        member.JoinedDate.Date.Should().Be(DateTime.UtcNow.Date);
        member.OrganisationName.Should().Be("DFE");
        member.RegionId.Should().BeNull();
        member.Id.Should().Be(sut.MemberId);
    }
}
