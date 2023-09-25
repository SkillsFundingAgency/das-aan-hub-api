using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Members.Queries.GetMemberByEmail;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Application.UnitTests.Members.Queries.GetMemberByEmail;

public class GetMemberByEmailQueryHandlerTests
{
    [Test, RecursiveMoqAutoData]
    public async Task Handle_ReturnsMemberByEmail_IfMemberIsFound(
        [Frozen] Mock<IMembersReadRepository> membersReadRepositoryMock,
        GetMemberByEmailQueryHandler sut,
        Member member)
    {
        membersReadRepositoryMock.Setup(a => a.GetMemberByEmail(member.Email)).ReturnsAsync(member);
        var result = await sut.Handle(new GetMemberByEmailQuery(member.Email), new CancellationToken());
        result.Result.Should().BeEquivalentTo(member, c => c.ExcludingMissingMembers());
    }

    [Test, RecursiveMoqAutoData]
    public async Task Handle_ReturnsNull_IfMemberIsNotFound(
        [Frozen] Mock<IMembersReadRepository> membersReadRepositoryMock,
        GetMemberByEmailQueryHandler sut,
        string email)
    {
        membersReadRepositoryMock.Setup(a => a.GetMemberByEmail(email)).ReturnsAsync(() => null);
        var result = await sut.Handle(new GetMemberByEmailQuery(email), new CancellationToken());
        result.Result.Should().BeNull();
    }
}
