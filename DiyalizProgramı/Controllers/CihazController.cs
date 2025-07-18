
using BussinessLayer;
using EntityLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace DiyalizProgramı.Controllers
{
    public class CihazController : Controller
    {
        CihazBL Cihazlar = new CihazBL();
        public async Task<IActionResult> Listeleme(int SayfalamaSayısı = 1)
        {

            int Sayı = 25 * (SayfalamaSayısı - 1);
            List<Cihaz> CihazListeleme = await Cihazlar.Methods.List();
            var sayfalama1 = CihazListeleme.OrderByDescending(ah => ah.SonBakimTarihi).ToList();
            var sayfalama2 = sayfalama1.Skip(Sayı).Take(25).ToList();
            int kacsayfamolcak = 0;
            if (CihazListeleme.Count() % 25 == 0)
            {


                kacsayfamolcak = CihazListeleme.Count() / 25;
            }
            else
            {
                kacsayfamolcak = (CihazListeleme.Count() / 25) + 1;
            }


            ViewBag.sayfasayısı = kacsayfamolcak;
            return View(sayfalama2);

        }


        public async Task<IActionResult> Filtreleme(DateOnly BakımTarihi)
        {
            List<Cihaz> Filtrelisteleme = await Cihazlar.Methods.List();

            var filtre = Filtrelisteleme.Where(po => po.SonBakimTarihi >= BakımTarihi).ToList();

            return View("~/Views/Cihaz/Listeleme.cshtml", filtre.Take(25).ToList());
        }

        public async Task<IActionResult> Sil(int id)
        {
            Cihaz secili_cihaz = await Cihazlar.Methods.GetbyId(id);

            await Cihazlar.Methods.Delete(secili_cihaz);

            return RedirectToAction("Listeleme");
        }

        [HttpGet]
        public async Task<IActionResult> Ekle()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Ekle(Cihaz yenicihaz)
        {
            if (ModelState.IsValid)
            {
                await Cihazlar.Methods.Add(yenicihaz);
                return RedirectToAction("Listeleme");
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Guncelle(int id)
        {
            var cihaz_kaydi = await Cihazlar.Methods.GetbyId(id);
            return View(cihaz_kaydi);
        }

        [HttpPost]
        public async Task<IActionResult> Guncelle(Cihaz yenicihaz)
        {
            if (ModelState.IsValid)
            {
                await Cihazlar.Methods.Edit(yenicihaz);
                return RedirectToAction("Listeleme");
            }
            else
            {
                return View();
            }
        }
    }
}