using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeferTasiMODEL.ViewModels
{
    public class MusterininSepetiViewModel
    {
        public int UrunId { get; set; }
        public int Adet { get; set; }
        public string UrunAdi { get; set; }
        public decimal BirimFiyat { get; set; }
        public override string ToString()
        {
            return $"{Adet} x {UrunAdi} - Fiyat : {Adet * BirimFiyat:c2}";
        }
    }
}
