using BussinessLayer;
using EntityLayer;
using Microsoft.AspNetCore.Mvc;

namespace DiyalizProgramı.Controllers
{
    public class PersonelController : Controller
    {
        PersonelBL Personel = new PersonelBL();
        public async Task<IActionResult> Listeleme(int SayfaSayısı = 1)
        {

            int Sayı = 25 * (SayfaSayısı - 1);
            List<Personel> PersonelListeleme = await Personel.Methods.List(x => x.Hasta);
            var sayfalama1 = PersonelListeleme.OrderByDescending(ah => ah.Unvan).ToList();
            var sayfalama2 = sayfalama1.Skip(Sayı).Take(25).ToList();
            int kacsayfamolcak = 0;
            if (PersonelListeleme.Count() % 25 == 0)
            {


                kacsayfamolcak = PersonelListeleme.Count() / 25;
            }
            else
            {
                kacsayfamolcak = (PersonelListeleme.Count() / 25) + 1;
            }


            ViewBag.sayfasayısı = kacsayfamolcak;
            return View(sayfalama2);
        }

        public async Task<IActionResult> Filtreleme(string isimegöre)
        {
            List<Personel> Filtrelisteleme = await Personel.Methods.List();
            if (string.IsNullOrEmpty(isimegöre))
            {


                return View("~/Views/Personel/Listeleme.cshtml", Filtrelisteleme.Take(25).ToList());


            }
            else
            {

                var filtre = Filtrelisteleme.Where(po => (po.Ad.ToLower().Contains(isimegöre.ToLower()) || po.Soyad.ToLower().Contains(isimegöre.ToLower()))).ToList();
                return View("~/Views/Personel/Listeleme.cshtml", filtre);


            }
        }
        public async Task<IActionResult> Sil(int id)
        {
            var SeçiliPersonel = await Personel.Methods.GetbyId(id);
            if (SeçiliPersonel is null)
            {
                TempData["AlertMessage"] = "Müşteri bulunamadı.";
            }

            try
            {
                await Personel.Methods.Delete(SeçiliPersonel);
                TempData["AlertMessage"] = "Müşteri başarıyla silindi.";
            }
            catch (Exception)
            {
                TempData["AlertMessage"] = "Silme sırasında bir hata oluştu. Bu müşteriye ait hayvan veya veri bulunmakta.";

            }
            return RedirectToAction("Listeleme");

        }

        public async Task<IActionResult> ekle(int Id, string İsim)
        {
            ViewBag.İsim = İsim;
            ViewBag.Id = Id;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Ekle(Personel YeniPersonel)
        {
            if (ModelState.IsValid)
            {
                await Personel.Methods.Add(YeniPersonel);
                return RedirectToAction("Listeleme");
            }
            else
            {
                return View();
            }
        }
        public async Task<IActionResult> Güncelle(int id)
        {
           
            var Güncelleme = await Personel.Methods.GetbyId(id);

            return View(Güncelleme);

        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Güncelle(Personel PersonelGüncelle)
        {

            if (ModelState.IsValid)
            {
                await Personel.Methods.Edit(PersonelGüncelle);
                return RedirectToAction("Listeleme");
            }
            else
            {
              
                return View();
            }

        }


    }
}
            
        
    

        
