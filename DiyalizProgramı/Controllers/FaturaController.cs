using BussinessLayer;
using Microsoft.AspNetCore.Mvc;

namespace DiyalizProgramı.Controllers
{
    public class FaturaController : Controller
    {
        FaturaBL fatura_islemleri = new FaturaBL();
        [HttpGet]
        public async Task<IActionResult> HastayaGoreBilgiFatura(int id)
        {
            string? kullanici_girisli_mi = kullanici_girisli_mi = HttpContext.Session.GetString("kullanici_giris_yapti_mi");

            if (kullanici_girisli_mi == null)
            {
                return RedirectToAction("Giris", "Kullanici");
            }

            var tum_faturalar = await fatura_islemleri.Methods.List(x=>x.Hasta, x => x.Seans);
            var hastanin_faturalari = tum_faturalar.Where(a => a.HastaId == id).ToList(); 
            return View(hastanin_faturalari);
        }


    }
}
