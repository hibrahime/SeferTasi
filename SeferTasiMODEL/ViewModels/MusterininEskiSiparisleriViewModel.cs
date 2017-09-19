using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeferTasiMODEL.ViewModels
{
   public  class MusterininEskiSiparisleriViewModel
    {
        public int SiparisId { get; set; }
        public string FirmaAdi { get; set; }
        public decimal ToplamFiyat { get; set; }
        public override string ToString()
        {
            return $"{SiparisId}) {FirmaAdi} - {ToplamFiyat}";
        }

    }
}
