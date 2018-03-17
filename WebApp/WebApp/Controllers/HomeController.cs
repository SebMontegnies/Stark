using AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
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

		static async Task InvokeRequestResponseService()
		{
			using (var client = new HttpClient())
			{
				var scoreRequest = new
				{
					Inputs = new Dictionary<string, List<Dictionary<string, string>>>() {
						{
							"input1",
							new List<Dictionary<string, string>>(){new Dictionary<string, string>(){
											{
												"class", "1"
											},
											{
												"age", "45"
											},
											{
												"heartbeat", "100"
											},
											{
												"sp02", "70"
											},
											{
												"height", "175"
											},
											{
												"weight", "120"
											},
											{
												"sex", "1"
											},
											{
												"chol", "4"
											},
											{
												"temp", "38"
											},
								}
							}
						},
					},
					GlobalParameters = new Dictionary<string, string>()
					{
					}
				};

				const string apiKey = "+/yacIXKwg/gPh+mqVerS/j0WDLK1DcsY/FLVpj3VeuCapvcdMy4EELtzIZfUAK1eA68NvEj7UaSkF4QvJr4oQ=="; // Replace this with the API key for the web service
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
				client.BaseAddress = new Uri("https://ussouthcentral.services.azureml.net/workspaces/23533c5facbe443ca4e19c86e7493546/services/89cd6441d6d344509ecbf5e05cdc9433/execute?api-version=2.0&format=swagger");

				HttpResponseMessage response = await client.PostAsJsonAsync("", scoreRequest).ConfigureAwait(false);

				if (response.IsSuccessStatusCode)
				{
					string result = await response.Content.ReadAsStringAsync();
				}
				else
				{
					string responseContent = await response.Content.ReadAsStringAsync();
				}
			}
		}
	}


}