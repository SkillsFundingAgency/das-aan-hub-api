using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.AANHub.Api.Models;
using SFA.DAS.AANHub.Application.Partners;
using SFA.DAS.AANHub.Application.UnitTests;

namespace SFA.DAS.AANHub.Api.UnitTests.Models
{
    public class CreatePartnerModelTests
    {
        [Test]
        [AutoMoqData]
        public void CreatePartnerModel_WithExpectedFields(
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

            command.UserName.Should().Be(userName);
            command.Email.Should().Be(email);
            command.Organisation.Should().Be(organisation);
            command.Name.Should().Be(name);
            command.Information.Should().Be(info);
            command.Joined.Should().Be(date);
            command.Regions.Should().Equal(regions);
        }
    }
}