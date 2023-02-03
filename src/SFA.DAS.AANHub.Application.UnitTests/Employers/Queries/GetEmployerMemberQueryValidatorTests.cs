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
        [TestCase(123, 456, true)]
        [TestCase(null, 456, false)]
        [TestCase(0, 456, false)]
        public async Task Validates_AccountId_NotNull(long accountId, long externalUserId, bool isValid)
        {
            var query = new GetEmployerMemberQuery(accountId, externalUserId);
            var employer = isValid ? new Employer() : null;
            var employersReadRepositoryMock = new Mock<IEmployersReadRepository>();

            employersReadRepositoryMock.Setup(a => a.GetEmployerByAccountIdAndUserId(accountId, externalUserId)).ReturnsAsync(employer);
            var sut = new GetEmployerMemberQueryValidator();
            var result = await sut.TestValidateAsync(query);

            if (isValid)
                result.ShouldNotHaveValidationErrorFor(c => c.AccountId);
            else
            {
                result.ShouldHaveValidationErrorFor(c => c.AccountId);
            }
        }

        [TestCase(123, null, false)]
        [TestCase(123, 0, false)]
        public async Task Validates_ExternalUserId_NotNull(long accountId, long externalUserId, bool isValid)
        {
            var query = new GetEmployerMemberQuery(accountId, externalUserId);
            var employer = isValid ? new Employer() : null;
            var employersReadRepositoryMock = new Mock<IEmployersReadRepository>();

            employersReadRepositoryMock.Setup(a => a.GetEmployerByAccountIdAndUserId(accountId, externalUserId)).ReturnsAsync(employer);
            var sut = new GetEmployerMemberQueryValidator();
            var result = await sut.TestValidateAsync(query);

            result.ShouldHaveValidationErrorFor(c => c.ExternalUserId);
        }
    }
}