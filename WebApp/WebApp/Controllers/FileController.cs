using System;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

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
			var base64Data = Regex.Match(data, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;
			var binData = Convert.FromBase64String(base64Data);
			return "test";
		}
	}
}