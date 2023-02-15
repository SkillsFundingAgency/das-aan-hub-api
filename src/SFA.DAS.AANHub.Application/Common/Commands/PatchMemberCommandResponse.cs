namespace SFA.DAS.AANHub.Application.Common.Commands
{
    public class PatchMemberCommandResponse
    {
        public PatchMemberCommandResponse(bool isSuccess) => IsSuccess = isSuccess;

        public bool IsSuccess { get; set; }
    }
}