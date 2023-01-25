namespace SFA.DAS.AANHub.Application.Common.Commands
{
    public abstract class PatchMemberCommandResponseBase
    {
        public bool IsSuccess { get; }
        public PatchMemberCommandResponseBase(bool isSuccess)
        {
            IsSuccess = isSuccess;
        }
    }
}
