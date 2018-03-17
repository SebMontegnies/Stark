using Microsoft.AspNetCore.Mvc;
using WebApp.DataGenerator;
using WebApp.Handlers;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			return View(PatientGenerator.Create());
		}

		public IActionResult Scanner()
		{
			return View();
		}

		public IActionResult GeneralInformation()
		{
			return View();
		}

		public IActionResult Upload()
		{
			return View();
		}

		public IActionResult Confirmation()
		{
			var viewModel = new ConfirmationViewModel();
			viewModel.PositvePercentage = DiabetesDetectionHandler.Diabetes();
			viewModel.NegativePercentage = 100 - viewModel.PositvePercentage;
			return View(viewModel);
		}
	}
}