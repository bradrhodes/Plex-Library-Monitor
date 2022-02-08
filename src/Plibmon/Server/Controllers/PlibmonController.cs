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
    [HttpGet("CanConnectToPlex")]
    public Task<bool> CanConnectToPlex(CancellationToken cancellationToken)
    {
        return _plibmon.CanConnectToPlex(cancellationToken);
    }

    [HttpGet("GetPinLink")]
    // [Route("api/getpinlink")]
    public async Task<string> GetPinLink(CancellationToken cancellationToken)
    {
        // Get a pin link
        var pinResult = await _plibmon.GetPinLink(cancellationToken).ConfigureAwait(false);
        
        // Start polling to see if it is validated
        _plibmon.PollForPinAuthorization();

        return pinResult switch
        {
            PinLinkResult.PinLinkSuccess s => s.PinLink,
            PinLinkResult.PinLinkFailure f => f.ErrorMessage, 
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}