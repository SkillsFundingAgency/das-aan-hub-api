using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Common;

namespace SFA.DAS.AANHub.Application.UnitTests.Common;

public class ProfileValueValidatorTests
{
    [TestCase(null, false)]
    [TestCase("", false)]
    [TestCase("", false)]
    [TestCase("dfed", true)]
    public void Validate_Values(string? profileValue, bool isValid)
    {
        ProfileValue target = new(1, profileValue!);
        ProfileValueValidator sut = new();

        var result = sut.TestValidate(target);

        if (isValid)
        {
            result.ShouldNotHaveValidationErrorFor(t => t.Value);
        }
        else
        {
            result.ShouldHaveValidationErrorFor(t => t.Value);
        }
    }

    [TestCase(0, false)]
    [TestCase(1, true)]
    public void Validate_Ids(int id, bool isValid)
    {
        ProfileValue target = new(id, Guid.NewGuid().ToString());
        ProfileValueValidator sut = new();

        var result = sut.TestValidate(target);

        if (isValid)
        {
            result.ShouldNotHaveValidationErrorFor(t => t.Id);
        }
        else
        {
            result.ShouldHaveValidationErrorFor(t => t.Id);
        }
    }
}
