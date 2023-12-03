using Microsoft.AspNetCore.Mvc;

namespace webProgProje.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
