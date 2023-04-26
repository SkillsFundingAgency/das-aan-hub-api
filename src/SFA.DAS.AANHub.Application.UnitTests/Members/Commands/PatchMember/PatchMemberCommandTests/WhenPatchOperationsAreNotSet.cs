using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Members.Commands.PatchMember;

namespace SFA.DAS.AANHub.Application.UnitTests.Members.Commands.PatchMember.PatchMemberCommandTests;

public class WhenPatchOperationsAreNotSet
{
    private PatchMemberCommand _sut = null!;

    [SetUp]
    public void Init()
    {
        _sut = new();
    }

    [Test]
    public void HasEmailIsFalse() => _sut.HasEmail.Should().BeFalse();

    [Test]
    public void HasFirstNameIsFalse() => _sut.HasFirstName.Should().BeFalse();

    [Test]
    public void HasLastNameIsFalse() => _sut.HasLastName.Should().BeFalse();

    [Test]
    public void HasOrganisationNameIsFalse() => _sut.HasOrganisationName.Should().BeFalse();

    [Test]
    public void HasRgionIdIsFalse() => _sut.HasRegionId.Should().BeFalse();

    [Test]
    public void HasStatuIsFalse() => _sut.HasStatus.Should().BeFalse();

}

