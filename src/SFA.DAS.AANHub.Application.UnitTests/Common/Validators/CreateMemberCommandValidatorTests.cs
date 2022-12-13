using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Commands.CreateMember;
using SFA.DAS.AANHub.Application.Common.Validators;

namespace SFA.DAS.AANHub.Application.UnitTests.Common.Validators
{
    [TestFixture]
    public class CreateMemberCommandValidatorTests
    {
        [TestCase("ThisIsAValidNameLength", true)]
        [TestCase("ThisIsTooLongOfANameThisIsTooLongOfANameThisIsTooLongOfAName" +
                  "ThisIsTooLongOfANameThisIsTooLongOfANameThisIsTooLongOfAName" +
                  "ThisIsTooLongOfANameThisIsTooLongOfANameThisIsTooLongOfAName" +
                  "ThisIsTooLongOfANameThisIsTooLongOfANameThisIsTooLongOfANameThisIsTooLongOfAName", false)]
        public async Task Validates_Name_Length(string name, bool isValid)
        {
            var command = new CreateMemberCommand() { Name = name };
            var sut = new CreateMemberCommandValidator();

            var result = await sut.TestValidateAsync(command);

            if (isValid)
                result.ShouldNotHaveValidationErrorFor(c => c.Name);
            else
                result.ShouldHaveValidationErrorFor(c => c.Name);
        }
        [TestCase("ThisIsAValidInformationLength", true)]
        [TestCase("ThisIsTooLongOfANameThisIsTooLongOfANameThisIsTooLongOfAName" +
                  "ThisIsTooLongOfANameThisIsTooLongOfANameThisIsTooLongOfAName" +
                  "ThisIsTooLongOfANameThisIsTooLongOfANameThisIsTooLongOfAName" +
                  "ThisIsTooLongOfANameThisIsTooLongOfANameThisIsTooLongOfANameThisIsTooLongOfAName", false)]
        public async Task Validates_Information_Length(string information, bool isValid)
        {
            var command = new CreateMemberCommand() { Information = information };
            var sut = new CreateMemberCommandValidator();

            var result = await sut.TestValidateAsync(command);

            if (isValid)
                result.ShouldNotHaveValidationErrorFor(c => c.Information);
            else
                result.ShouldHaveValidationErrorFor(c => c.Information);
        }
        [TestCase("thisIsAValid@email.com", true)]
        [TestCase("thisIsAnInvalid @email/com", false)]
        [TestCase("ThisIsTooLongOfAnEmailThisIsTooLongOfAnEmailThisIsTooLongOfAnEmailThisIsTooLongOfAnEmailThisIsTooLongOfAnEmailThisIsTooLongOfAnEmailThisIsTooLongOfAnEmail" +
                  "ThisIsTooLongOfAnEmailThisIsTooLongOfAnEmail" +
                  "ThisIsTooLongOfAnEmailThisIsTooLongOfAnEmail" +
                  "ThisIsTooLongOfAnEmail@email.com", false)]
        public async Task Validates_Email(string email, bool isValid)
        {
            var command = new CreateMemberCommand() { Email = email };
            var sut = new CreateMemberCommandValidator();

            var result = await sut.TestValidateAsync(command);

            if (isValid)
                result.ShouldNotHaveValidationErrorFor(c => c.Email);
            else
                result.ShouldHaveValidationErrorFor(c => c.Email);
        }
    }
}
