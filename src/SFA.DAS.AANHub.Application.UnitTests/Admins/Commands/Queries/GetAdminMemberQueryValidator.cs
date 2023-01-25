using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Admins.Queries;

namespace SFA.DAS.AANHub.Application.UnitTests.Admins.Commands.Queries
{
    public class GetAdminMemberQueryValidatorTests
    {
        [TestCase("userName", true)]
        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase(" ", false)]
        public async Task Validates_Admin_UserName_NotNull_Not_Empty(string userName, bool isValid)
        {
            var query = new GetAdminMemberQuery(userName);
            var sut = new GetAdminMemberQueryValidator();
            var result = await sut.TestValidateAsync(query);

            if (isValid)
                result.ShouldNotHaveValidationErrorFor(c => c.UserName);
            else
                result.ShouldHaveValidationErrorFor(c => c.UserName);
        }

        [TestCase(5, true)]
        [TestCase(201, false)]
        public async Task Validates_Admin_UserName_Length(int length, bool isValid)
        {
            var query = new GetAdminMemberQuery(new string('c', length));
            var sut = new GetAdminMemberQueryValidator();
            var result = await sut.TestValidateAsync(query);

            if (isValid)
                result.ShouldNotHaveValidationErrorFor(c => c.UserName);
            else
                result.ShouldHaveValidationErrorFor(c => c.UserName);
        }
    }
}