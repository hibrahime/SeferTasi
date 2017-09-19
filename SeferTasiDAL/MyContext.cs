using SeferTasiMODEL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeferTasiDAL
{
   public class MyContext:DbContext
    {
        public MyContext()
            : base("name=LocalCon")
        {
        }

        public virtual DbSet<Firma> Firma { get; set; }
        public virtual DbSet<Kategori> Kategori { get; set; }
        public virtual DbSet<Kullanici> Kullanici { get; set; }
        public virtual DbSet<Musteri> Musteri { get; set; }
        public virtual DbSet<Odeme> Odeme { get; set; }
        public virtual DbSet<Rol> Rol { get; set; }
        public virtual DbSet<Siparis> Siparis { get; set; }
        public virtual DbSet<Siparis_Detay> Siparis_Detay { get; set; }
        public virtual DbSet<Urun> Urun { get; set; }
        public virtual DbSet<Urun_Detay> Urun_Detay { get; set; }
        public virtual DbSet<Yonetici> Yonetici { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Firma>()
                .Property(e => e.FirmaAdi)
                .IsUnicode(false);

            modelBuilder.Entity<Firma>()
                .Property(e => e.Adres)
                .IsUnicode(false);

            modelBuilder.Entity<Firma>()
                .Property(e => e.Telefon)
                .IsFixedLength();

            modelBuilder.Entity<Firma>()
                .HasMany(e => e.Urun_Detay)
                .WithRequired(e => e.Firma)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Kullanici>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Kullanici>()
                .Property(e => e.KullaniciAdi)
                .IsUnicode(false);

            modelBuilder.Entity<Kullanici>()
                .Property(e => e.Parola)
                .IsFixedLength();

            modelBuilder.Entity<Musteri>()
                .Property(e => e.Adi)
                .IsUnicode(false);

            modelBuilder.Entity<Musteri>()
                .Property(e => e.Soyadi)
                .IsUnicode(false);

            modelBuilder.Entity<Musteri>()
                .Property(e => e.Adres)
                .IsUnicode(false);

            modelBuilder.Entity<Musteri>()
                .Property(e => e.Telefon)
                .IsFixedLength();

            modelBuilder.Entity<Rol>()
                .Property(e => e.RolAdi)
                .IsUnicode(false);

            modelBuilder.Entity<Siparis>()
                .HasMany(e => e.Siparis_Detay)
                .WithRequired(e => e.Siparis)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Siparis_Detay>()
                .Property(e => e.BirimFiyat)
                .HasPrecision(7, 2);

            modelBuilder.Entity<Urun>()
                .HasMany(e => e.Siparis_Detay)
                .WithRequired(e => e.Urun)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Urun>()
                .HasMany(e => e.Urun_Detay)
                .WithRequired(e => e.Urun)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Urun_Detay>()
                .Property(e => e.BirimFiyat)
                .HasPrecision(7, 2);

            modelBuilder.Entity<Yonetici>()
                .Property(e => e.Adi)
                .IsUnicode(false);

            modelBuilder.Entity<Yonetici>()
                .Property(e => e.Soyadi)
                .IsUnicode(false);
        }
    }

}
