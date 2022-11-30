using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.AANHub.Domain.Interfaces;

namespace SFA.DAS.AANHub.Application.Commands.ModifyMember
{
    public class ModifyMemberCommandHandler : IRequestHandler<ModifyMemberCommand, ModifyMemberResponse>
    {
        private readonly IMembersContext _membersContext;
        private readonly ILogger<ModifyMemberCommandHandler> _logger;

        public ModifyMemberCommandHandler(
            IMembersContext membersContext,
            ILogger<ModifyMemberCommandHandler> logger)
        {
            _membersContext = membersContext;
            _logger = logger;
        }

        public async Task<ModifyMemberResponse> Handle(ModifyMemberCommand request, CancellationToken cancellationToken)
        {
            var validId = Guid.TryParse(request.UserId, out var parsedId);

            if (!validId)
            {
                var error = $"Invalid MemberId: {request.UserId}";
                _logger.LogError("{error}", error);
                throw new InvalidOperationException(error);
            }

            var retrievedMember = await _membersContext.FindByIdAsync(parsedId);

            if (retrievedMember == null)
            {
                var error = $"Unable to retrieve Member with Id: {request.UserId}";
                _logger.LogError("{error}", error);
                throw new KeyNotFoundException(error);
            }
            //Patch values to member
            retrievedMember.Status = request.Status.ToString();
            //TODO: Add regions when confirmed

            await _membersContext.SaveChangesAsync(cancellationToken);

            return new ModifyMemberResponse();
        }
    }
}
