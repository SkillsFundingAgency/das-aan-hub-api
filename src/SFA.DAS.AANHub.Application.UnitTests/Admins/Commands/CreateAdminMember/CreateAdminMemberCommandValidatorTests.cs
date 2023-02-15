using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Admins.Commands.CreateAdminMember;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.UnitTests.Admins.Commands.CreateAdminMember
{
    public class CreateAdminMemberCommandValidatorTests
    {
        private readonly Mock<IAdminsReadRepository> _adminsReadRepository;
        private readonly Mock<IMembersReadRepository> _membersReadRepository;
        private readonly Mock<IRegionsReadRepository> _regionsReadRepository;

        public CreateAdminMemberCommandValidatorTests()
        {
            _regionsReadRepository = new Mock<IRegionsReadRepository>();
            _membersReadRepository = new Mock<IMembersReadRepository>();
            _adminsReadRepository = new Mock<IAdminsReadRepository>();
        }

        [TestCase("userName", true)]
        [TestCase("", false)]
        [TestCase(null, false)]
        public async Task Validates_UserName_NotNullOrEmpty(string? userName, bool isValid)
        {
            var command = new CreateAdminMemberCommand
            {
                UserName = userName!
            };

            var sut = new CreateAdminMemberCommandValidator(_regionsReadRepository.Object, _membersReadRepository.Object, _adminsReadRepository.Object);

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
            var command = new CreateAdminMemberCommand
            {
                UserName = new string('a', length)
            };

            var sut = new CreateAdminMemberCommandValidator(_regionsReadRepository.Object, _membersReadRepository.Object, _adminsReadRepository.Object);

            var result = await sut.TestValidateAsync(command);

            if (isValid)
                result.ShouldNotHaveValidationErrorFor(c => c.UserName);
            else
                result.ShouldHaveValidationErrorFor(c => c.UserName);
        }
    }
}