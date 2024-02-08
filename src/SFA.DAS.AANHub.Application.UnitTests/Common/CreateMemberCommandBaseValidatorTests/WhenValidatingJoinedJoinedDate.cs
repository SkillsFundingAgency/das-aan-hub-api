using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.UnitTests.Common.CreateMemberCommandBaseValidatorTests;

public class WhenValidatingJoinedJoinedDate
{
    private readonly int _validRegionId = 1;
    private Mock<IRegionsReadRepository> _regionsReadRepositoryMock = null!;
    [SetUp]
    public void Init()
    {
        _regionsReadRepositoryMock = new Mock<IRegionsReadRepository>();
        _regionsReadRepositoryMock.Setup(x => x.GetAllRegions(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Region> { new() { Id = _validRegionId, Area = "test" } });
    }

    [Test]
    public async Task ThenFutureDatesAreInvalid()
    {
        CreateMemberCommandBaseValidator sut = new(Mock.Of<IMembersReadRepository>(), _regionsReadRepositoryMock.Object);
        TestTarget target = new()
        {
            JoinedDate = DateTime.Today.AddDays(1),
            RegionId = _validRegionId
        };

        var result = await sut.TestValidateAsync(target);

        result.ShouldHaveValidationErrorFor(c => c.JoinedDate);
    }

    [Test]
    public async Task ThenTodaysDateIsValid()
    {
        CreateMemberCommandBaseValidator sut = new(Mock.Of<IMembersReadRepository>(), _regionsReadRepositoryMock.Object);
        TestTarget target = new()
        {
            JoinedDate = DateTime.Today,
            RegionId = _validRegionId
        };

        var result = await sut.TestValidateAsync(target);

        result.ShouldNotHaveValidationErrorFor(c => c.JoinedDate);
    }

    [Test]
    public async Task ThenPastTodaysDatesAreValid()
    {
        CreateMemberCommandBaseValidator sut = new(Mock.Of<IMembersReadRepository>(), _regionsReadRepositoryMock.Object);
        TestTarget target = new()
        {
            JoinedDate = DateTime.Today.AddDays(-1),
            RegionId = _validRegionId
        };

        var result = await sut.TestValidateAsync(target);

        result.ShouldNotHaveValidationErrorFor(c => c.JoinedDate);
    }
}
