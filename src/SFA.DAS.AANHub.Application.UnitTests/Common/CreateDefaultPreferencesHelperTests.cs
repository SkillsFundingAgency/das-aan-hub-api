using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Application.UnitTests.Common;
public class CreateDefaultPreferencesHelperTests
{

    [Test, MoqAutoData]
    public void Create_DefaultPreferences(Guid memberId, [Frozen] Mock<IMemberPreferenceWriteRepository> memberPreferenceWriteRepository)
    {
        var sut = new CreateDefaultPreferencesHelper(memberPreferenceWriteRepository.Object);
        sut.CreateDefaultPreferences(memberId);

        memberPreferenceWriteRepository.Verify(m => m.Create(It.Is<MemberPreference>(x => x.MemberId == memberId)), Times.Exactly(4));
    }
}
