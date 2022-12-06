using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Commands.CreateMember;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Enums;
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
        private readonly Mock<IEmployersWriteRepository> _employersWriteRepository;

        public WhenPostingMember()
        {

            _membersWriteRepository = new Mock<IMembersWriteRepository>();
            _adminsWriteRepository = new Mock<IAdminsWriteRepository>();
            _apprenticesWriteRepository = new Mock<IApprenticesWriteRepository>();
            _employersWriteRepository = new Mock<IEmployersWriteRepository>();
            _partnersWriteRepository = new Mock<IPartnersWriteRepository>();

            _handler = new CreateMemberCommandHandler(_membersWriteRepository.Object, _apprenticesWriteRepository.Object, _employersWriteRepository.Object, _partnersWriteRepository.Object, _adminsWriteRepository.Object);
        }

        [Test, AutoMoqData]
        public async Task And_HandleSuccessfulApprenticeMember_Then_ReturnResponse(CreateMemberCommand command)
        {
            command.UserType = MembershipUserTypes.Apprentice;
            var result = await ExecuteTest(command);
            result.Should().BeOfType<CreateMemberResponse>();
            result.Member.Should().NotBeNull();
            result?.Member?.UserType.Should().Be("Apprentice");
        }

        [Test, AutoMoqData]
        public async Task And_HandleSuccessfulEmployerMember_Then_ReturnResponse(CreateMemberCommand command)
        {
            command.UserType = MembershipUserTypes.Employer;
            var result = await ExecuteTest(command);
            result.Should().BeOfType<CreateMemberResponse>();
            result.Member.Should().NotBeNull();
            result?.Member?.UserType.Should().Be("Employer");
        }

        [Test, AutoMoqData]
        public async Task And_HandleSuccessfulAdminMember_Then_ReturnResponse(CreateMemberCommand command)
        {
            command.UserType = MembershipUserTypes.Admin;
            var result = await ExecuteTest(command);
            result.Should().BeOfType<CreateMemberResponse>();
            result.Member.Should().NotBeNull();
            result?.Member?.UserType.Should().Be("Admin");
        }
        [Test, AutoMoqData]
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
            _membersWriteRepository.Setup(m => m.Create(It.IsAny<Member>()));
            _apprenticesWriteRepository.Setup(m => m.Create(It.IsAny<Apprentice>()));
            _adminsWriteRepository.Setup(m => m.Create(It.IsAny<Admin>()));
            _partnersWriteRepository.Setup(m => m.Create(It.IsAny<Partner>()));
            _employersWriteRepository.Setup(m => m.Create(It.IsAny<Employer>()));

            return await _handler.Handle(command, CancellationToken.None);
        }
    }
}
