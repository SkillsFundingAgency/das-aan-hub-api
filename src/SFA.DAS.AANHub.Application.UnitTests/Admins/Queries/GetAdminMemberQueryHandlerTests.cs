using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Admins.Queries;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.UnitTests.Admins.Queries
{
    [TestFixture]
    public class GetAdminMemberQueryHandlerTests
    {
        [Test]
        public async Task Handle_GetAdmin()
        {
            var adminUserName = "UserName";

            var adminReadRepositoryMock = new Mock<IAdminsReadRepository>();
            var admin = new Admin
            {
                Name = "ThisIsAName",
                MemberId = Guid.NewGuid(),
                Email = "email@email.com",
                Member = new Member() { Status = "live" }
            };

            adminReadRepositoryMock.Setup(a => a.GetAdminByUserName(adminUserName)).ReturnsAsync(admin);
            var sut = new GetAdminMemberQueryHandler(adminReadRepositoryMock.Object);

            var result = await sut.Handle(new GetAdminMemberQuery(adminUserName), new CancellationToken());

            Assert.AreEqual(admin.MemberId, result.Result.MemberId);
            Assert.AreEqual(admin.Name, result.Result.Name);
            Assert.AreEqual(admin.Email, result.Result.Email);
            Assert.AreEqual(admin.Member.Status, result.Result.Status);

        }
    }
}