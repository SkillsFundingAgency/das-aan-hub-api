using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Apprentices.Commands.CreateApprenticeMember;
using SFA.DAS.AANHub.Domain.Common;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.Testing.AutoFixture;
using static SFA.DAS.AANHub.Domain.Common.Constants;

namespace SFA.DAS.AANHub.Application.UnitTests.Apprentices.Commands;

public class CreateApprenticeMemberCommandTests
{
    [Test]
    [MoqAutoData]
    public void Operator_ConvertsToMember(CreateApprenticeMemberCommand sut)
    {
        Member member = sut;

        member.Apprentice.Should().NotBeNull();
        member.Id.Should().Be(sut.MemberId);
        member.UserType.Should().Be(UserType.Apprentice);
        member.Status.Should().Be(MembershipStatus.Live);
        member.Email.Should().Be(sut.Email);
        member.FirstName.Should().Be(sut.FirstName);
        member.LastName.Should().Be(sut.LastName);
        member.JoinedDate.Should().Be(sut.JoinedDate);
        member.OrganisationName.Should().Be(sut.OrganisationName);
        member.IsRegionalChair.Should().BeFalse();
        member.Apprentice!.MemberId.Should().Be(sut.MemberId);
        member.Apprentice!.ApprenticeId.Should().Be(sut.ApprenticeId);
        member.MemberProfiles.Should().NotBeEmpty().And.HaveCount(sut.ProfileValues.Count);
        member.MemberProfiles.All(p => p.MemberId == sut.MemberId).Should().BeTrue();
        member.MemberProfiles.Select(p => p.ProfileId).Should().BeSubsetOf(sut.ProfileValues.Select(v => v.Id));
    }
}