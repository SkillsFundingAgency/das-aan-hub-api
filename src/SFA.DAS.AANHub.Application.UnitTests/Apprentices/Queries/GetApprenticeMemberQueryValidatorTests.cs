using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Apprentices.Queries;

namespace SFA.DAS.AANHub.Application.UnitTests.Apprentices.Queries;

public class GetApprenticeMemberQueryValidatorTests
{
    [TestCase("8fad718f-4c72-4fb4-8da0-6763efa8fb9c", true)]
    [TestCase(null, false)]
    [TestCase("00000000-0000-0000-0000-000000000000", false)]
    public async Task Validates_ApprenticeId_NotNull_NotFound(Guid apprenticeId, bool isValid)
    {
        var query = new GetApprenticeMemberQuery(apprenticeId);
        var sut = new GetApprenticeMemberQueryValidator();

        var result = await sut.TestValidateAsync(query);

        if (isValid)
            result.ShouldNotHaveValidationErrorFor(c => c.ApprenticeId);
        else
            result.ShouldHaveValidationErrorFor(c => c.ApprenticeId);
    }
}