using MediatR;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Enums;
using SFA.DAS.AANHub.Domain.Interfaces;

namespace SFA.DAS.AANHub.Application.Commands.CreateMember
{
    public class CreateMemberCommandHandler : IRequestHandler<CreateMemberCommand, CreateMemberResponse>
    {
        private readonly IMembersContext _membersContext;
        private readonly IApprenticesContext _apprenticesContext;
        private readonly IEmployersContext _employersContext;
        private readonly IPartnersContext _partnersContext;
        private readonly IAdminsContext _adminsContext;

        public CreateMemberCommandHandler(
            IMembersContext membersContext,
            IApprenticesContext apprenticesContext,
            IEmployersContext employersContext,
            IPartnersContext partnersContext,
            IAdminsContext adminsContext)
        {
            _membersContext = membersContext;
            _apprenticesContext = apprenticesContext;
            _employersContext = employersContext;
            _partnersContext = partnersContext;
            _adminsContext = adminsContext;
        }

        public async Task<CreateMemberResponse> Handle(CreateMemberCommand command, CancellationToken cancellationToken)
        {
            var memberId = Guid.NewGuid();

            var member = await _membersContext.Entities.AddAsync(
                new Member()
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
                }
            );
            await _membersContext.SaveChangesAsync();

            long id = long.TryParse(command.Id, out id) ? id : 0;

            switch (command.UserType)
            {
                case MembershipUserTypes.Apprentice:
                    var apprentice = await _apprenticesContext.Entities.AddAsync(
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
                    await _apprenticesContext.SaveChangesAsync();
                    break;

                case MembershipUserTypes.Employer:
                    var employer = await _employersContext.Entities.AddAsync(
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
                    await _employersContext.SaveChangesAsync();
                    break;

                case MembershipUserTypes.Partner:
                    var partner = await _partnersContext.Entities.AddAsync(
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
                    await _partnersContext.SaveChangesAsync();
                    break;

                case MembershipUserTypes.Admin:
                    var admin = await _adminsContext.Entities.AddAsync(
                        new Admin()
                        {
                            MemberId = memberId,
                            Email = null,
                            LastUpdated = DateTime.Now,
                            IsActive = true
                        }
                    );
                    await _adminsContext.SaveChangesAsync();
                    break;

                default:
                    break;
            }

            return new CreateMemberResponse() { Member = member.Entity };
        }
    }
}
