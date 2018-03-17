using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using WebApp.DataGenerator;

namespace WebApp.Controllers
{
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			//InvokeRequestResponseService();
			return View(PatientGenerator.Create());
		}

		static async Task InvokeRequestResponseService()
		{
			using (var client = new HttpClient())
			{
				var scoreRequest = new
				{

					Inputs = new Dictionary<string, StringTable>() {
						{
							"input1",
							new StringTable()
							{
								ColumnNames = new string[] {"Class", "age", "menopause", "tumor-size", "inv-nodes", "node-caps", "deg-malig", "breast", "breast-quad", "irradiat"},
								Values = new string[,] {  { "0", "8", "2", "5", "10", "1", "2", "3", "6", "3" } }
							}
						},
					},
					GlobalParameters = new Dictionary<string, string>()
					{
					}
				};
				const string apiKey = "IilteOioPVH/YIBZv0vrVTrlNagxlLoy5xycrISG4v9BmoraYxkLgbn4bEaVpad+MHQtXDOnGUAn98ZXuCIujw=="; // Replace this with the API key for the web service
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

				client.BaseAddress = new Uri("https://ussouthcentral.services.azureml.net/workspaces/23533c5facbe443ca4e19c86e7493546/services/d21a77dce923426f90b92ee402a097f6/execute?api-version=2.0&details=true");


				HttpResponseMessage response = await client.PostAsJsonAsync("https://ussouthcentral.services.azureml.net/workspaces/23533c5facbe443ca4e19c86e7493546/services/d21a77dce923426f90b92ee402a097f6/execute?api-version=2.0&details=true", scoreRequest).ConfigureAwait(false);

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

	public class StringTable
	{
		public string[] ColumnNames { get; set; }
		public string[,] Values { get; set; }
	}
}
