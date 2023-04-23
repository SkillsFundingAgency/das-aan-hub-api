using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Admins.Queries;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Application.UnitTests.Admins.Queries;

[TestFixture]
public class GetAdminMemberQueryHandlerTests
{
    [Test]
    [RecursiveMoqAutoData]
    public async Task Handle_AdminFound_ReturnsMemder(
        [Frozen] Mock<IAdminsReadRepository> adminReadRepositoryMock,
        GetAdminMemberQueryHandler sut,
        GetAdminMemberQuery query,
        Admin admin)
    {
        adminReadRepositoryMock.Setup(a => a.GetAdminByUserName(query.UserName)).ReturnsAsync(admin);

        var result = await sut.Handle(new GetAdminMemberQuery(query.UserName), new CancellationToken());

        result.Result.Should().BeEquivalentTo(admin.Member, c => c.ExcludingMissingMembers());
    }

    [Test]
    [RecursiveMoqAutoData]
    public async Task Handle_ApprenticeNotFound_ReturnsNull(
        [Frozen] Mock<IAdminsReadRepository> adminReadRepositoryMock,
        GetAdminMemberQueryHandler sut,
        GetAdminMemberQuery query)
    {
        adminReadRepositoryMock.Setup(a => a.GetAdminByUserName(query.UserName)).ReturnsAsync(() => null);

        var result = await sut.Handle(new GetAdminMemberQuery(query.UserName), new CancellationToken());

        result.Result.Should().BeNull();
    }
}
