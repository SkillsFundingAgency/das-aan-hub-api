using System.Text.Json;
using MediatR;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Common;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using SFA.DAS.AANHub.Domain.Models;
using static SFA.DAS.AANHub.Domain.Common.Constants;

namespace SFA.DAS.AANHub.Application.Employers.Commands.CreateEmployerMember
{
    public class CreateEmployerMemberCommandHandler :
        IRequestHandler<CreateEmployerMemberCommand, ValidatedResponse<CreateMemberCommandResponse>>
    {
        private readonly IAanDataContext _aanDataContext;
        private readonly IAuditWriteRepository _auditWriteRepository;
        private readonly IMembersWriteRepository _membersWriteRepository;
        private readonly INotificationsWriteRepository _notificationsWriteRepository;
        private readonly IRegionsReadRepository _regionsReadRepository;

        public CreateEmployerMemberCommandHandler(IMembersWriteRepository membersWriteRepository,
            IAanDataContext aanDataContext, IAuditWriteRepository auditWriteRepository,
            INotificationsWriteRepository notificationsWriteRepository, IRegionsReadRepository regionsReadRepository)
        {
            _membersWriteRepository = membersWriteRepository;
            _aanDataContext = aanDataContext;
            _auditWriteRepository = auditWriteRepository;
            _notificationsWriteRepository = notificationsWriteRepository;
            _regionsReadRepository = regionsReadRepository;
        }

        public async Task<ValidatedResponse<CreateMemberCommandResponse>> Handle(CreateEmployerMemberCommand command,
            CancellationToken cancellationToken)
        {
            Member member = command;

            _membersWriteRepository.Create(member);

            _auditWriteRepository.Create(new Audit
            {
                Action = "Create",
                ActionedBy = command.MemberId,
                AuditTime = DateTime.UtcNow,
                After = JsonSerializer.Serialize(member.Employer),
                Resource = MembershipUserType.Employer
            });

            var tokens = await GetTokens(command, cancellationToken);
            Notification notification = NotificationHelper.CreateNotification(Guid.NewGuid(),command.MemberId, EmailTemplateName.EmployerOnboardingTemplate, tokens, command.MemberId, true, null);
            _notificationsWriteRepository.Create(notification);

            await _aanDataContext.SaveChangesAsync(cancellationToken);

            return new ValidatedResponse<CreateMemberCommandResponse>(new CreateMemberCommandResponse(member.Id));
        }

        private async Task<string> GetTokens(CreateEmployerMemberCommand command, CancellationToken cancellationToken)
        {
            var region = new Region();

            if (command.RegionId == null)
            {
                region.Area = "Multi-region team";
            }
            else
            {
                region = await _regionsReadRepository.GetRegionById(command.RegionId.GetValueOrDefault(), cancellationToken);
            }

            var employerOnboardingEmailTemplate = new OnboardingEmailTemplate(command.FirstName!, command.LastName!, $"{region?.Area!} team");
            return JsonSerializer.Serialize(employerOnboardingEmailTemplate);
        }
    }
}