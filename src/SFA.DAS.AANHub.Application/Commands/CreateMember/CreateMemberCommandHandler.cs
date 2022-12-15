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
        private readonly IPartnersWriteRepository _partnersWriteRepository;
        private readonly IAdminsWriteRepository _adminsWriteRepository;
        private readonly IAuditWriteRepository _auditWriteRepository;
        private readonly IAanDataContext _aanDataContext;
        public CreateMemberCommandHandler(
            IMembersWriteRepository membersWriteRepository,
            IApprenticesWriteRepository apprenticesRepository,
            IPartnersWriteRepository partnersWriteRepository,
            IAdminsWriteRepository adminsWriteRepository,
            IAuditWriteRepository auditWriteRepository,
            IAanDataContext aanDataContext)
        {
            _membersWriteRepository = membersWriteRepository;
            _apprenticesWriteRepository = apprenticesRepository;
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
                UserType = command.UserType,
                Joined = command.Joined,
                Information = command.Information,
                Created = DateTime.Now,
                Updated = DateTime.Now,
                ReviewStatus = MembershipReviewStatus.New,
                Deleted = null,
                Status = MembershipStatus.Live
            };

            _membersWriteRepository.Create(member);

            _auditWriteRepository.Create(new Audit
            {
                Action = "Create",
                ActionedBy = member.Id,
                AuditTime = DateTime.UtcNow,
                After = JsonSerializer.Serialize(member),
                Resource = member?.UserType?.ToString() ?? string.Empty,
            });

            switch (command.UserType)
            {
                case MembershipUserType.Apprentice:
                    _apprenticesWriteRepository.Create(
                        new Apprentice()
                        {
                            MemberId = memberId,
                            Email = null,
                            Name = null,
                            LastUpdated = DateTime.Now,
                            IsActive = true
                        });
                    break;

                case MembershipUserType.Partner:
                    _partnersWriteRepository.Create(
                        new Partner()
                        {
                            MemberId = memberId,
                            Email = null,
                            Name = null,
                            LastUpdated = DateTime.Now,
                            IsActive = true
                        });
                    break;

                case MembershipUserType.Admin:
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
