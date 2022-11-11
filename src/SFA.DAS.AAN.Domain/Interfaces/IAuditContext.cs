using SFA.DAS.AAN.Domain.Entities;
using SFA.DAS.AAN.Domain.Entities.Audit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.AAN.Domain.Interfaces
{
    public interface IAuditContext : ISaveableEntityContext<AuditData>
    {
    }
}
