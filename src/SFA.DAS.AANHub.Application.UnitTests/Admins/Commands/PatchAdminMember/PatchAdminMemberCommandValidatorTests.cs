using FluentValidation.TestHelper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Admins.Commands.PatchAdminMember;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.UnitTests.Admins.Commands.PatchAdminMember
{
    [TestFixture]
    public class PatchAdminMemberCommandValidatorTests
    {
        private readonly Mock<IMembersReadRepository> _memberReadRepository;
        private readonly Mock<IAdminsReadRepository> _adminsReadRepository;

        public PatchAdminMemberCommandValidatorTests()
        {
            _memberReadRepository = new Mock<IMembersReadRepository>();
            _adminsReadRepository = new Mock<IAdminsReadRepository>();
        }

        private const string Name = "Name";
        private const string Email = "Email";

        [TestCase("replace", true)]
        [TestCase("remove", false)]
        public async Task ValidatePatchDoc_InvalidOperation_InvalidResponse(string operation, bool isValid)
        {
            var memberId = Guid.NewGuid();
            var userName = "userName123";
            var nameValue = "test";

            var command = new PatchAdminMemberCommand
            {
                RequestedByMemberId = memberId,
                UserName = userName,
                PatchDoc = new JsonPatchDocument<Admin>()
            };

            command.PatchDoc = new JsonPatchDocument<Admin>
            {
                Operations =
                {
                    new Operation<Admin>
                    {
                        op = operation,
                        path = Name,
                        value = nameValue
                    }
                }
            };

            var member = new Member
            {
                Status = Domain.Common.Constants.MembershipStatus.Live
            };

            _memberReadRepository.Setup(a => a.GetMember(memberId)).ReturnsAsync(member);
            _adminsReadRepository.Setup(a => a.GetAdminByUserName(userName)).ReturnsAsync(new Admin());

            var sut = new PatchAdminMemberCommandValidator(_memberReadRepository.Object, _adminsReadRepository.Object);

            var result = await sut.TestValidateAsync(command);

            if (isValid)
                result.ShouldNotHaveValidationErrorFor(p => p.PatchDoc.Operations.Count(c => c.op.Equals(operation)));
            else
                result.ShouldHaveValidationErrorFor(p => p.PatchDoc.Operations.Count(c => c.op.Equals(operation)));
        }

        [Test]
        public async Task ValidatePatchDoc_NoOperations_InvalidResponse()
        {
            var memberId = Guid.NewGuid();
            var userName = "userName123";

            var command = new PatchAdminMemberCommand
            {
                RequestedByMemberId = memberId,
                UserName = userName,
                PatchDoc = new JsonPatchDocument<Admin>()
            };

            var member = new Member
            {
                Status = Domain.Common.Constants.MembershipStatus.Live
            };

            _memberReadRepository.Setup(a => a.GetMember(memberId)).ReturnsAsync(member);
            _adminsReadRepository.Setup(a => a.GetAdminByUserName(userName)).ReturnsAsync(new Admin());

            var sut = new PatchAdminMemberCommandValidator(_memberReadRepository.Object, _adminsReadRepository.Object);

            var result = await sut.TestValidateAsync(command);

            result.ShouldHaveValidationErrorFor(c => c.PatchDoc.Operations.Count);
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.Count > 0);
            Assert.AreEqual("There are no patch operations in this request", result.Errors[0].ErrorMessage);
        }

        [TestCase(Name, "nameValue1", "nameValue1")]
        [TestCase(Email, "email1@email.com", "email2@email.com")]
        public async Task ValidatePatchDoc_DuplicateReplaceOperation_InvalidResponse(string patchField, string value1, string value2)
        {
            var memberId = Guid.NewGuid();
            var userName = "userName123";

            var command = new PatchAdminMemberCommand
            {
                RequestedByMemberId = memberId,
                UserName = userName,
                PatchDoc = new JsonPatchDocument<Admin>()
            };

            command.PatchDoc = new JsonPatchDocument<Admin>
            {
                Operations =
                {
                    new Operation<Admin>
                    {
                        op = nameof(OperationType.Replace),
                        path = patchField,
                        value = value1
                    },
                    new Operation<Admin>
                    {
                        op = nameof(OperationType.Replace),
                        path = patchField,
                        value = value2
                    }
                }
            };

            var member = new Member
            {
                Status = Domain.Common.Constants.MembershipStatus.Live
            };

            _memberReadRepository.Setup(a => a.GetMember(memberId)).ReturnsAsync(member);
            _adminsReadRepository.Setup(a => a.GetAdminByUserName(userName)).ReturnsAsync(new Admin());

            var sut = new PatchAdminMemberCommandValidator(_memberReadRepository.Object, _adminsReadRepository.Object);
            var result = await sut.TestValidateAsync(command);

            result.ShouldHaveValidationErrorFor(c => c.PatchDoc.Operations.Count(operation
                => operation.path == Name && operation.op.Equals(nameof(OperationType.Replace), StringComparison.CurrentCultureIgnoreCase)));

            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.Count > 0);
            Assert.AreEqual("There are duplicate patch operations in this request", result.Errors[0].ErrorMessage);
        }
    }
}