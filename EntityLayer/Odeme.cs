using System;
using System.Collections.Generic;

namespace EntityLayer;

public partial class Odeme : IDataBaseEntity
{
    public int OdemeId { get; set; }

    public int HastaId { get; set; }

    public int? SeansId { get; set; }

    public decimal? Tutar { get; set; }

    public DateOnly? OdemeTarihi { get; set; }

    public string? OdemeTuru { get; set; }

    public string? Aciklama { get; set; }

    public virtual Hasta Hasta { get; set; } = null!;

    public virtual Seans? Seans { get; set; }
}
