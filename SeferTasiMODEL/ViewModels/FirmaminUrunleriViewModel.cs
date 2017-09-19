using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeferTasiMODEL.ViewModels
{
    public class FirmaminUrunleriViewModel
    {
        public int UrunId { get; set; }
        public int FirmaId { get; set; }
        public string UrunAdi { get; set; }
        public decimal Fiyat { get; set; }
        
        //public byte[] Fotograf { get; set; }
    }
}
