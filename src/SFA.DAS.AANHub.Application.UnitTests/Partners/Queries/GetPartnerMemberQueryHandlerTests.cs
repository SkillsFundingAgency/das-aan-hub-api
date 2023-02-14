using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Partners.Queries;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.UnitTests.Partners.Queries
{
    [TestFixture]
    public class GetPartnerMemberQueryHandlerTests
    {

        [Test]
        public async Task Handle_GetPartnerMember()
        {
            string userName = "";
            string email = "email@email.com";
            string name = "lorem epsum";
            string organisation = "w3c";
            string status = "live";
            Guid memberid = new();

            var partnersReadRepositoryMock = new Mock<IPartnersReadRepository>();
            var partner = new Partner()
            {
                MemberId = memberid,
                Organisation = organisation,
                Name = name,
                Email = email,
                Member = new Member() { Status = status }
            };

            partnersReadRepositoryMock.Setup(a => a.GetPartnerByUserName(userName)).ReturnsAsync(partner);
            var sut = new GetPartnerMemberQueryHandler(partnersReadRepositoryMock.Object);

            var result = await sut.Handle(new GetPartnerMemberQuery(userName), new CancellationToken());

            Assert.AreEqual(memberid, result!.Result.MemberId);
            Assert.AreEqual(email, result!.Result.Email);
            Assert.AreEqual(name, result!.Result.Name);
            Assert.AreEqual(organisation, result!.Result.Organisation);
            Assert.AreEqual(status, result!.Result.Status);

        }
    }
}
