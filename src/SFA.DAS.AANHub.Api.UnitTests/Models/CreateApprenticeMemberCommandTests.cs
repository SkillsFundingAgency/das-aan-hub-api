using NUnit.Framework;
using SFA.DAS.AANHub.Api.Models;
using SFA.DAS.AANHub.Application.Apprentices.Commands.CreateApprenticeMember;
using SFA.DAS.AANHub.Application.UnitTests;

namespace SFA.DAS.AANHub.Api.UnitTests.Models
{
    [TestFixture]
    public class CreateApprenticeMemberCommandTests
    {
        [Test]
        [AutoMoqData]
        public void Operator_PopulatesModelFromSummaryModel(CreateApprenticeModel createApprenticeModel)
        {
            var model = (CreateApprenticeMemberCommand)createApprenticeModel;

            Assert.That(model, Is.Not.Null);

            Assert.AreEqual(createApprenticeModel.ApprenticeId, model.ApprenticeId);
            Assert.AreEqual(createApprenticeModel.Joined, model.Joined);
            Assert.AreEqual(createApprenticeModel.Information, model.Information);
            Assert.AreEqual(createApprenticeModel.Email, model.Email);
            Assert.AreEqual(createApprenticeModel.Regions, model.Regions);
            Assert.AreEqual(createApprenticeModel.Name, model.Name);
        }
    }
}