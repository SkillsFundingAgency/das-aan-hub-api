using NUnit.Framework;
using SFA.DAS.AANHub.Application.Models;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Application.UnitTests.Models;

public class EventGuestModelTests
{
    [Test]
    [RecursiveMoqAutoData]
    public void EventGuest_IsConvertedTo_EventGuestModel_WithPropertiesFromEventGuest(EventGuest source)
    {
        EventGuestModel sut = source;

        Assert.Multiple(() =>
        {
            Assert.That(sut.GuestName, Is.EqualTo(source.GuestName));
            Assert.That(sut.GuestJobTitle, Is.EqualTo(source.GuestJobTitle));
        });
    }
}
