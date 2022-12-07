using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.AANHub.Data.Repositories
{
    [ExcludeFromCodeCoverage]
    internal class EmployersWriteRepository : IEmployersWriteRepository
    {
        private readonly AanDataContext _aanDataContext;

        public EmployersWriteRepository(AanDataContext aanDataContext) => _aanDataContext = aanDataContext;

        public void Create(Employer employer) => _aanDataContext.Employers.Add(employer);
    }
}
