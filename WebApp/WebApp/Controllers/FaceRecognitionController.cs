using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApp.DataGenerator;
using WebApp.Helpers;
using WebApp.Models;

namespace WebApp.Controllers
{
	[Produces("application/json")]
	[Route("api/FaceRecognition")]
	public class FaceRecognitionController : Controller
	{
		[HttpPost]
		public Patient Post(string data)
		{
			try
			{
				//string realBase = base64.Substring(base64.IndexOf(',') + 1);
				//realBase = realBase.Trim('\0');
				//byte[] bytes = System.Convert.FromBase64String(base64);
				var base64Data = Regex.Match(data, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;
				var binData = Convert.FromBase64String(base64Data);
				var service = new FaceServiceHelper();
				var info = service.UploadAndDetectFaces(binData);
				return info;
			}
			catch(Exception e)
			{
				return PatientGenerator.Create().FirstOrDefault();
			}
		}
	}
}
