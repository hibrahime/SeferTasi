using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeferTasiMODEL.ViewModels
{
    public class FirmaminSiparisleriViewModel
    {
        public int SiparisId { get; set; }
        public int UrunId { get; set; }
        public string MusteriAdi { get; set; }
        public string Adres { get; set; }
        public int Puan { get; set; }
        public decimal BirimFiyat { get; set; }
        public int Adet { get; set; }
        public decimal Toplam { get; set; }
        public override string ToString()
        {
            return $"{SiparisId} - {MusteriAdi} : {Toplam:c2}";
        }
    }
}
