using NUnit.Framework;
using SFA.DAS.AANHub.Domain.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Application.UnitTests.Domain.Models;
public class GetMemberOptionsTests
{
    [Test, RecursiveMoqAutoData]
    public void GetMembersOptions_Keyword_HasValues_ResultsIn_KeywordCount(GetMembersOptions sut)
    {
        sut.Keyword = "0 1 2";

        Assert.That(sut.KeywordCount, Is.EqualTo(3));
    }

    [Test, RecursiveMoqAutoData]
    public void GetMembersOptions_Keyword_HasNoValues_ResultsIn_KeywordCount(GetMembersOptions sut)
    {
        sut.Keyword = null;

        Assert.That(sut.KeywordCount, Is.EqualTo(0));
    }

    [Test, RecursiveMoqAutoData]
    public void GetMembersOptions_Keyword_HasSpace_ResultsIn_KeywordCount(GetMembersOptions sut)
    {
        sut.Keyword = " ";

        Assert.That(sut.KeywordCount, Is.EqualTo(0));
    }
}