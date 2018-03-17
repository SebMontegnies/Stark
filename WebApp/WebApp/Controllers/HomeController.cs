using Microsoft.AspNetCore.Mvc;
using WebApp.DataGenerator;

namespace WebApp.Controllers
{
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			return View(PatientGenerator.Create());
		}

		public IActionResult GeneralInformation()
		{
			return View();
		}
	}
}