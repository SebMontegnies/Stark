using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AspNetCore.Http.Extensions;
using Bogus;
using Newtonsoft.Json;
using WebApp.Models;

namespace WebApp.Handlers
{
	public class DiabetesDetectionHandler
	{
		public static int Diabetes()
		{
			using (var client = new HttpClient())
			{
				var scoreRequest = CreateRequest();
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "HwgRcg4xmeojLv2XR+HUAqwlycanM2qSqqO8QYULH9J2VzeEGrWtaHq7UET2G/J7w+jD9ccO2PpG6w8g3E1MwA==");
				HttpResponseMessage response = client.PostAsJsonAsync("https://ussouthcentral.services.azureml.net/workspaces/23533c5facbe443ca4e19c86e7493546/services/90da378ee107449ab26e57e4334967d6/execute?api-version=2.0&details=true", scoreRequest).Result;

				string result;
				if (response.IsSuccessStatusCode)
				{
					result = response.Content.ReadAsStringAsync().Result;
				}
				else
				{
					result = response.Content.ReadAsStringAsync().Result;
				}

				var root = JsonConvert.DeserializeObject<Rootobject>(result);
				var values = root.Results.output1.value.Values.LastOrDefault();
				var percentage = Double.Parse(values.LastOrDefault());
				return (int)(percentage * 10);
			}
		}

		private static object CreateRequest()
		{
			var rand = new Faker().Random;
			int TimesPregnant = rand.Int(0, 3);
			int PlasmaGlucoseConcentration = rand.Int(50, 100);
			int DiastolicBloodPressure = rand.Int(50, 100);
			int TricepsSkinFold = rand.Int(15, 50);
			int Insulin = rand.Int(0, 3);
			double BMI = rand.Int(22, 42);
			double DiabeteFunction = rand.Double(0, 1);
			int Age = rand.Int(1, 99);
			int ClassVariable = rand.Int(0, 1);

			var scoreRequest = new
			{
				Inputs = new Dictionary<string, StringTable>()
				{
					{
						"input1",
						new StringTable()
						{
							ColumnNames = new string[]
							{
								"Number of times pregnant", "Plasma glucose concentration a 2 hours in an oral glucose tolerance test",
								"Diastolic blood pressure (mm Hg)", "Triceps skin fold thickness (mm)", "2-Hour serum insulin (mu U/ml)",
								"Body mass index (weight in kg/(height in m)^2)", "Diabetes pedigree function", "Age (years)",
								"Class variable (0 or 1)"
							},
							Values = new string[,]
							{
								{
									TimesPregnant.ToString(), PlasmaGlucoseConcentration.ToString(), DiastolicBloodPressure.ToString(),
									TricepsSkinFold.ToString(), Insulin.ToString(), BMI.ToString(), DiabeteFunction.ToString(), Age.ToString(),
									ClassVariable.ToString()
								}
							}
						}
					},
				}
			};
			return scoreRequest;
		}
	}
}