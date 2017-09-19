namespace SeferTasiMODEL.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Musteriler")]
    public partial class Musteri
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Musteri()
        {
            Siparis = new HashSet<Siparis>();
        }

        public int MusteriId { get; set; }

        [Required]
        [StringLength(20)]
        public string Adi { get; set; }

        [Required]
        [StringLength(20)]
        public string Soyadi { get; set; }

        [Required]
        public string Adres { get; set; }

        [Required]
        [StringLength(11)]
        public string Telefon { get; set; }

        public bool? MusteriAktifMi { get; set; }

        public int? KullaniciId { get; set; }

        public virtual Kullanici Kullanici { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Siparis> Siparis { get; set; }
    }
}
