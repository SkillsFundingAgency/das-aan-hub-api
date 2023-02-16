using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Partners.Commands.PatchPartnerMember;
using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Application.UnitTests.Partners.Commands.PatchPartnerMember
{
    [TestFixture]
    public class PatchPartnerMemberCommandTests
    {
        private const string Email = "Email";
        private const string Name = "Name";
        private const string Organisation = "Organisation";

        [Test]
        public void Command_PatchContainsEmail_EmailIsSet()
        {
            var memberId = Guid.NewGuid();
            var userName = "username";

            var testValue = "value";
            var patchDoc = new JsonPatchDocument<Partner>();
            patchDoc.Operations.Add(new Operation<Partner>
            {
                op = nameof(OperationType.Replace),
                path = Email,
                value = testValue
            });

            var command = new PatchPartnerMemberCommand
            {
                RequestedByMemberId = memberId,
                UserName = userName,
                PatchDoc = patchDoc
            };

            Assert.AreEqual(testValue, command.Email);
            Assert.IsTrue(command.IsPresentEmail);
            Assert.IsFalse(command.IsPresentName);
            Assert.IsFalse(command.IsPresentOrganisation);
            Assert.AreEqual(testValue, patchDoc.Operations.FirstOrDefault(operation =>
                            operation.path == Email && operation.op.Equals(nameof(OperationType.Replace), StringComparison.CurrentCultureIgnoreCase))?.value.ToString());
        }

        [Test]
        public void Command_PatchContainsName_NameIsSet()
        {
            var memberId = Guid.NewGuid();
            var userName = "username";

            var testValue = "value";
            var patchDoc = new JsonPatchDocument<Partner>();
            patchDoc.Operations.Add(new Operation<Partner>
            {
                op = nameof(OperationType.Replace),
                path = Name,
                value = testValue
            });

            var command = new PatchPartnerMemberCommand
            {
                RequestedByMemberId = memberId,
                UserName = userName,
                PatchDoc = patchDoc
            };

            Assert.AreEqual(testValue, command.Name);
            Assert.IsTrue(command.IsPresentName);
            Assert.IsFalse(command.IsPresentEmail);
            Assert.IsFalse(command.IsPresentOrganisation);
            Assert.AreEqual(testValue, patchDoc.Operations.FirstOrDefault(operation =>
                            operation.path == Name && operation.op.Equals(nameof(OperationType.Replace), StringComparison.CurrentCultureIgnoreCase))?.value.ToString());

        }

        [Test]
        public void Command_PatchContainsOrganisation_OrganisationIsSet()
        {
            var memberId = Guid.NewGuid();
            var userName = "username";

            var testValue = "value";
            var patchDoc = new JsonPatchDocument<Partner>();
            patchDoc.Operations.Add(new Operation<Partner>
            {
                op = nameof(OperationType.Replace),
                path = Organisation,
                value = testValue
            });

            var command = new PatchPartnerMemberCommand
            {
                RequestedByMemberId = memberId,
                UserName = userName,
                PatchDoc = patchDoc
            };

            Assert.AreEqual(testValue, command.Organisation);
            Assert.IsTrue(command.IsPresentOrganisation);
            Assert.IsFalse(command.IsPresentEmail);
            Assert.IsFalse(command.IsPresentName);
            Assert.AreEqual(testValue, patchDoc.Operations.FirstOrDefault(operation =>
                            operation.path == Organisation && operation.op.Equals(nameof(OperationType.Replace), StringComparison.CurrentCultureIgnoreCase))?.value.ToString());

        }

        [Test]
        public void Command_PatchContainsNoDetails_FieldsAreNotSet()
        {
            var memberId = Guid.NewGuid();
            var userName = "username";
            var patchDoc = new JsonPatchDocument<Partner>();

            var command = new PatchPartnerMemberCommand
            {
                RequestedByMemberId = memberId,
                UserName = userName,
                PatchDoc = patchDoc
            };

            Assert.AreEqual(null, command.Email);
            Assert.IsFalse(command.IsPresentEmail);
            Assert.AreEqual(null, command.Name);
            Assert.IsFalse(command.IsPresentOrganisation);
            Assert.AreEqual(null, command.Organisation);
            Assert.IsFalse(command.IsPresentName);
        }
    }
}
