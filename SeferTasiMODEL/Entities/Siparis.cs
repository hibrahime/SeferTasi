namespace SeferTasiMODEL.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    [Table("Siparisler")]
    public partial class Siparis
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Siparis()
        {
            Siparis_Detay = new HashSet<Siparis_Detay>();
        }

        public int SiparisId { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime SiparisVerildigiTarih { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime SiparisIstendigiTarih { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? SiparisinUlastigiTarih { get; set; }

        [Required]
        [StringLength(150)]
        public string Adres { get; set; }

        public bool? OnaylandiMi { get; set; } = true;

        public int? Puan { get; set; }

        public int? OdemeId { get; set; }

        public int? MusteriId { get; set; }

        public int? FirmaId { get; set; }

        public virtual Firma Firma { get; set; }

        public virtual Musteri Musteri { get; set; }

        public virtual Odeme Odeme { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Siparis_Detay> Siparis_Detay { get; set; }
    }
}
