using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Partners.Commands.CreatePartnerMember;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.UnitTests.Partners.Commands.CreatePartnerMember
{
    public class CreatePartnerMemberCommandValidatorTests
    {
        private readonly Mock<IMembersReadRepository> _membersReadRepository;
        private readonly Mock<IPartnersReadRepository> _partnersReadRepository;
        private readonly Mock<IRegionsReadRepository> _regionsReadRepository;

        public CreatePartnerMemberCommandValidatorTests()
        {
            _regionsReadRepository = new Mock<IRegionsReadRepository>();
            _membersReadRepository = new Mock<IMembersReadRepository>();
            _partnersReadRepository = new Mock<IPartnersReadRepository>();
        }

        [TestCase("userName", true)]
        [TestCase("", false)]
        [TestCase(null, false)]
        [TestCase(" ", false)]
        public async Task Validates_UserName_NotNullOrEmpty(string? userName, bool isValid)
        {
            var command = new CreatePartnerMemberCommand
            {
                UserName = userName!
            };

            var sut = new CreatePartnerMemberCommandValidator(_regionsReadRepository.Object, _membersReadRepository.Object, _partnersReadRepository.Object);

            var result = await sut.TestValidateAsync(command);

            if (isValid)
                result.ShouldNotHaveValidationErrorFor(c => c.UserName);
            else
                result.ShouldHaveValidationErrorFor(c => c.UserName);
        }

        [TestCase(5, true)]
        [TestCase(251, false)]
        public async Task Validates_UserName_Length(int length, bool isValid)
        {
            var command = new CreatePartnerMemberCommand
            {
                UserName = new string('a', length)
            };

            var sut = new CreatePartnerMemberCommandValidator(_regionsReadRepository.Object, _membersReadRepository.Object, _partnersReadRepository.Object);
            var result = await sut.TestValidateAsync(command);

            if (isValid)
                result.ShouldNotHaveValidationErrorFor(c => c.UserName);
            else
                result.ShouldHaveValidationErrorFor(c => c.UserName);
        }

        [TestCase("Organisation name", true)]
        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase(" ", false)]
        public async Task Validates_Organisation_NotNull(string? organisation, bool isValid)
        {
            var command = new CreatePartnerMemberCommand
            {
                Organisation = organisation!
            };

            var sut = new CreatePartnerMemberCommandValidator(_regionsReadRepository.Object, _membersReadRepository.Object, _partnersReadRepository.Object);

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
            var command = new CreatePartnerMemberCommand
            {
                Organisation = new string('a', stringLength)
            };

            var sut = new CreatePartnerMemberCommandValidator(_regionsReadRepository.Object, _membersReadRepository.Object, _partnersReadRepository.Object);

            var result = await sut.TestValidateAsync(command);

            if (isValid)
                result.ShouldNotHaveValidationErrorFor(c => c.Organisation);
            else
                result.ShouldHaveValidationErrorFor(c => c.Organisation);
        }
    }
}