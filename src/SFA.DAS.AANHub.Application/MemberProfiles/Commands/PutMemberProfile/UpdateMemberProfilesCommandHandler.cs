using System.Text.Json;
using MediatR;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.MemberProfiles.Commands.PutMemberProfile;
public class UpdateMemberProfilesCommandHandler : IRequestHandler<UpdateMemberProfilesCommand, ValidatedResponse<SuccessCommandResult>>
{
    private readonly IMembersWriteRepository _membersWriteRepository;
    private readonly IAanDataContext _aanDataContext;
    private readonly IAuditWriteRepository _auditWriteRepository;
    private readonly IDateTimeProvider _dateTimeProvider;


    public UpdateMemberProfilesCommandHandler(
        IMembersWriteRepository membersWriteRepository,
        IDateTimeProvider dateTimeProvider,
        IAuditWriteRepository auditWriteRepository,
        IAanDataContext aanDataContext)
    {
        _aanDataContext = aanDataContext;
        _auditWriteRepository = auditWriteRepository;
        _membersWriteRepository = membersWriteRepository;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<ValidatedResponse<SuccessCommandResult>> Handle(UpdateMemberProfilesCommand command, CancellationToken cancellationToken)
    {
        var existingMember = await _membersWriteRepository.Get(command.MemberId);

        if (command.MemberPreferences.Any()) UpdatePreferences(command.MemberPreferences, existingMember!);

        if (command.MemberProfiles.Any()) UpdateMemberProfile(command.MemberProfiles, existingMember!);

        await _aanDataContext.SaveChangesAsync(cancellationToken);

        return new ValidatedResponse<SuccessCommandResult>(new SuccessCommandResult());
    }

    private void UpdateMemberProfile(IEnumerable<UpdateProfileModel> profiles, Member existingMember)
    {
        var audit = new Audit()
        {
            Action = "Put",
            Before = JsonSerializer.Serialize(existingMember.MemberProfiles),
            ActionedBy = existingMember.Id,
            AuditTime = _dateTimeProvider.Now,
            Resource = nameof(MemberProfile),
        };

        foreach (var profile in profiles)
        {
            var memberProfile = existingMember.MemberProfiles.Find(x => x.ProfileId == profile.MemberProfileId);
            if (string.IsNullOrWhiteSpace(profile.Value) && memberProfile != null)
            {
                existingMember.MemberProfiles.Remove(memberProfile);
            }
            else if (!string.IsNullOrWhiteSpace(profile.Value))
            {
                if (memberProfile != null)
                {
                    memberProfile.ProfileValue = profile.Value;
                }
                else
                {
                    var newProfile = new MemberProfile() { Member = existingMember, ProfileId = profile.MemberProfileId, ProfileValue = profile.Value! };
                    existingMember.MemberProfiles.Add(newProfile);
                }
            }
        }

        audit.After = JsonSerializer.Serialize(existingMember.MemberProfiles);
        _auditWriteRepository.Create(audit);
    }

    public void UpdatePreferences(IEnumerable<UpdatePreferenceModel> preferences, Member member)
    {
        var audit = new Audit()
        {
            Action = "Put",
            Before = JsonSerializer.Serialize(member.MemberPreferences),
            ActionedBy = member.Id,
            AuditTime = _dateTimeProvider.Now,
            Resource = nameof(MemberPreference),
        };

        foreach (var preference in preferences)
        {
            var existingPreference = member.MemberPreferences.Single(x => x.PreferenceId == preference.PreferenceId);
            existingPreference.AllowSharing = preference.Value;
        }

        audit.After = JsonSerializer.Serialize(member.MemberPreferences);
        _auditWriteRepository.Create(audit);
    }
}
