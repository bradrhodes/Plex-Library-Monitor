using Microsoft.AspNetCore.Mvc;
using Plibmon.Domain;

namespace Plibmon.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlibmonController : Controller
{
    private readonly IPlibmonService _plibmon;

    public PlibmonController(IPlibmonService plibmon)
    {
        _plibmon = plibmon ?? throw new ArgumentNullException(nameof(plibmon));
    }
    
    // GET: api/Plibmon
    [HttpGet]
    public Task<bool> Get(CancellationToken cancellationToken)
    {
        return _plibmon.CanConnectToPlex(cancellationToken);
    }
}