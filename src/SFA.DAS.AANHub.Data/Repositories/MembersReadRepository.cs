﻿using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Data.Repositories
{
    [ExcludeFromCodeCoverage]
    internal class MembersReadRepository : IMembersReadRepository
    {
        private readonly AanDataContext _aanDataContext;

        public MembersReadRepository(AanDataContext aanDataContext) => _aanDataContext = aanDataContext;

        public async Task<Member?> GetMember(Guid? Id) => await _aanDataContext
                .Members
                .AsNoTracking().Where(m => m.Id == Id)
                .SingleOrDefaultAsync();
    }
}