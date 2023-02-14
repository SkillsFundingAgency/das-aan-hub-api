using NUnit.Framework;
using SFA.DAS.AANHub.Application.Employers.Queries;
using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Application.UnitTests.Employers.Queries
{
    [TestFixture]
    public class GetEmployerMemberResultTest
    {
        [Test, AutoMoqData]
        public void Employer_PopulatesGetEmployerMemberResultFromEmployer(Employer employer)
        {
            var response = (GetEmployerMemberResult?)employer;

            Assert.That(response, Is.Not.Null);

            Assert.AreEqual(employer.MemberId, response!.MemberId);
            Assert.AreEqual(employer.Name, response.Name);
            Assert.AreEqual(employer.Email, response.Email);
            Assert.AreEqual(employer.Organisation, response.Organisation);
            Assert.AreEqual(employer.Member.Status, response.Status);

        }

        [Test, AutoMoqData]
        public void Employer_GetEmployerMemberResultTest_EmployerIsNull()
        {
            Employer? employer = null;
            var response = (GetEmployerMemberResult?)employer!;
            Assert.That(response, Is.Null);
        }
    }
}
