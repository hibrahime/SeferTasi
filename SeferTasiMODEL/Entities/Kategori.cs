namespace SeferTasiMODEL.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Kategoriler")]
    public partial class Kategori
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Kategori()
        {
            Urun = new HashSet<Urun>();
        }

        public int KategoriId { get; set; }

        [Required]
        [StringLength(20)]
        public string KategoriAdi { get; set; }

        [StringLength(80)]
        public string Aciklama { get; set; }

        public bool? KategoriAktifMi { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Urun> Urun { get; set; }
    }
}
