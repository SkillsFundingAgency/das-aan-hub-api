using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Members.Commands.PatchMember;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.UnitTests.Members.Commands.PatchMember.PatchMemberCommandValidatorTests;

public class WhenValidatingRegionId
{
    [Test]
    public async Task AndOperationIsMissingThenAvoidsValidatingField()
    {
        Mock<IMembersReadRepository> repositoryMock = new();
        PatchMemberCommandValidator sut = new(repositoryMock.Object);
        PatchMemberCommand target = new();

        var result = await sut.TestValidateAsync(target);

        result.ShouldNotHaveValidationErrorFor(s => s.RegionId);
    }

    [TestCase(0, false)]
    [TestCase(1, true)]
    [TestCase(2, true)]
    [TestCase(3, true)]
    [TestCase(4, true)]
    [TestCase(5, true)]
    [TestCase(6, true)]
    [TestCase(7, true)]
    [TestCase(8, true)]
    [TestCase(9, true)]
    [TestCase(10, false)]
    public async Task ThenOnlySpecificValuesAreAllowed(int value, bool isValid)
    {
        Mock<IMembersReadRepository> repositoryMock = new();
        PatchMemberCommandValidator sut = new(repositoryMock.Object);
        PatchMemberCommand target = new();
        target.PatchDoc.Replace(c => c.RegionId, value);

        var result = await sut.TestValidateAsync(target);

        if (isValid)
        {
            result.ShouldNotHaveValidationErrorFor(s => s.RegionId);
        }
        else
        {
            result.ShouldHaveValidationErrorFor(s => s.RegionId);
        }
    }
}
