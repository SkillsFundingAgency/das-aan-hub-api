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

        [TestCase("00000000-0000-0000-0000-000000000000", false)]
        [TestCase("B46C2A2A-E35C-4788-B4B7-1F7E84081846", true)]
        public async Task Validates_UserRef_NotNull(string userRef, bool isValid)
        {
            var guid = Guid.Parse(userRef);
            var command = new CreateEmployerMemberCommand
            {
                UserRef = guid
            };

            var sut = new CreateEmployerMemberCommandValidator(_regionsReadRepository.Object, _membersReadRepository.Object, _employersReadRepository.Object);

            var result = await sut.TestValidateAsync(command);

            if (isValid)
                result.ShouldNotHaveValidationErrorFor(c => c.UserRef);
            else
                result.ShouldHaveValidationErrorFor(c => c.UserRef);
        }

        [TestCase("Organisation name", true)]
        [TestCase(null, false)]
        public async Task Validates_Organisation_NotNull(string organisation, bool isValid)
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
        public async Task Validates_RequestedByUserId_NotEmptyGuid(Guid id, bool isValid)
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
        public async Task Validates_UserRef_Exist(bool userRefAlreadyExist)
        {
            Employer? employer = null;

            if (userRefAlreadyExist) employer = new Employer();
            var command = new CreateEmployerMemberCommand
            {
                UserRef = Guid.NewGuid()
            };

            _employersReadRepository.Setup(x => x.GetEmployerByUserRef(It.IsAny<Guid>())).ReturnsAsync(employer);
            var sut = new CreateEmployerMemberCommandValidator(_regionsReadRepository.Object, _membersReadRepository.Object, _employersReadRepository.Object);

            var result = await sut.TestValidateAsync(command);

            if (userRefAlreadyExist)
            {
                result.ShouldHaveValidationErrorFor(c => c.UserRef);
                result.Errors[6].PropertyName.Should().Be("UserRef");
                result.Errors[6].ErrorMessage.Should().Be("UserRef already exists");
            }
            else { result.ShouldNotHaveValidationErrorFor(c => c.UserRef); }
        }
    }
}