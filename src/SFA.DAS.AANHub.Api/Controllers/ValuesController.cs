using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.AANHub.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ValuesController : ControllerBase
{
    [HttpGet]
    public IActionResult Index([FromHeader(Name = "X-RequestedByMemberId")] string requestedByMemberId)
    {
        return Ok(new { requestedByMemberId });
    }
}
