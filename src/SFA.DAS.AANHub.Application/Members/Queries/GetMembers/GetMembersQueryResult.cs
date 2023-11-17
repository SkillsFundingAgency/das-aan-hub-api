namespace SFA.DAS.AANHub.Application.Members.Queries.GetMembers;
public class GetMembersQueryResult
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int TotalCount { get; set; }
    public IEnumerable<MembersSummaryModel> Members { get; set; } = Enumerable.Empty<MembersSummaryModel>();
}