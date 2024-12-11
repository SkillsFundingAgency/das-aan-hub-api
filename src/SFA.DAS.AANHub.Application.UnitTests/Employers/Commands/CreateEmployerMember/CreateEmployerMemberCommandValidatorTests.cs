using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Application.Employers.Commands.CreateEmployerMember;
using SFA.DAS.AANHub.Domain.Common;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.UnitTests.Employers.Commands.CreateEmployerMember;

public class CreateEmployerMemberCommandValidatorTests
{
    private Mock<IEmployersReadRepository> employersReadRepository = null!;
    private Mock<IProfilesReadRepository> profilesReadRepository = null!;
    private Mock<IRegionsReadRepository> regionsReadRepository = null!;
    private CreateEmployerMemberCommandValidator sut = null!;
    const int ValidRegionId = 1;

    [SetUp]
    public void Init()
    {
        employersReadRepository = new();
        profilesReadRepository = new();
        regionsReadRepository = new();

        regionsReadRepository.Setup(x => x.GetAllRegions(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Region> { new() { Id = ValidRegionId, Area = "test" } });
        profilesReadRepository.Setup(r => r.GetProfilesByUserType(UserType.Employer)).ReturnsAsync(new List<Profile>
        {
            new Profile{ Id = 1 },
            new Profile{ Id = 2 }
        });

        sut = new(employersReadRepository.Object, profilesReadRepository.Object, Mock.Of<IMembersReadRepository>(), regionsReadRepository.Object);
    }

    [TestCase("00000000-0000-0000-0000-000000000000", false)]
    [TestCase("B46C2A2A-E35C-4788-B4B7-1F7E84081846", true)]
    public async Task Validates_UserRef_NotNull(string userRef, bool isValid)
    {
        var guid = Guid.Parse(userRef);
        var command = new CreateEmployerMemberCommand
        {
            UserRef = guid
        };

        var result = await sut.TestValidateAsync(command);

        if (isValid)
            result.ShouldNotHaveValidationErrorFor(c => c.UserRef);
        else
            result.ShouldHaveValidationErrorFor(c => c.UserRef);
    }

    [TestCase(true)]
    [TestCase(false)]
    public async Task Validates_UserRef_Exist(bool userRefAlreadyExist)
    {
        Employer? employer = null;

        if (userRefAlreadyExist) employer = new Employer();
        var command = new CreateEmployerMemberCommand
        {
            UserRef = Guid.NewGuid()
        };

        employersReadRepository.Setup(x => x.GetEmployerByUserRef(It.IsAny<Guid>())).ReturnsAsync(employer);

        var result = await sut.TestValidateAsync(command);

        if (userRefAlreadyExist)
        {
            result.ShouldHaveValidationErrorFor(c => c.UserRef).WithErrorMessage(CreateEmployerMemberCommandValidator.UserRefAlreadyCreatedErrorMessage);
        }
        else
        {
            result.ShouldNotHaveValidationErrorFor(c => c.UserRef);
        }
    }

    [TestCase(123, true)]
    [TestCase(null, false)]
    [TestCase(0, false)]
    public async Task Validates_AccountId(long id, bool isValid)
    {
        var command = new CreateEmployerMemberCommand
        {
            AccountId = id
        };

        var result = await sut.TestValidateAsync(command);

        if (isValid)
            result.ShouldNotHaveValidationErrorFor(c => c.AccountId);
        else
            result.ShouldHaveValidationErrorFor(c => c.AccountId);
    }

    [TestCase(1, true)]
    [TestCase(2, true)]
    [TestCase(50, false)]
    public async Task Validate_ProfileValues(int profileId, bool isValid)
    {
        var command = new CreateEmployerMemberCommand();
        command.ProfileValues.Add(new(profileId, Guid.NewGuid().ToString()));

        var result = await sut.TestValidateAsync(command);

        if (isValid)
        {
            result.ShouldNotHaveValidationErrorFor(c => c.ProfileValues);
        }
        else
        {
            result.ShouldHaveValidationErrorFor(c => c.ProfileValues).WithErrorMessage(CreateEmployerMemberCommandValidator.InvalidProfileIdsErrorMessage);
        }
    }

    [Test]
    public async Task Validate_BaseClassFields()
    {
        CreateEmployerMemberCommand command = new()
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
        var command = new CreateEmployerMemberCommand
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
