﻿using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Apprentices.Commands.CreateApprenticeMember;
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
        member.Id.Should().Be(sut.Id);
        member.UserType.Should().Be(MembershipUserType.Apprentice);
        member.Status.Should().Be(MembershipStatus.Live);
        member.Email.Should().Be(sut.Email);
        member.FirstName.Should().Be(sut.FirstName);
        member.LastName.Should().Be(sut.LastName);
        member.Joined.Should().Be(sut.Joined);
        member.OrganisationName.Should().Be(sut.OrganisationName);
        member.Apprentice!.MemberId.Should().Be(sut.Id);
        member.Apprentice!.ApprenticeId.Should().Be(sut.ApprenticeId);
    }
}