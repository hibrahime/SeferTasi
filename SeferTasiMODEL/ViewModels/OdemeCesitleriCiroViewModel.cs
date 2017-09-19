using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeferTasiMODEL.ViewModels
{
    public class OdemeCesitleriCiroViewModel
    {
        public int OdemeId { get; set; }
        public string OdemeCesidiAdi { get; set; }
        public decimal Fiyat { get; set; }
        public int Adet { get; set; }
        public decimal Toplam { get; set; }
    }
}
