using System.Diagnostics.CodeAnalysis;
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
        var status = GenerateStatusSql(options.Status);

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
                      {((!string.IsNullOrEmpty(keywordSql) || !string.IsNullOrEmpty(regions) || !string.IsNullOrEmpty(userType) || !string.IsNullOrEmpty(status)) ? " WHERE  " : "")}
                      {keywordSql}
                      {((!string.IsNullOrEmpty(keywordSql) && !string.IsNullOrEmpty(regions)) ? " AND " : "") + regions}
                      {(((!string.IsNullOrEmpty(keywordSql) || !string.IsNullOrEmpty(regions)) && !string.IsNullOrEmpty(userType)) ? " AND " : "") + userType}
                      {(((!string.IsNullOrEmpty(keywordSql) || !string.IsNullOrEmpty(regions) || !string.IsNullOrEmpty(userType)) && !string.IsNullOrEmpty(status)) ? " AND " : "") + status}
                      ORDER BY Mem.[FullName]  
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
                    subSqlQuery = $" Mem.[UserType] = '{userType[0]}'";
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

    private static string GenerateStatusSql(List<MembershipStatusType> status)
    {
        string subSqlQuery = string.Empty;
        if (status != null && status.Count > 0)
        {
            switch (status.Count)
            {
                case 1:
                    subSqlQuery = $" Mem.[Status] = '{status[0]}'";
                    break;
                default:
                    subSqlQuery = " Mem.[Status] IN ('";
                    subSqlQuery += string.Join("','", status.ToList());
                    subSqlQuery += "')";
                    break;
            }
        }
        return subSqlQuery;
    }
}
