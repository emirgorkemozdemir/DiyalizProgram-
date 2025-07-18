using System;
using System.Collections.Generic;

namespace EntityLayer;

public partial class Cihaz : IDataBaseEntity
{
    public int CihazId { get; set; }

    public string? MarkaModel { get; set; }

    public string? SeriNo { get; set; }

    public string? CihazTipi { get; set; }

    public DateOnly? SonBakimTarihi { get; set; }

    public string? Durum { get; set; }

    public virtual ICollection<Seans> Seans { get; set; } = new List<Seans>();
}
