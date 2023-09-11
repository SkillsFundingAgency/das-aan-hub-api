using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;
using SFA.DAS.AANHub.Domain.Common;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Application.UnitTests.Common;
public class NotificationHelperTests
{
    [Test, MoqAutoData]
    public void Create_Notification(Guid memberId, string templateName, string tokens, bool isSystem)
    {
        var result = NotificationHelper.CreateNotification(Guid.NewGuid(), memberId, templateName, tokens, memberId, isSystem, null);

        using (new AssertionScope())
        {
            result.Should().BeOfType<Notification>();
            result.MemberId.Should().Be(memberId);
            result.TemplateName.Should().Be(templateName);
            result.Tokens.Should().Be(tokens);
            result.CreatedDate.Minute.Should().Be(DateTime.UtcNow.Minute);
            result.IsSystem.Should().Be(isSystem);
            result.ReferenceId.Should().BeNull();
        };
    }
}
