using System.Collections.Generic;
using BussinessLayer;
using EntityLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DiyalizProgramı.Controllers
{
    public class HastaController : Controller
    {
        HastaBL Hasta = new HastaBL();
        PersonelBL personel = new PersonelBL(); 
        ÖdemeBL odemeler = new ÖdemeBL();
        FaturaBL faturalar = new FaturaBL();
        SeansBL seanslar = new SeansBL();
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
        
        public async Task<IActionResult> Listeleme(int SayfaSayısı = 1)
        {
            string? kullanici_girisli_mi = kullanici_girisli_mi = HttpContext.Session.GetString("kullanici_giris_yapti_mi");

            if (kullanici_girisli_mi == null)
            {
                return RedirectToAction("Giris","Kullanici");
            }

            int Sayı = 25 * (SayfaSayısı - 1);
            List<Hasta> HastaListeleme = await Hasta.Methods.List(x => x.Doktor);
            var sayfalama1 = HastaListeleme.OrderByDescending(ah => ah.DogumTarihi).ToList();
            var sayfalama2 = sayfalama1.Skip(Sayı).Take(25).ToList();
            int kacsayfamolcak = 0;
            if (HastaListeleme.Count() % 25 == 0)
            {


                kacsayfamolcak = HastaListeleme.Count() / 25;
            }
            else
            {
                kacsayfamolcak = (HastaListeleme.Count() / 25) + 1;
            }


            ViewBag.sayfasayısı = kacsayfamolcak;
            return View(sayfalama2);
        }

  
       public async Task<IActionResult> Filtreleme(string isimegöre)
        {
            string? kullanici_girisli_mi = kullanici_girisli_mi = HttpContext.Session.GetString("kullanici_giris_yapti_mi");

            if (kullanici_girisli_mi == null)
            {
                return RedirectToAction("Giris", "Kullanici");
            }
            List<Hasta> Filtrelisteleme = await Hasta.Methods.List(a=>a.Doktor);
            if (string.IsNullOrEmpty(isimegöre))
            {
             

                return View("~/Views/Hasta/Listeleme.cshtml", Filtrelisteleme.Take(25).ToList());


            }
            else
            {
                var filtre = Filtrelisteleme.Where(a => (a.Ad.ToLower().Contains(isimegöre.ToLower()) || a.Soyad.ToLower().Contains(isimegöre.ToLower()) || a.Telefon.Contains(isimegöre.ToLower()) || a.TckimlikNo.ToLower().Contains(isimegöre.ToLower()) || a.Doktor.Ad.ToLower().Contains(isimegöre.ToLower()) || a.Doktor.Soyad.ToLower().Contains(isimegöre.ToLower()))).ToList();
                
                return View("~/Views/Hasta/Listeleme.cshtml", filtre);
            }

        }
        public async Task<IActionResult> Sil(int id )
        {
            string? kullanici_girisli_mi = kullanici_girisli_mi = HttpContext.Session.GetString("kullanici_giris_yapti_mi");

            if (kullanici_girisli_mi == null)
            {
                return RedirectToAction("Giris", "Kullanici");
            }

            var SeçiliHasta = await Hasta.Methods.GetbyId(id);
            if (SeçiliHasta is null)
            {
                TempData["AlertMessage"] = "Müşteri bulunamadı.";
            }

            // önce müşteriye ait ödeme varsa onları siliyorum.
            var tum_odemeler = await odemeler.Methods.List();
            var musterinin_odemeleri = tum_odemeler.Where(x=>x.HastaId == id).ToList();
            foreach (var odeme in musterinin_odemeleri)
            {
               await odemeler.Methods.Delete(odeme);
            }

            //  müşteriye ait fatura varsa onları siliyorum.
            var tum_faturalar = await faturalar.Methods.List();
            var musterinin_faturaları = tum_faturalar.Where(x => x.HastaId == id).ToList();
            foreach (var fatura in musterinin_faturaları)
            {
                await faturalar.Methods.Delete(fatura);
            }

            //  müşteriye ait seans varsa onları siliyorum.
            var tum_seanslar = await seanslar.Methods.List();
            var musterinin_seansları = tum_seanslar.Where(x => x.HastaId == id).ToList();
            foreach (var seans in musterinin_seansları)
            {
                await seanslar.Methods.Delete(seans);
            }

            try
            {
                await Hasta.Methods.Delete(SeçiliHasta);
                TempData["AlertMessage"] = "Müşteri başarıyla silindi.";
            }
            catch (Exception)
            {
                TempData["AlertMessage"] = "Silme sırasında bir hata oluştu. Bu müşteriye ait veri bulunmakta.";

            }
            return RedirectToAction("Listeleme");
          
        }

        [HttpGet]
        public async Task<IActionResult> Ekle(int id, string isim)
        {
            string? kullanici_girisli_mi = kullanici_girisli_mi = HttpContext.Session.GetString("kullanici_giris_yapti_mi");

            if (kullanici_girisli_mi == null)
            {
                return RedirectToAction("Giris", "Kullanici");
            }
            DoktorlarıYükle();           
            return View();  
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Ekle(Hasta YeniHasta)
        {
            string? kullanici_girisli_mi = kullanici_girisli_mi = HttpContext.Session.GetString("kullanici_giris_yapti_mi");

            if (kullanici_girisli_mi == null)
            {
                return RedirectToAction("Giris", "Kullanici");
            }
            if (ModelState.IsValid)
            {
                await Hasta.Methods.Add(YeniHasta);
                return RedirectToAction("Listeleme");
            }
            else
            {
                DoktorlarıYükle();
                return View();
            }
        }
        public async Task<IActionResult> Güncelle(int id)
        {
            string? kullanici_girisli_mi = kullanici_girisli_mi = HttpContext.Session.GetString("kullanici_giris_yapti_mi");

            if (kullanici_girisli_mi == null)
            {
                return RedirectToAction("Giris", "Kullanici");
            }
            DoktorlarıYükle();
            var Güncelleme = await Hasta.Methods.GetbyId(id);

            return View(Güncelleme);

        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Güncelle(Hasta HastaGüncelle)
        {

            string? kullanici_girisli_mi = kullanici_girisli_mi = HttpContext.Session.GetString("kullanici_giris_yapti_mi");

            if (kullanici_girisli_mi == null)
            {
                return RedirectToAction("Giris", "Kullanici");
            }
            if (ModelState.IsValid)
            {
                await Hasta.Methods.Edit(HastaGüncelle);
                return RedirectToAction("Listeleme");
            }
            else
            {
                DoktorlarıYükle();
                return View();
            }
           
        }
    }
    
}

