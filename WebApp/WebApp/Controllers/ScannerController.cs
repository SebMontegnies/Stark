using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class ScannerController : Controller
    {
        // GET: Scanner
        public ActionResult Index()
        {
            return View();
        }
    }
}