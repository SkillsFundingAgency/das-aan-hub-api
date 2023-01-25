using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Employers.Queries;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.UnitTests.Employers.Queries
{
    [TestFixture]
    public class GetEmployerMemberQueryValidatorTests
    {
        [TestCase(123, true)]
        [TestCase(null, false)]
        [TestCase(0, false)]
        public async Task Validates_UserId_NotNull_NotFound(long userId, bool isValid)
        {
            var query = new GetEmployerMemberQuery(userId);
            var employer = isValid ? new Employer() : null;
            var employersReadRepositoryMock = new Mock<IEmployersReadRepository>();

            employersReadRepositoryMock.Setup(a => a.GetEmployerByUserId(userId)).ReturnsAsync(employer);
            var sut = new GetEmployerMemberQueryValidator(employersReadRepositoryMock.Object);
            var result = await sut.TestValidateAsync(query);

            if (isValid)
                result.ShouldNotHaveValidationErrorFor(c => c.UserId);
            else
                result.ShouldHaveValidationErrorFor(c => c.UserId);
        }
    }
}