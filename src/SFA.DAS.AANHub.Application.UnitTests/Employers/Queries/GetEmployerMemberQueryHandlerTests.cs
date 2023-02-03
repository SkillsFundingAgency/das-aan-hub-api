using Microsoft.Extensions.Logging;
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
            var accountId = 123;
            var externalUserId = 0;
            Guid memberid = new();

            var employersReadRepositoryMock = new Mock<IEmployersReadRepository>();
            var employer = new Employer();
            employersReadRepositoryMock.Setup(a => a.GetEmployerByAccountIdAndUserId(accountId, externalUserId)).ReturnsAsync(employer);
            var sut = new GetEmployerMemberQueryHandler(employersReadRepositoryMock.Object);

            var result = await sut.Handle(new GetEmployerMemberQuery(accountId, externalUserId), new CancellationToken());

            Assert.AreEqual(memberid, result!.Result.MemberId);
        }

        [Test]
        public async Task Handle_NoDataFound()
        {
            var accountId = 123;
            var externalUserId = 0;

            var employersReadRepositoryMock = new Mock<IEmployersReadRepository>();
            employersReadRepositoryMock.Setup(a => a.GetEmployerByAccountIdAndUserId(accountId, externalUserId)).ReturnsAsync((Employer?)null);
            var sut = new GetEmployerMemberQueryHandler(employersReadRepositoryMock.Object);

            var result = await sut.Handle(new GetEmployerMemberQuery(accountId, externalUserId), new CancellationToken());

            Assert.IsNull(result.Result);
        }
    }
}
