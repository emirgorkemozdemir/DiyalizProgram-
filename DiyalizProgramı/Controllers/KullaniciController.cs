
using BussinessLayer;
using DiyalizProgramı.Extra;
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
            if(user.KullaniciAdi == null)
            {
                ViewBag.msg = "Kullanıcı adı boş olamaz";
                return View();
            }
            else if (user.SifreHash == null)
            {
                ViewBag.msg = "Şifre boş olamaz";
                return View();
            }
                var tum_kullanicilar = await kullanici_islemleri.Methods.List();

            // kullanıcı şifresini sha256 ile hashle
            string sha_sifre = SifreSha.ComputeSha256Hash(user.SifreHash);

            var secili_kullanici = tum_kullanicilar.Where(m => m.KullaniciAdi == user.KullaniciAdi && m.SifreHash == sha_sifre).SingleOrDefault();
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