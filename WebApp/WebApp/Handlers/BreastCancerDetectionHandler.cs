using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AspNetCore.Http.Extensions;
using Newtonsoft.Json;
using WebApp.Models;

namespace WebApp.Handlers
{
	public class BreastCancerDetectionHandler
	{
		public static string Detect()
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
								Values = new[,] {  { "0", "8", "2", "5", "10", "1", "2", "3", "6", "3" } }
							}
						},
					}
				};

				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "IilteOioPVH/YIBZv0vrVTrlNagxlLoy5xycrISG4v9BmoraYxkLgbn4bEaVpad+MHQtXDOnGUAn98ZXuCIujw==");
				HttpResponseMessage response = client.PostAsJsonAsync("https://ussouthcentral.services.azureml.net/workspaces/23533c5facbe443ca4e19c86e7493546/services/d21a77dce923426f90b92ee402a097f6/execute?api-version=2.0&details=true", scoreRequest).Result;

				string result;
				if (response.IsSuccessStatusCode)
				{
					result = response.Content.ReadAsStringAsync().Result;
				}
				else
				{
					result = response.Content.ReadAsStringAsync().Result;
				}
				var root = JsonConvert.DeserializeObject<BreastCancerObject>(result);
				var values = root.Results.output1.value.Values.LastOrDefault();
				var Sp = values.LastOrDefault();

				var t2 = Double.Parse(Sp.Replace(".", ","));
				t2 = t2 * 100;

				return ((int)t2).ToString();
			}
		}
	}


	public class BreastCancerObject
	{
		public Results Results { get; set; }
	}

	public class Results
	{
		public Output1 output1 { get; set; }
	}

	public class Output1
	{
		public string type { get; set; }
		public Value value { get; set; }
	}

	public class Value
	{
		public string[] ColumnNames { get; set; }
		public string[] ColumnTypes { get; set; }
		public string[][] Values { get; set; }
	}

}
