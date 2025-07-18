using System;
using System.Collections.Generic;

namespace EntityLayer;
public partial class TibbiDegerler : IDataBaseEntity
{
    public int DegerId { get; set; }

    public int SeansId { get; set; }

    public string? TansiyonOncesi { get; set; }

    public string? TansiyonSonrasi { get; set; }

    public int? NabizOncesi { get; set; }

    public int? NabizSonrasi { get; set; }

    public decimal? KiloOncesi { get; set; }

    public decimal? KiloSonrasi { get; set; }

    public decimal? Hemoglobin { get; set; }

    public decimal? Ure { get; set; }

    public decimal? Kreatinin { get; set; }

    public string? Notlar { get; set; }

    public virtual Seans Seans { get; set; } = null!;
}
