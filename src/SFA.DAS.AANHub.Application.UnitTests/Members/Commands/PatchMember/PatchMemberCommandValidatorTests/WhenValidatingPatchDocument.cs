using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Members.Commands.PatchMember;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.UnitTests.Members.Commands.PatchMember.PatchMemberCommandValidatorTests;

public class WhenValidatingPatchDocument
{
    [Test]
    public async Task ThenThereShouldBeAtLeastOneOperation()
    {
        Mock<IMembersReadRepository> repositoryMock = new();
        PatchMemberCommandValidator sut = new(repositoryMock.Object);
        PatchMemberCommand target = new();

        var result = await sut.TestValidateAsync(target);

        result.ShouldHaveValidationErrorFor(s => s.PatchDoc).WithErrorMessage(PatchMemberCommandValidator.NoPatchOperationsPresentErrorMessage);
    }

    [Test]
    public async Task ThenOperationsCountCannotExceedPatchableFieldCount()
    {
        Mock<IMembersReadRepository> repositoryMock = new();
        PatchMemberCommandValidator sut = new(repositoryMock.Object);
        PatchMemberCommand target = new();
        for (int i = 0; i < 10; i++) target.PatchDoc.Replace(f => f.RegionId, i);

        var result = await sut.TestValidateAsync(target);

        result.ShouldHaveValidationErrorFor(s => s.PatchDoc).WithErrorMessage(PatchMemberCommandValidator.TooManyPatchOperationsPresentErrorMessage);
    }

    [Test]
    public async Task ThenOnlyReplaceOperationsAreAllowed()
    {
        Mock<IMembersReadRepository> repositoryMock = new();
        PatchMemberCommandValidator sut = new(repositoryMock.Object);
        PatchMemberCommand target = new();
        target.PatchDoc.Add(f => f.RegionId, 1);

        var result = await sut.TestValidateAsync(target);

        result.ShouldHaveValidationErrorFor(s => s.PatchDoc).WithErrorMessage(PatchMemberCommandValidator.PatchOperationContainsUnavailableOperationErrorMessage);
    }

    [TestCase(true)]
    [TestCase(false)]
    public async Task ThenOperationCannotHaveDuplicates(bool hasDuplicate)
    {
        Mock<IMembersReadRepository> repositoryMock = new();
        PatchMemberCommandValidator sut = new(repositoryMock.Object);
        PatchMemberCommand target = new();
        target.PatchDoc.Replace(f => f.RegionId, 1);
        if (hasDuplicate) target.PatchDoc.Replace(f => f.RegionId, 2);

        var result = await sut.TestValidateAsync(target);

        if (hasDuplicate)
        {
            result.ShouldHaveValidationErrorFor(s => s.PatchDoc).WithErrorMessage(PatchMemberCommandValidator.FoundDuplicatePatchOperationsErrorMessage);
        }
        else
        {
            result.ShouldNotHaveValidationErrorFor(s => s.PatchDoc);
        }
    }

    [Test]
    public async Task ThenOnlyAllowableFieldsCanHaveOperation()
    {
        Mock<IMembersReadRepository> repositoryMock = new();
        PatchMemberCommandValidator sut = new(repositoryMock.Object);
        PatchMemberCommand target = new();
        for (int i = 0; i < 10; i++) target.PatchDoc.Replace(f => f.UserType, i.ToString());

        var result = await sut.TestValidateAsync(target);

        result.ShouldHaveValidationErrorFor(s => s.PatchDoc).WithErrorMessage(PatchMemberCommandValidator.PatchOperationContainsUnavailableFieldErrorMessage);
    }
}
