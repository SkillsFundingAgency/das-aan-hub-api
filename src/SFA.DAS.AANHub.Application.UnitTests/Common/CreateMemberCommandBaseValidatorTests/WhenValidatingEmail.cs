using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Common;

namespace SFA.DAS.AANHub.Application.UnitTests.Common.CreateMemberCommandBaseValidatorTests;

public class WhenValidatingEmail
{
    [TestCase("")]
    [TestCase(" ")]
    [TestCase(null)]
    public async Task ThenEmptyNullAndWhiteSpaceAreNotAllowed(string? email)
    {
        CreateMemberCommandBaseValidator sut = new();
        TestTarget target = new()
        {
            Email = email
        };

        var result = await sut.TestValidateAsync(target);

        result
            .ShouldHaveValidationErrorFor(s => s.Email)
            .WithErrorMessage(string.Format(CreateMemberCommandBaseValidator.ValueIsRequiredErrorMessage, nameof(TestTarget.Email)));
    }

    [Test]
    public async Task ThenValueCannotExceedAllowableLength()
    {
        CreateMemberCommandBaseValidator sut = new();
        TestTarget target = new()
        {
            Email = new string('a', 257)
        };

        var result = await sut.TestValidateAsync(target);

        result
            .ShouldHaveValidationErrorFor(s => s.Email)
            .WithErrorMessage(string.Format(CreateMemberCommandBaseValidator.ExceededAllowableLengthErrorMessage, nameof(TestTarget.Email), 256));
    }

    [TestCase("john.doe@example.com", true)]
    [TestCase("@example.com", false)]
    [TestCase("sfgg", false)]
    [TestCase("sfsfv@sdfd", false)]
    public async Task ThenOnlyAcceptsEmailsInCorrectFormat(string email, bool isValid)
    {
        CreateMemberCommandBaseValidator sut = new();
        TestTarget target = new()
        {
            Email = email
        };

        var result = await sut.TestValidateAsync(target);

        if (isValid)
        {
            result.ShouldNotHaveValidationErrorFor(s => s.Email);
        }
        else
        {
            result.ShouldHaveValidationErrorFor(s => s.Email)
                .WithErrorMessage(CreateMemberCommandBaseValidator.InvalidEmailFormatErrorMessage);
        }
    }
}
