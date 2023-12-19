using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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

        //LİSTELEME İŞLEMLERİ
        public IActionResult KullaniciListele()
        {
            var kullanicilar = from Kullanici in _combineContext.Kullanicilar
                               where Kullanici.KullaniciTipi != "ADMIN"
                               select Kullanici;
            return View(kullanicilar);
        }
        public IActionResult DoktorListele()
        {
            //revize edilecek
            var doktorlar = _combineContext.Doktorlar.Include(x => x.Kullanici);
            var s = _combineContext.Kullanicilar.Where(x=>x.KullaniciTipi=="doktor").Include(x => x.Doktor).ToList();
            return View(s);
        }
        public IActionResult HastaListele()
        {
            return View();
        }

        //EKLEME BÖLÜMÜ
        public IActionResult KisiEklePage()
        {
            return View();
        }

        //-------------------------------DOKTOR İŞLEMLERİ---------------------------------
        
        //DOKTOR EKLEME
        public IActionResult DoktorEkle()
        {
            Kullanici kullanici=new Kullanici();
            var anadallar = _combineContext.Anadallar.OrderBy(x => x.AnadalAd).ToList();
            kullanici.AnadalList = anadallar;
            return View(kullanici);
        }
        [HttpPost]
        public IActionResult DoktorEkle(Kullanici k)
        {
            Doktor d = new Doktor();
            string hata = "ekleme başarısız";
            string hata2 = "bu tc'ye sahip bir doktor bulunmaktadır.";
            string valid = k.TC + " tc'li doktor başarıyla eklendi.";
            var varmi = _combineContext.Kullanicilar.FirstOrDefault(x => x.TC == k.TC);
            if (ModelState.IsValid)
            {
                d.TC = k.TC;
                d.AnadalID = k.AnadalID;
                d.DoktorDerece = k.Doktor.DoktorDerece;
                if (varmi == null)
                {
                    _combineContext.Kullanicilar.Add(k);
                    _combineContext.SaveChanges();
                    _combineContext.Doktorlar.Add(d);
                    _combineContext.SaveChanges();
                    TempData["admin_kisiEkle"] = valid;
                    return RedirectToAction("Index","Admin");
                }
                else
                {
                    TempData["admin_kisiEkle"] = hata2;
                    return RedirectToAction("AdminKisiMesaj","Admin");
                }
            }
            TempData["admin_kisiEkle"] = hata;
            return RedirectToAction("AdminKisiMesaj", "Admin");
        }

        //DOKTOR DUZENLEME
        public IActionResult DoktorDuzenle(int? id)
        {
            if (id is null)
            {
                TempData["admin_kisiMesaj"] = "boş gecme";
                return View("AdminKisiMesaj");
            }
            var d = _combineContext.Doktorlar.FirstOrDefault(x => x.DoktorID == id);
            var doktorlar = _combineContext.Kullanicilar.Include(x => x.Doktor);
            if (d == null)
            {
                TempData["admin_kisiMesaj"] = "geçerli doktor gir";
                return View("AdminKisiMesaj");
            }
            if(d.AktifRandevular is not null)
            {
                TempData["admin_kisiMesaj"] = "aktif randevu bulunmaktadır, randevuları iptal edip tekrar deneyiniz.";
                return View("AdminKisiMesaj");
            }
            return View(doktorlar);
        }

        [HttpPost]
        public IActionResult DoktorDuzenle(int? id, Kullanici k)
        {
            Doktor d=new Doktor();
            var doktorlar=_combineContext.Doktorlar.Include(x=>x.Kullanici).ToList();
            d.TC = k.TC;
            d.AnadalID = k.AnadalID;
            if (ModelState.IsValid)
            {
                _combineContext.Kullanicilar.Update(k);
                _combineContext.SaveChanges();
                _combineContext.Doktorlar.Update(d);
                _combineContext.SaveChanges();
                TempData["admin_kisiMesaj"] = "doktor duzenlendi";
                return RedirectToAction("Index","Admin");
            }
            TempData["admin_kisiMesaj"] = "tüm alanları doldurun";
            return View();
        }

        //DOKTOR SILME
        public IActionResult DoktorSil()
        {
            return View();
        }
        [HttpPost]
        public IActionResult DoktorSil(int? id)
        {
            if (id is null)
            {
                TempData["admin_kisiMesaj"] = "boş geçme";
                return View("AdminKisiMesaj");
            }
            var doktorlar = _combineContext.Doktorlar.Include(x=>x.Kullanici).FirstOrDefault(x => x.DoktorID == id);
            if (doktorlar == null)
            {
                TempData["admin_kisiMesaj"] = "geçerli doktor giriniz";
                return View("AdminKisiMesaj");
            }
            if (doktorlar.AktifRandevular is not null)
            {
                TempData["admin_kisiMesaj"] = "aktif randevu bulunmaktadır, randevuları iptal edip tekrar deneyiniz.";
                return View("AdminKisiMesaj");
            }
            var k = _combineContext.Kullanicilar.FirstOrDefault(x => x.TC == doktorlar.TC);
            _combineContext.Kullanicilar.Remove(k);
            _combineContext.SaveChanges();
            TempData["admin_kisiMesaj"] = doktorlar.TC+ " TC'li doktor başarıyla silindi.";
            return RedirectToAction("AdminKisiMesaj", "Admin");
        }

        //-------------------------------HASTA İŞLEMLERİ---------------------------------

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
                    return RedirectToAction("Index","Admin");
                }
                else
                {
                    TempData["admin_kisiEkle"] = hata2;
                    return RedirectToAction("AdminKisiMesaj","Admin");
                }
            }
            TempData["admin_kisiEkle"] = hata;
            return RedirectToAction("AdminKisiMesaj","Admin");
        }

        //--------------------------RANDEVU İŞLEMLERİ-----------------------------

        //SIKINTILARI(seçili anadalın doktorları gelmeli) ÇÖZMEK LAZIM EKLERKEN

        //RANDEVU EKLEME
        public IActionResult RandevuEkle()
        {
            var anadallar = _combineContext.Anadallar.OrderBy(x => x.AnadalAd).ToList();
            var doktorlar=_combineContext.Doktorlar.Include(x=>x.Kullanici)
                .Include(x=>x.Anadal).ToList();
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
                    TempData["admin_randevuMesaj"] = "Doktor veya Anadal kaydı bulunamadı. Kontrol ediniz.";
                    return RedirectToAction("AdminRandevuMesaj","Admin");
                }
                if (varmi == null)
                {
                    _combineContext.Randevular.Add(r);
                    _combineContext.SaveChanges();
                    TempData["admin_randevuMesaj"] = valid;
                    return RedirectToAction("Index","Admin");
                }
                else
                {
                    TempData["admin_randevuMesaj"] = hata2;
                    return RedirectToAction("AdminRandevuMesaj","Admin");
                }
            }
            TempData["admin_randevuMesaj"] = hata;
            return RedirectToAction("AdminRandevuMesaj","Admin");
        }

        //RANDEVU LİSTELEME
        public IActionResult RandevuListele()
        {
            var randevular = from r in _combineContext.Randevular
                             select r;
            
            return View(randevular);
        }

        //RANDEVU DÜZENLEME
        public IActionResult RandevuDuzenle(int? id)
        {
            if (id is null)
            {
                TempData["admin_randevuMesaj"] = "boş gecme";
                return View("AdminRandevuMesaj");
            }
            var r = _combineContext.Randevular.FirstOrDefault(x => x.RandevuID == id);
            var anadallar = _combineContext.Anadallar.OrderBy(x => x.AnadalAd).ToList();
            var doktorlar = _combineContext.Doktorlar.Include(x => x.Kullanici).ToList();
            if (r == null)
            {
                TempData["admin_randevuMesaj"] = "geçerli randevu gir";
                return View("AdminRandevuMesaj");
            }
            r.AnadalList = anadallar;
            r.DoktorList = doktorlar;
            return View(r);
        }
        [HttpPost]
        public IActionResult RandevuDuzenle(int? id, Randevu r)
        {
            if (id != r.RandevuID)
            {
                TempData["admin_randevuMesaj"] = "hop hemşerim nereye";
                return View("AdminRandevuMesaj");
            }
            if (ModelState.IsValid)
            {
                _combineContext.Randevular.Update(r);
                _combineContext.SaveChanges();
                TempData["admin_randevuMesaj"] = "randevu duzenlendi";
                return RedirectToAction("Index","Admin");
            }
            TempData["admin_randevuMesaj"] = "tüm alanları doldurun";
            return View();
        }
        
        //RANDEVU SİLME
        public IActionResult RandevuSil()
        {
            return View();
        }
        [HttpPost]
        public IActionResult RandevuSil(int? id)
        {
            if (id is null)
            {
                TempData["admin_randevuMesaj"] = "boş geçme";
                return View("AdminRandevuMesaj");
            }
            var randevular=_combineContext.Randevular.FirstOrDefault(x=>x.RandevuID == id);
            if (randevular == null)
            {
                TempData["admin_randevuMesaj"] = "geçerli randevu giriniz";
                return View("AdminRandevuMesaj");
            }
            if(randevular.Durum==true)
            {
                TempData["admin_randevuMesaj"] = "randevu durumu AKTİF. Önce inaktif hale getiriniz.";
                return View("AdminRandevuMesaj");
            }
            _combineContext.Randevular.Remove(randevular);
            _combineContext.SaveChanges();
            TempData["admin_randevuMesaj"] = randevular.RandevuID + " id'li randevu başarıyla silindi.";
            return RedirectToAction("AdminRandevuMesaj","Admin");
        }

        //BİLGİLENDİRME YÖNLENDİRMELERİ

        public IActionResult AdminKisiMesaj()
        {
            if (TempData["admin_kisiEkle"] is null)
                return RedirectToAction("KisiEklePage","Admin");
            return View();
        }
        public IActionResult AdminRandevuMesaj()
        {
            if (TempData["admin_randevuMesaj"]is null)
                return RedirectToAction("RandevuEkle","Admin");
            return View();
        }
    }   
}