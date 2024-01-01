using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using webProgProje.Models;

namespace webProgProje.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CallKullaniciApiController : Controller
    {
        private CombineContext _combineContext = new CombineContext();

        public async Task<IActionResult> Index()
        {
            List<Kullanici> kullanicilar = new List<Kullanici>();
            HttpClient client = new HttpClient();
            var response = await client.GetAsync("https://localhost:7229/api/KullaniciApi");
            var jsonResponse = await response.Content.ReadAsStringAsync();
            kullanicilar = JsonConvert.DeserializeObject<List<Kullanici>>(jsonResponse);

            return View(kullanicilar);
        }
    }
}
