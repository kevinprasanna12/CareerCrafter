using Microsoft.AspNetCore.Mvc;

namespace CareerCrafterMVC.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
