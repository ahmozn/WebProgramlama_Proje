using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using webProgProje.Models;

namespace webProgProje.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private CombineContext _combineContext=new CombineContext();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var doktorlar=from Kullanici in _combineContext.Kullanicilar
                         where Kullanici.KullaniciTipi=="doktor"
                         select Kullanici;
            return View(doktorlar);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}