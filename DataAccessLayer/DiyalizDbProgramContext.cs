using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using EntityLayer;
namespace DataAccessLayer;

public partial class DiyalizDbProgramContext : DbContext
{
    public DiyalizDbProgramContext()
    {
    }

    public DiyalizDbProgramContext(DbContextOptions<DiyalizDbProgramContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cihaz> Cihazs { get; set; }

    public virtual DbSet<Fatura> Faturas { get; set; }

    public virtual DbSet<Hasta> Hasta { get; set; }

    public virtual DbSet<Kullanici> Kullanicis { get; set; }

    public virtual DbSet<Odeme> Odemes { get; set; }

    public virtual DbSet<Personel> Personels { get; set; }

    public virtual DbSet<Seans> Seans { get; set; }

    public virtual DbSet<TibbiDegerler> TibbiDegerlers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)

        => optionsBuilder.UseSqlServer("Server=.;Database=DiyalizDbProgram;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cihaz>(entity =>
        {
            entity.HasKey(e => e.CihazId).HasName("PK__Cihaz__B560B48797044CA0");

            entity.ToTable("Cihaz");

            entity.HasIndex(e => e.SeriNo, "UQ__Cihaz__1A24D142BFA1D306").IsUnique();

            entity.Property(e => e.CihazId).HasColumnName("CihazID");
            entity.Property(e => e.CihazTipi)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Durum)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.MarkaModel)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.SeriNo)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Fatura>(entity =>
        {
            entity.HasKey(e => e.FaturaId).HasName("PK__Fatura__84301C40C2627B8C");

            entity.ToTable("Fatura");

            entity.HasIndex(e => e.FaturaNo, "UQ__Fatura__8431C5C24D85BAE3").IsUnique();

            entity.Property(e => e.FaturaId).HasColumnName("FaturaID");
            entity.Property(e => e.FaturaNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.HastaId).HasColumnName("HastaID");
            entity.Property(e => e.SeansId).HasColumnName("SeansID");
            entity.Property(e => e.SgkislemKodu)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("SGKIslemKodu");
            entity.Property(e => e.Tutar).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Hasta).WithMany(p => p.Faturas)
                .HasForeignKey(d => d.HastaId)
                .HasConstraintName("FK__Fatura__HastaID__4D94879B");

            entity.HasOne(d => d.Seans).WithMany(p => p.Faturas)
                .HasForeignKey(d => d.SeansId)
                .HasConstraintName("FK__Fatura__SeansID__4E88ABD4");
        });

        modelBuilder.Entity<Hasta>(entity =>
        {
            entity.HasKey(e => e.HastaId).HasName("PK__Hasta__114C5CAB69936E66");

            entity.HasIndex(e => e.TckimlikNo, "UQ__Hasta__7E1935EDCEBB0798").IsUnique();

            entity.Property(e => e.HastaId).HasColumnName("HastaID");
            entity.Property(e => e.Ad)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Adres)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.AktifMi).HasDefaultValue(true);
            entity.Property(e => e.Alerjiler)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Cinsiyet)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.DoktorId).HasColumnName("DoktorID");
            entity.Property(e => e.KanGrubu)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.KronikHastaliklar)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Soyad)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TckimlikNo)
                .HasMaxLength(11)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("TCKimlikNo");
            entity.Property(e => e.Telefon)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.Doktor).WithMany(p => p.Hasta)
                .HasForeignKey(d => d.DoktorId)
                .HasConstraintName("FK__Hasta__DoktorID__3B75D760");
        });

        modelBuilder.Entity<Kullanici>(entity =>
        {
            entity.HasKey(e => e.KullaniciId).HasName("PK__Kullanic__E011F09B170FE3C4");

            entity.ToTable("Kullanici");

            entity.HasIndex(e => e.KullaniciAdi, "UQ__Kullanic__5BAE6A75D2A1D046").IsUnique();

            entity.Property(e => e.KullaniciId).HasColumnName("KullaniciID");
            entity.Property(e => e.KullaniciAdi)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PersonelId).HasColumnName("PersonelID");
            entity.Property(e => e.Rol)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.SifreHash).HasMaxLength(64);

            entity.HasOne(d => d.Personel).WithMany(p => p.Kullanicis)
                .HasForeignKey(d => d.PersonelId)
                .HasConstraintName("FK__Kullanici__Perso__52593CB8");
        });

        modelBuilder.Entity<Odeme>(entity =>
        {
            entity.HasKey(e => e.OdemeId).HasName("PK__Odeme__B11B66AD7269B549");

            entity.ToTable("Odeme");

            entity.Property(e => e.OdemeId).HasColumnName("OdemeID");
            entity.Property(e => e.Aciklama)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.HastaId).HasColumnName("HastaID");
            entity.Property(e => e.OdemeTuru)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.SeansId).HasColumnName("SeansID");
            entity.Property(e => e.Tutar).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Hasta).WithMany(p => p.Odemes)
                .HasForeignKey(d => d.HastaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Odeme__HastaID__48CFD27E");

            entity.HasOne(d => d.Seans).WithMany(p => p.Odemes)
                .HasForeignKey(d => d.SeansId)
                .HasConstraintName("FK__Odeme__SeansID__49C3F6B7");
        });

        modelBuilder.Entity<Personel>(entity =>
        {
            entity.HasKey(e => e.PersonelId).HasName("PK__Personel__0F0C575124FFD2B9");

            entity.ToTable("Personel");

            entity.Property(e => e.PersonelId).HasColumnName("PersonelID");
            entity.Property(e => e.Ad)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Brans)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Soyad)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Telefon)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Unvan)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Seans>(entity =>
        {
            entity.HasKey(e => e.SeansId).HasName("PK__Seans__0359BD6060B4234B");

            entity.Property(e => e.SeansId).HasColumnName("SeansID");
            entity.Property(e => e.CihazId).HasColumnName("CihazID");
            entity.Property(e => e.Durum)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.HastaId).HasColumnName("HastaID");
            entity.Property(e => e.Notlar)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.PersonelId).HasColumnName("PersonelID");
            entity.Property(e => e.TarihSaat).HasColumnType("datetime");

            entity.HasOne(d => d.Cihaz).WithMany(p => p.Seans)
                .HasForeignKey(d => d.CihazId)
                .HasConstraintName("FK__Seans__CihazID__4222D4EF");

            entity.HasOne(d => d.Hasta).WithMany(p => p.Seans)
                .HasForeignKey(d => d.HastaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Seans__HastaID__412EB0B6");

            entity.HasOne(d => d.Personel).WithMany(p => p.Seans)
                .HasForeignKey(d => d.PersonelId)
                .HasConstraintName("FK__Seans__PersonelI__4316F928");
        });

        modelBuilder.Entity<TibbiDegerler>(entity =>
        {
            entity.HasKey(e => e.DegerId).HasName("PK__TibbiDeg__6E218F8C723EEE43");

            entity.ToTable("TibbiDegerler");

            entity.Property(e => e.DegerId).HasColumnName("DegerID");
            entity.Property(e => e.Hemoglobin).HasColumnType("decimal(4, 2)");
            entity.Property(e => e.KiloOncesi).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.KiloSonrasi).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.Kreatinin).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.Notlar)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.SeansId).HasColumnName("SeansID");
            entity.Property(e => e.TansiyonOncesi)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.TansiyonSonrasi)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Ure).HasColumnType("decimal(5, 2)");

            entity.HasOne(d => d.Seans).WithMany(p => p.TibbiDegerlers)
                .HasForeignKey(d => d.SeansId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TibbiDege__Seans__45F365D3");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
