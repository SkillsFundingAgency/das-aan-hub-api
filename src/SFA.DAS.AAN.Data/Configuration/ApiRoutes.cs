using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.AAN.Data.Configuration
{
    public static class ApiRoutes
    {
        private readonly static string memberBaseUri = "/member";
        public static string CreateMemberUri { get { return memberBaseUri; } }
        public static string CreateApprenticeMemberUri { get { return $"{memberBaseUri}/apprentice"; } }
        public static string CreateEmployerMemberUri { get { return $"{memberBaseUri}/employer"; } }
        public static string CreatePartnerMemberUri { get { return $"{memberBaseUri}/partner"; } }
        public static string CreateAdminMemberUri { get { return $"{memberBaseUri}/admin"; } }
        
    }
}
