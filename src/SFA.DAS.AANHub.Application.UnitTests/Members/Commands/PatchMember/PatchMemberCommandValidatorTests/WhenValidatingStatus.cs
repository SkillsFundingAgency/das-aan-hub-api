using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Members.Commands.PatchMember;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.UnitTests.Members.Commands.PatchMember.PatchMemberCommandValidatorTests;

public class WhenValidatingStatus
{
    [Test]
    public async Task AndOperationIsMissingThenAvoidsValidatingField()
    {
        Mock<IMembersReadRepository> repositoryMock = new();
        PatchMemberCommandValidator sut = new(repositoryMock.Object);
        PatchMemberCommand target = new();

        var result = await sut.TestValidateAsync(target);

        result.ShouldNotHaveValidationErrorFor(s => s.Status);
    }

    [TestCase("Deleted", true)]
    [TestCase("Withdrawn", true)]
    [TestCase("Live", false)]
    public async Task ThenOnlySpecificValuesAreAllowed(string value, bool isValid)
    {
        Mock<IMembersReadRepository> repositoryMock = new();
        PatchMemberCommandValidator sut = new(repositoryMock.Object);
        PatchMemberCommand target = new();
        target.PatchDoc.Replace(c => c.Status, value);

        var result = await sut.TestValidateAsync(target);

        if (isValid)
        {
            result.ShouldNotHaveValidationErrorFor(s => s.Status);
        }
        else
        {
            result.ShouldHaveValidationErrorFor(s => s.Status);
        }
    }
}
