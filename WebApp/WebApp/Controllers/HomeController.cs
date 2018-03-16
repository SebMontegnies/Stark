using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using WebApp.Helpers;
using WebApp.Models;

namespace WebApp.Controllers
{
	public class HomeController : Controller
	{


		public IActionResult Index()
		{
			var filePath = "C:\\Users\\sebastien.montegnies\\Desktop\\clara.jpg";
			var faceService = new FaceServiceHelper();
			var face = faceService.UploadAndDetectFaces(filePath);


			var age= (int)face.FaceAttributes.Age;
			string gender = face.FaceAttributes.Gender;
			return View();
		}

		public IActionResult About()
		{
			ViewData["Message"] = "Your application description page.";

			return View();
		}

		public IActionResult Contact()
		{
			ViewData["Message"] = "Your contact page.";

			return View();
		}

		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
