namespace SeferTasiMODEL.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Yoneticiler")]
    public partial class Yonetici
    {
        public int YoneticiId { get; set; }

        [Required]
        [StringLength(20)]
        public string Adi { get; set; }

        [Required]
        [StringLength(20)]
        public string Soyadi { get; set; }

        public bool? YoneticiAktifMi { get; set; }

        public int? KullaniciId { get; set; }

        public virtual Kullanici Kullanici { get; set; }
    }
}
