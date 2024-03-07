using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.RegionsCount;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.UnitTests.RegionsCount;

[TestFixture]
public class RegionsCountHandlerTests
{
    private Mock<IRegionsReadRepository> _repository = null!;
    private RegionsCountHandler _handler = null!;

    [SetUp]
    public void Before_each_test()
    {
        _repository = new Mock<IRegionsReadRepository>();
        _handler = new RegionsCountHandler(_repository.Object,
            Mock.Of<ILogger<RegionsCountHandler>>());
    }

    [TestCase(0)]
    [TestCase(1)]
    [TestCase(10000)]
    public async Task RegionsCount_ReturnsExpectedCount(int regionsCount)
    {
        // Arrange
        _repository.Setup(x => x.GetRegionsCount(CancellationToken.None)).ReturnsAsync(regionsCount);

        // Act
        var result = await _handler.Handle(new RegionsCountRequest(), new CancellationToken());

        // Assert
        Assert.AreEqual(regionsCount, result);
        _repository.Verify(x => x.GetRegionsCount(CancellationToken.None), Times.Once);
    }
}