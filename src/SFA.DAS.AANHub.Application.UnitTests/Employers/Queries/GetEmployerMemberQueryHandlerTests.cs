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
            long userId = 0;
            Guid memberid = new();

            var employersReadRepositoryMock = new Mock<IEmployersReadRepository>();
            var employer = new Employer();
            employersReadRepositoryMock.Setup(a => a.GetEmployerByUserId(userId)).ReturnsAsync(employer);
            var sut = new GetEmployerMemberQueryHandler(employersReadRepositoryMock.Object);

            var result = await sut.Handle(new GetEmployerMemberQuery(userId), new CancellationToken());

            Assert.AreEqual(memberid, result!.Result.MemberId);
        }
    }
}
