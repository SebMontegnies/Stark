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
		// api/health/
		[Route("health")]
		[HttpPost]
		public string Health(double temperature, double heartbeat, double bloodoxygenation)
		{
			return temperature + " " + heartbeat + " " + bloodoxygenation;
		}
	}
}
