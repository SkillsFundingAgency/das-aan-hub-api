using SFA.DAS.AANHub.Domain.Common;

namespace SFA.DAS.AANHub.Domain.Models;
public class GetMembersOptions
{
    public string? Keyword { get; set; }
    public List<UserType> UserTypes { get; set; } = new();
    public bool? IsRegionalChair { get; set; }
    public List<int> RegionIds { get; set; } = new();
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = Domain.Common.Constants.Members.PageSize;

}