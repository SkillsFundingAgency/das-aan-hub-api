
using MediatR;
using Microsoft.EntityFrameworkCore;
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

        public CreateMemberCommandHandler(IMembersContext membersContext, IApprenticesContext apprenticesContext)
        {
            _membersContext = membersContext;
            _apprenticesContext = apprenticesContext;
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

            switch (command.UserType.ToLower())
            {
                case "apprentice":
                    long id = 0;
                    bool success = long.TryParse(command.id, out id);
                    EntityEntry<Apprentice> apprentice = await _apprenticesContext.Entities.AddAsync(
                        new Apprentice()
                        {
                            MemberId = memberId,
                            ApprenticeId = id,
                            Email = "",
                            Name = "",
                            LastUpdated = DateTime.Now,
                            IsActive = true
                        }
                    );
                    await _apprenticesContext.SaveChangesAsync();
                    break;

                case "employer":
                    break;
                case "partner":
                    break;
                case "admin":
                    break;
                default:
                    break;
            }

            return new CreateMemberResponse() { Member = member.Entity };
        }
    }
}
