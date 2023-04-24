using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Partners.Commands.CreatePartnerMember;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.UnitTests.Partners.Commands.CreatePartnerMember;

public class CreatePartnerMemberCommandValidatorTests
{
    [TestCase("userName", true)]
    [TestCase("", false)]
    [TestCase(null, false)]
    [TestCase(" ", false)]
    public async Task Validates_UserName_NotNullOrEmpty(string? userName, bool isValid)
    {
        Mock<IPartnersReadRepository> partnersReadRepository = new();
        var command = new CreatePartnerMemberCommand
        {
            UserName = userName!
        };

        var sut = new CreatePartnerMemberCommandValidator(partnersReadRepository.Object);

        var result = await sut.TestValidateAsync(command);

        if (isValid)
            result.ShouldNotHaveValidationErrorFor(c => c.UserName);
        else
            result.ShouldHaveValidationErrorFor(c => c.UserName);
    }

    [TestCase(5, true)]
    [TestCase(251, false)]
    public async Task Validates_UserName_Length(int length, bool isValid)
    {
        Mock<IPartnersReadRepository> partnersReadRepository = new();
        var command = new CreatePartnerMemberCommand
        {
            UserName = new string('a', length)
        };

        var sut = new CreatePartnerMemberCommandValidator(partnersReadRepository.Object);
        var result = await sut.TestValidateAsync(command);

        if (isValid)
            result.ShouldNotHaveValidationErrorFor(c => c.UserName);
        else
            result.ShouldHaveValidationErrorFor(c => c.UserName);
    }

    [Test]
    public async Task Validate_BaseClassFields()
    {
        Mock<IPartnersReadRepository> partnersReadRepository = new();
        CreatePartnerMemberCommand command = new()
        {
            Email = "bad email",
            FirstName = string.Empty,
            LastName = string.Empty,
            JoinedDate = DateTime.Today.AddDays(1),
            RegionId = 999,
            OrganisationName = new string('a', 251)
        };
        CreatePartnerMemberCommandValidator sut = new(partnersReadRepository.Object);

        var result = await sut.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(c => c.Email);
        result.ShouldHaveValidationErrorFor(c => c.FirstName);
        result.ShouldHaveValidationErrorFor(c => c.LastName);
        result.ShouldHaveValidationErrorFor(c => c.JoinedDate);
        result.ShouldHaveValidationErrorFor(c => c.RegionId);
        result.ShouldHaveValidationErrorFor(c => c.OrganisationName);
    }
}