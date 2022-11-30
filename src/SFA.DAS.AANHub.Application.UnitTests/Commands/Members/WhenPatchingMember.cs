using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Commands.ModifyMember;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces;

namespace SFA.DAS.AANHub.Application.UnitTests.Commands.Members
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

        [Test, AutoMoqData]
        public async Task And_HandleSuccessful_Then_ReturnResponse(ModifyMemberCommand command, Member member)
        {
            command.UserId = _memberId;

            var result = await ExecuteTest(command, member);
            result.Should().BeOfType<ModifyMemberResponse>();

        }
        [Test, AutoMoqData]
        public void And_InvalidMemberIdAsGuid_Then_ThrowException(ModifyMemberCommand command, Member member)
        {
            Assert.ThrowsAsync<InvalidOperationException>(async () => await ExecuteTest(command, member));
        }
        [Test, AutoMoqData]
        public void And_MemberNotFound_Then_ThrowException(ModifyMemberCommand command)
        {
            command.UserId = _memberId;
            _context.Setup(m => m.FindByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult<Member?>(null));
            Assert.ThrowsAsync<KeyNotFoundException>(async () => await _handler.Handle(command, CancellationToken.None));
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
