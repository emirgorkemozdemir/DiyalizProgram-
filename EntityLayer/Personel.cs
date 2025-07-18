using System;
using System.Collections.Generic;

namespace EntityLayer;
public partial class Personel : IDataBaseEntity
{
    public int PersonelId { get; set; }

    public string Ad { get; set; } = null!;

    public string Soyad { get; set; } = null!;

    public string? Unvan { get; set; }

    public string? Brans { get; set; }

    public string? Telefon { get; set; }

    public virtual ICollection<Hasta> Hasta { get; set; } = new List<Hasta>();

    public virtual ICollection<Kullanici> Kullanicis { get; set; } = new List<Kullanici>();

    public virtual ICollection<Seans> Seans { get; set; } = new List<Seans>();
}
