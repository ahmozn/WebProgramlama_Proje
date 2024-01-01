using Microsoft.AspNetCore.Mvc;
using webProgProje.Models;

namespace webProgProje.Controllers
{
    public class HastaController : Controller
    {
        private CombineContext _combineContext = new CombineContext();
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Hesap()
        {
            var aktifrandevular = _combineContext.Randevular.Where(x => x.HastaID == 1);
            foreach (var a in aktifrandevular)
            {

            }
            return View();
        }
        public IActionResult RandevuAl()
        {
            var randevular = from r in _combineContext.Randevular
                             where r.Durum==true
                             select r;
            return View(randevular);
        }
        [HttpPost]
        public IActionResult RandevuAl(int? id)
        {
            if (id == null)
            {
                TempData["hata"] = "ID bilgisi giriniz.";
                return View("HastaHata");
            }
            var r = _combineContext.Randevular.FirstOrDefault(x => x.RandevuID == id);
            if (r == null)
            {
                TempData["hata"] = "Bu ID'ye sahip randevu bulunamadı.";
                return View("HastaHata");
            }

            Hasta hasta = new Hasta();
            hasta.AktifRandevular.Add(r);
            return RedirectToAction("Hesap");
        }


        public IActionResult HastaHata()
        {
            return View();
        }
    }
}
