using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Admins.Commands.CreateAdminMember;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.UnitTests.Admins.Commands.CreateAdminMember;

public class CreateAdminMemberCommandValidatorTests
{
    [Test]
    public async Task Validate_BaseClassFields()
    {
        Mock<IMembersReadRepository> membersReadRepository = new();
        CreateAdminMemberCommand command = new()
        {
            Email = "bad email",
            FirstName = string.Empty,
            LastName = string.Empty,
            JoinedDate = DateTime.Today.AddDays(1),
            RegionId = 999,
            OrganisationName = new string('a', 251)
        };
        CreateAdminMemberCommandValidator sut = new(membersReadRepository.Object);

        var result = await sut.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(c => c.Email);
        result.ShouldHaveValidationErrorFor(c => c.FirstName);
        result.ShouldHaveValidationErrorFor(c => c.LastName);
        result.ShouldHaveValidationErrorFor(c => c.JoinedDate);
        result.ShouldHaveValidationErrorFor(c => c.RegionId);
        result.ShouldHaveValidationErrorFor(c => c.OrganisationName);
    }
}