using Microsoft.AspNetCore.Mvc;
using WebApp.DataGenerator;

namespace WebApp.Controllers
{
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			var patient = PatientGenerator.Create();
			return View();
		}
	}
}
