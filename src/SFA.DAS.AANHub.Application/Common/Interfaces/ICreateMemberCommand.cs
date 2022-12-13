namespace SFA.DAS.AANHub.Application.Common.Interfaces
{
    public interface ICreateMemberCommand
    {
        public string? Email { get; set; }
        public string? Name { get; set; }
        public DateTime Joined { get; set; }
        public int[]? Regions { get; set; }
        public string? Information { get; set; }
    }
}
