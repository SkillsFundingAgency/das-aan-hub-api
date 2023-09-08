using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.MemberProfiles.Queries.GetMemberProfilesWithPreferences;

namespace SFA.DAS.AANHub.Application.UnitTests.MemberProfiles.Queries.GetMemberProfilesWithPreferences;
public class GetMemberProfilesWithPreferencesQueryValidatorTests
{
    [TestCase("8fad718f-4c72-4fb4-8da0-6763efa8fb9c", true, true)]
    [TestCase("8fad718f-4c72-4fb4-8da0-6763efa8fb9c", false, true)]
    [TestCase("00000000-0000-0000-0000-000000000000", true, false)]
    [TestCase("00000000-0000-0000-0000-000000000000", false, false)]
    public async Task Validates_ApprenticeId_NotNull_NotFound(Guid employerId, bool isPublicView, bool isValid)
    {
        var query = new GetMemberProfilesWithPreferencesQuery(employerId, isPublicView);
        var sut = new GetMemberProfilesWithPreferencesQueryValidator();

        var result = await sut.TestValidateAsync(query);

        result.IsValid.Equals(isValid);
    }
}
