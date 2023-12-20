using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AANHub.Application.Members.Queries.GetMembers;
using SFA.DAS.AANHub.Domain.Common;

namespace SFA.DAS.AANHub.Api.Models;

public class GetMembersModel
{
    [FromQuery]
    public string Keyword { get; set; } = string.Empty;

    [FromQuery]
    public List<int> RegionId { get; set; } = new();

    [FromQuery]
    public List<UserType> UserType { get; set; } = new();

    [FromQuery]
    public bool? IsRegionalChair { get; set; }

    [FromQuery]
    public int Page { get; set; } = 1;

    [FromQuery] public int PageSize { get; set; } = Domain.Common.Constants.Members.PageSize;

    [FromQuery] public bool? IsNew { get; set; }

    public static implicit operator GetMembersQuery(GetMembersModel model) => new()
    {
        Keyword = model.Keyword,
        RegionIds = model.RegionId,
        UserTypes = model.UserType,
        IsRegionalChair = model.IsRegionalChair,
        IsNew = model.IsNew,
        Page = model.Page,
        PageSize = model.PageSize
    };
}
