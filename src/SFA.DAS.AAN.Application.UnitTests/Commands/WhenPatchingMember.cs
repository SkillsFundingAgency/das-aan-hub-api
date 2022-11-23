using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using SFA.DAS.AAN.Application.Commands.ModifyMember;
using SFA.DAS.AAN.Domain.Entities;
using SFA.DAS.AAN.Domain.Interfaces;

namespace SFA.DAS.AAN.Application.UnitTests.Commands
{
    public class WhenPatchingMember
    {
        private readonly ModifyMemberCommandHandler _handler;
        private readonly Mock<IMembersContext> _context;
        private readonly string _memberId = Guid.NewGuid().ToString();
        public WhenPatchingMember()
        {

            _context = new Mock<IMembersContext>();
            _handler = new ModifyMemberCommandHandler(_context.Object, Mock.Of<ILogger<ModifyMemberCommandHandler>>());
        }

        [Theory, AutoMoqData]
        public async Task And_HandleSuccessful_Then_ReturnResponse(ModifyMemberCommand command, Member member)
        {
            command.UserId = _memberId;

            var result = await ExecuteTest(command, member);
            result.Should().BeOfType<ModifyMemberResponse>();

        }
        [Theory, AutoMoqData]
        public async Task And_InvalidMemberIdAsGuid_Then_ThrowException(ModifyMemberCommand command, Member member)
        {
            await Assert.ThrowsAsync<InvalidOperationException>(() => ExecuteTest(command, member));
        }
        [Theory, AutoMoqData]
        public async Task And_MemberNotFound_Then_ThrowException(ModifyMemberCommand command)
        {
            command.UserId = _memberId;
            _context.Setup(m => m.FindByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult<Member?>(null));
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }

        private async Task<ModifyMemberResponse> ExecuteTest(
            ModifyMemberCommand command,
            Member member
        )
        {
            _context.Setup(m => m.FindByIdAsync(It.IsAny<Guid>())).ReturnsAsync(member);
            _context.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(0);
            return await _handler.Handle(command, CancellationToken.None);
        }



    }


}
