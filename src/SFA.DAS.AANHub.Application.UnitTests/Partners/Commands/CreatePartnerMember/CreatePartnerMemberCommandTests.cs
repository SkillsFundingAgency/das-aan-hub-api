using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.AANHub.Api.Models;
using SFA.DAS.AANHub.Application.Partners.Commands.CreatePartnerMember;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Application.UnitTests.Partners.Commands.CreatePartnerMember;

public class CreatePartnerMemberCommandTests
{
    [Test]
    [MoqAutoData]
    public void CreatePartnerCommand_WithExpectedFields(
    )
    {
        const string email = "email@email.com";
        const string info = "ThisIsInformation";
        var date = DateTime.Now;
        const string name = "ThisIsAName";
        const string userName = "ThisIsAUserName";
        const string organisation = "ThisIsAnOrganisation";
        var regionId = 1;

        var model = new CreatePartnerModel
        {
            Email = email,
            Information = info,
            Joined = date,
            Name = name,
            RegionId = regionId,
            UserName = userName,
            Organisation = organisation
        };

        var command = (CreatePartnerMemberCommand)model;
        var member = (Member)command;

        member.UserType.Should().Be(Domain.Common.Constants.MembershipUserType.Partner);
        member.Joined.Should().Be(date);
        member.Information.Should().Be(info);
        member.Status.Should().Be(Domain.Common.Constants.MembershipStatus.Live);
        member?.Partner?.MemberId.Should().Be(member.Id);
        member?.Partner?.Email.Should().Be(email);
        member?.Partner?.Name.Should().Be(name);
        member?.Partner?.UserName.Should().Be(userName);
        member?.Partner?.Organisation.Should().Be(organisation);
    }
}