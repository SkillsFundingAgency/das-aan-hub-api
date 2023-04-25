using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Apprentices.Commands.CreateApprenticeMember;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using static SFA.DAS.AANHub.Domain.Common.Constants;

namespace SFA.DAS.AANHub.Application.UnitTests.Apprentices.Commands;

public class CreateApprenticeMemberCommandValidatorTests
{
    private Mock<IApprenticesReadRepository> apprenticesReadRepository = null!;
    private Mock<IProfilesReadRepository> profilesReadRepository = null!;
    CreateApprenticeMemberCommandValidator sut = null!;

    [SetUp]
    public void Init()
    {
        apprenticesReadRepository = new();

        profilesReadRepository = new();
        profilesReadRepository.Setup(r => r.GetProfilesByUserType(MembershipUserType.Apprentice)).ReturnsAsync(new List<Profile>
        {
            new Profile{ Id = 1 },
            new Profile{ Id = 2 }
        });

        sut = new(apprenticesReadRepository.Object, profilesReadRepository.Object);
    }

    [TestCase("684b9829-d882-4733-938e-bcee6d6bfe81", true)]
    [TestCase(null, false)]
    [TestCase("00000000-0000-0000-0000-000000000000", false)]
    public async Task Validates_ApprenticeId_NotNull(Guid apprenticeId, bool isValid)
    {
        var command = new CreateApprenticeMemberCommand
        {
            ApprenticeId = apprenticeId
        };

        var result = await sut.TestValidateAsync(command);

        if (isValid)
            result.ShouldNotHaveValidationErrorFor(c => c.ApprenticeId);
        else
            result.ShouldHaveValidationErrorFor(c => c.ApprenticeId);
    }

    [TestCase(1, true)]
    [TestCase(2, true)]
    [TestCase(50, false)]
    public async Task Validate_ProfileValues(int profileId, bool isValid)
    {
        CreateApprenticeMemberCommand command = new();
        command.ProfileValues.Add(new(profileId, Guid.NewGuid().ToString()));

        var result = await sut.TestValidateAsync(command);

        if (isValid)
        {
            result.ShouldNotHaveValidationErrorFor(c => c.ProfileValues);
        }
        else
        {
            result.ShouldHaveValidationErrorFor(c => c.ProfileValues).WithErrorMessage(CreateApprenticeMemberCommandValidator.InvalidProfileIdsErrorMessage);
        }
    }

    [Test]
    public async Task Validate_BaseClassFields()
    {
        CreateApprenticeMemberCommand command = new()
        {
            Email = "bad email",
            FirstName = string.Empty,
            LastName = string.Empty,
            JoinedDate = DateTime.Today.AddDays(1),
            RegionId = 999,
            OrganisationName = new string('a', 251)
        };

        var result = await sut.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(c => c.Email);
        result.ShouldHaveValidationErrorFor(c => c.FirstName);
        result.ShouldHaveValidationErrorFor(c => c.LastName);
        result.ShouldHaveValidationErrorFor(c => c.JoinedDate);
        result.ShouldHaveValidationErrorFor(c => c.RegionId);
        result.ShouldHaveValidationErrorFor(c => c.OrganisationName);
    }
}