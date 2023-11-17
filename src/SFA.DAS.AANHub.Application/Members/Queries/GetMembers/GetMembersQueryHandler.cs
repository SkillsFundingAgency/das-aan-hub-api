using System.Text.RegularExpressions;
using MediatR;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using SFA.DAS.AANHub.Domain.Models;


namespace SFA.DAS.AANHub.Application.Members.Queries.GetMembers;

public class GetMembersQueryHandler : IRequestHandler<GetMembersQuery, GetMembersQueryResult>
{
    private readonly IMembersReadRepository _membersReadRepository;

    public GetMembersQueryHandler(IMembersReadRepository membersReadRepository)
    {
        _membersReadRepository = membersReadRepository;
    }

    public async Task<GetMembersQueryResult> Handle(GetMembersQuery query,
        CancellationToken cancellationToken)
    {
        var pageSize = query.PageSize;
        var page = query.Page;

        var options = new GetMembersOptions
        {
            Keyword = ProcessedKeyword(query.Keyword),
            UserType = query.UserType,
            IsRegionalChair = query.IsRegionalChair,
            RegionIds = query.RegionIds,
            Page = page,
            PageSize = pageSize
        };

        var response =
            await _membersReadRepository.GetMembers(options, cancellationToken);

        var totalCount = 0;

        if (response.Any())
        {
            totalCount = response[0].TotalCount;
        }

        var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

        var responseProcessed = response.Select(summary => (MembersSummaryModel)summary);

        var result = new GetMembersQueryResult
        {
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = totalPages,
            Members = responseProcessed
        };

        return result;
    }

    private static string? ProcessedKeyword(string? keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword)) return null;
        var rgx = new Regex("[^a-zA-Z0-9 ]", RegexOptions.None, TimeSpan.FromMilliseconds(100));
        return rgx.Replace(keyword, " ").Trim();

    }
}