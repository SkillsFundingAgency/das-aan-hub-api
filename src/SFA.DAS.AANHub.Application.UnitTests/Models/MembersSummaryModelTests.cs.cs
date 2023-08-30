using NUnit.Framework;
using SFA.DAS.AANHub.Application.Members.Queries.GetMembers;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Application.UnitTests.Models;
public class MembersSummaryModelTests
{
    [Test, RecursiveMoqAutoData]
    public void MembersSummary_IsConvertedTo_MembersSummaryModel_WithPropertiesFromMembersSummary(MembersSummary source)
    {
        MembersSummaryModel sut = source;

        Assert.Multiple(() =>
        {
            Assert.That(sut.MemberId, Is.EqualTo(source.MemberId));
            Assert.That(sut.FullName, Is.EqualTo(source.FullName));
            Assert.That(sut.RegionId, Is.EqualTo(source.RegionId));
            Assert.That(sut.RegionName, Is.EqualTo(source.RegionName));
            Assert.That(sut.UserType, Is.EqualTo(source.UserType));
            Assert.That(sut.IsRegionalChair, Is.EqualTo(source.IsRegionalChair));
            Assert.That(sut.JoinedDate, Is.EqualTo(source.JoinedDate));
        });
    }
}