using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Members.Commands.PatchMember;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.UnitTests.Members.Commands.PatchMember.PatchMemberCommandValidatorTests;

public class WhenValidatingEmail
{
    [Test]
    public async Task AndEmailOperationIsMissingThenAvoidsValidatingEmail()
    {
        Mock<IMembersReadRepository> repositoryMock = new();
        PatchMemberCommandValidator sut = new(repositoryMock.Object);
        PatchMemberCommand target = new();

        var result = await sut.TestValidateAsync(target);

        result.ShouldNotHaveValidationErrorFor(s => s.Email);
    }

    [TestCase("")]
    [TestCase(" ")]
    [TestCase(null)]
    public async Task ThenEmptyNullAndWhiteSpaceAreNotAllowed(string? email)
    {
        Mock<IMembersReadRepository> repositoryMock = new();
        PatchMemberCommandValidator sut = new(repositoryMock.Object);
        PatchMemberCommand target = new();
        target.PatchDoc.Replace(f => f.Email, email);

        var result = await sut.TestValidateAsync(target);

        result.ShouldHaveValidationErrorFor(s => s.Email).WithErrorMessage(string.Format(PatchMemberCommandValidator.ValueIsRequiredErrorMessage, MemberPatchFields.Email));
    }

    [Test]
    public async Task ThenValueCannotExceedAllowableLength()
    {
        Mock<IMembersReadRepository> repositoryMock = new();
        PatchMemberCommandValidator sut = new(repositoryMock.Object);
        PatchMemberCommand target = new();
        target.PatchDoc.Replace(f => f.Email, new string('a', 257));

        var result = await sut.TestValidateAsync(target);

        result.ShouldHaveValidationErrorFor(s => s.Email).WithErrorMessage(string.Format(PatchMemberCommandValidator.ExceededAllowableLengthErrorMessage, MemberPatchFields.Email, 256));
    }

    [TestCase("john.doe@example.com", true)]
    [TestCase("@example.com", false)]
    [TestCase("sfgg", false)]
    [TestCase("sfsfv@sdfd", false)]
    public async Task ThenOnlyAcceptsEmailsInCorrectFormat(string email, bool isValid)
    {
        Mock<IMembersReadRepository> repositoryMock = new();
        PatchMemberCommandValidator sut = new(repositoryMock.Object);
        PatchMemberCommand target = new();
        target.PatchDoc.Replace(f => f.Email, email);

        var result = await sut.TestValidateAsync(target);

        if (isValid)
        {
            result.ShouldNotHaveValidationErrorFor(s => s.Email);
        }
        else
        {
            result.ShouldHaveValidationErrorFor(s => s.Email).WithErrorMessage(PatchMemberCommandValidator.InvalidEmailFormatErrorMessage);
        }
    }
}
