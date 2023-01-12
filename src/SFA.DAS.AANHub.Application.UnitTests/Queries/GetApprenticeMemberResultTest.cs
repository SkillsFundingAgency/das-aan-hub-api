using Azure;
using FluentAssertions;
using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Queries.GetApprentice;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.UnitTests.Queries
{
    [TestFixture]
    public class GetApprenticeMemberResultTest
    {
        [Test, AutoMoqData]
        public void Apprentice_PopulatesGetApprenticeMemberResultFromApprentice(Apprentice apprentice)
        {
            var response = (GetApprenticeMemberResult?)apprentice;

            Assert.That(response, Is.Not.Null);

            Assert.AreEqual(apprentice.MemberId, response!.MemberId);
            Assert.AreEqual(apprentice.Name, response.Name);
            Assert.AreEqual(apprentice.Email, response.Email);

        }

        [Test, AutoMoqData]
        public void Apprentice_GetApprenticeMemberResultTest_ApprenticeIsNull()
        {
            Apprentice? apprentice = null;
            var response = (GetApprenticeMemberResult?)apprentice!;
            Assert.That(response, Is.Null);
        }
    }
}
