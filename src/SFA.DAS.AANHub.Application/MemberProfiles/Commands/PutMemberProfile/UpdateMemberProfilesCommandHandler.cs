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

        if (existingMember != null) return await UpdateMemberProfile(existingMember, command, cancellationToken);
        else return new ValidatedResponse<SuccessCommandResult>(new SuccessCommandResult(false));
    }

    private async Task<ValidatedResponse<SuccessCommandResult>> UpdateMemberProfile(
        Member existingMember,
        UpdateMemberProfilesCommand command,
        CancellationToken token)
    {
        var audit = new Audit()
        {
            Action = "Put",
            Before = JsonSerializer.Serialize(existingMember),
            ActionedBy = command.RequestedByMemberId,
            AuditTime = _dateTimeProvider.Now,
            Resource = nameof(Attendance),
        };

        command.Profiles.ToList().ForEach(p =>
        {
            var memberProfile = existingMember.MemberProfiles.Find(x => x.ProfileId == p.ProfileId);
            if (memberProfile != null) memberProfile.ProfileValue = p.Value!;
            else InsertMemberProfile(existingMember, p.ProfileId, p.Value);

        });

        command.Preferences.ToList().ForEach(p =>
        {
            var preference = existingMember.MemberPreferences.Find(x => x.PreferenceId == p.PreferenceId);
            if (preference != null) preference.AllowSharing = p.Value;
            else InsertMemberPreference(existingMember, p.PreferenceId, p.Value);

        });

        audit.After = JsonSerializer.Serialize(existingMember);
        _auditWriteRepository.Create(audit);

        await _aanDataContext.SaveChangesAsync(token);

        return new ValidatedResponse<SuccessCommandResult>(new SuccessCommandResult());
    }

    private static void InsertMemberProfile(Member existingMember, int profileId, string? profileValue)
    {
        existingMember.MemberProfiles.Add(new MemberProfile() { Member = existingMember, ProfileId = profileId, ProfileValue = profileValue! });
    }

    private static void InsertMemberPreference(Member existingMember, int preferenceId, bool profileValue)
    {
        existingMember.MemberPreferences.Add(new MemberPreference() { Member = existingMember, PreferenceId = preferenceId, AllowSharing = profileValue });
    }
}
