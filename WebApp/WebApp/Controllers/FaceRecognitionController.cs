using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
		[HttpGet]
		public Patient Get()
		{
			try
			{
				var client = new WebClient();
				var image = client.DownloadData("http://s1.lprs1.fr/images/2016/12/16/6464877_e1e93498-c39d-11e6-92ce-2cd01abc5747-1.jpg");
				var service = new FaceServiceHelper();
				var info = service.UploadAndDetectFaces(image);
				return info;
			}
			catch
			{
				return PatientGenerator.Create().FirstOrDefault();
			}
		}
	}
}
