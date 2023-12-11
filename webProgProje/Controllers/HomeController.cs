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
        public IActionResult Anasayfa()
        {
            return View();
        }

        public IActionResult Hesap() 
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
        [HttpPost]
        public IActionResult Login(Kullanici usr)
        {
            var kullanicilar = _combineContext.Kullanicilar.FirstOrDefault(x => x.TC == usr.TC && x.Sifre == usr.Sifre);
            if (kullanicilar != null)
            {
                if (kullanicilar.KullaniciTipi == "doktor")
                {
                    HttpContext.Session.SetString("SessionUser", kullanicilar.Ad);
                    return RedirectToActionPermanent("","Doktor");
                }
                else if (kullanicilar.KullaniciTipi == "hasta")
                {
                    HttpContext.Session.SetString("SessionUser", kullanicilar.Ad);
                    return RedirectToActionPermanent("Hesap","Hasta");
                }
            }
            TempData["hata"] = "kullanıcı adı veya şifre hatalı";
            return RedirectToAction("LoginPage");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        public IActionResult Signup()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Signup(Kullanici k)
        {
            Hasta h = new Hasta();
            string hata = "Kayıt başarısız";
            string hata2 = "Bu tc'ye sahip bir kayıt bulunmaktadır.";
            string valid = "Kayıt başarılı.";
            var varmi = _combineContext.Kullanicilar.FirstOrDefault(x => x.TC == k.TC);
            if (ModelState.IsValid)
            {
                h.TC = k.TC;
                if (varmi == null)
                {
                    _combineContext.Kullanicilar.Add(k);
                    _combineContext.SaveChanges();
                    _combineContext.Hastalar.Add(h);
                    _combineContext.SaveChanges();
                    TempData["signup"] = valid;
                    return RedirectToAction("Hesap");
                }
                else
                {
                    TempData["signup"] = hata2;
                    return RedirectToAction("Signup");
                }
            }
            TempData["signup"] = hata;
            return RedirectToAction("Anasayfa");
        }

        public IActionResult RandevuAl()
        {
            if (HttpContext.Session.GetString("SessionUser") is null)
            {
                TempData["msj"] = "Lütfen giriş yapınız.";
                return RedirectToAction("LoginPage");
            }
            return View();
        }
        public IActionResult RandevuAl1(int id)
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