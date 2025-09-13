using BussinessLayer;
using DataAccessLayer;
using EntityLayer;
using Microsoft.AspNetCore.Mvc;

namespace DiyalizProgramı.Controllers
{

    public class OdemeController : Controller
    {
    

        ÖdemeBL odeme_islemleri = new ÖdemeBL();
        [HttpGet]
        public async Task<IActionResult> HastayaGoreBilgiOdeme(int id)
        {
            string? kullanici_girisli_mi = kullanici_girisli_mi = HttpContext.Session.GetString("kullanici_giris_yapti_mi");
            HttpContext.Session.SetInt32("odeme_hasta_id", id);
            if (kullanici_girisli_mi == null)
            {
                return RedirectToAction("Giris", "Kullanici");
            }
            var tum_odemeler = await odeme_islemleri.Methods.List(x=>x.Hasta, x=>x.Seans);
            var hastanin_odemeleri = tum_odemeler.Where(a => a.HastaId == id).ToList();
            return View(hastanin_odemeleri);
        }
        [HttpGet]
        public async Task<IActionResult> Sil(int odemeid)
        {
            string? kullanici_girisli_mi = kullanici_girisli_mi = HttpContext.Session.GetString("kullanici_giris_yapti_mi");

            if (kullanici_girisli_mi == null)
            {
                return RedirectToAction("Giris", "Kullanici");
            }      


            int? hasta_id = HttpContext.Session.GetInt32("odeme_hasta_id");
            var secili_odeme = await odeme_islemleri.Methods.GetbyId(odemeid);
            try
            {
                await odeme_islemleri.Methods.Delete(secili_odeme);
                TempData["AlertMessage"] = "Ödeme başarıyla silindi.";
            }
            catch (Exception)
            {
                TempData["AlertMessage"] = "Silme sırasında bir hata oluştu. Bu ödemeye ait veri bulunmakta.";

            }
            // fatura ödeme tıbbi değerler silinecek ama yukarıdan gelen seans id'ye bağlı olarak

            return RedirectToAction("HastayaGoreBilgiOdeme", new { id = hasta_id });
        }
    }
}
