using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.AANHub.Api.Models;
using SFA.DAS.AANHub.Application.Partners;
using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Application.UnitTests.Partners.Commands
{
    public class CreatePartnerMemberCommandTests
    {
        [Test]
        [AutoMoqData]
        public void CreatePartnerCommand_WithExpectedFields(
        )
        {
            const string email = "email@email.com";
            const string info = "ThisIsInformation";
            var date = DateTime.Now;
            const string name = "ThisIsAName";
            const string userName = "ThisIsAUserName";
            const string organisation = "ThisIsAnOrganisation";
            var regions = new List<int>(new[]
            {
                1, 2
            });

            var model = new CreatePartnerModel
            {
                Email = email,
                Information = info,
                Joined = date,
                Name = name,
                Regions = regions,
                UserName = userName,
                Organisation = organisation
            };

            var command = (CreatePartnerMemberCommand)model;
            var member = (Member)command;

            member.UserType.Should().Be(Domain.Common.Constants.MembershipUserType.Partner);
            member.Joined.Should().Be(date);
            member.Information.Should().Be(info);
            member.ReviewStatus.Should().Be(Domain.Common.Constants.MembershipReviewStatus.New);
            member.Status.Should().Be(Domain.Common.Constants.MembershipStatus.Live);
            member?.Partner?.MemberId.Should().Be(member.Id);
            member?.Partner?.Email.Should().Be(email);
            member?.Partner?.UserName.Should().Be(userName);
            member?.Partner?.Organisation.Should().Be(organisation);
        }
    }
}