using System;
using System.Collections.Generic;

namespace EntityLayer;

public partial class Kullanici : IDataBaseEntity
{
    public int KullaniciId { get; set; }

    public string KullaniciAdi { get; set; } = null!;

    public string SifreHash { get; set; } = null!;

    public string? Rol { get; set; }

    public int? PersonelId { get; set; }

    public virtual Personel? Personel { get; set; }
}
