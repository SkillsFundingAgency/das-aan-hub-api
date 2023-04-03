using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.StagedApprentices.Queries;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.UnitTests.StagedApprentices.Queries
{
    [TestFixture]
    public class GetStagedApprenticeQueryHandlerTests
    {
        [Test]
        public async Task Handle_GetStagedApprenticeMember()
        {
            var lastname = "Smith";
            var dateofbirth = Convert.ToDateTime("01/01/2000");
            var email = "email@email.com";

            var stagedApprenticesReadRepositoryMock = new Mock<IStagedApprenticesReadRepository>();
            var stagedApprentice = new StagedApprentice
            {
                Uln = 1000,
                ApprenticeshipId = 1001,
                EmployerName = "Test Employer",
                StartDate = Convert.ToDateTime("01/01/2023"),
                EndDate = Convert.ToDateTime("01/01/2025"),
                TrainingProviderId = 1002,
                TrainingProviderName = "Test TrainingProviderName",
                TrainingCode = "TestTrainingCode",
                StandardUId = "TestStandardUId"
            };

            stagedApprenticesReadRepositoryMock.Setup(a => a.GetStagedApprentice(lastname, dateofbirth, email)).ReturnsAsync(stagedApprentice);
            var sut = new GetStagedApprenticeMemberQueryHandler(stagedApprenticesReadRepositoryMock.Object);

            var result = await sut.Handle(new GetStagedApprenticeQuery(lastname, dateofbirth, email), new CancellationToken());

            Assert.AreEqual(stagedApprentice.Uln, result.Result.Uln);
            Assert.AreEqual(stagedApprentice.ApprenticeshipId, result.Result.ApprenticeshipId);
            Assert.AreEqual(stagedApprentice.EmployerName, result.Result.EmployerName);
            Assert.AreEqual(stagedApprentice.StartDate, result.Result.StartDate);
            Assert.AreEqual(stagedApprentice.EndDate, result.Result.EndDate);
            Assert.AreEqual(stagedApprentice.TrainingProviderId, result.Result.TrainingProviderId);
            Assert.AreEqual(stagedApprentice.TrainingProviderName, result.Result.TrainingProviderName);
            Assert.AreEqual(stagedApprentice.TrainingCode, result.Result.TrainingCode);
            Assert.AreEqual(stagedApprentice.StandardUId, result.Result.StandardUId);
        }
    }
}