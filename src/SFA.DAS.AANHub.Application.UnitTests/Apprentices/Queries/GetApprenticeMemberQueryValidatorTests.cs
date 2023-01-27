using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Apprentices.Queries;

namespace SFA.DAS.AANHub.Application.UnitTests.Apprentices.Queries
{
    [TestFixture]
    public class GetApprenticeMemberQueryValidatorTests
    {
        [TestCase(123, true)]
        [TestCase(null, false)]
        [TestCase(0, false)]
        public async Task Validates_ApprenticeId_NotNull_NotFound(long apprenticeId, bool isValid)
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
}