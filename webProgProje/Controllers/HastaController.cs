using Microsoft.AspNetCore.Mvc;

namespace webProgProje.Controllers
{
    public class HastaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult RandevuAl()
        {
            return View();
        }
    }
}
