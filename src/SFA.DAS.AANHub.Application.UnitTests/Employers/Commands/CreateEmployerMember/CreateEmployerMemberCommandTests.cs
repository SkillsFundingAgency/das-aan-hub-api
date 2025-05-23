﻿using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Application.Employers.Commands.CreateEmployerMember;
using SFA.DAS.AANHub.Domain.Common;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.Testing.AutoFixture;
using static SFA.DAS.AANHub.Domain.Common.Constants;

namespace SFA.DAS.AANHub.Application.UnitTests.Employers.Commands.CreateEmployerMember;
public class CreateEmployerMemberCommandTests
{
    [Test, MoqAutoData]
    public void Operator_ConvertsToMember(CreateEmployerMemberCommand sut)
    {
        Member member = sut;

        member.Employer.Should().NotBeNull();
        member.Id.Should().Be(sut.MemberId);
        member.UserType.Should().Be(UserType.Employer);
        member.Status.Should().Be(MembershipStatus.Live);
        member.Email.Should().Be(sut.Email);
        member.FirstName.Should().Be(sut.FirstName);
        member.LastName.Should().Be(sut.LastName);
        member.JoinedDate.Should().Be(sut.JoinedDate);
        member.OrganisationName.Should().Be(sut.OrganisationName);
        member.IsRegionalChair.Should().BeFalse();
        member.Employer!.MemberId.Should().Be(sut.MemberId);
        member.Employer!.AccountId.Should().Be(sut.AccountId);
        member.Employer!.UserRef.Should().Be(sut.UserRef);
    }

    [Test, MoqAutoData]
    public void Operator_Converts_All_EventType_To_Individual_Items(CreateEmployerMemberCommand sut)
    {
        sut.MemberNotificationEventFormatValues.Clear();
        sut.MemberNotificationEventFormatValues.Add(new MemberNotificationEventFormatValues
        {
            EventFormat = "All",
            Ordering = 0,
            ReceiveNotifications = true
        });

        Member member = sut;

        member.MemberNotificationEventFormats.Count.Should().Be(3);

        member.MemberNotificationEventFormats.Should()
            .Contain(format => format.EventFormat == "InPerson" && format.ReceiveNotifications);

        member.MemberNotificationEventFormats.Should()
            .Contain(format => format.EventFormat == "Online" && format.ReceiveNotifications);

        member.MemberNotificationEventFormats.Should()
            .Contain(format => format.EventFormat == "Hybrid" && format.ReceiveNotifications);

        member.MemberNotificationEventFormats.Should()
            .NotContain(format => format.EventFormat == "All");
    }

    [Test, MoqAutoData]
    public void ProfileConverter_ConvertsToMember(
        ProfileValue sutProfileValue,
        Guid sutMemberId)
    {
        MemberProfile expected = new MemberProfile() { MemberId = sutMemberId, ProfileId = sutProfileValue.Id, ProfileValue = sutProfileValue.Value };

        var result = CreateEmployerMemberCommand.ProfileConverter(sutProfileValue, sutMemberId);

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void MemberNotificationLocationsConverter_ConvertsCorrectly()
    {
        var memberId = Guid.NewGuid();
        var source = new MemberNotificationLocationValues
        {
            Name = "Test Location",
            Radius = 10,
            Latitude = 51.5074,
            Longitude = -0.1278
        };

        var expected = new MemberNotificationLocation
        {
            MemberId = memberId,
            Name = source.Name,
            Radius = source.Radius,
            Latitude = source.Latitude,
            Longitude = source.Longitude
        };

        var result = CreateEmployerMemberCommand.MemberNotificationLocationsConverter(source, memberId);

        result.Should().BeEquivalentTo(expected);
    }
}
