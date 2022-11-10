
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

        public CreateMemberCommandHandler(IMembersContext membersContext)
        {
            _membersContext = membersContext;
        }

        public async Task<CreateMemberResponse> Handle(CreateMemberCommand command, CancellationToken cancellationToken)
        {
            EntityEntry<Member> member = await _membersContext.Entities.AddAsync(
                new Member()
                {
                    Id = Guid.NewGuid(),
                    UserType = command.UserType,
                    Joined = command.joined,
                    RegionId = command.region,
                    Information = command.information,
                    Organisation = command.organisation,
                    Created = DateTime.Now,
                    Updated = DateTime.Now,
                    Deleted = null,
                    Status = "live"
                }
            );
            await _membersContext.SaveChangesAsync();

            return new CreateMemberResponse() { Member = member.Entity };
        }
    }
}
