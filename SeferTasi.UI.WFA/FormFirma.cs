using SeferTasiBLL.Repository;
using SeferTasiBLL.Utilities;
using SeferTasiMODEL.Entities;
using SeferTasiMODEL.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace SeferTasi.UI.WFA
{
    public partial class FormFirma : FormBaseTasarim
    {
        public FormFirma()
        {
            InitializeComponent();
        }

        Firma firma;

        private void FormFirma_Load(object sender, EventArgs e)
        {
            firma = new FirmaRepo().IDYiGetir(FormSeferTasiGiris.firma.FirmaId);
            this.Text = FormSeferTasiGiris.kullanici.KullaniciAdi;
            siparislerimiDoldur();
            urunlerimiDoldur();
            chartDoldur();
            profilimiDoldur();
            trbTeslimSuresi.Value = (int)firma.MinTeslimSuresi;
            lblTeslimSuresi.Text = firma.MinTeslimSuresi.ToString();
            lblFirmaPuan.Text = new SiparisRepo().HepsiniGetir().Where(x => x.FirmaId == firma.FirmaId).Average(x => x.Puan).ToString();
            lblPuanlananOrani.Text = new SiparisRepo().HepsiniGetir().Where(x=> x.Puan!=null&& x.FirmaId == firma.FirmaId).ToList().Count.ToString() +"/"+ new SiparisRepo().HepsiniGetir().Where(x=>x.FirmaId==firma.FirmaId).ToList().Count.ToString();
        }

        private void siparislerimiDoldur()
        {
            lstSiparişler.DataSource = new FirmaRepo().FirmaminSiparisleri(firma.FirmaId);
        }

        private void urunlerimiDoldur()
        {
            cmbKategoriler.DisplayMember = "KategoriAdi";
            cmbKategoriler.DataSource = new KategoriRepo().HepsiniGetir();

            lstÜrünlerim.DisplayMember = "UrunAdi";
            lstÜrünlerim.DataSource = new FirmaRepo().FirmaninUrunleri(firma.FirmaId);
        }

        private void seciliUrunuDoldur()
        {
            if (seciliUrun == null) return;
            foreach (Kategori item in cmbKategoriler.Items)
            {
                if (item.KategoriId == seciliUrun.KategoriId)
                    cmbKategoriler.SelectedItem = item;
            }
            txtUrunAdi.Text = seciliUrun.UrunAdi;
            txtFiyat.Text = new UrunDetayRepo().IDYiGetir(seciliUrun.UrunId, firma.FirmaId).BirimFiyat.ToString();

            if (seciliUrun.Fotograf.Length != 0)
            {
                memoryStream = new MemoryStream(seciliUrun.Fotograf);
                pbFotograf.Image = new Bitmap(new MemoryStream(seciliUrun.Fotograf));
            }
            else
            {
                pbFotograf.Image = Properties.Resources.Food;
            }
        }

        private void profilimiDoldur()
        {
            profilimKullanici = new KullaniciRepo().IDYiGetir(FormSeferTasiGiris.kullanici.KullaniciId);
            txtProfilimFirmaAdı.Text = firma.FirmaAdi;
            txtProfilimAdres.Text = firma.Adres;
            txtProfilimTelefon.Text = firma.Telefon;
            txtProfilimKullaniciAdi.Text = profilimKullanici.KullaniciAdi;
            txtProfilimParola.Text = profilimKullanici.Parola;
            txtProfilimParolaTekrar.Text = profilimKullanici.Parola;
            txtProfilimEMail.Text = profilimKullanici.Email;
        }

        Siparis seciliSiparis;

        private void lstSiparişler_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstSiparişler.SelectedItem == null) return;
            seciliSiparis = new SiparisRepo().IDYiGetir((lstSiparişler.SelectedItem as FirmaminSiparisleriViewModel).SiparisId);
            
        }

        private void btnTeslimEdildi_Click(object sender, EventArgs e)
        {
            if (lstSiparişler.SelectedItem == null) return;
            seciliSiparis = new SiparisRepo().IDYiGetir((lstSiparişler.SelectedItem as FirmaminSiparisleriViewModel).SiparisId);

            seciliSiparis.SiparisinUlastigiTarih = DateTime.Now;
            new SiparisRepo().Guncelle();
            
        }

        private void btnReddet_Click(object sender, EventArgs e)
        {
            if (lstSiparişler.SelectedItem == null) return;
            seciliSiparis = new SiparisRepo().IDYiGetir((lstSiparişler.SelectedItem as FirmaminSiparisleriViewModel).SiparisId);

            foreach (Siparis_Detay item in (new SiparisDetayRepo().HepsiniGetir().Where(x => x.SiparisId == seciliSiparis.SiparisId).ToList()))
                new SiparisDetayRepo().Sil(item);

            new SiparisRepo().Sil(seciliSiparis);
            //seciliSiparis.OnaylandiMi = false;
            //siparis ve siparis detay silinmeli ya da aktifliği kalkmalı

        }

        Urun seciliUrun;
        Urun_Detay seciliUrunDetay;

        private void lstÜrünlerim_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstÜrünlerim.SelectedItem == null) return;
            seciliUrun = new UrunRepo().IDYiGetir((lstÜrünlerim.SelectedItem as Urun).UrunId);
            seciliUrunDetay = new UrunDetayRepo().IDYiGetir(seciliUrun.UrunId, firma.FirmaId);
            seciliUrunuDoldur();
        }

        bool UrunVarMi = false;
        int EklenecekUrunId;

        private void btnUrunEkle_Click(object sender, EventArgs e)
        {
            try
            {
                var kategori = cmbKategoriler.SelectedItem as Kategori;
                if (kategori != null)
                {

                    if (!UrunVarMi)
                    {
                        Urun yeniUrun = new Urun()
                        {
                            UrunAdi = txtUrunAdi.Text,
                            KategoriId = kategori.KategoriId,
                            Fotograf = memoryStream.ToArray(),
                            UrunAktifMi = true
                        };
                        new UrunRepo().Ekle(yeniUrun);
                        EklenecekUrunId = yeniUrun.UrunId;
                    }

                    new UrunDetayRepo().Ekle(new Urun_Detay()
                    {
                        BirimFiyat = Convert.ToDecimal(txtFiyat.Text),
                        FirmaId = firma.FirmaId,
                        UrunId = EklenecekUrunId
                    });
                }
                formuTemizle(this.tabPage2);
                urunlerimiDoldur();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void formuTemizle(TabPage tabPage)
        {

            foreach (Control item in tabPage.Controls)
            {
                if (item is TextBox)
                    (item as TextBox).Text = string.Empty;
                else if (item is ListBox)
                    if ((item as ListBox).Items.Count > 0)
                        (item as ListBox).SelectedIndex = 0;
                else if (item is ComboBox)
                    (item as ComboBox).SelectedIndex = 0;
                else if (item is PictureBox)
                    (item as PictureBox).Image = null;
                else if (item is RichTextBox)
                    (item as RichTextBox).Text = string.Empty;
            }
        }

        private void btnUrunSil_Click(object sender, EventArgs e)
        {
            try
            {
                seciliUrunDetay = new UrunDetayRepo().IDYiGetir(seciliUrun.UrunId, firma.FirmaId);
                new UrunDetayRepo().Sil(seciliUrunDetay);
                seciliUrun = new UrunRepo().IDYiGetir(seciliUrun.UrunId);
                new UrunRepo().Sil(seciliUrun);
                formuTemizle(this.tabPage2);
                urunlerimiDoldur();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnUrunGuncelle_Click(object sender, EventArgs e)
        {
            try
            {
                seciliUrun = new UrunRepo().IDYiGetir(seciliUrun.UrunId);
                seciliUrun.UrunAdi = txtUrunAdi.Text;
                seciliUrun.Fotograf = memoryStream.ToArray();
                seciliUrunDetay = new UrunDetayRepo().IDYiGetir(seciliUrun.UrunId, firma.FirmaId);
                seciliUrunDetay.BirimFiyat = Convert.ToDecimal(txtFiyat.Text);
                new UrunDetayRepo().Guncelle();
                new UrunRepo().Guncelle();
                formuTemizle(this.tabPage2);
                urunlerimiDoldur();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        Kullanici profilimKullanici;

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            try
            {
                profilimKullanici = new KullaniciRepo().IDYiGetir(FormSeferTasiGiris.kullanici.KullaniciId);
                profilimKullanici.KullaniciAdi = txtProfilimKullaniciAdi.Text;
                profilimKullanici.Parola = txtProfilimParola.Text;
                profilimKullanici.Email = txtProfilimEMail.Text;
                new KullaniciRepo().Guncelle();
                firma = new FirmaRepo().IDYiGetir(firma.FirmaId);
                firma.FirmaAdi = txtProfilimFirmaAdı.Text;
                firma.Telefon = txtProfilimTelefon.Text;
                firma.Adres = txtProfilimAdres.Text;
                new FirmaRepo().Guncelle();
                formuTemizle(this.tabPage4);
                profilimiDoldur();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnUyelikSil_Click(object sender, EventArgs e)
        {
            try
            {
                firma = new FirmaRepo().IDYiGetir(firma.FirmaId);
                new FirmaRepo().Sil(firma);
                profilimKullanici = new KullaniciRepo().IDYiGetir(FormSeferTasiGiris.kullanici.KullaniciId);
                new KullaniciRepo().Sil(profilimKullanici);
                formuTemizle(this.tabPage4);
                this.Close();
                this.Dispose();
            }
            catch (Exception ex)
            {
                throw new Exception("Firma Silinemedi! Diğer tablolarla ilişki devam ediyor.\n" + ex.Message);
            }
        }

        MemoryStream memoryStream = new MemoryStream();
        int bufferSize = 64;
        byte[] resimArray = new byte[64];

        private void pbFotograf_Click(object sender, EventArgs e)
        {
            pbFotograf.Image = null;
            memoryStream = new MemoryStream();
            FotoAç.Title = "Bir fotoğraf dosyasını seçiniz";
            FotoAç.Filter = "JPG | *.jpg";
            FotoAç.Multiselect = false;
            FotoAç.FileName = string.Empty;
            FotoAç.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            if (FotoAç.ShowDialog() == DialogResult.OK)
            {
                FileStream dosya = File.Open(FotoAç.FileName, FileMode.Open);
                while (dosya.Read(resimArray, 0, bufferSize) != 0)
                {
                    memoryStream.Write(resimArray, 0, resimArray.Length);
                }
                dosya.Close();
                dosya.Dispose();
                pbFotograf.Image = new Bitmap(memoryStream);
            }
            else
            {
                pbFotograf.Image = Properties.Resources.Food;
            }
        }

        private void trbTeslimSuresi_Scroll(object sender, EventArgs e)
        {
            firma = new FirmaRepo().IDYiGetir(firma.FirmaId);
            lblTeslimSuresi.Text = trbTeslimSuresi.Value.ToString();
            firma.MinTeslimSuresi = trbTeslimSuresi.Value;
            new FirmaRepo().Guncelle();
        }

        public void chartDoldur()
        {
            foreach (var item in new FirmaRepo().FirmaninUrunleriTL(firma.FirmaId/*, dtpRaporBasla.Value, dtpRaporBitis.Value*/))
            {
                this.chrUrunTL.Series["Urunler"].Points.AddXY(item.UrunId + ")" + item.UrunAdi + ":" + item.Toplam, item.Toplam);
            }

            foreach (var item in new OdemeRepo().OdemeCesitleriCiro(firma.FirmaId))
            {
                this.chrOdemeCesitleriTL.Series["Ciro"].Points.AddXY(item.OdemeCesidiAdi, item.Toplam);
            }

            foreach (Control item in this.tabPage3.Controls)
            {
                if (item is Chart)
                {
                    cmbRaporCesitleri.Items.Add((item as Chart).Text);
                }
            }
        }

        private void cmbRaporCesitleri_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbRaporCesitleri.SelectedItem == null) return;
            foreach (Control item in this.tabPage3.Controls)
            {
                if (item is Chart)
                {
                    (item as Chart).Visible = false;
                    if ((item as Chart).Text == cmbRaporCesitleri.SelectedItem.ToString())
                    {
                        (item as Chart).Visible = true;
                    }
                }
            }

        }

        private void txtUrunAdi_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtUrunAdi.Text.Length == 0)
                    formuTemizle(tabPage2);

                string aranacak = txtUrunAdi.Text;
                var sorgu = new UrunRepo().HepsiniGetir().Where(x => x.UrunAdi.ToLower() == aranacak.ToLower()).FirstOrDefault();

                if (sorgu == null)
                {
                    UrunVarMi = false;
                    return;
                }

                UrunVarMi = true;
                EklenecekUrunId = sorgu.UrunId;

                foreach (Kategori item in cmbKategoriler.Items)
                    if (item.KategoriId == sorgu.KategoriId)
                        cmbKategoriler.SelectedItem = item;
                txtUrunAdi.Text = sorgu.UrunAdi;
                if (new UrunDetayRepo().HepsiniGetir().Where(x => x.FirmaId == firma.FirmaId && x.UrunId == sorgu.UrunId).Any())
                    txtFiyat.Text = new UrunDetayRepo().IDYiGetir(EklenecekUrunId, firma.FirmaId).BirimFiyat.ToString();
                else
                    txtFiyat.Text = string.Empty;
                if (sorgu.Fotograf.Length != 0)
                    pbFotograf.Image = new Bitmap(new MemoryStream(sorgu.Fotograf));
                else
                    pbFotograf.Image = Properties.Resources.Food;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Hatalı Arama - " + ex.Message);
            }
        }

        private void dtpRaporBasla_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}