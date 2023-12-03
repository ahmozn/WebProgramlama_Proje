using Microsoft.AspNetCore.Mvc;

namespace webProgProje.Controllers
{
    public class DoktorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
