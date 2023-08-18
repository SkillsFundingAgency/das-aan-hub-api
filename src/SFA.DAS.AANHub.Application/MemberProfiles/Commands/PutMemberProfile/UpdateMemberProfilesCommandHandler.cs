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
        var existingMemberProfile = await _membersWriteRepository.Get(command.MemberId);

        //existingMemberProfile.

        return existingMemberProfile switch
        {
            null => new ValidatedResponse<SuccessCommandResult>(new SuccessCommandResult()),
            not null => await UpdateMemberProfile(existingMemberProfile, command, cancellationToken),
        };
    }

    private async Task<ValidatedResponse<SuccessCommandResult>> UpdateMemberProfile(
        Member existingMemberProfile,
        UpdateMemberProfilesCommand command,
        CancellationToken token)
    {
        var audit = new Audit()
        {
            Action = "Put",
            Before = JsonSerializer.Serialize(existingMemberProfile),
            ActionedBy = command.RequestedByMemberId,
            AuditTime = _dateTimeProvider.Now,// DateTime.UtcNow,
            Resource = nameof(Attendance),
        };

        //update the existing member profile
        //existingMemberProfile.MemberProfiles = command.PutMemberProfileModel.Profiles;

        audit.After = JsonSerializer.Serialize(existingMemberProfile);
        _auditWriteRepository.Create(audit);

        await _aanDataContext.SaveChangesAsync(token);

        return new ValidatedResponse<SuccessCommandResult>(new SuccessCommandResult());
    }
}
