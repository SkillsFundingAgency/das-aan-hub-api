using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.StagedApprentices.Queries;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.UnitTests.StagedApprentices.Queries;

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

        result.Result.Uln.Should().Be(stagedApprentice.Uln);
        result.Result.ApprenticeshipId.Should().Be(stagedApprentice.ApprenticeshipId);
        result.Result.EmployerName.Should().BeEquivalentTo(stagedApprentice.EmployerName);
        result.Result.StartDate.Should().Be(stagedApprentice.StartDate);
        result.Result.EndDate.Should().Be(stagedApprentice.EndDate);
        result.Result.TrainingProviderId.Should().Be(stagedApprentice.TrainingProviderId);
        result.Result.TrainingProviderName.Should().BeEquivalentTo(stagedApprentice.TrainingProviderName);
        result.Result.TrainingCode.Should().BeEquivalentTo(stagedApprentice.TrainingCode);
        result.Result.StandardUId.Should().BeEquivalentTo(stagedApprentice.StandardUId);
    }

    [Test]
    public async Task Handle_NoDataFound()
    {
        var lastname = "Smith";
        var dateofbirth = Convert.ToDateTime("01/01/2000");
        var email = "email@email.com";

        var stagedApprenticesReadRepositoryMock = new Mock<IStagedApprenticesReadRepository>();
        stagedApprenticesReadRepositoryMock.Setup(a => a.GetStagedApprentice(lastname, dateofbirth, email)).ReturnsAsync((StagedApprentice?)null);
        var sut = new GetStagedApprenticeMemberQueryHandler(stagedApprenticesReadRepositoryMock.Object);

        var result = await sut.Handle(new GetStagedApprenticeQuery(lastname, dateofbirth, email), new CancellationToken());

        Assert.IsNull(result.Result);
    }
}