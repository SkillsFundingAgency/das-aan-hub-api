using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Employers.Queries;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Application.UnitTests.Employers.Queries;

public class GetEmployerMemberQueryHandlerTests
{
    [Test]
    [RecursiveMoqAutoData]
    public async Task Handle_EmployerFound_ReturnsMember(
        [Frozen] Mock<IEmployersReadRepository> employersReadRepositoryMock,
        GetEmployerMemberQueryHandler sut,
        Employer employer)
    {
        employersReadRepositoryMock.Setup(a => a.GetEmployerByUserRef(employer.UserRef)).ReturnsAsync(employer);

        var result = await sut.Handle(new GetEmployerMemberQuery(employer.UserRef), new CancellationToken());

        result.Result.Should().BeEquivalentTo(employer.Member, c => c.ExcludingMissingMembers());
    }

    [Test]
    [RecursiveMoqAutoData]
    public async Task Handle_EmployerNotFound_ReturnsNull(
        [Frozen] Mock<IEmployersReadRepository> employersReadRepositoryMock,
        GetEmployerMemberQueryHandler sut,
        Guid userRef)
    {
        employersReadRepositoryMock.Setup(a => a.GetEmployerByUserRef(userRef)).ReturnsAsync(() => null);

        var result = await sut.Handle(new GetEmployerMemberQuery(userRef), new CancellationToken());

        result.Result.Should().BeNull();
    }
}