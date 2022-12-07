using MediatR;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Enums;
using SFA.DAS.AANHub.Domain.Interfaces;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.Apprentices.Commands
{
    public class CreateApprenticesCommandHandler : IRequestHandler<CreateApprenticesCommand, Guid>
    {
        private readonly IMembersWriteRepository _membersWriteRepository;
        private readonly IAanDataContext _aanDataContext;

        public CreateApprenticesCommandHandler(IMembersWriteRepository membersWriteRepository, IApprenticesWriteRepository apprenticesWriteRepository, IAanDataContext aanDataContext)
        {
            _membersWriteRepository = membersWriteRepository;
            _aanDataContext = aanDataContext;
        }

        public async Task<Guid> Handle(CreateApprenticesCommand command, CancellationToken cancellationToken)
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
                Status = MembershipStatuses.Live.ToString(),
                Apprentice = new Apprentice()
                {
                    ApprenticeId = command.ApprenticeId,
                    Email = null,
                    Name = null,
                    LastUpdated = DateTime.Now,
                    IsActive = true
                }
            };

            _membersWriteRepository.Create(member);

            await _aanDataContext.SaveChangesAsync(cancellationToken);

            return memberId;
        }
    }

}
