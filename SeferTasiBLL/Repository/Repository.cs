using SeferTasiDAL;
using SeferTasiMODEL.Entities;
using SeferTasiMODEL.ViewModels;
//using SeferTasiMODEL.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeferTasiBLL.Repository
{
    public class RolRepo : RepositoryBase<Rol, int> { }
    public class KullaniciRepo : RepositoryBase<Kullanici, int>
    {
        public virtual Kullanici KullaniciGetir(string kullaniciAdi, string parola)
        {
            return new KullaniciRepo()
                .HepsiniGetir()
                .Where(x => x.KullaniciAdi.ToLower() == kullaniciAdi.ToLower() && x.Parola == parola)
                .FirstOrDefault();
        }
    }
    public class YoneticiRepo : RepositoryBase<Yonetici, int>
    {
        public Yonetici YoneticiyiGetir(int KullaniciId)
        {
            return new YoneticiRepo().HepsiniGetir().Where(x => x.KullaniciId == KullaniciId).FirstOrDefault();
        }
    }
    public class MusteriRepo : RepositoryBase<Musteri, int>
    {
        public Musteri MusteriyiGetir(int KullaniciId)
        {
            return new MusteriRepo().HepsiniGetir().Where(x => x.KullaniciId == KullaniciId).FirstOrDefault();
        }
        public List<MusterininEskiSiparisleriViewModel>EskiSiparislerim(int id)
        {
            MyContext db = new MyContext();
            var rapor = from s in db.Siparis
                        join sd in db.Siparis_Detay
                        on s.SiparisId equals sd.SiparisId
                        join f in db.Firma
                        on s.FirmaId equals f.FirmaId
                        where (s.MusteriId == id&& s.SiparisinUlastigiTarih!=null)
                        group new
                        {
                            sd,
                            s,
                            f
                        } by new
                        {
                            sipId = sd.SiparisId,
                            firAdi = f.FirmaAdi

                        } into sipIdfirAdiGrup
                        select new MusterininEskiSiparisleriViewModel()
                        {
                            SiparisId = sipIdfirAdiGrup.Key.sipId,
                            FirmaAdi = sipIdfirAdiGrup.Key.firAdi,
                            ToplamFiyat = sipIdfirAdiGrup.Sum(x=>x.sd.BirimFiyat*x.sd.Adet)
                        };
            return rapor.ToList();
         
                       
        }
    }
    public class FirmaRepo : RepositoryBase<Firma, int>
    {
        public Firma FirmayiGetir(int KullaniciId)
        {
            return new FirmaRepo().HepsiniGetir().Where(x => x.KullaniciId == KullaniciId).FirstOrDefault();
        }

        public List<FirmaninUrunleriTLViewModel> FirmaninUrunleriTL(int id/*, DateTime basZamani, DateTime bitZamani*/)
        {
            MyContext db = new MyContext();
            var rapor = from sd in db.Siparis_Detay
                        join s in db.Siparis on sd.SiparisId equals s.SiparisId
                        join u in db.Urun on sd.UrunId equals u.UrunId
                        where (s.FirmaId == id /*&& (s.SiparisVerildigiTarih.Subtract(basZamani).TotalHours >= 0 && bitZamani.Subtract(s.SiparisVerildigiTarih).TotalHours>=0)*/)
                        group new
                        {
                            sd,
                            s,
                            u
                        } by new
                        {
                            urnId = u.UrunId,
                            urnAdi = u.UrunAdi
                        } into UrunIdGrup
                        select new FirmaninUrunleriTLViewModel()
                        {
                            UrunId = UrunIdGrup.Key.urnId,
                            UrunAdi = UrunIdGrup.Key.urnAdi,
                            Toplam = UrunIdGrup.Sum(x => x.sd.BirimFiyat * x.sd.Adet)
                        };
            return rapor.ToList();
        }

        public List<Urun> FirmaninUrunleri(int id)
        {
            List<Urun> FirmaninUrunleri = new List<Urun>();
            foreach (var item in new UrunDetayRepo()
                .HepsiniGetir()
                .Where(x => x.FirmaId == id)
                .Select(x => x.UrunId)
                .ToList())
            {
                FirmaninUrunleri.Add(new UrunRepo().IDYiGetir(item));
            }
            return FirmaninUrunleri;
        }

        public List<FirmaminSiparisleriViewModel> FirmaminSiparisleri(int id)
        {
            MyContext db = new MyContext();
            var rapor = from sd in db.Siparis_Detay
                        join s in db.Siparis on sd.SiparisId equals s.SiparisId
                        join m in db.Musteri on s.MusteriId equals m.MusteriId
                        where (s.FirmaId == id)
                        group new
                        {
                            sd,
                            s,
                            m
                        } by new
                        {
                            sipId = sd.SiparisId,
                            musAdi = m.Adi + m.Soyadi

                        } into siparisIdGrup
                        select new FirmaminSiparisleriViewModel()
                        {
                            SiparisId = siparisIdGrup.Key.sipId,
                            MusteriAdi = siparisIdGrup.Key.musAdi,
                            Toplam = siparisIdGrup.Sum(x => x.sd.BirimFiyat * x.sd.Adet)
                        };
            return rapor.ToList();
        }
    }
    public class KategoriRepo : RepositoryBase<Kategori, int> { }
    public class UrunRepo : RepositoryBase<Urun, int> { }
    public class SiparisRepo : RepositoryBase<Siparis, int>
    {
        //public virtual Siparis SiparisGetir(int siparisID)
        //{
        //    return new SiparisRepo().HepsiniGetir().Where(x => x.SiparisId == siparisID).FirstOrDefault();
        //}
    }
    public class OdemeRepo : RepositoryBase<Odeme, int>
    {
        public List<OdemeCesitleriCiroViewModel> OdemeCesitleriCiro(int id)
        {
            MyContext db = new MyContext();
            var rapor = from sd in db.Siparis_Detay
                        join s in db.Siparis on sd.SiparisId equals s.SiparisId
                        join o in db.Odeme on s.OdemeId equals o.OdemeId
                        where (s.FirmaId == id)
                        group new
                        {
                            sd,
                            s,
                            o
                        } by new
                        {
                            odemeid = o.OdemeId,
                            odemeadi = o.OdemeCesidiAdi

                        } into odemeIdGrup
                        select new OdemeCesitleriCiroViewModel()
                        {
                            OdemeId = odemeIdGrup.Key.odemeid,
                            OdemeCesidiAdi = odemeIdGrup.Key.odemeadi,
                            Toplam = odemeIdGrup.Sum(x => x.sd.Adet * x.sd.BirimFiyat)
                        };
            return rapor.ToList();
        }
    }
    public class SiparisDetayRepo : RepositoryBase<Siparis_Detay, int>
    {
        public Siparis_Detay IDYiGetir(int UrunId, int SiparisId)
        {
            return new SiparisDetayRepo()
                .HepsiniGetir()
                .Where(x => x.UrunId == UrunId && x.SiparisId == SiparisId)
                .FirstOrDefault();
        }
    }
    public class UrunDetayRepo: RepositoryBase<Urun_Detay, int>
    {
        public Urun_Detay IDYiGetir(int UrunId, int FirmaId)
        {
            return new UrunDetayRepo()
                .HepsiniGetir()
                .Where(x => x.UrunId == UrunId && x.FirmaId == FirmaId)
                .FirstOrDefault();
        }
    }
}
