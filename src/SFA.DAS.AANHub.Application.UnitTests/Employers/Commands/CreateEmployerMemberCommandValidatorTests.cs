using FluentAssertions;
using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Employers.Commands;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.UnitTests.Employers.Commands
{
    public class CreateEmployerMemberCommandValidatorTests
    {
        private static readonly object[] FailGuidTestCases =
        {
            new object[]
            {
                Guid.NewGuid(), false
            },
            new object[]
            {
                Guid.Empty, false
            }
        };

        private readonly Mock<IEmployersReadRepository> _employersReadRepository;
        private readonly Mock<IMembersReadRepository> _membersReadRepository;
        private readonly Mock<IRegionsReadRepository> _regionsReadRepository;

        public CreateEmployerMemberCommandValidatorTests()
        {
            _regionsReadRepository = new Mock<IRegionsReadRepository>();
            _membersReadRepository = new Mock<IMembersReadRepository>();
            _employersReadRepository = new Mock<IEmployersReadRepository>();
        }

        [TestCase(123, true)]
        [TestCase(null, false)]
        [TestCase(0, false)]
        public async Task Validates_UserId_NotNull(long id, bool isValid)
        {
            var command = new CreateEmployerMemberCommand
            {
                UserId = id
            };

            var sut = new CreateEmployerMemberCommandValidator(_regionsReadRepository.Object, _membersReadRepository.Object, _employersReadRepository.Object);

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
            var command = new CreateEmployerMemberCommand
            {
                Organisation = organisation
            };

            var sut = new CreateEmployerMemberCommandValidator(_regionsReadRepository.Object, _membersReadRepository.Object, _employersReadRepository.Object);

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
            var command = new CreateEmployerMemberCommand
            {
                Organisation = new string('a', stringLength)
            };

            var sut = new CreateEmployerMemberCommandValidator(_regionsReadRepository.Object, _membersReadRepository.Object, _employersReadRepository.Object);

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
            var command = new CreateEmployerMemberCommand
            {
                AccountId = id
            };

            var sut = new CreateEmployerMemberCommandValidator(_regionsReadRepository.Object, _membersReadRepository.Object, _employersReadRepository.Object);

            var result = await sut.TestValidateAsync(command);

            if (isValid)
                result.ShouldNotHaveValidationErrorFor(c => c.AccountId);
            else
                result.ShouldHaveValidationErrorFor(c => c.AccountId);
        }

        [TestCaseSource(nameof(FailGuidTestCases))]
        public async Task Validates_RequestedByUserId_NotEmptyGuid(Guid? id, bool isValid)
        {
            var command = new CreateEmployerMemberCommand
            {
                RequestedByMemberId = id
            };

            var sut = new CreateEmployerMemberCommandValidator(_regionsReadRepository.Object, _membersReadRepository.Object, _employersReadRepository.Object);

            var result = await sut.TestValidateAsync(command);

            if (isValid)
                result.ShouldNotHaveValidationErrorFor(c => c.RequestedByMemberId);
            else
                result.ShouldHaveValidationErrorFor(c => c.RequestedByMemberId);
        }

        [TestCase(true)]
        [TestCase(false)]
        public async Task Validates_AccountId_UserId_Pair_Exist(bool pairExistsAlready)
        {
            Employer? employer = null;

            if (pairExistsAlready) employer = new Employer();
            var command = new CreateEmployerMemberCommand
            {
                AccountId = 1234,
                UserId = 2345
            };

            _employersReadRepository.Setup(x => x.GetEmployerByAccountIdAndUserId(It.IsAny<long>(), It.IsAny<long>())).ReturnsAsync(employer);
            var sut = new CreateEmployerMemberCommandValidator(_regionsReadRepository.Object, _membersReadRepository.Object, _employersReadRepository.Object);

            var result = await sut.TestValidateAsync(command);

            if (pairExistsAlready)
            {
                result.ShouldHaveValidationErrorFor(c => c.AccountId);
                result.ShouldHaveValidationErrorFor(c => c.UserId);
                result.Errors[8].PropertyName.Should().Be("AccountId");
                result.Errors[8].ErrorMessage.Should().Be("UserId and AccountId pair already exist");
                result.Errors[9].PropertyName.Should().Be("UserId");
                result.Errors[9].ErrorMessage.Should().Be("UserId and AccountId pair already exist");
            }
            else
            {
                result.ShouldNotHaveValidationErrorFor(c => c.AccountId);
                result.ShouldNotHaveValidationErrorFor(c => c.UserId);
            }
        }

        [TestCase(null, false)]
        public async Task Validates_RequestedByUserId_NotNull(Guid? id, bool isValid)
        {
            var command = new CreateEmployerMemberCommand
            {
                RequestedByMemberId = id
            };

            var sut = new CreateEmployerMemberCommandValidator(_regionsReadRepository.Object, _membersReadRepository.Object, _employersReadRepository.Object);

            var result = await sut.TestValidateAsync(command);

            if (isValid)
                result.ShouldNotHaveValidationErrorFor(c => c.RequestedByMemberId);
            else
                result.ShouldHaveValidationErrorFor(c => c.RequestedByMemberId);
        }
    }
}