namespace SFA.DAS.AANHub.Domain.Models;
public class GetMembersOptions
{
    public Guid MemberId { get; set; }
    public string? Keyword { get; set; }
    public int KeywordCount => string.IsNullOrWhiteSpace(Keyword) ? 0 : Keyword.Split(" ").Length;
    public string? UserType { get; set; }
    public bool? IsRegionalChair { get; set; }
    public List<int> RegionIds { get; set; } = new List<int>();
    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = Domain.Common.Constants.Members.PageSize;

}