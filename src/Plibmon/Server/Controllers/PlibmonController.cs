using Microsoft.AspNetCore.Mvc;
using Plibmon.Domain;
using Plibmon.Shared;

namespace Plibmon.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
// [Route("api")]
public class PlibmonController : Controller
{
    private readonly IPlibmonService _plibmon;

    public PlibmonController(IPlibmonService plibmon)
    {
        _plibmon = plibmon ?? throw new ArgumentNullException(nameof(plibmon));
    }
    
    // GET: api/Plibmon
    // [HttpGet]
    // public Task<bool> Get(CancellationToken cancellationToken)
    // {
    //     return _plibmon.CanConnectToPlex(cancellationToken);
    // }

    [HttpGet]
    // [Route("api/getpinlink")]
    public async Task<string> GetPinLink(CancellationToken cancellationToken)
    {
        var pinLink = await _plibmon.GetPinLink(cancellationToken);
        return pinLink switch
        {
            PinLinkResult.PinLinkSuccess s => s.PinLink,
            PinLinkResult.PinLinkFailure f => f.ErrorMessage,
            _ => "An unknown failure has occurred."
        };
    }
}