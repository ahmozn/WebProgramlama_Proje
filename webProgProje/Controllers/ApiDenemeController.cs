using Microsoft.AspNetCore.Mvc;
using webProgProje.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace webProgProje.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiDenemeController : ControllerBase
    {
        private CombineContext _combineContext = new CombineContext();

        // GET: api/<ApiDenemeController>
        [HttpGet]
        public IEnumerable<Randevu> Get()
        {
            //var doktorlar = from Kullanici in _combineContext.Kullanicilar
            //                where Kullanici.KullaniciTipi == "doktor"
            //                select Kullanici;
            var randevular= _combineContext.Randevular.OrderBy(x=>x.RandevuID);
            return randevular;
        }

        // GET api/<ApiDenemeController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ApiDenemeController>
        [HttpPost]
        public void Post([FromBody] Kullanici k)
        {
            _combineContext.Add(k);
            _combineContext.SaveChanges();
        }

        // PUT api/<ApiDenemeController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ApiDenemeController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
