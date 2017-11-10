using Microsoft.AspNetCore.Mvc;

namespace Lab43George.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
