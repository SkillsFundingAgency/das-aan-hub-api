﻿using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.UnitTests.Common.CreateMemberCommandBaseValidatorTests;

public class WhenValidatingFirstName
{
    [TestCase(null, false, null)]
    [TestCase(0, false, null)]
    [TestCase(1, true, null)]
    [TestCase(200, true, null)]
    [TestCase(201, false, CreateMemberCommandBaseValidator.ExceededAllowableLengthErrorMessage)]
    public async Task ThenShouldHaveValidValue(int? stringLength, bool isValid, string? errorMessage)
    {
        var validRegionId = 1;

        var regionsReadRepositoryMock = new Mock<IRegionsReadRepository>();

        regionsReadRepositoryMock.Setup(x => x.GetAllRegions(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Region> { new() { Id = validRegionId, Area = "test" } });

        CreateMemberCommandBaseValidator sut = new(Mock.Of<IMembersReadRepository>(), regionsReadRepositoryMock.Object);
        TestTarget target = new();

        string? value = stringLength switch
        {
            null => null,
            0 => string.Empty,
            _ => new string('q', stringLength.Value)
        };
        target.FirstName = value;
        target.RegionId = validRegionId;

        var result = await sut.TestValidateAsync(target);

        if (isValid)
        {
            result.ShouldNotHaveValidationErrorFor(s => s.FirstName);
        }
        else
        {
            result
                .ShouldHaveValidationErrorFor(s => s.FirstName)
                .WithErrorMessage(
                    string.IsNullOrWhiteSpace(errorMessage)
                    ? string.Format(CreateMemberCommandBaseValidator.ValueIsRequiredErrorMessage, nameof(TestTarget.FirstName))
                    : string.Format(CreateMemberCommandBaseValidator.ExceededAllowableLengthErrorMessage, nameof(TestTarget.FirstName), 200));
        }
    }
}
