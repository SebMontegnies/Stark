using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    [Produces("application/json")]
    [Route("api")]
    public class DataCollectorController : Controller
    {
		// api/temperature/
		[Route("temperature")]
        [HttpPost]
        public HttpResponseMessage Temperature(double value)
		{
			return new HttpResponseMessage(HttpStatusCode.OK);
		}

		// api/Heartbeat/
		[Route("heartbeat")]
	    [HttpPost]
	    public HttpResponseMessage Heartbeat(double value)
	    {
		    return new HttpResponseMessage(HttpStatusCode.OK);
	    }

	    // api/bloodoxgyneation
	    [Route("bloodoxygenation")]
	    [HttpPost]
	    public HttpResponseMessage BloodOxygenation(double value)
	    {
		    return new HttpResponseMessage(HttpStatusCode.OK);
	    }
    }
}
