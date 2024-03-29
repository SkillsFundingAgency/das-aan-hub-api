﻿using System.Diagnostics.CodeAnalysis;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Data.Repositories;

[ExcludeFromCodeCoverage]
internal class ApprenticesWriteRepository : IApprenticesWriteRepository
{
    private readonly AanDataContext _aanDataContext;

    public ApprenticesWriteRepository(AanDataContext aanDataContext) => _aanDataContext = aanDataContext;

    public void Create(Apprentice apprentice) => _aanDataContext.Apprentices.Add(apprentice);
}