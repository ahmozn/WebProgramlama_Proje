using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webProgProje.Models;

namespace webProgProje.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KullaniciApiController : ControllerBase
    {
        private CombineContext _combineContext=new CombineContext();
        
        [HttpGet]
        public List<Kullanici> Get()
        {
            var kullanicilar = (from Kullanici in _combineContext.Kullanicilar
                               where Kullanici.KullaniciTipi != "Admin"
                               select Kullanici).ToList();
            // normalde json formatına cevirip gondermem lazım  [ApiController] bunu otomatik yapıyor
            return kullanicilar;
        }
    }
}
