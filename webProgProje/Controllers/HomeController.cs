using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        //ANASAYFA VIEW
        public IActionResult Anasayfa()
        {
            return View();
        }

        //HESABA YÖNLENDİRME
        public IActionResult Hesap() 
        {
            return RedirectToAction("LoginPage");
        }

        //GİRİŞ İŞLEMİ
        public IActionResult LoginPage()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index");
            }
            //if (HttpContext.Session.GetString("SessionUser") is not null)
            //{
            //    return RedirectToAction("Index");
            //}
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
            TempData["hata"] = "kullanıcı adı veya şifre hatalı";
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
                TempData["signup"] = "Telefon numaranızı kontrol ediniz.";
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
                    TempData["signup"] = valid;
                    return RedirectToAction("Hesap","Hasta");
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

        //ÇIKIŞ İŞLEMİ
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Anasayfa");
        }

        //RANDEVU AL SECENEGI
        public IActionResult RandevuAl()
        {
            //identity yonlendirme
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                TempData["msj"] = "Lütfen giriş yapınız.";
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }
            return RedirectToAction("RandevuListele","Hasta");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}