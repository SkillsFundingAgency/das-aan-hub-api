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
            long apprenticeId = 0;
            Guid memberid = new();

            var apprenticesReadRepositoryMock = new Mock<IApprenticesReadRepository>();
            var apprentice = new Apprentice();
            apprenticesReadRepositoryMock.Setup(a => a.GetApprentice(apprenticeId)).ReturnsAsync(apprentice);
            var sut = new GetApprenticeMemberQueryHandler(apprenticesReadRepositoryMock.Object);

            var result = await sut.Handle(new GetApprenticeMemberQuery(apprenticeId), new CancellationToken());

            Assert.AreEqual(memberid, result!.MemberId);
        }
    }
}
