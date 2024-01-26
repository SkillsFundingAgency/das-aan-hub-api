using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.UnitTests.Common.CreateMemberCommandBaseValidatorTests;

public class WhenValidatingEmail
{
    private const string ExistingEmail = "existing.email@test.com";

    [TestCase("")]
    [TestCase(" ")]
    [TestCase(null)]
    public async Task ThenEmptyNullAndWhiteSpaceAreNotAllowed(string? email)
    {
        CreateMemberCommandBaseValidator sut = new(Mock.Of<IMembersReadRepository>());
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
        CreateMemberCommandBaseValidator sut = new(Mock.Of<IMembersReadRepository>());
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
        CreateMemberCommandBaseValidator sut = new(Mock.Of<IMembersReadRepository>());
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

    [TestCase("new.email@test.com", true)]
    [TestCase(ExistingEmail, false)]
    public async Task ThenOnlyAcceptsEmailsNotInMemberTable(string email, bool isValid)
    {
        var membersReadRepository = new Mock<IMembersReadRepository>();
        membersReadRepository.Setup(x => x.GetMemberByEmail(ExistingEmail)).ReturnsAsync(new Member { Email = ExistingEmail });

        CreateMemberCommandBaseValidator sut = new(membersReadRepository.Object);
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
                .WithErrorMessage(CreateMemberCommandBaseValidator.EmailAlreadyExistsErrorMessage);
        }
    }

}
