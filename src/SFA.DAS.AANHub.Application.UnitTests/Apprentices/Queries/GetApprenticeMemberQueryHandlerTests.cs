using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Apprentices.Queries;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Application.UnitTests.Apprentices.Queries;

public class GetApprenticeMemberQueryHandlerTests
{
    [Test]
    [RecursiveMoqAutoData]
    public async Task Handle_ApprenticeFound_ReturnsMember(
        [Frozen] Mock<IApprenticesReadRepository> apprenticesReadRepositoryMock,
        GetApprenticeMemberQueryHandler sut,
        Apprentice apprentice)
    {
        apprenticesReadRepositoryMock.Setup(a => a.GetApprentice(apprentice.ApprenticeId)).ReturnsAsync(apprentice);

        var result = await sut.Handle(new GetApprenticeMemberQuery(apprentice.ApprenticeId), new CancellationToken());

        result.Result.Should().BeEquivalentTo(apprentice.Member, c => c.ExcludingMissingMembers());
    }

    [Test]
    [RecursiveMoqAutoData]
    public async Task Handle_ApprenticeNotFound_ReturnsNull(
        [Frozen] Mock<IApprenticesReadRepository> apprenticesReadRepositoryMock,
        GetApprenticeMemberQueryHandler sut,
        Guid apprenticeId)
    {
        apprenticesReadRepositoryMock.Setup(a => a.GetApprentice(apprenticeId)).ReturnsAsync(() => null);

        var result = await sut.Handle(new GetApprenticeMemberQuery(apprenticeId), new CancellationToken());

        result.Result.Should().BeNull();
    }
}
