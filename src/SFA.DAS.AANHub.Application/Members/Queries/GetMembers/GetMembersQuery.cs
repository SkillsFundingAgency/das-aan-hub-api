﻿using MediatR;
using SFA.DAS.AANHub.Domain.Common;

namespace SFA.DAS.AANHub.Application.Members.Queries.GetMembers;

public class GetMembersQuery : IRequest<GetMembersQueryResult>
{
    public List<UserType> UserTypes { get; set; } = new();
    public bool? IsRegionalChair { get; set; }
    public List<int> RegionIds { get; set; } = new();
    public string? Keyword { get; set; }
    public bool? IsNew { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = Domain.Common.Constants.Members.PageSize;
}