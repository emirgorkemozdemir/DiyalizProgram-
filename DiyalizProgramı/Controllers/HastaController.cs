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
            List<Hasta> Filtrelisteleme = await Hasta.Methods.List();
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
            var SeçiliHasta = await Hasta.Methods.GetbyId(id);
            if (SeçiliHasta is null)
            {
                TempData["AlertMessage"] = "Müşteri bulunamadı.";
            }

            try
            {
                await Hasta.Methods.Delete(SeçiliHasta);
                TempData["AlertMessage"] = "Müşteri başarıyla silindi.";
            }
            catch (Exception)
            {
                TempData["AlertMessage"] = "Silme sırasında bir hata oluştu. Bu müşteriye ait hayvan veya veri bulunmakta.";

            }
            return RedirectToAction("Listeleme");
          
        }

        [HttpGet]
        public async Task<IActionResult> Ekle(int id, string isim)
        {
            DoktorlarıYükle();           
            return View();  
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Ekle(Hasta YeniHasta)
        {
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
            DoktorlarıYükle();
            var Güncelleme = await Hasta.Methods.GetbyId(id);

            return View(Güncelleme);

        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Güncelle(Hasta HastaGüncelle)
        {

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

