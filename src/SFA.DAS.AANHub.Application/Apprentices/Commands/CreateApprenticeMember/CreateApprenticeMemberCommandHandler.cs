using MediatR;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Application.Services;
using SFA.DAS.AANHub.Domain.Common;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using SFA.DAS.AANHub.Domain.Models;
using System.Text.Json;
using static SFA.DAS.AANHub.Domain.Common.Constants;

namespace SFA.DAS.AANHub.Application.Apprentices.Commands.CreateApprenticeMember;

public class CreateApprenticeMemberCommandHandler : IRequestHandler<CreateApprenticeMemberCommand,
    ValidatedResponse<CreateMemberCommandResponse>>
{
    private readonly IAanDataContext _aanDataContext;
    private readonly IAuditWriteRepository _auditWriteRepository;
    private readonly IMembersWriteRepository _membersWriteRepository;
    private readonly IRegionsReadRepository _regionsReadRepository;
    private readonly INotificationsWriteRepository _notificationsWriteRepository;
    private readonly IMemberPreferenceWriteRepository _memberPreferenceWriteRepository;

    public CreateApprenticeMemberCommandHandler(
        IMembersWriteRepository membersWriteRepository,
        IAanDataContext aanDataContext,
        IAuditWriteRepository auditWriteRepository,
        IRegionsReadRepository regionsReadRepository,
        INotificationsWriteRepository notificationsWriteRepository,
        IMemberPreferenceWriteRepository memberPreferenceWriteRepository)
    {
        _membersWriteRepository = membersWriteRepository;
        _aanDataContext = aanDataContext;
        _auditWriteRepository = auditWriteRepository;
        _regionsReadRepository = regionsReadRepository;
        _notificationsWriteRepository = notificationsWriteRepository;
        _memberPreferenceWriteRepository = memberPreferenceWriteRepository;
    }

    public async Task<ValidatedResponse<CreateMemberCommandResponse>> Handle(CreateApprenticeMemberCommand command,
        CancellationToken cancellationToken)
    {
        Member member = command;
        var defaultMemberPreferences = MemberPreferenceService.GetDefaultPreferencesForMember(UserType.Apprentice, member.Id);

        member.MemberPreferences = defaultMemberPreferences;
        _membersWriteRepository.Create(member);

        defaultMemberPreferences.ForEach(preference => _memberPreferenceWriteRepository.Create(preference));

        _auditWriteRepository.Create(new Audit
        {
            Action = "Create",
            ActionedBy = command.MemberId,
            AuditTime = DateTime.UtcNow,
            After = JsonSerializer.Serialize(member.Apprentice),
            Resource = MembershipUserType.Apprentice
        });

        var tokens = await GetTokens(command, cancellationToken);
        Notification notification = NotificationHelper.CreateNotification(Guid.NewGuid(), command.MemberId, EmailTemplateName.ApprenticeOnboardingTemplate, tokens, command.MemberId, true, null);
        _notificationsWriteRepository.Create(notification);

        await _aanDataContext.SaveChangesAsync(cancellationToken);

        return new ValidatedResponse<CreateMemberCommandResponse>(new CreateMemberCommandResponse(member.Id));
    }

    private async Task<string> GetTokens(CreateApprenticeMemberCommand command, CancellationToken cancellationToken)
    {
        var region = await _regionsReadRepository.GetRegionById(command.RegionId.GetValueOrDefault(), cancellationToken);
        var apprenticeOnboardingEmailTemplate = new OnboardingEmailTemplate(command.FirstName!, command.LastName!, $"{region?.Area!} team");
        return JsonSerializer.Serialize(apprenticeOnboardingEmailTemplate);
    }
}