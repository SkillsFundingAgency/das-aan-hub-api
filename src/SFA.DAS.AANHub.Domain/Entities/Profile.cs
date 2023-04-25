namespace SFA.DAS.AANHub.Domain.Entities;

public class Profile
{
    public int Id { get; set; }
    public string UserType { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Category { get; set; } = null!;
    public int Ordering { get; set; }
}
