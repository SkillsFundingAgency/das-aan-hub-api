using MediatR;
using SFA.DAS.AANHub.Application.Common.Validators.RequestedByMemberId;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Common;

namespace SFA.DAS.AANHub.Application.Members.Queries.GetMembers;

public class GetMembersQuery : IRequest<ValidatedResponse<GetMembersQueryResult>>, IRequestedByMemberId
{
    public Guid RequestedByMemberId { get; set; }
    public List<MemberUserType> UserType { get; set; } = new List<MemberUserType>();
    public bool? IsRegionalChair { get; set; }
    public List<int> RegionIds { get; set; } = new List<int>();
    public string? Keyword { get; set; }
    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = Domain.Common.Constants.Members.PageSize;
}