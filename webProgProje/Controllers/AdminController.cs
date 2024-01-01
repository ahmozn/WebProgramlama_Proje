using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Security.Cryptography.Xml;
using webProgProje.Models;


namespace webProgProje.Controllers
{
    [Authorize(Roles ="Admin")]
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

        //LİSTELEME İŞLEMLERİ
        public IActionResult KullaniciListele()
        {
            var kullanicilar = from Kullanici in _combineContext.Kullanicilar
                               where Kullanici.KullaniciTipi != "Admin"
                               select Kullanici;
            return View(kullanicilar);
        }
        public IActionResult DoktorListele()
        {
            var s = _combineContext.Kullanicilar.Where(x=>x.KullaniciTipi=="doktor").Include(x => x.Doktor).ToList();
            return View(s);
        }
        public IActionResult HastaListele()
        {
            var hastalar = _combineContext.Kullanicilar.Where(x => x.KullaniciTipi == "hasta").ToList();
            return View(hastalar);
        }
        //RANDEVU LİSTELEME
        public IActionResult RandevuListele()
        {
            var randevular = _combineContext.Randevular.Include(x=>x.Doktor.Kullanici).Include(x=>x.Anadal).ToList();
            return View(randevular);
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
            var varmi = _combineContext.Kullanicilar.FirstOrDefault(x => x.Id == k.Id);
            if (ModelState.IsValid)
            {
                d.Id = k.Id;
                d.AnadalID = k.AnadalID;
                d.DoktorDerece = k.Doktor.DoktorDerece;
                if (varmi == null)
                {
                    _combineContext.Kullanicilar.Add(k);
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
        public IActionResult DoktorDuzenle(string? id)
        {
            if (id is null)
            {
                TempData["admin_kisiEkle"] = "boş gecme";
                return View("AdminKisiMesaj");
            }
            var d = _combineContext.Kullanicilar.Where(x=>x.KullaniciTipi=="doktor").Include(x => x.Doktor)
                .FirstOrDefault(x=>x.Id==id);
            if (d== null)
            {
                TempData["admin_kisiEkle"] = "doktor bulunamadı";
                return View("AdminKisiMesaj");
            }
            if(d.Doktor.AktifRandevular is not null)
            {
                TempData["admin_kisiEkle"] = "aktif randevu bulunmaktadır, randevuları iptal edip tekrar deneyiniz.";
                return View("AdminKisiMesaj");
            }
            return View(d);
        }

        [HttpPost]
        public IActionResult DoktorDuzenle(string? id, Kullanici k)
        {
            Doktor d=new Doktor();
            d.Id = k.Id;
            d.AnadalID = k.AnadalID;
            if (ModelState.IsValid)
            {
                _combineContext.Kullanicilar.Update(k);
                _combineContext.Doktorlar.Update(d);
                _combineContext.SaveChanges();
                TempData["admin_kisiEkle"] = "doktor duzenlendi";
                return RedirectToAction("Index","Admin");
            }
            TempData["admin_kisiEkle"] = "tüm alanları doldurun";
            return View();
        }

        //DOKTOR SILME
        public IActionResult DoktorSil(string? id)
        {
            if (id == null || _combineContext.Kullanicilar == null)
            {
                return NotFound();
            }
            var doktor = _combineContext.Kullanicilar.FirstOrDefault(x => x.Id == id);
            if (doktor == null)
            {
                return NotFound();
            }
            return View(doktor);
        }
        [HttpPost, ActionName("DoktorSil")]
        public IActionResult DoktorSilTamam(string? id)
        {
            if (id is null)
            {
                TempData["admin_kisiEkle"] = "boş geçme";
                return View("AdminKisiMesaj");
            }
            var d = _combineContext.Kullanicilar.Include(x=>x.Doktor).FirstOrDefault(x => x.Id == id);
            if (d== null)
            {
                TempData["admin_kisiEkle"] = "geçerli doktor giriniz";
                return View("AdminKisiMesaj");
            }
            if (d.Doktor.AktifRandevular is not null)
            {
                TempData["admin_kisiEkle"] = "aktif randevu bulunmaktadır, randevuları iptal edip tekrar deneyiniz.";
                return View("AdminKisiMesaj");
            }
            _combineContext.Kullanicilar.Remove(d);
            _combineContext.SaveChanges();
            TempData["admin_kisiEkle"] = d.TC+ " TC'li doktor başarıyla silindi.";
            return RedirectToAction("AdminKisiMesaj", "Admin");
        }


        //-------------------------------HASTA İŞLEMLERİ---------------------------------

        //HASTA EKLEME
        public IActionResult HastaEkle()
        {
            Kullanici k = new Kullanici();//random pass fonk calıssın diye
            return View(k);
        }
        [HttpPost]
        public IActionResult HastaEkle(Kullanici k)
        {
            Hasta h = new Hasta();
            string hata = "ekleme başarısız";
            string hata2 = "bu tc'ye sahip bir kayıt bulunmaktadır.";
            string valid = k.TC + " tc'li hasta başarıyla eklendi.";
            var varmi = _combineContext.Kullanicilar.FirstOrDefault(x => x.Id == k.Id);
            if (ModelState.IsValid)
            {
                h.Id = k.Id;
                if (varmi == null)
                {
                    _combineContext.Kullanicilar.Add(k);
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

        //HASTA DUZENLEME
        public IActionResult HastaDuzenle(string? id)
        {
            if (id is null)
            {
                TempData["admin_kisiEkle"] = "boş gecme";
                return View("AdminKisiMesaj");
            }
            var h = _combineContext.Kullanicilar.FirstOrDefault(x => x.Id == id);
            if (h == null)
            {
                TempData["admin_kisiEkle"] = "geçerli hasta gir";
                return View("AdminKisiMesaj");
            }
            return View(h);
        }
        [HttpPost]
        public IActionResult HastaDuzenle(string? id, Kullanici k)
        {
            Hasta h = new Hasta();
            if (id != k.Id)
            {
                TempData["admin_kisiEkle"] = "hop hemşerim nereye";
                return View("AdminKisiMesaj");
            }
            if (ModelState.IsValid)
            {
                h.Id = k.Id;
                _combineContext.Kullanicilar.Update(k);
                _combineContext.Hastalar.Update(h);
                _combineContext.SaveChanges();
                TempData["admin_kisiEkle"] = "Hasta düzenleme başarılı, TC: "+k.TC;
                return RedirectToAction("Index", "Admin");
            }
            TempData["admin_kisiEkle"] = "tüm alanları doldurun";
            return View();
        }

        //HASTA SILME
        public IActionResult HastaSil(string? id)
        {
            if (id == null || _combineContext.Hastalar == null)
            {
                return NotFound();
            }
            var hasta= _combineContext.Kullanicilar.FirstOrDefault(x=>x.Id==id);
            if (hasta == null)
            {
                return NotFound();
            }
            return View(hasta);
        }
        [HttpPost,ActionName("HastaSil")]
        public IActionResult HastaSilTamam(string? id)
        {
            if (id is null)
            {
                TempData["admin_kisiEkle"] = "boş geçme";
                return View("AdminKisiMesaj");
            }
            var h = _combineContext.Kullanicilar.Include(x => x.Hasta).FirstOrDefault(x => x.Id == id);
            if (h == null)
            {
                TempData["admin_kisiEkle"] = "geçerli hasta giriniz";
                return View("AdminKisiMesaj");
            }
            if (h.Hasta.AktifRandevular is not null)
            {
                TempData["admin_kisiEkle"] = "aktif randevu bulunmaktadır, randevuları iptal edip tekrar deneyiniz.";
                return View("AdminKisiMesaj");
            }
            _combineContext.Kullanicilar.Remove(h);
            _combineContext.SaveChanges();
            TempData["admin_kisiEkle"] = h.TC + " TC'li hasta başarıyla silindi.";
            return RedirectToAction("AdminKisiMesaj", "Admin");
        }



        //--------------------------RANDEVU İŞLEMLERİ-----------------------------

        //SIKINTILARI(seçili anadalın doktorları gelmeli) ÇÖZMEK LAZIM EKLERKEN



        //RANDEVU EKLEME
        public IActionResult RandevuEkle()
        {
            var anadallar = _combineContext.Anadallar.OrderBy(x => x.AnadalAd).ToList();
            var doktorlar=_combineContext.Doktorlar.Include(x=>x.Kullanici)
                .Include(x=>x.Anadal).OrderBy(x=>x.AnadalID).ToList();
            Randevu randevu = new Randevu();
            randevu.AnadalList = anadallar;
            randevu.DoktorList = doktorlar;
            randevu.dateANDtime = DateTime.Now.Date;
            return View(randevu);
        }
        
        [HttpPost]
        public IActionResult RandevuEkle(Randevu r)
        {
            Doktor d = new Doktor();
            string hata = "ekleme başarısız";
            string hata2 = "bu id'ye sahip bir randevu bulunmaktadır.";
            string valid = r.RandevuID + " id'li randevu başarıyla eklendi.";
            var varmi=_combineContext.Randevular.FirstOrDefault(x=>x.RandevuID == r.RandevuID);
            ModelState.Remove(nameof(r.HastaID));
            r.RandevuDate = DateOnly.FromDateTime(r.dateANDtime);
            r.RandevuTime=TimeOnly.FromDateTime(r.dateANDtime);
            r.Durum = true;
            if(ModelState.IsValid)
            {
                if (r.AnadalID == 0 || r.DoktorID == 0)
                {
                    TempData["admin_randevuMesaj"] = "Doktor veya Anadal kaydı bulunamadı. Kontrol ediniz.";
                    return RedirectToAction("AdminRandevuMesaj","Admin");
                }
                //if (doktor.AnadalID != r.AnadalID)
                //{
                //    TempData["admin_randevuMesaj"] = "Anadala uygun doktor seçiniz.";
                //    return View(r);
                //}
                if (varmi == null)
                {
                    _combineContext.Randevular.Add(r);
                    _combineContext.SaveChanges();
                    TempData["admin_randevuMesaj"] = valid;
                    return RedirectToAction("RandevuListele","Admin");
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

        //RANDEVU DÜZENLEME
        public IActionResult RandevuDuzenle(int? id)
        {
            if (id is null)
            {
                TempData["admin_randevuMesaj"] = "boş gecme";
                return View("AdminRandevuMesaj");
            }
            var r = _combineContext.Randevular.FirstOrDefault(x => x.RandevuID == id);
            if (r == null)
            {
                TempData["admin_randevuMesaj"] = "geçerli randevu gir";
                return View("AdminRandevuMesaj");
            }
            var anadallar = _combineContext.Anadallar.OrderBy(x => x.AnadalAd).ToList();
            var doktorlar = _combineContext.Doktorlar.Include(x => x.Kullanici)
                .Include(x => x.Anadal).OrderBy(x => x.AnadalID).ToList();
            r.AnadalList = anadallar;
            r.DoktorList = doktorlar;
            return View(r);
        }
        [HttpPost]
        public IActionResult RandevuDuzenle(int? id, Randevu r)
        {
            r.RandevuDate = DateOnly.FromDateTime(r.dateANDtime);
            r.RandevuTime = TimeOnly.FromDateTime(r.dateANDtime);
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
                return RedirectToAction("RandevuListele","Admin");
            }
            TempData["admin_randevuMesaj"] = "tüm alanları doldurun";
            return View();
        }
        
        //RANDEVU SİLME
        public IActionResult RandevuSil(int? id)
        {
            if (id == null || _combineContext.Randevular == null)
            {
                return NotFound();
            }
            var randevu = _combineContext.Randevular
                .Include(x => x.Doktor.Kullanici)
                .Include(x=>x.Hasta.Kullanici)
                .Include(x => x.Anadal)
                .FirstOrDefault(x => x.RandevuID == id);
            if (randevu == null)
            {
                return NotFound();
            }
            return View(randevu);
        }
        [HttpPost, ActionName("RandevuSil")]
        public IActionResult RandevuSilTamam(int? id)
        {
            if (id is null)
            {
                TempData["admin_randevuMesaj"] = "boş geçme";
                return View("AdminRandevuMesaj");
            }
            var randevu=_combineContext.Randevular.FirstOrDefault(x=>x.RandevuID == id);
            if (randevu == null)
            {
                TempData["admin_randevuMesaj"] = "geçerli randevu giriniz";
                return View("AdminRandevuMesaj");
            }
            if(randevu.Durum==false||randevu.HastaID!=null)
            {
                TempData["admin_randevuMesaj"] = "randevu durumu AKTİF. Önce inaktif hale getiriniz.";
                return View("AdminRandevuMesaj");
            }
            _combineContext.Randevular.Remove(randevu);
            _combineContext.SaveChanges();
            TempData["admin_randevuMesaj"] = randevu.RandevuID + " id'li randevu başarıyla silindi.";
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