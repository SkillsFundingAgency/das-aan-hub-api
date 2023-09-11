﻿using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.AANHub.Domain.Common;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using SFA.DAS.AANHub.Domain.Models;

namespace SFA.DAS.AANHub.Data.Repositories;

[ExcludeFromCodeCoverage]
internal class MembersReadRepository : IMembersReadRepository
{
    private readonly AanDataContext _aanDataContext;

    public MembersReadRepository(AanDataContext aanDataContext) => _aanDataContext = aanDataContext;

    public async Task<Member?> GetMember(Guid id) => await _aanDataContext
        .Members
        .AsNoTracking()
        .Where(m => m.Id == id)
        .SingleOrDefaultAsync();

    public async Task<Member?> GetMemberByEmail(string email) => await _aanDataContext
        .Members
        .AsNoTracking()
        .Where(m => m.Email == email)
        .SingleOrDefaultAsync();

    public async Task<List<MembersSummary>> GetMembers(GetMembersOptions options, CancellationToken cancellationToken)
    {
        var regions = GenerateRegionsSql(options.RegionIds);
        var userType = GenerateUserTypeSql(options.UserType, options.IsRegionalChair);
        var keywordSql = GenerateKeywordSql(options.Keyword?.Trim());

        var sql = $@"SELECT Mem.[Id] AS MemberId
                      ,COUNT(*) OVER () TotalCount
                      ,Mem.[FullName]
	                  ,Mem.[RegionId]
	                  ,Reg.[Area] AS RegionName
	                  ,Mem.[UserType]
	                  ,Mem.[IsRegionalChair]
                      ,Mem.[JoinedDate]
                      FROM [SFA.DAS.AANHub.Database].[dbo].[Member] AS Mem
                      LEFT JOIN [SFA.DAS.AANHub.Database].[dbo].[Region] AS Reg ON Mem.RegionId = Reg.Id
                       WHERE  Mem.[Status] = '{MembershipStatusType.Live}'
                      {keywordSql}
                      {((!string.IsNullOrEmpty(regions)) ? " AND " : string.Empty) + regions}
                      {((!string.IsNullOrEmpty(userType)) ? " AND " : string.Empty) + userType}
                      ORDER BY Mem.[FullName]  
                      OFFSET {(options.Page - 1) * options.PageSize} ROWS 
                      FETCH NEXT {options.PageSize} ROWS ONLY";

        var members = await _aanDataContext.MembersSummaries!
            .FromSqlRaw(sql)
            .ToListAsync(cancellationToken);
        return members;
    }

    private static string GenerateKeywordSql(string? keyword)
    {
        if (!string.IsNullOrEmpty(keyword))
        {
            return $" AND Mem.[FullName] LIKE '%{keyword}%' ";
        }
        return string.Empty;
    }

    private static string GenerateRegionsSql(IReadOnlyCollection<int> regions)
    {
        switch (regions.Count)
        {
            case 0:
                return string.Empty;
            case 1:
                if (regions.Any(region => region == 0))
                {
                    return $" Reg.Id IS NULL";
                }
                else
                {
                    return $" Reg.Id = {regions.First()}";
                }
            default:
                var eventTypes = " Reg.Id IN (";
                eventTypes += string.Join(",", regions.Where(region => region != 0).ToList());
                eventTypes += ")";
                if (regions.Any(region => region == 0))
                {
                    eventTypes = " ( " + eventTypes + " OR Reg.Id IS NULL)";
                }
                return eventTypes;
        }
    }

    private static string GenerateUserTypeSql(List<MemberUserType> userType, bool? isRegionalChair)
    {
        string subSqlQuery = string.Empty;
        if (userType != null && userType.Count > 0)
        {
            switch (userType.Count)
            {
                case 1:
                    subSqlQuery = $" Mem.[UserType] = '{userType[0]}' " + ((isRegionalChair is not null && isRegionalChair.Value) ? " OR Mem.[IsRegionalChair] = 1" : string.Empty);
                    break;
                default:
                    subSqlQuery = " Mem.[UserType] IN ('";
                    subSqlQuery += string.Join("','", userType.ToList());
                    subSqlQuery += "')  " + ((isRegionalChair is not null && isRegionalChair.Value) ? " OR Mem.[IsRegionalChair] = 1" : string.Empty);
                    break;
            }
        }
        else if (isRegionalChair is not null)
        {
            subSqlQuery = $" Mem.[IsRegionalChair] = {(isRegionalChair.Value ? 1 : 0) + " " + (!isRegionalChair.Value ? " OR Mem.[IsRegionalChair] IS NULL" : string.Empty)}";
        }
        return subSqlQuery;
    }

    public async Task<List<Member>> GetMembers(List<Guid> memberIds, CancellationToken cancellationToken) => await _aanDataContext
        .Members
        .AsNoTracking()
        .Where(m => memberIds.Contains(m.Id))
        .ToListAsync(cancellationToken);

}
