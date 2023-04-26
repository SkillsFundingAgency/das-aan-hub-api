using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Partners.Queries;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Application.UnitTests.Partners.Queries;

public class GetPartnerMemberQueryHandlerTests
{
    [Test]
    [RecursiveMoqAutoData]
    public async Task Handle_PartnerFound_ReturnsMember(
        [Frozen] Mock<IPartnersReadRepository> partnersReadRepositoryMock,
        GetPartnerMemberQueryHandler sut,
        Partner partner)
    {
        partnersReadRepositoryMock.Setup(a => a.GetPartnerByUserName(partner.UserName)).ReturnsAsync(partner);

        var result = await sut.Handle(new GetPartnerMemberQuery(partner.UserName), new CancellationToken());

        result.Result.Should().BeEquivalentTo(partner.Member, c => c.ExcludingMissingMembers());
    }

    [Test]
    [RecursiveMoqAutoData]
    public async Task Handle_PartnerNotFound_ReturnNull(
        [Frozen] Mock<IPartnersReadRepository> partnersReadRepositoryMock,
        GetPartnerMemberQueryHandler sut,
        string userName)
    {
        partnersReadRepositoryMock.Setup(p => p.GetPartnerByUserName(userName)).ReturnsAsync(() => null);

        var result = await sut.Handle(new GetPartnerMemberQuery(userName), new CancellationToken());

        result.Result.Should().BeNull();
    }
}
