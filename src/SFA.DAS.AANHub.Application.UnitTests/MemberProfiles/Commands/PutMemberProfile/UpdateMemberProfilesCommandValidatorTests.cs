using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Common.Validators.MemberId;
using SFA.DAS.AANHub.Application.MemberProfiles.Commands.PutMemberProfile;
using SFA.DAS.AANHub.Domain.Common;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using SFA.DAS.Testing.AutoFixture;
using static SFA.DAS.AANHub.Domain.Common.Constants;

namespace SFA.DAS.AANHub.Application.UnitTests.MemberProfiles.Commands.PutMemberProfile;
public class UpdateMemberProfilesCommandValidatorTests
{
    [Test, MoqAutoData]
    public async Task Validate_EmptyMemberId_Fails(
        Mock<IMembersReadRepository> membersReadRepository,
        UpdateMemberProfilesCommand command)
    {
        membersReadRepository.Setup(m => m.GetMember(It.IsAny<Guid>()))
                             .ReturnsAsync(new Member());

        command.MemberId = Guid.Empty;

        var sut = new UpdateMemberProfilesCommandValidator(membersReadRepository.Object);
        var result = await sut.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(c => c.MemberId);
    }

    [Test, MoqAutoData]
    public async Task Validate_InvalidMemberId_Fails(
        Mock<IMembersReadRepository> membersReadRepository,
        UpdateMemberProfilesCommand command)
    {
        membersReadRepository.Setup(m => m.GetMember(command.MemberId))
                             .ReturnsAsync(() => null);

        var sut = new UpdateMemberProfilesCommandValidator(membersReadRepository.Object);
        var result = await sut.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(c => c.MemberId).WithErrorMessage(MemberIdValidator.MemberIdNotFoundErrorMessage);
    }

    [Test, MoqAutoData]
    public async Task Validate_ExistingMemberId_NoError(
        Mock<IMembersReadRepository> membersReadRepository,
        UpdateMemberProfilesCommand command)
    {
        membersReadRepository.Setup(m => m.GetMember(command.MemberId))
                             .ReturnsAsync(new Member() { Id = command.MemberId, Status = MembershipStatus.Live, UserType = MemberUserType.Apprentice.ToString() });

        var sut = new UpdateMemberProfilesCommandValidator(membersReadRepository.Object);
        var result = await sut.TestValidateAsync(command);

        result.ShouldNotHaveValidationErrorFor(c => c.MemberId);
    }

    [MoqInlineAutoData(MembershipStatus.Live)]
    [MoqInlineAutoData(MembershipStatus.Pending)]
    [MoqInlineAutoData(MembershipStatus.Cancelled)]
    [MoqInlineAutoData(MembershipStatus.Withdrawn)]
    [MoqInlineAutoData(MembershipStatus.Deleted)]
    public async Task Validate_ExistingMemberStatus_ErrorNoError(
        string membershipStatus,
        MemberUserType memberUserType,
        Mock<IMembersReadRepository> membersReadRepository,
        UpdateMemberProfilesCommand command)
    {
        membersReadRepository.Setup(m => m.GetMember(It.IsAny<Guid>()))
                             .ReturnsAsync(new Member() { Id = command.MemberId, Status = membershipStatus, UserType = memberUserType.ToString() });

        var sut = new UpdateMemberProfilesCommandValidator(membersReadRepository.Object);
        var result = await sut.TestValidateAsync(command);

        if (membershipStatus == MembershipStatus.Live) result.ShouldNotHaveValidationErrorFor(c => c.MemberId);
        else result.ShouldHaveValidationErrorFor(c => c.MemberId).WithErrorMessage(MemberIdValidator.MemberIdNotFoundErrorMessage);
    }
}
