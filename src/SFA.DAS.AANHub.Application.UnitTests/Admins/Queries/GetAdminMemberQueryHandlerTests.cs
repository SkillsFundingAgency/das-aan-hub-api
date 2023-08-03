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
        [Frozen] Mock<IMembersReadRepository> membersReadRepositoryMock,
        GetAdminMemberQueryHandler sut,
        GetAdminMemberQuery query,
        Member member)
    {
        membersReadRepositoryMock.Setup(a => a.GetMemberByEmail(query.Email)).ReturnsAsync(member);

        var result = await sut.Handle(new GetAdminMemberQuery(query.Email), new CancellationToken());

        result.Result.Should().BeEquivalentTo(member, c => c.ExcludingMissingMembers());
    }

    [Test]
    [RecursiveMoqAutoData]
    public async Task Handle_ApprenticeNotFound_ReturnsNull(
        [Frozen] Mock<IMembersReadRepository> membersReadRepositoryMock,
        GetAdminMemberQueryHandler sut,
        GetAdminMemberQuery query)
    {
        membersReadRepositoryMock.Setup(a => a.GetMemberByEmail(query.Email)).ReturnsAsync(() => null);

        var result = await sut.Handle(new GetAdminMemberQuery(query.Email), new CancellationToken());

        result.Result.Should().BeNull();
    }
}
