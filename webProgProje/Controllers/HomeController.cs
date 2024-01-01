using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using webProgProje.Languages;
using webProgProje.Models;

namespace webProgProje.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHtmlLocalizer<Lang> _htmlLocalizer;
        public HomeController(IHtmlLocalizer<Lang> htmlLocalizer)
            => _htmlLocalizer = htmlLocalizer;
        //private readonly ILogger<HomeController> _logger;
        private CombineContext _combineContext=new CombineContext();

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        //ANASAYFA VIEW
        public IActionResult Anasayfa(string culture="tr-TR")
        {
            if (User.IsInRole("Admin"))
            {
                return RedirectToActionPermanent("Index", "Admin");
            }
            ViewBag.PageAbout = _htmlLocalizer["page.About"];
            ViewBag.PageHome = _htmlLocalizer["page.Home"];
            return View();
        }

        //HESABA YÖNLENDİRME
        public IActionResult Hesap() 
        {
            return RedirectToAction("LoginPage");
        }


        //IDENTITY OLMADAN GIRIS CIKIS FALAN FILAN
        /*
        //GİRİŞ İŞLEMİ
        public IActionResult LoginPage()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Anasayfa");
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
                    return RedirectToActionPermanent("Hesap","Doktor");
                }
                else if (kullanicilar.KullaniciTipi == "hasta")
                {
                    HttpContext.Session.SetString("SessionUser", kullanicilar.Ad);
                    return RedirectToActionPermanent("Hesap","Hasta");
                }
                else if (kullanicilar.KullaniciTipi == "admin")
                {
                    return RedirectToActionPermanent("Index", "Admin");
                }
            }
            ViewData["hata"] = "kullanıcı adı veya şifre hatalı";
            return RedirectToAction("LoginPage");
        }

        //KAYDOLMA İŞLEMİ
        public IActionResult Signup()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Signup(Kullanici k)
        {
            if (k.Telefon.StartsWith("05") != true)
            {
                ViewData["signup"] = "Telefon numaranızı kontrol ediniz.";
                return RedirectToAction("Signup");
            }
            Hasta h = new Hasta();
            string hata = "Kayıt başarısız";
            string hata2 = "Bu tc'ye sahip bir kayıt bulunmaktadır.";
            string valid = "Kayıt başarılı.";
            var varmi = _combineContext.Kullanicilar.FirstOrDefault(x => x.TC == k.TC);
            if (ModelState.IsValid)
            {
                h.Id = k.Id;
                if (varmi == null)
                {
                    _combineContext.Kullanicilar.Add(k);
                    _combineContext.SaveChanges();
                    _combineContext.Hastalar.Add(h);
                    _combineContext.SaveChanges();
                    ViewData["signup"] = valid;
                    return RedirectToAction("Hesap","Hasta");
                }
                else
                {
                    ViewData["signup"] = hata2;
                    return RedirectToAction("Signup");
                }
            }
            ViewData["signup"] = hata;
            return RedirectToAction("Anasayfa");
        }

        //ÇIKIŞ İŞLEMİ
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Anasayfa");
        }
        */

        //RANDEVU AL SECENEGI
        public IActionResult RandevuAl()
        {
            //identity yonlendirme
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                ViewData["msj"] = "Lütfen giriş yapınız.";
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }
            return RedirectToAction("RandevuAl","Hasta");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}