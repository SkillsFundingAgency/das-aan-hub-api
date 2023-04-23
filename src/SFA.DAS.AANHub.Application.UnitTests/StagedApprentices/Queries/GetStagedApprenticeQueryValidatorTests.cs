using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.StagedApprentices.Queries;

namespace SFA.DAS.AANHub.Application.UnitTests.StagedApprentices.Queries;

public class GetStagedApprenticeQueryValidatorTests
{
    const string lastName = "Smith";
    private static readonly DateTime dateOfBirth = new(2000, 1, 1);
    const string emailAddress = "email@email.com";

    [TestCase(null, false)]
    [TestCase("", false)]
    [TestCase(" ", false)]
    [TestCase("Smith", true)]
    public async Task Validate_LastName(string lastname, bool isValid)
    {
        var query = new GetStagedApprenticeQuery(lastname, dateOfBirth, emailAddress);

        var sut = new GetStagedApprenticeQueryValidator();
        var result = await sut.TestValidateAsync(query);

        if (isValid)
            result.ShouldNotHaveValidationErrorFor(c => c.LastName);
        else
            result.ShouldHaveValidationErrorFor(c => c.LastName);
    }

    [TestCase(null, false)]
    [TestCase("01/01/2001", true)]
    public async Task Validate_DateOfBirth(DateTime dateofbirth, bool isValid)
    {
        var query = new GetStagedApprenticeQuery(lastName, dateofbirth, emailAddress);

        var sut = new GetStagedApprenticeQueryValidator();
        var result = await sut.TestValidateAsync(query);

        if (isValid)
            result.ShouldNotHaveValidationErrorFor(c => c.DateOfBirth);
        else
            result.ShouldHaveValidationErrorFor(c => c.DateOfBirth);
    }

    [TestCase("@example.com", false)]
    [TestCase("", false)]
    [TestCase(" ", false)]
    [TestCase("email@email.com", true)]
    public async Task Validate_Email(string email, bool isValid)
    {
        var query = new GetStagedApprenticeQuery(lastName, dateOfBirth, email);

        var sut = new GetStagedApprenticeQueryValidator();
        var result = await sut.TestValidateAsync(query);

        if (isValid)
            result.ShouldNotHaveValidationErrorFor(c => c.Email);
        else
            result.ShouldHaveValidationErrorFor(c => c.Email);
    }
}