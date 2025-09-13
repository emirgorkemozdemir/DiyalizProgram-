
using BussinessLayer;
using DiyalizProgramı.Extra;
using EntityLayer;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

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
                HttpContext.Session.SetString("kullanici_giris_yapti_mi","evet");
                return RedirectToAction("Listeleme", "Hasta");
            }
            else
            {
                // giriş başarısız : şifre veya kullanıcı adı yanlış
                ViewBag.msg = "şifre veya kullanıcı adı yanlış";
                return View();
            }


        }


        [HttpGet]
        public async Task<IActionResult> KayitOl()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> KayitOl(string ka, string sifre , string kod)
        {
            try
            {
                if (kod.ToLower() != "diyaliz123")
                {
                    ViewBag.msg = "Davet kodunuz yanlış";
                    return View();
                }

                if (string.IsNullOrWhiteSpace(ka) || string.IsNullOrWhiteSpace(sifre) || string.IsNullOrWhiteSpace(kod))
                {
                    ViewBag.msg = "Kısımlardan birisi boş girildi.";
                    return View();
                }

                
                string pattern = @"^(?=.*[A-Z])(?=.*\d).{8,}$";
                bool icerdi = Regex.IsMatch(sifre, pattern);
                if(icerdi == false)
                {
                    ViewBag.msg = "Şifrenizde en az 1 rakam, 1 büyük karakter ve en az 8 karakter uzunluğunda olmalı";
                    return View();
                }
                List<Kullanici> kullanicilar = await kullanici_islemleri.Methods.List();
                if (kullanicilar.Where(a => a.KullaniciAdi == ka).SingleOrDefault() != null)
                {
                    ViewBag.msg = "Bu kullanıcı adı alındı.";
                    return View();
                }

                Kullanici user = new Kullanici();
                user.KullaniciAdi = ka.ToLower();
                user.SifreHash = SifreSha.ComputeSha256Hash(sifre);
                user.Rol = "1";
                await kullanici_islemleri.Methods.Add(user);
                return View();
            }
            catch (Exception)
            {
                ViewBag.msg = "Kayıt sırasında bir hata oluştu , bilgileri gözden geçirerek tekrar deneyiniz.";
                return View();
            }
        }
    }
}