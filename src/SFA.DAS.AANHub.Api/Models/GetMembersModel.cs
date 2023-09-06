using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AANHub.Application.Members.Queries.GetMembers;
using SFA.DAS.AANHub.Domain.Common;
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
    public List<MemberUserType> UserType { get; set; } = new List<MemberUserType>();

    [FromQuery]
    public List<MembershipStatusType> Status { get; set; } = new List<MembershipStatusType>();

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
        Status = model.Status,
        IsRegionalChair = model.IsRegionalChair,
        Page = model.Page,
        PageSize = model.PageSize
    };
}
