using BussinessLayer;
using DataAccessLayer;
using EntityLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DiyalizProgramı.Controllers
{
    public class SeansController : Controller
    {

        SeansBL seans_islemleri = new SeansBL();
        HastaBL Hasta = new HastaBL();
        PersonelBL personel = new PersonelBL();
        ÖdemeBL odemeler = new ÖdemeBL();
        FaturaBL faturalar = new FaturaBL();
        TibbiDeğerlerBL tibbidegerler = new TibbiDeğerlerBL();
        CihazBL cihazlar_bl = new CihazBL();



        [HttpGet]
        public async Task<IActionResult> HastayaGoreBilgi(int id)
        {
            string? kullanici_girisli_mi = kullanici_girisli_mi = HttpContext.Session.GetString("kullanici_giris_yapti_mi");
            HttpContext.Session.SetInt32("seans_hasta_id",id);
            if (kullanici_girisli_mi == null)
            {
                return RedirectToAction("Giris", "Kullanici");
            }
            var tum_seanslar = await seans_islemleri.Methods.List(a=>a.Hasta,a=>a.Cihaz,a=>a.Personel);
            var hastanin_seanslari = tum_seanslar.Where(a=>a.HastaId == id).ToList();
            return View(hastanin_seanslari);
        }

        [HttpGet]
        public async Task<IActionResult> Sil(int seansid)
        {
            string? kullanici_girisli_mi = kullanici_girisli_mi = HttpContext.Session.GetString("kullanici_giris_yapti_mi");

            if (kullanici_girisli_mi == null)
            {
                return RedirectToAction("Giris", "Kullanici");
            }

            // önce müşteriye ait ödeme varsa onları siliyorum.
            var tum_odemeler = await odemeler.Methods.List();
            var musterinin_odemeleri = tum_odemeler.Where(x => x.SeansId == seansid).ToList();
            foreach (var odeme in musterinin_odemeleri)
            {
                await odemeler.Methods.Delete(odeme);
            }

            //  müşteriye ait fatura varsa onları siliyorum.
            var tum_faturalar = await faturalar.Methods.List();
            var musterinin_faturaları = tum_faturalar.Where(x => x.SeansId == seansid).ToList();
            foreach (var fatura in musterinin_faturaları)
            {
                await faturalar.Methods.Delete(fatura);
            }

            //  müşteriye ait seans varsa onları siliyorum.
            var tum_tibbidegerler = await tibbidegerler.Methods.List();
            var musterinin_seansları = tum_tibbidegerler.Where(x => x.SeansId == seansid).ToList();
            foreach (var seans in musterinin_seansları)
            {
                await tibbidegerler.Methods.Delete(seans);
            }

            int? hasta_id = HttpContext.Session.GetInt32("seans_hasta_id");
            var secili_seans = await seans_islemleri.Methods.GetbyId(seansid);
            try
            {
                await seans_islemleri.Methods.Delete(secili_seans);
                TempData["AlertMessage"] = "Müşteri başarıyla silindi.";
            }
            catch (Exception)
            {
                TempData["AlertMessage"] = "Silme sırasında bir hata oluştu. Bu müşteriye ait veri bulunmakta.";

            }
            // fatura ödeme tıbbi değerler silinecek ama yukarıdan gelen seans id'ye bağlı olarak

            return RedirectToAction("HastayaGoreBilgi", new { id = hasta_id });
        }


        private async void DoktorlarıYükle()
        {


            List<Personel> doktor_listesi = await personel.Methods.List(x => x.Hasta, x => x.Kullanicis, x => x.Seans);

            List<SelectListItem> seçenekler = new List<SelectListItem>();
            foreach (var doktor in doktor_listesi)
            {
                SelectListItem seçenek = new SelectListItem();
                seçenek.Text = doktor.Unvan + " " + doktor.Ad + " " + doktor.Soyad + " - " + doktor.Brans;
                seçenek.Value = doktor.PersonelId.ToString();

                seçenekler.Add(seçenek);
            }

            ViewBag.doktorlar = seçenekler;
        }

        private async void CihazlarıYükle()
        {


            List<Cihaz> cihaz_listesi = await cihazlar_bl.Methods.List(x => x.Seans);

            List<SelectListItem> seçenekler = new List<SelectListItem>();
            foreach (var cihaz in cihaz_listesi)
            {
                SelectListItem seçenek = new SelectListItem();
                seçenek.Text = cihaz.MarkaModel;
                seçenek.Value = cihaz.CihazId.ToString();

                seçenekler.Add(seçenek);
            }

            ViewBag.cihazlar = seçenekler;
        }


        [HttpGet]
        public async Task<IActionResult> Ekle()
        {
             DoktorlarıYükle();
            CihazlarıYükle();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Ekle(Seans eklenecek_seans)
        {
            DoktorlarıYükle();
            CihazlarıYükle();

            int? hasta_id = HttpContext.Session.GetInt32("seans_hasta_id");
            if (hasta_id == null)
            {
                ViewBag.hata = "Hasta id seçilmedi";
                return View();
            }

            eklenecek_seans.HastaId = Convert.ToInt32(hasta_id);
            bool sonuc = await seans_islemleri.Methods.Add(eklenecek_seans);

            if (sonuc==true)
            {
                return RedirectToAction("HastayaGoreBilgi", new { id = hasta_id });
            }
            else
            {
                    return View();
            }
        }
    }
}
