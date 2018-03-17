using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    [Produces("application/json")]
    [Route("api/File")]
    public class FileController : Controller
    {
        // GET: api/File
        [HttpGet]
        public string Get(byte[] bytes)
        {
	        return "test";
        }
    }
}