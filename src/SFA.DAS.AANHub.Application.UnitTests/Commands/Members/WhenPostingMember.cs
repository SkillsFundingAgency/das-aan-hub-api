using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Commands.CreateMember;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Enums;
using SFA.DAS.AANHub.Domain.Interfaces;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.UnitTests.Commands.Members
{
    public class WhenPostingMember
    {
        private readonly CreateMemberCommandHandler _handler;
        private readonly Mock<IMembersWriteRepository> _membersWriteRepository;
        private readonly Mock<IApprenticesWriteRepository> _apprenticesWriteRepository;
        private readonly Mock<IAdminsWriteRepository> _adminsWriteRepository;
        private readonly Mock<IPartnersWriteRepository> _partnersWriteRepository;
        private readonly Mock<IAuditWriteRepository> _auditWriteRepository;
        private readonly Mock<IAanDataContext> _aanDataContext;

        public WhenPostingMember()
        {

            _membersWriteRepository = new Mock<IMembersWriteRepository>();
            _adminsWriteRepository = new Mock<IAdminsWriteRepository>();
            _apprenticesWriteRepository = new Mock<IApprenticesWriteRepository>();
            _partnersWriteRepository = new Mock<IPartnersWriteRepository>();
            _auditWriteRepository = new Mock<IAuditWriteRepository>();
            _aanDataContext = new Mock<IAanDataContext>();

            _handler = new CreateMemberCommandHandler(_membersWriteRepository.Object, _apprenticesWriteRepository.Object, _partnersWriteRepository.Object, _adminsWriteRepository.Object, _auditWriteRepository.Object, _aanDataContext.Object);
        }

        [Test, AutoMoqData]
        public async Task And_HandleSuccessfulApprenticeMember_Then_ReturnResponse(CreateMemberCommand command)
        {
            command.UserType = MembershipUserType.Apprentice;
            var result = await ExecuteTest(command);
            result.Should().BeOfType<CreateMemberResponse>();
            result.Member.Should().NotBeNull();
            result?.Member?.UserType.Should().Be(MembershipUserType.Apprentice);
        }

        [Test, AutoMoqData]
        public async Task And_HandleSuccessfulEmployerMember_Then_ReturnResponse(CreateMemberCommand command)
        {
            command.UserType = MembershipUserType.Employer;
            var result = await ExecuteTest(command);
            result.Should().BeOfType<CreateMemberResponse>();
            result.Member.Should().NotBeNull();
            result?.Member?.UserType.Should().Be(MembershipUserType.Employer);
        }

        [Test, AutoMoqData]
        public async Task And_HandleSuccessfulAdminMember_Then_ReturnResponse(CreateMemberCommand command)
        {
            command.UserType = MembershipUserType.Admin;
            var result = await ExecuteTest(command);
            result.Should().BeOfType<CreateMemberResponse>();
            result.Member.Should().NotBeNull();
            result?.Member?.UserType.Should().Be(MembershipUserType.Admin);
        }
        [Test, AutoMoqData]
        public async Task And_HandleSuccessfulPartnerMember_Then_ReturnResponse(CreateMemberCommand command)
        {
            command.UserType = MembershipUserType.Partner;
            var result = await ExecuteTest(command);
            result.Should().BeOfType<CreateMemberResponse>();
            result.Member.Should().NotBeNull();
            result?.Member?.UserType.Should().Be(MembershipUserType.Partner);
        }

        private async Task<CreateMemberResponse> ExecuteTest(
            CreateMemberCommand command
        )
        {
            _membersWriteRepository.Setup(m => m.Create(It.IsAny<Member>()));
            _apprenticesWriteRepository.Setup(m => m.Create(It.IsAny<Apprentice>()));
            _adminsWriteRepository.Setup(m => m.Create(It.IsAny<Admin>()));
            _partnersWriteRepository.Setup(m => m.Create(It.IsAny<Partner>()));
            _auditWriteRepository.Setup(m => m.Create(It.IsAny<Audit>()));

            return await _handler.Handle(command, CancellationToken.None);
        }
    }
}
