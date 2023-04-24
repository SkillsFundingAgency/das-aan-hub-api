using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Admins.Commands.CreateAdminMember;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.UnitTests.Admins.Commands.CreateAdminMember;

public class CreateAdminMemberCommandValidatorTests
{

    [TestCase("userName", true)]
    [TestCase("", false)]
    [TestCase(null, false)]
    public async Task Validates_UserName_NotNullOrEmpty(string? userName, bool isValid)
    {
        Mock<IAdminsReadRepository> adminsReadRepository = new();
        var command = new CreateAdminMemberCommand
        {
            UserName = userName!
        };

        var sut = new CreateAdminMemberCommandValidator(adminsReadRepository.Object);

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
        Mock<IAdminsReadRepository> adminsReadRepository = new();
        var command = new CreateAdminMemberCommand
        {
            UserName = new string('a', length)
        };

        var sut = new CreateAdminMemberCommandValidator(adminsReadRepository.Object);

        var result = await sut.TestValidateAsync(command);

        if (isValid)
            result.ShouldNotHaveValidationErrorFor(c => c.UserName);
        else
            result.ShouldHaveValidationErrorFor(c => c.UserName);
    }

    [Test]
    public async Task Validate_BaseClassFields()
    {
        Mock<IAdminsReadRepository> adminsReadRepository = new();
        CreateAdminMemberCommand command = new()
        {
            Email = "bad email",
            FirstName = string.Empty,
            LastName = string.Empty,
            Joined = DateTime.Today.AddDays(1),
            RegionId = 999,
            OrganisationName = new string('a', 251)
        };
        CreateAdminMemberCommandValidator sut = new(adminsReadRepository.Object);

        var result = await sut.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(c => c.Email);
        result.ShouldHaveValidationErrorFor(c => c.FirstName);
        result.ShouldHaveValidationErrorFor(c => c.LastName);
        result.ShouldHaveValidationErrorFor(c => c.Joined);
        result.ShouldHaveValidationErrorFor(c => c.RegionId);
        result.ShouldHaveValidationErrorFor(c => c.OrganisationName);
    }
}