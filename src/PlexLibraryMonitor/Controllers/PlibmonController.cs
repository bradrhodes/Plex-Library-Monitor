using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Plibmon.Domain;

namespace PlexLibraryMonitor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlibmonController : ControllerBase
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

        // GET: api/Plibmon/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Plibmon
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Plibmon/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/Plibmon/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
