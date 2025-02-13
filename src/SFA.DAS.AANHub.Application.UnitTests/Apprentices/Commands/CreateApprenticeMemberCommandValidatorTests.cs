using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Apprentices.Commands.CreateApprenticeMember;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Domain.Common;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.UnitTests.Apprentices.Commands;

public class CreateApprenticeMemberCommandValidatorTests
{
    private Mock<IApprenticesReadRepository> apprenticesReadRepository = null!;
    private Mock<IProfilesReadRepository> profilesReadRepository = null!;
    private Mock<IRegionsReadRepository> regionsReadRepository = null!;
    CreateApprenticeMemberCommandValidator sut = null!;

    [SetUp]
    public void Init()
    {
        apprenticesReadRepository = new();

        profilesReadRepository = new();
        profilesReadRepository.Setup(r => r.GetProfilesByUserType(UserType.Apprentice)).ReturnsAsync(new List<Profile>
        {
            new Profile{ Id = 1 },
            new Profile{ Id = 2 }
        });

        regionsReadRepository = new();
        regionsReadRepository.Setup(r => r.GetAllRegions(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Region>());
        sut = new(apprenticesReadRepository.Object, profilesReadRepository.Object, Mock.Of<IMembersReadRepository>(), regionsReadRepository.Object);
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
    [TestCase("InPerson", 1, "Test Location", 10, true)]
    [TestCase("InPerson", 1, null, 0, false)]
    [TestCase("Online", 2, null, 0, true)]
    [TestCase("Online", 2, "Test Location", 10, true)]
    public async Task Validate_MemberNotificationEventFormatAndLocationValues(
        string eventFormat, int ordering, string name, int radius, bool isValid)
    {
        var command = new CreateApprenticeMemberCommand
        {
            ReceiveNotifications = true,
            MemberNotificationEventFormatValues = new List<MemberNotificationEventFormatValues>
            {
                new MemberNotificationEventFormatValues
                {
                    EventFormat = eventFormat,
                    Ordering = ordering,
                    ReceiveNotifications = true
                }
            },
            MemberNotificationLocationValues = string.IsNullOrEmpty(name)
                ? new List<MemberNotificationLocationValues>()
                : new List<MemberNotificationLocationValues>
                {
                    new MemberNotificationLocationValues
                    {
                        Name = name,
                        Radius = radius,
                        Latitude = 51.5074,
                        Longitude = -0.1278
                    }
                }
        };

        var result = await sut.TestValidateAsync(command);

        if (isValid)
        {
            result.ShouldNotHaveValidationErrorFor(c => c.MemberNotificationEventFormatValues);
            result.ShouldNotHaveValidationErrorFor(c => c.MemberNotificationLocationValues);
        }
        else
        {
            if (eventFormat != null && !string.Equals(eventFormat, "Online", StringComparison.OrdinalIgnoreCase) && string.IsNullOrEmpty(name))
            {
                result.ShouldHaveValidationErrorFor(c => c.MemberNotificationLocationValues);
            }
        }
    }
}