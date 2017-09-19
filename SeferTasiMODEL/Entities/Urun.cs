namespace SeferTasiMODEL.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Urunler")]
    public partial class Urun
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Urun()
        {
            Siparis_Detay = new HashSet<Siparis_Detay>();
            Urun_Detay = new HashSet<Urun_Detay>();
        }

        public int UrunId { get; set; }

        [Required]
        [StringLength(40)]
        public string UrunAdi { get; set; }

        public int? KategoriId { get; set; }

        [Column(TypeName ="varbinary(max)")]
        public byte[] Fotograf { get; set; }

        public bool? UrunAktifMi { get; set; }

        public virtual Kategori Kategori { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Siparis_Detay> Siparis_Detay { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Urun_Detay> Urun_Detay { get; set; }
    }
}
