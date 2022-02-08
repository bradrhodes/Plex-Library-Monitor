using Hangfire;
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
    private readonly IRecurringJobManager _recurringJobManager;

    public PlibmonController(IPlibmonService plibmon, IRecurringJobManager recurringJobManager)
    {
        _plibmon = plibmon ?? throw new ArgumentNullException(nameof(plibmon));
        _recurringJobManager = recurringJobManager ?? throw new ArgumentNullException(nameof(recurringJobManager));
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
        var pinResult = await _plibmon.GetPinLink(cancellationToken);
        
        // Start polling
        _recurringJobManager.AddOrUpdate("PinValidationPoller", () => _plibmon.);

        return pinResult switch
        {
            PinLinkResult.PinLinkSuccess s => s.PinLink,
            PinLinkResult.PinLinkFailure f => f.ErrorMessage, 
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}