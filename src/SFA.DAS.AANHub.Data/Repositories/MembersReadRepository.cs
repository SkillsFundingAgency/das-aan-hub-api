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

    public async Task<Member?> GetMemberByEmail(string Email) => await _aanDataContext
        .Members
        .AsNoTracking()
        .Where(m => m.Email == Email)
        .SingleOrDefaultAsync();

    public async Task<List<MembersSummary>> GetMembers(GetMembersOptions options, CancellationToken cancellationToken)
    {
        var regions = GenerateRegionsSql(options.RegionIds);
        var userType = GenerateUserTypeSql(options.UserType, options.IsRegionalChair);

        var keywordSql = options.KeywordCount switch
        {
            1 => " FREETEXT(FullName,'" + options.Keyword?.Trim() + "') ",
            > 1 => " CONTAINS(FullName,'\"" + options.Keyword?.Trim() + "\"') ",
            _ => string.Empty
        };

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
                      {((!string.IsNullOrEmpty(keywordSql) || !string.IsNullOrEmpty(regions) || !string.IsNullOrEmpty(userType)) ? " WHERE  " : "")}
                      {keywordSql}
                      {((!string.IsNullOrEmpty(keywordSql) && !string.IsNullOrEmpty(regions)) ? " AND " : "") + regions}
                      {(((!string.IsNullOrEmpty(keywordSql) || !string.IsNullOrEmpty(regions)) && !string.IsNullOrEmpty(userType)) ? " AND " : "") + userType}
                      ORDER BY Mem.[Id]  
                      OFFSET {(options.Page - 1) * options.PageSize} ROWS 
                      FETCH NEXT {options.PageSize} ROWS ONLY";

        var members = await _aanDataContext.MembersSummaries!
            .FromSqlRaw(sql)
            .ToListAsync(cancellationToken);
        return members;
    }
    private static string GenerateRegionsSql(IReadOnlyCollection<int> regions)
    {
        switch (regions.Count)
        {
            case 0:
                return "";
            case 1:
                return $" Reg.Id = {regions.First()}";
            default:
                var eventTypes = " Reg.Id IN (";
                eventTypes += string.Join(",", regions.ToList());
                eventTypes += ")";
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
                    subSqlQuery = $" Mem.[UserType] = '{userType.First()}'";
                    break;
                default:
                    subSqlQuery = " Mem.[UserType] IN ('";
                    subSqlQuery += string.Join("','", userType.ToList());
                    subSqlQuery += "')";
                    break;
            }
        }
        else if (isRegionalChair is not null)
        {
            subSqlQuery = $" Mem.[IsRegionalChair] = {(isRegionalChair.Value ? 1 : 0)}";
        }
        return subSqlQuery;
    }
}
