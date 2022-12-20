using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Employers.Commands;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.UnitTests.Employers.Commands
{
    public class CreateEmployerMemberCommandValidatorTests
    {
        private readonly Mock<IRegionsReadRepository> _regionsReadRepository;

        public CreateEmployerMemberCommandValidatorTests() => _regionsReadRepository = new Mock<IRegionsReadRepository>();

        [TestCase(123, true)]
        [TestCase(null, false)]
        [TestCase(0, false)]
        public async Task Validates_UserId_NotNull(long id, bool isValid)
        {

            var command = new CreateEmployerMemberCommand() { UserId = id };
            var sut = new CreateEmployerMemberCommandValidator(_regionsReadRepository.Object);

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

            var command = new CreateEmployerMemberCommand() { Organisation = organisation };
            var sut = new CreateEmployerMemberCommandValidator(_regionsReadRepository.Object);

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

            var command = new CreateEmployerMemberCommand() { Organisation = new string('a', stringLength) };
            var sut = new CreateEmployerMemberCommandValidator(_regionsReadRepository.Object);

            var result = await sut.TestValidateAsync(command);

            if (isValid)
                result.ShouldNotHaveValidationErrorFor(c => c.Organisation);
            else
                result.ShouldHaveValidationErrorFor(c => c.Organisation);
        }
        [TestCase(123, true)]
        [TestCase(null, false)]
        [TestCase(0, false)]
        public async Task Validates_AccountId_NotNull(long id, bool isValid)
        {

            var command = new CreateEmployerMemberCommand() { AccountId = id };
            var sut = new CreateEmployerMemberCommandValidator(_regionsReadRepository.Object);

            var result = await sut.TestValidateAsync(command);

            if (isValid)
                result.ShouldNotHaveValidationErrorFor(c => c.AccountId);
            else
                result.ShouldHaveValidationErrorFor(c => c.AccountId);
        }
    }
}
