using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Members.Commands.PatchMember;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.UnitTests.Members.Commands.PatchMember.PatchMemberCommandValidatorTests;

public class WhenValidatingLastName
{
    [Test]
    public async Task AndOperationIsMissingThenAvoidsValidatingField()
    {
        Mock<IMembersReadRepository> repositoryMock = new();
        PatchMemberCommandValidator sut = new(repositoryMock.Object);
        PatchMemberCommand target = new();

        var result = await sut.TestValidateAsync(target);

        result.ShouldNotHaveValidationErrorFor(s => s.LastName);
    }

    [TestCase(null, false, null)]
    [TestCase(0, false, null)]
    [TestCase(1, true, null)]
    [TestCase(200, true, null)]
    [TestCase(201, false, PatchMemberCommandValidator.ExceededAllowableLengthErrorMessage)]
    public async Task ThenShouldHaveValidValue(int? stringLength, bool isValid, string? errorMessage)
    {
        Mock<IMembersReadRepository> repositoryMock = new();
        PatchMemberCommandValidator sut = new(repositoryMock.Object);
        PatchMemberCommand target = new();

        string? value = stringLength switch
        {
            null => null,
            0 => string.Empty,
            _ => new string('q', stringLength.Value)
        };
        target.PatchDoc.Replace(f => f.LastName, value);

        var result = await sut.TestValidateAsync(target);

        if (isValid)
        {
            result.ShouldNotHaveValidationErrorFor(s => s.LastName);
        }
        else
        {
            result
                .ShouldHaveValidationErrorFor(s => s.LastName)
                .WithErrorMessage(
                    string.IsNullOrWhiteSpace(errorMessage)
                    ? string.Format(PatchMemberCommandValidator.ValueIsRequiredErrorMessage, MemberPatchFields.LastName)
                    : string.Format(PatchMemberCommandValidator.ExceededAllowableLengthErrorMessage, MemberPatchFields.LastName, 200));
        }
    }
}
