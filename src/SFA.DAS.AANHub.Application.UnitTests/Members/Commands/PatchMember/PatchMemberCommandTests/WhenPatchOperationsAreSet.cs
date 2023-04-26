using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Members.Commands.PatchMember;

namespace SFA.DAS.AANHub.Application.UnitTests.Members.Commands.PatchMember.PatchMemberCommandTests;

public class WhenPatchOperationsAreSet
{
    private const string Email = "abc@gmail.com";
    private const string FirstName = nameof(FirstName);
    private const string LastName = nameof(LastName);
    private const string OrganisationName = nameof(OrganisationName);
    private const int RegionId = 1;
    private const string Status = "Withdrawn";
    private PatchMemberCommand _sut = null!;

    [SetUp]
    public void Init()
    {
        _sut = new();
        _sut.PatchDoc.Replace(c => c.Email, Email);
        _sut.PatchDoc.Replace(c => c.FirstName, FirstName);
        _sut.PatchDoc.Replace(c => c.LastName, LastName);
        _sut.PatchDoc.Replace(c => c.OrganisationName, OrganisationName);
        _sut.PatchDoc.Replace(c => c.RegionId, RegionId);
        _sut.PatchDoc.Replace(c => c.Status, Status);
    }

    [Test]
    public void HasEmailOperation()
    {
        _sut.Email.Should().Be(Email);
        _sut.HasEmail.Should().BeTrue();
    }

    [Test]
    public void HasFirstNameOperation()
    {
        _sut.FirstName.Should().Be(FirstName);
        _sut.HasFirstName.Should().BeTrue();
    }

    [Test]
    public void HasLastNameOperation()
    {
        _sut.LastName.Should().Be(LastName);
        _sut.HasLastName.Should().BeTrue();
    }

    [Test]
    public void HasOrganisationNameOperation()
    {
        _sut.OrganisationName.Should().Be(OrganisationName);
        _sut.HasOrganisationName.Should().BeTrue();
    }

    [Test]
    public void HasRegionIdOperation()
    {
        _sut.RegionId.Should().Be(RegionId);
        _sut.HasRegionId.Should().BeTrue();
    }

    [Test]
    public void HasStatusOperation()
    {
        _sut.Status.Should().Be(Status);
        _sut.HasStatus.Should().BeTrue();
    }
}
