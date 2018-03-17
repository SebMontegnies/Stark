using System.Globalization;
using Bogus;
using Microsoft.AspNetCore.Mvc;
using WebApp.DataGenerator;
using WebApp.Handlers;
using WebApp.Models;
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

		public IActionResult GeneralInformation(GeneralInformationViewModel model)
		{
			if (model.Temperature < 30)
			{
				var rand = new Faker().Random;
				model.Gender = Gender.Male;
				model.Temperature = rand.Int(36, 40) + rand.Decimal();
				model.Age = rand.Int(15, 85);
				model.BloodoOxygenationRate = rand.Int(70, 100) + rand.Decimal();
				model.Heartbeat = rand.Int(70, 130);
			}

			return View(model);
		}

		public IActionResult Upload(GeneralInformationViewModel model)
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