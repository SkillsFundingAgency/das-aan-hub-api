using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Apprentices.Queries;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

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
            var apprentice = isValid ? new Apprentice() : null;
            var apprenticesReadRepositoryMock = new Mock<IApprenticesReadRepository>();

            apprenticesReadRepositoryMock.Setup(a => a.GetApprentice(apprenticeId)).ReturnsAsync(apprentice);
            var sut = new GetApprenticeMemberQueryValidator(apprenticesReadRepositoryMock.Object);
            var result = await sut.TestValidateAsync(query);

            if (isValid)
                result.ShouldNotHaveValidationErrorFor(c => c.ApprenticeId);
            else
                result.ShouldHaveValidationErrorFor(c => c.ApprenticeId);
        }
    }
}