namespace SeferTasiMODEL.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Firmalar")]
    public partial class Firma
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Firma()
        {
            Siparis = new HashSet<Siparis>();
            Urun_Detay = new HashSet<Urun_Detay>();
        }

        public int FirmaId { get; set; }

        [Required]
        [StringLength(20)]
        public string FirmaAdi { get; set; }

        [Required]
        public string Adres { get; set; }

        [Required]
        [StringLength(11)]
        public string Telefon { get; set; }

        public int? KullaniciId { get; set; }

        public bool? FirmaAktifMi { get; set; }

        public int? MinTeslimSuresi { get; set; } = 30;

        public virtual Kullanici Kullanici { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Siparis> Siparis { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Urun_Detay> Urun_Detay { get; set; }
    }
}
