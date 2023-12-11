using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using webProgProje.Models;

/*
 
    MODEL GERÇEKLEMESİ İÇİN
    BAŞTA PARAMETRESİZ FONK SONRA AYNI İSİMDE PARAMETRELİ VE [HTTPPOST] ETIKETIYLE KULLANMAK LAZIM
 
 */

namespace webProgProje.Controllers
{
    public class AdminController : Controller
    {
        private CombineContext _combineContext=new CombineContext();
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult KisiEkle()
        {
            return View();
        }

        //[HttpPost]
        //public IActionResult KisiEkle(Kullanici k)
        //{
        //    //Doktor d = new Doktor();
        //    //Hasta h = new Hasta();
        //    string hata = "ekleme başarısız";
        //    string hata2 = "bu tc'ye sahip bir kullanıcı bulunmaktadır.";
        //    string valid = k.TC + " tc'li kullanıcı başarıyla eklendi.";
        //    var varmi = _combineContext.Kullanicilar.FirstOrDefault(x => x.TC == k.TC);
        //    if (ModelState.IsValid)
        //    {
        //        if (varmi == null)
        //        {
        //            _combineContext.Kullanicilar.Add(k);
        //            _combineContext.SaveChanges();
        //            //if (k.KullaniciTipi == "doktor")
        //            //{
        //            //    doktorEkle(d);
        //            //}
        //            //else if (k.KullaniciTipi == "hasta")
        //            //{
        //            //    hastaEkle(h);
        //            //}
        //            TempData["admin_kisiEkle"] = valid;
        //            return RedirectToAction("Index");
        //        }
        //        else
        //        {
        //            TempData["admin_kisiEkle"] = hata2;
        //            return RedirectToAction("AdminKisiMesaj");
        //        }
        //    }
        //    TempData["admin_kisiEkle"] = hata;
        //    return RedirectToAction("AdminKisiMesaj");
        //}
        public IActionResult KisiEklePage()
        {
            return View();
        }

        //DOKTOR EKLEME
        public IActionResult DoktorEkle()
        {
            return View();
        }
        public IActionResult DoktorEkle(Kullanici k)
        {
            Doktor d = new Doktor();
            string hata = "ekleme başarısız";
            string hata2 = "bu tc'ye sahip bir kullanıcı bulunmaktadır.";
            string valid = k.TC + " tc'li kullanıcı başarıyla eklendi.";
            var varmi = _combineContext.Kullanicilar.FirstOrDefault(x => x.TC == k.TC);
            if (ModelState.IsValid)
            {
                d.TC = k.TC;
                d.AnadalID = k.AnadalID;
                if (varmi == null)
                {
                    _combineContext.Kullanicilar.Add(k);
                    _combineContext.SaveChanges();
                    _combineContext.Doktorlar.Add(d);
                    _combineContext.SaveChanges();
                    TempData["admin_kisiEkle"] = valid;
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["admin_kisiEkle"] = hata2;
                    return RedirectToAction("AdminKisiMesaj");
                }
            }
            TempData["admin_kisiEkle"] = hata;
            return RedirectToAction("AdminKisiMesaj");
        }

        //HASTA EKLEME
        public IActionResult HastaEkle()
        {
            return View();
        }
        [HttpPost]
        public IActionResult HastaEkle(Kullanici k)
        {
            Hasta h = new Hasta();
            string hata = "ekleme başarısız";
            string hata2 = "bu tc'ye sahip bir kayıt bulunmaktadır.";
            string valid = k.TC + " tc'li hasta başarıyla eklendi.";
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
                    TempData["admin_kisiEkle"] = valid;
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["admin_kisiEkle"] = hata2;
                    return RedirectToAction("AdminKisiMesaj");
                }
            }
            TempData["admin_kisiEkle"] = hata;
            return RedirectToAction("AdminKisiMesaj");
        }

        //RANDEVU EKLEME
        public IActionResult RandevuEkle()
        {
            var anadallar = _combineContext.Anadallar.OrderBy(x => x.AnadalAd).ToList();
            var doktorlar=_combineContext.Doktorlar.Include(x=>x.Kullanici).ToList();
            Randevu randevu = new Randevu();
            randevu.AnadalList = anadallar;
            randevu.DoktorList = doktorlar;
            return View(randevu);
        }
        
        [HttpPost]
        public IActionResult RandevuEkle(Randevu r)
        {
            string hata = "ekleme başarısız";
            string hata2 = "bu id'ye sahip bir randevu bulunmaktadır.";
            string valid = r.RandevuID + " id'li randevu başarıyla eklendi.";
            var varmi=_combineContext.Randevular.FirstOrDefault(x=>x.RandevuID == r.RandevuID);
            ModelState.Remove(nameof(r.HastaID));
            if(ModelState.IsValid)
            {
                if (r.AnadalID == 0 || r.DoktorID == 0)
                {
                    TempData["admin_randevuEkle"] = "Doktor veya Anadal kaydı bulunamadı. Kontrol ediniz.";
                    return RedirectToAction("AdminRandevuMesaj");
                }
                if (varmi == null)
                {
                    _combineContext.Randevular.Add(r);
                    _combineContext.SaveChanges();
                    TempData["admin_randevuEkle"] = valid;
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["admin_randevuEkle"] = hata2;
                    return RedirectToAction("AdminRandevuMesaj");
                }
            }
            TempData["admin_randevuEkle"] = hata;
            return RedirectToAction("AdminRandevuMesaj");
        }

        //BİLGİLENDİRME YÖNLENDİRMELERİ

        public IActionResult AdminKisiMesaj()
        {
            if (TempData["admin_kisiEkle"] is null)
                return RedirectToAction("KisiEkle");
            return View();
        }
        public IActionResult AdminRandevuMesaj()
        {
            if (TempData["admin_randevuEkle"]is null)
                return RedirectToAction("RandevuEkle");
            return View();
        }
        public IActionResult KullaniciListele()
        {
			var kullanicilar = from Kullanici in _combineContext.Kullanicilar
							where Kullanici.KullaniciTipi != "ADMIN"
							select Kullanici;
			return View(kullanicilar);
        }
        public IActionResult RandevuListele()
        {
			var randevular = from r in _combineContext.Randevular
                             select r;
			return View(randevular);  
        }
    }   

}
