using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Partners.Commands.CreatePartnerMember;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.Testing.AutoFixture;
using static SFA.DAS.AANHub.Domain.Common.Constants;

namespace SFA.DAS.AANHub.Application.UnitTests.Partners.Commands.CreatePartnerMember;

public class CreatePartnerMemberCommandTests
{
    [Test]
    [MoqAutoData]
    public void Operator_ConvertsToMember(CreatePartnerMemberCommand sut)
    {
        Member member = sut;

        member.Partner.Should().NotBeNull();
        member.Id.Should().Be(sut.Id);
        member.UserType.Should().Be(MembershipUserType.Partner);
        member.Status.Should().Be(MembershipStatus.Live);
        member.Email.Should().Be(sut.Email);
        member.FirstName.Should().Be(sut.FirstName);
        member.LastName.Should().Be(sut.LastName);
        member.JoinedDate.Should().Be(sut.JoinedDate);
        member.OrganisationName.Should().Be(sut.OrganisationName);
        member.Partner!.MemberId.Should().Be(sut.Id);
        member.Partner!.UserName.Should().Be(sut.UserName);
    }
}
