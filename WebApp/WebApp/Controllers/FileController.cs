using System;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
	[Produces("application/json")]
	[Route("api/File")]
	public class FileController : Controller
	{
		// GET: api/File
		[HttpPost]
		public string Get(string data)
		{
			var tag = Analyse();
			return tag;
		}

		public static string Analyse()
		{
			var url = "https://southcentralus.api.cognitive.microsoft.com/customvision/v1.1/Prediction/f94bd0b1-d322-4786-b06b-cfb396601790/url?iterationId=15a5ed16-2514-4784-9be3-2ebc040700ef";

			using (var wc = new WebClient())
			{
				wc.Headers.Add("Prediction-Key", "2de03941aa554a84858ef0d63f0cc2f9");
				wc.Headers.Add("content-type", "application/json");


				string HtmlResult = wc.UploadString(url, "{\"Url\":\"https://image.noelshack.com/fichiers/2018/11/6/1521274337-1.png\"}");


				var prediction = Newtonsoft.Json.JsonConvert.DeserializeObject<FileRootObject>(HtmlResult);

				return prediction.Predictions.OrderByDescending(p => p.Probability).First().Tag;
			}
		}
	}

	public class FileRootObject
	{
		public string Id { get; set; }
		public string Project { get; set; }
		public string Iteration { get; set; }
		public DateTime Created { get; set; }
		public Prediction[] Predictions { get; set; }
	}

	public class Prediction
	{
		public string TagId { get; set; }
		public string Tag { get; set; }
		public float Probability { get; set; }
	}
}