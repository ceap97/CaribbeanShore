using Microsoft.AspNetCore.Mvc;

namespace RefugioVerde.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
