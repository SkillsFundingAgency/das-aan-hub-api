
using MediatR;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SFA.DAS.AAN.Domain.Entities;
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
            Guid memberId = Guid.NewGuid();

            EntityEntry<Member> member = await _membersContext.Entities.AddAsync(
                new Member()
                {
                    Id = memberId,
                    UserType = command.UserType,
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

            switch (command.UserType.ToLower())
            {
                case "apprentice":
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
                    await _apprenticesContext.SaveChangesAsync();
                    break;

                case "employer":
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
                    await _employersContext.SaveChangesAsync();
                    break;

                case "partner":
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
                    await _partnersContext.SaveChangesAsync();
                    break;

                case "admin":
                    EntityEntry<Admin> admin = await _adminsContext.Entities.AddAsync(
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
