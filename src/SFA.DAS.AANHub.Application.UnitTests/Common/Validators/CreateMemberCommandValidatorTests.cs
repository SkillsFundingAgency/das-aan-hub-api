using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Commands.CreateMember;
using SFA.DAS.AANHub.Application.Common.Validators;

namespace SFA.DAS.AANHub.Application.UnitTests.Common.Validators
{
    [TestFixture]
    public class CreateMemberCommandValidatorTests
    {
        [TestCase(250, true)]
        [TestCase(251, false)]
        [TestCase(0, false)]
        public async Task Validates_Name_Length(int stringLength, bool isValid)
        {
            var command = new CreateMemberCommand { Name = new string('a', stringLength) };
            var sut = new CreateMemberCommandValidator();

            var result = await sut.TestValidateAsync(command);

            if (isValid)
                result.ShouldNotHaveValidationErrorFor(c => c.Name);
            else
                result.ShouldHaveValidationErrorFor(c => c.Name);
        }
        [TestCase(250, true)]
        [TestCase(251, false)]
        [TestCase(0, false)]
        public async Task Validates_Information_Length(int stringLength, bool isValid)
        {
            var command = new CreateMemberCommand { Information = new string('a', stringLength) };
            var sut = new CreateMemberCommandValidator();

            var result = await sut.TestValidateAsync(command);

            if (isValid)
                result.ShouldNotHaveValidationErrorFor(c => c.Information);
            else
                result.ShouldHaveValidationErrorFor(c => c.Information);
        }
        [TestCase(10, "@email.com", true)]
        [TestCase(257, "@email.com", false)]
        [TestCase(0, "", false)]
        public async Task Validates_Email_Length(int stringLength, string emailSuffix, bool isValid)
        {
            var emailString = new string('a', stringLength) + emailSuffix;
            var command = new CreateMemberCommand { Email = emailString };
            var sut = new CreateMemberCommandValidator();

            var result = await sut.TestValidateAsync(command);

            if (isValid)
                result.ShouldNotHaveValidationErrorFor(c => c.Email);
            else
                result.ShouldHaveValidationErrorFor(c => c.Email);
        }
    }
}
