using AutoFixture.NUnit3;
using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Common.Validators.MemberId;
using SFA.DAS.AANHub.Domain.Common;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using SFA.DAS.Testing.AutoFixture;
using static SFA.DAS.AANHub.Domain.Common.Constants;

namespace SFA.DAS.AANHub.Application.UnitTests.Common.Validators.MemberId;
public class MemberIdValidatorTests
{
    [TestCase("00000000-0000-0000-0000-000000000000", UserType.Admin, false)]
    [TestCase("B46C2A2A-E35C-4788-B4B7-1F7E84081846", UserType.Admin, false)]
    [TestCase("00000000-0000-0000-0000-000000000000", UserType.Apprentice, false)]
    [TestCase("B46C2A2A-E35C-4788-B4B7-1F7E84081846", UserType.Apprentice, true)]
    [TestCase("00000000-0000-0000-0000-000000000000", UserType.Employer, false)]
    [TestCase("B46C2A2A-E35C-4788-B4B7-1F7E84081846", UserType.Employer, true)]
    public async Task Validate_MemberIdExists_And_MemberIsNotAdmin(string memberId, UserType userType, bool isValid)
    {
        var guid = Guid.Parse(memberId);
        var command = new MemberIdCommandTest
        {
            MemberId = guid
        };

        var membersReadRepositoryMock = new Mock<IMembersReadRepository>();
        var member = isValid
            ? new Member
            {
                Status = MembershipStatus.Live,
                UserType = userType
            }
            : null;

        membersReadRepositoryMock.Setup(m => m.GetMember(guid)).ReturnsAsync(member);
        var sut = new MemberIdValidator(membersReadRepositoryMock.Object);

        var result = await sut.TestValidateAsync(command);

        if (isValid)
            result.ShouldNotHaveValidationErrorFor(c => c.MemberId);
        else
            result.ShouldHaveValidationErrorFor(c => c.MemberId);
    }

    [Test, RecursiveMoqAutoData]
    public async Task Validate_MemberIdIsNotEmpty_AndMemberIsNotFound([Frozen] Mock<IMembersReadRepository> membersReadRepositoryMock, Member member)
    {
        membersReadRepositoryMock.Setup(m => m.GetMember(member.Id)).ReturnsAsync(() => null);
        var command = new MemberIdCommandTest() { MemberId = member.Id };

        var sut = new MemberIdValidator(membersReadRepositoryMock.Object);
        var result = await sut.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(c => c.MemberId).WithErrorMessage(MemberIdValidator.MemberIdNotFoundErrorMessage);
    }

    private class MemberIdCommandTest : IMemberId
    {
        public Guid MemberId { get; set; }
    }
}
