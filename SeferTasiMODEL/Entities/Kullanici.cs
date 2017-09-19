namespace SeferTasiMODEL.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Kullanicilar")]
    public partial class Kullanici
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Kullanici()
        {
            Firma = new HashSet<Firma>();
            Musteri = new HashSet<Musteri>();
            Yonetici = new HashSet<Yonetici>();
        }

        public int KullaniciId { get; set; }

        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        [Required]
        [StringLength(10)]
        public string KullaniciAdi { get; set; }

        [Required]
        [StringLength(8)]
        public string Parola { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? KayitTarihi { get; set; } = DateTime.Now;

        public int? RolId { get; set; }

        public bool? KullaniciAktifMi { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Firma> Firma { get; set; }

        public virtual Rol Rol { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Musteri> Musteri { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Yonetici> Yonetici { get; set; }
    }
}
