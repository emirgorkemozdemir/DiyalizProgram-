
using BussinessLayer;
using EntityLayer;
using Microsoft.AspNetCore.Mvc;

namespace DiyalizProgramı.Controllers
{
    public class KullaniciController : Controller
    {
        KulanıcıBL kullanici_islemleri = new KulanıcıBL();

        [HttpGet]
        public async Task<IActionResult> Giris()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Giris(Kullanici user)
        {
            var tum_kullanicilar = await kullanici_islemleri.Methods.List();
            var secili_kullanici = tum_kullanicilar.Where(m => m.KullaniciAdi == user.KullaniciAdi && m.SifreHash == user.SifreHash).SingleOrDefault();
            if (secili_kullanici != null)
            {
                // giriş başarılı
                return RedirectToAction("Listeleme", "Hasta");
            }
            else
            {
                // giriş başarısız : şifre veya kullanıcı adı yanlış
                ViewBag.msg = "şifre veya kullanıcı adı yanlış";
                return View();
            }


        }
    }
}