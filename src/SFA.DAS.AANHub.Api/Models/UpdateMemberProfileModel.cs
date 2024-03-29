﻿using SFA.DAS.AANHub.Application.MemberProfiles.Commands.PutMemberProfile;

namespace SFA.DAS.AANHub.Api.Models;

public class UpdateMemberProfileModel
{
    public IEnumerable<UpdateProfileModel> MemberProfiles { get; set; } = Enumerable.Empty<UpdateProfileModel>();
    public IEnumerable<UpdatePreferenceModel> MemberPreferences { get; set; } = Enumerable.Empty<UpdatePreferenceModel>();
}
