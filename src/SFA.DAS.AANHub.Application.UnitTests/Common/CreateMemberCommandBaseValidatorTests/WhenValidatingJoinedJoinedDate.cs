using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Common;

namespace SFA.DAS.AANHub.Application.UnitTests.Common.CreateMemberCommandBaseValidatorTests;

public class WhenValidatingJoinedJoinedDate
{
    [Test]
    public async Task ThenFutureDatesAreInvalid()
    {
        CreateMemberCommandBaseValidator sut = new();
        TestTarget target = new()
        {
            JoinedDate = DateTime.Today.AddDays(1)
        };

        var result = await sut.TestValidateAsync(target);

        result.ShouldHaveValidationErrorFor(c => c.JoinedDate);
    }

    [Test]
    public async Task ThenTodaysDateIsValid()
    {
        CreateMemberCommandBaseValidator sut = new();
        TestTarget target = new()
        {
            JoinedDate = DateTime.Today
        };

        var result = await sut.TestValidateAsync(target);

        result.ShouldNotHaveValidationErrorFor(c => c.JoinedDate);
    }

    [Test]
    public async Task ThenPastTodaysDatesAreValid()
    {
        CreateMemberCommandBaseValidator sut = new();
        TestTarget target = new()
        {
            JoinedDate = DateTime.Today.AddDays(-1)
        };

        var result = await sut.TestValidateAsync(target);

        result.ShouldNotHaveValidationErrorFor(c => c.JoinedDate);
    }
}
