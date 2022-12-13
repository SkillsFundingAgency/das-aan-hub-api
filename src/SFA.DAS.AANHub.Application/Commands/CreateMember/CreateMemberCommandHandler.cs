using MediatR;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Enums;
using SFA.DAS.AANHub.Domain.Interfaces;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using System.Text.Json;

namespace SFA.DAS.AANHub.Application.Commands.CreateMember
{
    public class CreateMemberCommandHandler : IRequestHandler<CreateMemberCommand, CreateMemberResponse>
    {
        private readonly IMembersWriteRepository _membersWriteRepository;
        private readonly IApprenticesWriteRepository _apprenticesWriteRepository;
        private readonly IEmployersWriteRepository _employersWriteRepository;
        private readonly IPartnersWriteRepository _partnersWriteRepository;
        private readonly IAdminsWriteRepository _adminsWriteRepository;
        private readonly IAuditWriteRepository _auditWriteRepository;
        private readonly IAanDataContext _aanDataContext;
        public CreateMemberCommandHandler(
            IMembersWriteRepository membersWriteRepository,
            IApprenticesWriteRepository apprenticesRepository,
            IEmployersWriteRepository employersWriteRepository,
            IPartnersWriteRepository partnersWriteRepository,
            IAdminsWriteRepository adminsWriteRepository,
            IAuditWriteRepository auditWriteRepository,
            IAanDataContext aanDataContext)
        {
            _membersWriteRepository = membersWriteRepository;
            _apprenticesWriteRepository = apprenticesRepository;
            _employersWriteRepository = employersWriteRepository;
            _partnersWriteRepository = partnersWriteRepository;
            _adminsWriteRepository = adminsWriteRepository;
            _auditWriteRepository = auditWriteRepository;
            _aanDataContext = aanDataContext;
        }

        public async Task<CreateMemberResponse> Handle(CreateMemberCommand command, CancellationToken cancellationToken)
        {
            var memberId = Guid.NewGuid();

            var member = new Member()
            {
                Id = memberId,
                UserType = command.UserType?.ToString() ?? "unknown",
                Joined = command.Joined,
                RegionId = command.Region,
                Information = command.Information,
                Organisation = command.Organisation,
                Created = DateTime.Now,
                Updated = DateTime.Now,
                Deleted = null,
                Status = MembershipStatuses.Live.ToString()
            };

            _membersWriteRepository.Create(member);

            _auditWriteRepository.Create(new Audit
            {
                Action = "Create",
                ActionedBy = member.Id,
                AuditTime = DateTime.UtcNow,
                After = JsonSerializer.Serialize(member),
                Resource = member.UserType
            });

            long id = long.TryParse(command.Id, out id) ? id : 0;

            switch (command.UserType)
            {
                case MembershipUserTypes.Apprentice:
                    _apprenticesWriteRepository.Create(
                        new Apprentice()
                        {
                            MemberId = memberId,
                            ApprenticeId = id,
                            Email = null,
                            Name = null,
                            LastUpdated = DateTime.Now,
                            IsActive = true
                        });
                    break;

                case MembershipUserTypes.Employer:
                    _employersWriteRepository.Create(
                        new Employer()
                        {
                            MemberId = memberId,
                            AccountId = id,
                            UserId = null,
                            Email = null,
                            Name = null,
                            LastUpdated = DateTime.Now,
                            IsActive = true
                        });
                    break;

                case MembershipUserTypes.Partner:
                    _partnersWriteRepository.Create(
                        new Partner()
                        {
                            MemberId = memberId,
                            UKPRN = id,
                            Email = null,
                            Name = null,
                            LastUpdated = DateTime.Now,
                            IsActive = true
                        });
                    break;

                case MembershipUserTypes.Admin:
                    _adminsWriteRepository.Create(
                        new Admin()
                        {
                            MemberId = memberId,
                            Email = null,
                            LastUpdated = DateTime.Now,
                            IsActive = true
                        });
                    break;
            }

            await _aanDataContext.SaveChangesAsync(cancellationToken);

            return new CreateMemberResponse() { Member = member };
        }
    }
}
