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

        public IActionResult LoginPage()
        {
            if (HttpContext.Session.GetString("SessionUser") is not null)
            {
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Login(Kullanici usr)
        {
            var kullanicilar = _combineContext.Kullanicilar.FirstOrDefault(x => x.TC == usr.TC && x.Sifre == usr.Sifre);
            if (kullanicilar != null)
            {
                HttpContext.Session.SetString("SessionUser", kullanicilar.Ad);
                TempData["msj"] = "hoş geldiniz"+kullanicilar.Ad;
                return RedirectToAction("Index");
            }
            TempData["hata"] = "kullanıcı adı veya şifre hatalı";
            return RedirectToAction("LoginPage");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        public IActionResult SignupPage()
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