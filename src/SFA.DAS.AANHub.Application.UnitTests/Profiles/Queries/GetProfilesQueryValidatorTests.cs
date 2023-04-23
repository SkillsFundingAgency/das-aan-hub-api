using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Profiles.Queries.GetProfiles;

namespace SFA.DAS.AANHub.Application.UnitTests.Profiles.Queries;

[TestFixture]
public class GetProfilesQueryValidatorTests
{
    [TestCase("apprenticE", true)]
    [TestCase("Employer", true)]
    [TestCase("PARTNER", true)]
    [TestCase("admin", true)]
    [TestCase("None", true)]
    [TestCase(null, false)]
    [TestCase("", false)]
    [TestCase(" ", false)]
    [TestCase("xyz", false)]
    public async Task Validates_UserType(string userType, bool isValid)
    {
        var query = new GetProfilesByUserTypeQuery(userType);
        var sut = new GetProfilesByUserTypeQueryValidator();
        var result = await sut.TestValidateAsync(query);

        if (isValid)
            result.ShouldNotHaveValidationErrorFor(c => c.UserType);
        else
            result.ShouldHaveValidationErrorFor(c => c.UserType);
    }
}
