using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Apprentices.Queries;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.UnitTests.Apprentices.Queries
{
    [TestFixture]
    public class GetApprenticeMemberQueryHandlerTests
    {
        [Test]
        public async Task Handle_GetApprenticeMember()
        {
            var apprenticeId = Guid.NewGuid();

            var apprenticesReadRepositoryMock = new Mock<IApprenticesReadRepository>();
            var apprentice = new Apprentice
            {
                Name = "ThisIsAName",
                MemberId = Guid.NewGuid(),
                Email = "email@email.com",
                Member = new Member() { Status = "live" }
            };
            apprenticesReadRepositoryMock.Setup(a => a.GetApprentice(apprenticeId)).ReturnsAsync(apprentice);
            var sut = new GetApprenticeMemberQueryHandler(apprenticesReadRepositoryMock.Object);

            var result = await sut.Handle(new GetApprenticeMemberQuery(apprenticeId), new CancellationToken());

            Assert.AreEqual(apprentice.MemberId, result.Result.MemberId);
            Assert.AreEqual(apprentice.Member.FirstName, result.Result.FirstName);
            Assert.AreEqual(apprentice.Member.LastName, result.Result.LastName);
            Assert.AreEqual(apprentice.Member.Email, result.Result.Email);
            Assert.AreEqual(apprentice.Member.Status, result.Result.Status);
        }
    }
}