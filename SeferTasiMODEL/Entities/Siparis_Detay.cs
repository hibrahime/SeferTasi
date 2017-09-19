namespace SeferTasiMODEL.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Siparis Detaylari")]
    public partial class Siparis_Detay
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SiparisId { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UrunId { get; set; }

        public decimal BirimFiyat { get; set; }

        public int Adet { get; set; }

        public virtual Siparis Siparis { get; set; }

        public virtual Urun Urun { get; set; }
        
    }
}
