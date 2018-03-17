using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using AspNetCore.Http.Extensions;
using Newtonsoft.Json;
using WebApp.Models;

namespace WebApp.Handlers
{
	public class CardiacDetectionHandler
	{
		public static string Detect()
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
										"heartbeat", "200"
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
									{
										"arythmie","1"
									}
								}
							}
						},
					},
					GlobalParameters = new Dictionary<string, string>()
					{
					}
				};

				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "+/yacIXKwg/gPh+mqVerS/j0WDLK1DcsY/FLVpj3VeuCapvcdMy4EELtzIZfUAK1eA68NvEj7UaSkF4QvJr4oQ==");
				client.BaseAddress = new Uri("https://ussouthcentral.services.azureml.net/workspaces/23533c5facbe443ca4e19c86e7493546/services/89cd6441d6d344509ecbf5e05cdc9433/execute?api-version=2.0&format=swagger");

				HttpResponseMessage response = client.PostAsJsonAsync("", scoreRequest).Result;

				string result;
				if (response.IsSuccessStatusCode)
				{
					result = response.Content.ReadAsStringAsync().Result;
				}
				else
				{
					result = response.Content.ReadAsStringAsync().Result;
				}


				var test = JsonConvert.DeserializeObject<CardiacObject>(result, new JsonSerializerSettings()
				{
					FloatFormatHandling = FloatFormatHandling.String
				});

				var root = JsonConvert.DeserializeObject<CardiacObject>(result, new DecimalFormatConverter());
				var Sp = root.Results.output1.FirstOrDefault().ScoredProbabilities;

				var t2 = Double.Parse(Sp.Replace(".",","));
				t2 = t2 * 100;

				return t2.ToString();
			}
		}
	}


	public class CardiacObject
	{
		public ResultsCardiac Results { get; set; }
	}

	public class ResultsCardiac
	{
		public OutputCardiac[] output1 { get; set; }
	}

	public class OutputCardiac
	{
		public string _class { get; set; }
		public string age { get; set; }
		public string heartbeat { get; set; }
		public string sp02 { get; set; }
		public string height { get; set; }
		public string weight { get; set; }
		public string sex { get; set; }
		public string chol { get; set; }
		public string temp { get; set; }
		public string arythmie { get; set; }
		public string ScoredLabels { get; set; }
		[JsonProperty("Scored Probabilities")]
		public string ScoredProbabilities { get; set; }
	}

	public class DecimalFormatConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			return (objectType == typeof(decimal));
		}

		public override void WriteJson(JsonWriter writer, object value,
			JsonSerializer serializer)
		{
			writer.WriteValue(string.Format("{0:N2}", value));
		}

		public override bool CanRead
		{
			get { return false; }
		}

		public override object ReadJson(JsonReader reader, Type objectType,
			object existingValue, JsonSerializer serializer)
		{
			throw new NotImplementedException();
		}
	}

}
