using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Apprentices.Commands.CreateApprenticeMember;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.UnitTests.Apprentices.Commands;

public class CreateApprenticeMemberCommandValidatorTests
{
    [TestCase("684b9829-d882-4733-938e-bcee6d6bfe81", true)]
    [TestCase(null, false)]
    [TestCase("00000000-0000-0000-0000-000000000000", false)]
    public async Task Validates_ApprenticeId_NotNull(Guid apprenticeId, bool isValid)
    {
        Mock<IApprenticesReadRepository> apprenticesReadRepository = new();
        var command = new CreateApprenticeMemberCommand
        {
            ApprenticeId = apprenticeId
        };

        var sut = new CreateApprenticeMemberCommandValidator(
            apprenticesReadRepository.Object);

        var result = await sut.TestValidateAsync(command);

        if (isValid)
            result.ShouldNotHaveValidationErrorFor(c => c.ApprenticeId);
        else
            result.ShouldHaveValidationErrorFor(c => c.ApprenticeId);
    }

    [Test]
    public async Task Validate_BaseClassFields()
    {
        Mock<IApprenticesReadRepository> apprenticesReadRepository = new();
        CreateApprenticeMemberCommand command = new()
        {
            Email = "bad email",
            FirstName = string.Empty,
            LastName = string.Empty,
            Joined = DateTime.Today.AddDays(1),
            RegionId = 999,
            OrganisationName = new string('a', 251)
        };
        CreateApprenticeMemberCommandValidator sut = new(apprenticesReadRepository.Object);

        var result = await sut.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(c => c.Email);
        result.ShouldHaveValidationErrorFor(c => c.FirstName);
        result.ShouldHaveValidationErrorFor(c => c.LastName);
        result.ShouldHaveValidationErrorFor(c => c.Joined);
        result.ShouldHaveValidationErrorFor(c => c.RegionId);
        result.ShouldHaveValidationErrorFor(c => c.OrganisationName);
    }
}