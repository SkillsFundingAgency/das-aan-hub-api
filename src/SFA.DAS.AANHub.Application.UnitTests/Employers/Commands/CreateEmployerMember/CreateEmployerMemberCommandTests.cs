using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Employers.Commands.CreateEmployerMember;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.Testing.AutoFixture;
using static SFA.DAS.AANHub.Domain.Common.Constants;

namespace SFA.DAS.AANHub.Application.UnitTests.Employers.Commands.CreateEmployerMember;
public class CreateEmployerMemberCommandTests
{
    [Test]
    [MoqAutoData]
    public void Operator_ConvertsToMember(CreateEmployerMemberCommand sut)
    {
        Member member = sut;

        member.Employer.Should().NotBeNull();
        member.Id.Should().Be(sut.Id);
        member.UserType.Should().Be(MembershipUserType.Employer);
        member.Status.Should().Be(MembershipStatus.Live);
        member.Email.Should().Be(sut.Email);
        member.FirstName.Should().Be(sut.FirstName);
        member.LastName.Should().Be(sut.LastName);
        member.JoinedDate.Should().Be(sut.JoinedDate);
        member.OrganisationName.Should().Be(sut.OrganisationName);
        member.Employer!.MemberId.Should().Be(sut.Id);
        member.Employer!.AccountId.Should().Be(sut.AccountId);
        member.Employer!.UserRef.Should().Be(sut.UserRef);
    }
}
