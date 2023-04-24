using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Common;

namespace SFA.DAS.AANHub.Application.UnitTests.Common.CreateMemberCommandBaseValidatorTests;

public class WhenValidatingLastName
{
    [TestCase(null, false, null)]
    [TestCase(0, false, null)]
    [TestCase(1, true, null)]
    [TestCase(200, true, null)]
    [TestCase(201, false, CreateMemberCommandBaseValidator.ExceededAllowableLengthErrorMessage)]
    public async Task ThenShouldHaveValidValue(int? stringLength, bool isValid, string? errorMessage)
    {
        CreateMemberCommandBaseValidator sut = new();
        TestTarget target = new();

        string? value = stringLength switch
        {
            null => null,
            0 => string.Empty,
            _ => new string('q', stringLength.Value)
        };
        target.LastName = value;

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
                    ? string.Format(CreateMemberCommandBaseValidator.ValueIsRequiredErrorMessage, nameof(CreateMemberCommandBase.LastName))
                    : string.Format(CreateMemberCommandBaseValidator.ExceededAllowableLengthErrorMessage, nameof(CreateMemberCommandBase.LastName), 200));
        }
    }
}
