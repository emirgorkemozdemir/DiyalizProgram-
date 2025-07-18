using System;
using System.Collections.Generic;

namespace EntityLayer;

public partial class Fatura : IDataBaseEntity
{
    public int FaturaId { get; set; }

    public int? HastaId { get; set; }

    public int? SeansId { get; set; }

    public DateOnly? FaturaTarihi { get; set; }

    public decimal? Tutar { get; set; }

    public string? SgkislemKodu { get; set; }

    public string? FaturaNo { get; set; }

    public virtual Hasta? Hasta { get; set; }

    public virtual Seans? Seans { get; set; }
}
