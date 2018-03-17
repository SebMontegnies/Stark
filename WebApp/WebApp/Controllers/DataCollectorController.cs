using Bogus;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
	[Produces("application/json")]
	[Route("api")]
	public class DataCollectorController : Controller
	{
		private static double _temperature { get; set; }
		private static double _hearbeat { get; set; }
		private static double _bloodoxygenationRate { get; set; }
		private static bool _isEnable { get; set; }

		// api/health
		[Route("health")]
		[HttpPost]
		public string Health(double temperature, double heartbeat, double bloodoxygenation)
		{
			_temperature = temperature;
			_hearbeat = heartbeat;
			_bloodoxygenationRate = bloodoxygenation;
			return temperature + " " + heartbeat + " " + bloodoxygenation;
		}

		[Route("health")]
		[HttpGet]
		public DataViewModel Health()
		{
			var model = new DataViewModel();

			model.BloodoxygenationRate = _bloodoxygenationRate;
			model.Hearbeat = _hearbeat;
			model.Temperature = _temperature;


			return model;

		}

		// api/active
		[Route("active")]
		[HttpGet]
		public bool Activate()
		{
			return _isEnable;
		}

		// api/active
		[Route("active")]
		[HttpPost]
		public bool Activate(bool enable)
		{
			_isEnable = enable;
			return enable;
		}
	}
}