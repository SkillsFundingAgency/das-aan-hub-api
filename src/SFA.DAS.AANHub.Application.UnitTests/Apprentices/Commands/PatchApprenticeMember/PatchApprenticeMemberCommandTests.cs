using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Apprentices.Commands.PatchApprenticeMember;
using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Application.UnitTests.Apprentices.Commands.PatchApprenticeMember
{
    [TestFixture]
    public class PatchApprenticeMemberCommandTests
    {
        private const string Email = "Email";
        private const string Name = "Name";

        [Test]
        public void Command_PatchContainsEmail_EmailIsSet()
        {
            var memberId = Guid.NewGuid();
            var apprenticeId = 123;

            var testValue = "value";
            var patchDoc = new JsonPatchDocument<Apprentice>();
            patchDoc.Operations.Add(new Operation<Apprentice> { op = nameof(OperationType.Replace), path = Email, value = testValue });

            var command = new PatchApprenticeMemberCommand
            {
                RequestedByMemberId = memberId,
                ApprenticeId = apprenticeId,
                Patchdoc = patchDoc
            };

            Assert.AreEqual(testValue, command.Email);
            Assert.IsTrue(command.IsPresentEmail);
            Assert.IsFalse(command.IsPresentName);
            Assert.AreEqual(testValue, patchDoc.Operations.FirstOrDefault(operation =>
                operation.path == Email && operation.op.Equals(nameof(OperationType.Replace), StringComparison.CurrentCultureIgnoreCase))?.value.ToString());
        }

        [Test]
        public void Command_PatchContainsName_NameIsSet()
        {
            var memberId = Guid.NewGuid();
            var apprenticeId = 123;

            var testValue = "value";
            var patchDoc = new JsonPatchDocument<Apprentice>();
            patchDoc.Operations.Add(new Operation<Apprentice> { op = nameof(OperationType.Replace), path = Name, value = testValue });

            var command = new PatchApprenticeMemberCommand
            {
                RequestedByMemberId = memberId,
                ApprenticeId = apprenticeId,
                Patchdoc = patchDoc
            };

            Assert.AreEqual(testValue, command.Name);
            Assert.IsTrue(command.IsPresentName);
            Assert.IsFalse(command.IsPresentEmail);
            Assert.AreEqual(testValue, patchDoc.Operations.FirstOrDefault(operation =>
                operation.path == Name && operation.op.Equals(nameof(OperationType.Replace), StringComparison.CurrentCultureIgnoreCase))?.value.ToString());
        }

        [Test]
        public void Command_PatchContainsNoDetails_FieldsAreNotSet()
        {
            var memberId = Guid.NewGuid();
            var apprenticeId = 123;
            var patchDoc = new JsonPatchDocument<Apprentice>();

            var command = new PatchApprenticeMemberCommand
            {
                RequestedByMemberId = memberId,
                ApprenticeId = apprenticeId,
                Patchdoc = patchDoc
            };

            Assert.AreEqual(null, command.Email);
            Assert.IsFalse(command.IsPresentEmail);
            Assert.AreEqual(null, command.Name);
            Assert.IsFalse(command.IsPresentName);
        }
    }
}