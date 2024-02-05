using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.UnitTests.Common.CreateMemberCommandBaseValidatorTests;

public class WhenValidatingRegionId
{
    [TestCase(null, true)]
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
    public async Task ThenOnlySpecificValuesAreAllowed(int? value, bool isValid)
    {
        CreateMemberCommandBaseValidator sut = new(Mock.Of<IMembersReadRepository>());
        TestTarget target = new()
        {
            RegionId = value
        };

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
