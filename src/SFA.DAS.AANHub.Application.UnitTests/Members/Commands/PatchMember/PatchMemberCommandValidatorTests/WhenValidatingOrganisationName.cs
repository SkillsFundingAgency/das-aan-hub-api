using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Members.Commands.PatchMember;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.UnitTests.Members.Commands.PatchMember.PatchMemberCommandValidatorTests;

public class WhenValidatingOrganisationName
{
    [Test]
    public async Task AndOperationIsMissingThenAvoidsValidatingField()
    {
        Mock<IMembersReadRepository> repositoryMock = new();
        PatchMemberCommandValidator sut = new(repositoryMock.Object);
        PatchMemberCommand target = new();

        var result = await sut.TestValidateAsync(target);

        result.ShouldNotHaveValidationErrorFor(s => s.OrganisationName);
    }

    [TestCase(null, false, null)]
    [TestCase(0, false, null)]
    [TestCase(1, true, null)]
    [TestCase(250, true, null)]
    [TestCase(251, false, PatchMemberCommandValidator.ExceededAllowableLengthErrorMessage)]
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
        target.PatchDoc.Replace(f => f.OrganisationName, value);

        var result = await sut.TestValidateAsync(target);

        if (isValid)
        {
            result.ShouldNotHaveValidationErrorFor(s => s.OrganisationName);
        }
        else
        {
            result
                .ShouldHaveValidationErrorFor(s => s.OrganisationName)
                .WithErrorMessage(
                    string.IsNullOrWhiteSpace(errorMessage)
                    ? string.Format(PatchMemberCommandValidator.ValueIsRequiredErrorMessage, MemberPatchFields.OrganisationName)
                    : string.Format(PatchMemberCommandValidator.ExceededAllowableLengthErrorMessage, MemberPatchFields.OrganisationName, 250));
        }
    }
}
