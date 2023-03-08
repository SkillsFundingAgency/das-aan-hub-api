using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Employers.Queries;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.UnitTests.Employers.Queries
{
    [TestFixture]
    public class GetEmployerMemberQueryHandlerTests
    {
        [Test]
        public async Task Handle_GetEmployerMember()
        {
            var name = "name";
            var email = "email@email.com";
            var organisation = "w3c";
            var status = "live";

            var userRef = new Guid();
            Guid memberId = new();

            var employersReadRepositoryMock = new Mock<IEmployersReadRepository>();
            var employer = new Employer
            {
                Name = name,
                Email = email,
                Organisation = organisation,
                Member = new Member() { Status = status }
            };

            employersReadRepositoryMock.Setup(a => a.GetEmployerByUserRef(userRef)).ReturnsAsync(employer);
            var sut = new GetEmployerMemberQueryHandler(employersReadRepositoryMock.Object);

            var result = await sut.Handle(new GetEmployerMemberQuery(userRef), new CancellationToken());

            Assert.AreEqual(memberId, result!.Result.MemberId);
            Assert.AreEqual(name, result!.Result.Name);
            Assert.AreEqual(email, result!.Result.Email);
            Assert.AreEqual(organisation, result!.Result.Organisation);
            Assert.AreEqual(status, result!.Result.Status);
        }

        [Test]
        public async Task Handle_NoDataFound()
        {
            var userRef = Guid.NewGuid();

            var employersReadRepositoryMock = new Mock<IEmployersReadRepository>();
            employersReadRepositoryMock.Setup(a => a.GetEmployerByUserRef(userRef)).ReturnsAsync((Employer?)null);
            var sut = new GetEmployerMemberQueryHandler(employersReadRepositoryMock.Object);

            var result = await sut.Handle(new GetEmployerMemberQuery(userRef), new CancellationToken());

            Assert.IsNull(result.Result);
        }
    }
}