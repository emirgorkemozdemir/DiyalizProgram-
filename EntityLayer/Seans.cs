using System;
using System.Collections.Generic;

namespace EntityLayer;

public partial class Seans : IDataBaseEntity
{
    public int SeansId { get; set; }

    public int HastaId { get; set; }

    public DateTime TarihSaat { get; set; }

    public int? CihazId { get; set; }

    public int? PersonelId { get; set; }

    public string? Durum { get; set; }

    public string? Notlar { get; set; }

    public virtual Cihaz? Cihaz { get; set; }

    public virtual ICollection<Fatura> Faturas { get; set; } = new List<Fatura>();

    public virtual Hasta Hasta { get; set; } = null!;

    public virtual ICollection<Odeme> Odemes { get; set; } = new List<Odeme>();

    public virtual Personel? Personel { get; set; }

    public virtual ICollection<TibbiDegerler> TibbiDegerlers { get; set; } = new List<TibbiDegerler>();
}
