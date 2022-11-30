using FluentAssertions;
using Moq;
using SFA.DAS.AANHub.Application.Commands.CreateMember;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Enums;
using SFA.DAS.AANHub.Domain.Interfaces;

namespace SFA.DAS.AANHub.Application.UnitTests.Commands.Members
{
    public class WhenPostingMember
    {
        private readonly CreateMemberCommandHandler _handler;
        private readonly Mock<IMembersContext> _memberContext;
        private readonly Mock<IApprenticesContext> _apprenticeContext;
        private readonly Mock<IAdminsContext> _adminContext;
        private readonly Mock<IPartnersContext> _partnerContext;
        private readonly Mock<IEmployersContext> _employerContext;

        public WhenPostingMember()
        {

            _memberContext = new Mock<IMembersContext>();
            _adminContext = new Mock<IAdminsContext>();
            _apprenticeContext = new Mock<IApprenticesContext>();
            _employerContext = new Mock<IEmployersContext>();
            _partnerContext = new Mock<IPartnersContext>();

            _handler = new CreateMemberCommandHandler(_memberContext.Object, _apprenticeContext.Object, _employerContext.Object, _partnerContext.Object, _adminContext.Object);
        }

        [Theory, AutoMoqData]
        public async Task And_HandleSuccessfulApprenticeMember_Then_ReturnResponse(CreateMemberCommand command)
        {
            command.UserType = MembershipUserTypes.Apprentice;
            var result = await ExecuteTest(command);
            result.Should().BeOfType<CreateMemberResponse>();
            result.Member.Should().NotBeNull();
            result?.Member?.UserType.Should().Be("Apprentice");
        }

        [Theory, AutoMoqData]
        public async Task And_HandleSuccessfulEmployerMember_Then_ReturnResponse(CreateMemberCommand command)
        {
            command.UserType = MembershipUserTypes.Employer;
            var result = await ExecuteTest(command);
            result.Should().BeOfType<CreateMemberResponse>();
            result.Member.Should().NotBeNull();
            result?.Member?.UserType.Should().Be("Employer");
        }

        [Theory, AutoMoqData]
        public async Task And_HandleSuccessfulAdminMember_Then_ReturnResponse(CreateMemberCommand command)
        {
            command.UserType = MembershipUserTypes.Admin;
            var result = await ExecuteTest(command);
            result.Should().BeOfType<CreateMemberResponse>();
            result.Member.Should().NotBeNull();
            result?.Member?.UserType.Should().Be("Admin");
        }
        [Theory, AutoMoqData]
        public async Task And_HandleSuccessfulPartnerMember_Then_ReturnResponse(CreateMemberCommand command)
        {
            command.UserType = MembershipUserTypes.Partner;
            var result = await ExecuteTest(command);
            result.Should().BeOfType<CreateMemberResponse>();
            result.Member.Should().NotBeNull();
            result?.Member?.UserType.Should().Be("Partner");
        }

        private async Task<CreateMemberResponse> ExecuteTest(
            CreateMemberCommand command
        )
        {
            _memberContext.Setup(m => m.Entities.AddAsync(It.IsAny<Member>(), It.IsAny<CancellationToken>()));
            _apprenticeContext.Setup(m => m.Entities.AddAsync(It.IsAny<Apprentice>(), It.IsAny<CancellationToken>()));
            _adminContext.Setup(m => m.Entities.AddAsync(It.IsAny<Admin>(), It.IsAny<CancellationToken>()));
            _partnerContext.Setup(m => m.Entities.AddAsync(It.IsAny<Partner>(), It.IsAny<CancellationToken>()));
            _employerContext.Setup(m => m.Entities.AddAsync(It.IsAny<Employer>(), It.IsAny<CancellationToken>()));

            _memberContext.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(0);
            _apprenticeContext.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(0);
            _adminContext.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(0);
            _employerContext.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(0);
            _partnerContext.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(0);
            return await _handler.Handle(command, CancellationToken.None);
        }
    }
}
