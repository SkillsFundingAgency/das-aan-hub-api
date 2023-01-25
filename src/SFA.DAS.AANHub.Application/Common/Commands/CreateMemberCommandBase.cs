namespace SFA.DAS.AANHub.Application.Common.Commands
{
    public abstract class CreateMemberCommandBase
    {
        protected CreateMemberCommandBase() => Id = Guid.NewGuid();
        public Guid Id { get; }
        public string? ReviewStatus { get; set; }
        public string Email { get; set; } = null!;
        public string Name { get; set; } = null!;
        public DateTime Joined { get; set; }
        public List<int>? Regions { get; set; }
        public string? Information { get; set; }
    }
}