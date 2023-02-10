using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Common.Validators.RequestedByMemberId;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using static SFA.DAS.AANHub.Domain.Common.Constants;

namespace SFA.DAS.AANHub.Application.UnitTests.Common.Validators.RequestedByMemberId
{
    [TestFixture]
    public class RequestedByMemberIdValidatorTests
    {
        [TestCase("00000000-0000-0000-0000-000000000000", false)]
        [TestCase("1e6a97dd-8162-41d2-ba78-7d893111efe1", false)]
        [TestCase("B46C2A2A-E35C-4788-B4B7-1F7E84081846", true)]
        public async Task Validate_RequestedByMemberId_Exists(string requestedByMemberId, bool isValid)
        {
            var guid = Guid.Parse(requestedByMemberId);
            var command = new RequestedByMemberIdCommandTest
            {
                RequestedByMemberId = guid
            };

            var membersReadRepositoryMock = new Mock<IMembersReadRepository>();
            var member = isValid
                ? new Member
                {
                    Status = MembershipStatus.Live
                }
                : null;

            membersReadRepositoryMock.Setup(m => m.GetMember(guid)).ReturnsAsync(member);
            var sut = new RequestedByMemberIdValidator(membersReadRepositoryMock.Object);

            var result = await sut.TestValidateAsync(command);

            if (isValid)
                result.ShouldNotHaveValidationErrorFor(c => c.RequestedByMemberId);
            else
                result.ShouldHaveValidationErrorFor(c => c.RequestedByMemberId);
        }

        private class RequestedByMemberIdCommandTest : IRequestedByMemberId
        {
            public Guid RequestedByMemberId { get; set; }
        }
    }
}