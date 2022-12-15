using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Employers.Commands;
using SFA.DAS.AANHub.Domain.Enums;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.UnitTests.Employers.Commands
{
    public class CreateEmployersCommandValidatorTests
    {
        private readonly Mock<IRegionsReadRepository> _regionsReadRepository;

        public CreateEmployersCommandValidatorTests() => _regionsReadRepository = new Mock<IRegionsReadRepository>();

        [TestCase(MembershipUserType.Employer, true)]
        [TestCase(null, false)]
        public async Task Validates_UserType_NotNull(MembershipUserType? userType, bool isValid)
        {

            var command = new CreateEmployersCommand() { UserType = userType };
            var sut = new CreateEmployersCommandValidator(_regionsReadRepository.Object);

            var result = await sut.TestValidateAsync(command);

            if (isValid)
                result.ShouldNotHaveValidationErrorFor(c => c.UserType);
            else
                result.ShouldHaveValidationErrorFor(c => c.UserType);
        }
        [TestCase(MembershipUserType.Employer, true)]
        [TestCase(MembershipUserType.Apprentice, false)]
        public async Task Validates_UserType_Employer(MembershipUserType? userType, bool isValid)
        {

            var command = new CreateEmployersCommand() { UserType = userType };
            var sut = new CreateEmployersCommandValidator(_regionsReadRepository.Object);

            var result = await sut.TestValidateAsync(command);

            if (isValid)
                result.ShouldNotHaveValidationErrorFor(c => c.UserType);
            else
                result.ShouldHaveValidationErrorFor(c => c.UserType);
        }

        [TestCase(123, true)]
        [TestCase(null, false)]
        public async Task Validates_UserId_NotNull(long? id, bool isValid)
        {

            var command = new CreateEmployersCommand() { UserId = id };
            var sut = new CreateEmployersCommandValidator(_regionsReadRepository.Object);

            var result = await sut.TestValidateAsync(command);

            if (isValid)
                result.ShouldNotHaveValidationErrorFor(c => c.UserId);
            else
                result.ShouldHaveValidationErrorFor(c => c.UserId);
        }

        [TestCase("Organisation name", true)]
        [TestCase(null, false)]
        public async Task Validates_Organisation_NotNull(string? organisation, bool isValid)
        {

            var command = new CreateEmployersCommand() { Organisation = organisation };
            var sut = new CreateEmployersCommandValidator(_regionsReadRepository.Object);

            var result = await sut.TestValidateAsync(command);

            if (isValid)
                result.ShouldNotHaveValidationErrorFor(c => c.Organisation);
            else
                result.ShouldHaveValidationErrorFor(c => c.Organisation);
        }
        [TestCase(123, true)]
        [TestCase(251, false)]
        public async Task Validates_Organisation_Length(int stringLength, bool isValid)
        {

            var command = new CreateEmployersCommand() { Organisation = new string('a', stringLength) };
            var sut = new CreateEmployersCommandValidator(_regionsReadRepository.Object);

            var result = await sut.TestValidateAsync(command);

            if (isValid)
                result.ShouldNotHaveValidationErrorFor(c => c.Organisation);
            else
                result.ShouldHaveValidationErrorFor(c => c.Organisation);
        }
        [TestCase(123, true)]
        [TestCase(null, false)]
        public async Task Validates_AccountId_NotNull(long? id, bool isValid)
        {

            var command = new CreateEmployersCommand() { AccountId = id };
            var sut = new CreateEmployersCommandValidator(_regionsReadRepository.Object);

            var result = await sut.TestValidateAsync(command);

            if (isValid)
                result.ShouldNotHaveValidationErrorFor(c => c.AccountId);
            else
                result.ShouldHaveValidationErrorFor(c => c.AccountId);
        }
    }
}
