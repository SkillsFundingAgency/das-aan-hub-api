using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.AANHub.Api.Models;
using SFA.DAS.AANHub.Application.Apprentices.Commands;
using SFA.DAS.AANHub.Application.UnitTests;

namespace SFA.DAS.AANHub.Api.UnitTests.Models
{
    [TestFixture]
    public class CreateApprenticeMemberCommandTests
    {
        [Test, AutoMoqData]
        public void Operator_PopulatesModelFromSummaryModel(CreateApprenticeModel createApprenticeModel)
        {
            var model = (CreateApprenticeMemberCommand)createApprenticeModel;

            Assert.That(model, Is.Not.Null);

            model.Should().BeEquivalentTo(createApprenticeModel, c => c
                .Excluding(m => m.Region)
            );

            Assert.AreEqual(createApprenticeModel.ApprenticeId, model.ApprenticeId);
            Assert.AreEqual(createApprenticeModel.Joined, model.Joined);
            Assert.AreEqual(createApprenticeModel.Information, model.Information);
            Assert.AreEqual(createApprenticeModel.Email, model.Email);
            Assert.AreEqual(createApprenticeModel.Name, model.Name);
        }
    }
}
