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
        [TestCase("00000000-0000-0000-0000-000000000000", false)]
        [TestCase("B46C2A2A-E35C-4788-B4B7-1F7E84081846", true)]
        public async Task Validates_UserRef_NotNull(string guid, bool isValid)
        {
            var userRef = Guid.Parse(guid);
            var query = new GetEmployerMemberQuery(userRef);
            var employer = isValid ? new Employer() : null;
            var employersReadRepositoryMock = new Mock<IEmployersReadRepository>();

            employersReadRepositoryMock.Setup(a => a.GetEmployerByUserRef(userRef)).ReturnsAsync(employer);
            var sut = new GetEmployerMemberQueryValidator();
            var result = await sut.TestValidateAsync(query);

            if (isValid)
                result.ShouldNotHaveValidationErrorFor(c => c.UserRef);
            else
                result.ShouldHaveValidationErrorFor(c => c.UserRef);
        }
    }
}