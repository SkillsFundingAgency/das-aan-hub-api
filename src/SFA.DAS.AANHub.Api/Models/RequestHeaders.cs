using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.AANHub.Api.Models
{
    public class RequestHeaders
    {
        [FromHeader(Name = "x-requested-by-userid")]
        public Guid? RequestedByUserId { get; set; }
    }
}
