using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AANHub.Application.Members.Queries.GetMembers;
using Constants = SFA.DAS.AANHub.Api.Common.Constants;

namespace SFA.DAS.AANHub.Api.Models;

public class GetMembersModel
{
    [FromHeader(Name = Constants.RequestHeaders.RequestedByMemberIdHeader)]
    public Guid RequestedByMemberId { get; set; }

    [FromQuery]
    public string Keyword { get; set; } = string.Empty;

    [FromQuery]
    public List<int> RegionId { get; set; } = new List<int>();

    [FromQuery]
    public string? UserType { get; set; }

    [FromQuery]
    public bool? IsRegionalChair { get; set; }

    [FromQuery]
    public int Page { get; set; } = 1;

    [FromQuery] public int PageSize { get; set; } = Domain.Common.Constants.Members.PageSize;

    public static implicit operator GetMembersQuery(GetMembersModel model) => new()
    {
        RequestedByMemberId = model.RequestedByMemberId,
        Keyword = model.Keyword,
        RegionIds = model.RegionId,
        UserType = model.UserType,
        IsRegionalChair = model.IsRegionalChair,
        Page = model.Page,
        PageSize = model.PageSize
    };
}
