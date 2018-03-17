using Microsoft.AspNetCore.Mvc;
using WebApp.DataGenerator;
using WebApp.Handlers;

namespace WebApp.Controllers
{
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			var result = DiabetesDetectionHandler.Diabetes();
			return View(PatientGenerator.Create());
		}

		public IActionResult GeneralInformation()
		{
			return View();
		}
	}
}