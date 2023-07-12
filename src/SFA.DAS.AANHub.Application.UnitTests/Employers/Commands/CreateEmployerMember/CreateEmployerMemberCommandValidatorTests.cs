using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Employers.Commands.CreateEmployerMember;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using static SFA.DAS.AANHub.Domain.Common.Constants;

namespace SFA.DAS.AANHub.Application.UnitTests.Employers.Commands.CreateEmployerMember;

public class CreateEmployerMemberCommandValidatorTests
{
    private Mock<IEmployersReadRepository> employersReadRepository = null!;
    private Mock<IProfilesReadRepository> profilesReadRepository = null!;
    CreateEmployerMemberCommandValidator sut = null!;

    [SetUp]
    public void Init()
    {
        employersReadRepository = new();
        profilesReadRepository = new();

        profilesReadRepository.Setup(r => r.GetProfilesByUserType(MembershipUserType.Employer)).ReturnsAsync(new List<Profile>
        {
            new Profile{ Id = 1 },
            new Profile{ Id = 2 }
        });

        sut = new(employersReadRepository.Object, profilesReadRepository.Object);
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
            result.ShouldHaveValidationErrorFor(c => c.UserRef).WithErrorMessage("UserRef already exists");
        }
        else
        {
            result.ShouldNotHaveValidationErrorFor(c => c.UserRef);
        }
    }

    [TestCase(123, true)]
    [TestCase(null, false)]
    [TestCase(0, false)]
    public async Task Validates_AccountId_NotNull(long id, bool isValid)
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
}
