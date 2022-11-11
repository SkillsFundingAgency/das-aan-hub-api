
using MediatR;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SFA.DAS.AAN.Application.Interfaces;
using SFA.DAS.AAN.Data.Configuration;
using SFA.DAS.AAN.Domain.Entities;
using SFA.DAS.AAN.Domain.Entities.Audit;
using SFA.DAS.AAN.Domain.Enums;
using SFA.DAS.AAN.Domain.Interfaces;


namespace SFA.DAS.AAN.Application.Commands.CreateMember
{
    public class CreateMemberCommandHandler : IRequestHandler<CreateMemberCommand, CreateMemberResponse>
    {
        private readonly IMembersContext _membersContext;
        private readonly IApprenticesContext _apprenticesContext;
        private readonly IEmployersContext _employersContext;
        private readonly IPartnersContext _partnersContext;
        private readonly IAdminsContext _adminsContext;
        private readonly IAuditContext _auditContext;
        private readonly IAuditLogService<Member> _auditLogService;

        public CreateMemberCommandHandler(
            IMembersContext membersContext,
            IApprenticesContext apprenticesContext,
            IEmployersContext employersContext,
            IPartnersContext partnersContext,
            IAdminsContext adminsContext,
            IAuditContext auditContext,
            IAuditLogService<Member> auditLogService)
        {
            _membersContext = membersContext;
            _apprenticesContext = apprenticesContext;
            _employersContext = employersContext;
            _partnersContext = partnersContext;
            _adminsContext = adminsContext;
            _auditContext = auditContext;
            _auditLogService = auditLogService;
        }

        public async Task<CreateMemberResponse> Handle(CreateMemberCommand command, CancellationToken cancellationToken)
        {
            Guid memberId = Guid.NewGuid();

            await _auditContext.Entities.AddAsync(
                _auditLogService.BuildAuditLog(
                    new AuditMetadata { EntityId = memberId, ActionedBy = memberId, Action = Actions.Create, Resource = ApiRoutes.CreateMemberUri }, null, null), cancellationToken);

            EntityEntry<Member> member = await _membersContext.Entities.AddAsync(
                new Member()
                {
                    Id = memberId,
                    UserType = command.UserType?.ToString() ?? "unknown",
                    Joined = command.joined,
                    RegionId = command.region,
                    Information = command.information,
                    Organisation = command.organisation,
                    Created = DateTime.Now,
                    Updated = DateTime.Now,
                    Deleted = null,
                    Status = MembershipStatuses.Live.ToString()
                }
            );
            
            await _membersContext.SaveChangesAsync();  

            long id = long.TryParse(command.id, out id) ? id : 0;

            switch (command.UserType)
            {
                case MembershipUserTypes.Apprentice:
                    EntityEntry<Apprentice> apprentice = await _apprenticesContext.Entities.AddAsync(
                        new Apprentice()
                        {
                            MemberId = memberId,
                            ApprenticeId = id,
                            Email = null,
                            Name = null,
                            LastUpdated = DateTime.Now,
                            IsActive = true
                        }
                    );
                    await _auditContext.Entities.AddAsync(
                        _auditLogService.BuildAuditLog(
                            new AuditMetadata { EntityId = memberId, ActionedBy = memberId, Action = Actions.Create, Resource = ApiRoutes.CreateApprenticeMemberUri }, null, null), cancellationToken);

                    await _apprenticesContext.SaveChangesAsync();
                    break;

                case MembershipUserTypes.Employer:
                    EntityEntry<Employer> employer = await _employersContext.Entities.AddAsync(
                        new Employer()
                        {
                            MemberId = memberId,
                            AccountId = id,
                            UserId = null,
                            Email = null,
                            Name = null,
                            LastUpdated = DateTime.Now,
                            IsActive = true
                        }
                    );
                    await _auditContext.Entities.AddAsync(
                       _auditLogService.BuildAuditLog(
                           new AuditMetadata { EntityId = memberId, ActionedBy = memberId, Action = Actions.Create, Resource = ApiRoutes.CreateEmployerMemberUri }, null, null), cancellationToken);
                    await _employersContext.SaveChangesAsync();
                    break;

                case MembershipUserTypes.Partner:
                    EntityEntry<Partner> partner = await _partnersContext.Entities.AddAsync(
                        new Partner()
                        {
                            MemberId = memberId,
                            UKPRN = id,
                            Email = null,
                            Name = null,
                            LastUpdated = DateTime.Now,
                            IsActive = true
                        }
                    );
                    await _auditContext.Entities.AddAsync(
                       _auditLogService.BuildAuditLog(
                           new AuditMetadata { EntityId = memberId, ActionedBy = memberId, Action = Actions.Create, Resource = ApiRoutes.CreatePartnerMemberUri }, null, null), cancellationToken);

                    await _partnersContext.SaveChangesAsync();
                    break;

                case MembershipUserTypes.Admin:
                    EntityEntry<Admin> admin = await _adminsContext.Entities.AddAsync(
                        new Admin()
                        {
                            MemberId = memberId,
                            Email = null,
                            LastUpdated = DateTime.Now,
                            IsActive = true
                        }
                    );
                    await _auditContext.Entities.AddAsync(
                       _auditLogService.BuildAuditLog(
                           new AuditMetadata { EntityId = memberId, ActionedBy = memberId, Action = Actions.Create, Resource = ApiRoutes.CreateAdminMemberUri }, null, null), cancellationToken);

                    await _adminsContext.SaveChangesAsync();
                    break;

                default:
                    break;
            }

            await _auditContext.SaveChangesAsync(cancellationToken);

            return new CreateMemberResponse() { Member = member.Entity };
        }
    }
}
