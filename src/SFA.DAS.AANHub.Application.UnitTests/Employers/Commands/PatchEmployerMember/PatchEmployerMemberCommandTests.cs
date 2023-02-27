using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Employers.Commands.PatchEmployerMember;
using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Application.UnitTests.Employers.Commands.PatchEmployerMember
{
    [TestFixture]
    public class PatchEmployerMemberCommandTests
    {
        private const string Email = "Email";
        private const string Name = "Name";

        [Test]
        public void Command_PatchContainsEmail_EmailIsSet()
        {
            var memberId = Guid.NewGuid();
            var userRef = Guid.NewGuid();

            const string testValue = "value";
            var patchDoc = new JsonPatchDocument<Employer>();
            patchDoc.Operations.Add(new Operation<Employer> { op = nameof(OperationType.Replace), path = Email, value = testValue });

            var command = new PatchEmployerMemberCommand
            {
                RequestedByMemberId = memberId,
                UserRef = userRef,
                PatchDoc = patchDoc
            };

            Assert.AreEqual(testValue, command.Email);
            Assert.IsTrue(command.IsPresentEmail);
            Assert.IsFalse(command.IsPresentName);
        }

        [Test]
        public void Command_PatchContainsName_NameIsSet()
        {
            var memberId = Guid.NewGuid();
            var userRef = Guid.NewGuid();

            const string testValue = "value";
            var patchDoc = new JsonPatchDocument<Employer>();
            patchDoc.Operations.Add(new Operation<Employer> { op = nameof(OperationType.Replace), path = Name, value = testValue });

            var command = new PatchEmployerMemberCommand
            {
                RequestedByMemberId = memberId,
                UserRef = userRef,
                PatchDoc = patchDoc
            };

            Assert.AreEqual(testValue, command.Name);
            Assert.IsTrue(command.IsPresentName);
            Assert.IsFalse(command.IsPresentEmail);
        }

        [Test]
        public void Command_PatchContainsNoDetails_FieldsAreNotSet()
        {
            var memberId = Guid.NewGuid();
            var userRef = Guid.NewGuid();
            var patchDoc = new JsonPatchDocument<Employer>();

            var command = new PatchEmployerMemberCommand
            {
                RequestedByMemberId = memberId,
                UserRef = userRef,
                PatchDoc = patchDoc
            };

            Assert.AreEqual(null, command.Email);
            Assert.IsFalse(command.IsPresentEmail);
            Assert.AreEqual(null, command.Name);
            Assert.IsFalse(command.IsPresentName);
        }
    }
}
