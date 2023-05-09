using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Apprentices.Queries;
using SFA.DAS.AANHub.Application.Employers.Queries;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Application.UnitTests.Members.Queries;
public class GetMemberQueryHandlerTests
{
    [Test]
    [RecursiveMoqAutoData]
    public async Task Handle_ReturnsMember_IfMemberIsFound(
        [Frozen] Mock<IMembersReadRepository> membersReadRepositoryMock,
        GetMemberQueryHandler sut,
        Member member)
    {
        membersReadRepositoryMock.Setup(a => a.GetMember(member.Id)).ReturnsAsync(member);

        var result = await sut.Handle(new GetMemberQuery(member.Id), new CancellationToken());

        result.Result.Should().BeEquivalentTo(member, c => c.ExcludingMissingMembers());
    }

    [Test]
    [RecursiveMoqAutoData]
    public async Task Handle_ReturnsNull_IfMemberIsNotFound(
        [Frozen] Mock<IMembersReadRepository> membersReadRepositoryMock,
        GetMemberQueryHandler sut,
        Guid memberId)
    {
        membersReadRepositoryMock.Setup(a => a.GetMember(memberId)).ReturnsAsync(() => null);

        var result = await sut.Handle(new GetMemberQuery(memberId), new CancellationToken());

        result.Result.Should().BeNull();
    }
}
