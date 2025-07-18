using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityLayer;

public partial class Hasta : IDataBaseEntity
{

    public int HastaId { get; set; }
    [Required(ErrorMessage ="Ad Boş Girilemez")]
    public string Ad { get; set; } = null!;
    [Required(ErrorMessage = "Soyad Boş Girilemez")]
    public string Soyad { get; set; } = null!;
    [Required(ErrorMessage = "TckimlikNo Boş Girilemez")]
    public string TckimlikNo { get; set; } = null!;

    public DateOnly? DogumTarihi { get; set; }

    public string? Cinsiyet { get; set; }

    public string? KanGrubu { get; set; }

    public string? Telefon { get; set; }

    public string? Adres { get; set; }

    public string? Alerjiler { get; set; }

    public string? KronikHastaliklar { get; set; }

    public int? DoktorId { get; set; }

    public bool? AktifMi { get; set; }

    public virtual Personel? Doktor { get; set; }

    public virtual ICollection<Fatura> Faturas { get; set; } = new List<Fatura>();

    public virtual ICollection<Odeme> Odemes { get; set; } = new List<Odeme>();

    public virtual ICollection<Seans> Seans { get; set; } = new List<Seans>();
}
