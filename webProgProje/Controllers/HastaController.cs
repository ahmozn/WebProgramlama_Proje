using Microsoft.AspNetCore.Mvc;

namespace webProgProje.Controllers
{
    public class HastaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
