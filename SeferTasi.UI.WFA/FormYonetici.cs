using SeferTasiBLL.Repository;
using SeferTasiDAL;
using SeferTasiMODEL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace SeferTasi.UI.WFA
{
    public partial class FormYonetici : FormBaseTasarim
    {
        public FormYonetici()
        {
            InitializeComponent();
        }

        Yonetici yonetici;
        MyContext db = new MyContext();
        public void ChartYenile()
        {
            
            Firma seciliFirma = cmbFirmalar.SelectedItem as Firma;
            foreach (var item in new FirmaRepo().FirmaninUrunleriTL(seciliFirma.FirmaId))
            {
                this.chartUrunler.Series["Urunler"].Points.AddXY(item.UrunId + ")" + item.UrunAdi + ":" + item.Toplam, item.Toplam);
            }

            foreach (var item in new OdemeRepo().OdemeCesitleriCiro(seciliFirma.FirmaId))
            {
                this.chartOdemeler.Series["Odemeler"].Points.AddXY(item.OdemeCesidiAdi, item.Toplam);
            }
        }
        private void FormYonetici_Load(object sender, EventArgs e)
        {
            yonetici = new YoneticiRepo().IDYiGetir(FormSeferTasiGiris.yonetici.YoneticiId);
            this.Text=FormSeferTasiGiris.kullanici.KullaniciAdi;
            firmalariGetir();
            profilimiDoldur();
            cmbFirmalar.DataSource = new FirmaRepo().HepsiniGetir().Where(x=>x.FirmaAktifMi==true).ToList();
            cmbFirmalar.DisplayMember = "FirmaAdi";
            //ChartDoldur();
        }
        
        /*private void ChartDoldur()
        {
            Chart chartUrunler = new Chart();
            Chart chartOdemeler = new Chart();
            Firma seciliFirma = cmbFirmalar.SelectedItem as Firma;
            foreach (var item in new FirmaRepo().FirmaninUrunleriTL(seciliFirma.FirmaId))
            {
                this.chartUrunler.Series["Urunler"].Points.AddXY(item.UrunId + ")" + item.UrunAdi + ":" + item.Toplam, item.Toplam);
            }

            foreach (var item in new OdemeRepo().OdemeCesitleriCiro(seciliFirma.FirmaId))
            {
                this.chartOdemeler.Series["Odemeler"].Points.AddXY(item.OdemeCesidiAdi, item.Toplam);
            }

            foreach (Control item in this.tabPage4.Controls)
            {
                if (item is Chart)
                {
                    cmbGrafikler.Items.Add((item as Chart).Text);
                }
            }
        }*/

        private void formuTemizle(TabPage tabPage)
        {
            foreach (Control item in tabPage.Controls)
            {
                if (item is TextBox)
                    (item as TextBox).Text = string.Empty;
                else if (item is ListBox)
                    (item as ListBox).SelectedIndex = 0;
            }
        }

        private void firmalariGetir()
        {
            lstFirmalar.DisplayMember = "FirmaAdi";
            lstFirmalar.DataSource = new FirmaRepo().HepsiniGetir();
        }
        private void FirmaListesiGuncelle()
        {
            cmbFirmalar.DataSource = new FirmaRepo().HepsiniGetir().Where(x => x.FirmaAktifMi == true).ToList();
        }

        private void profilimiDoldur()
        {
            txtProfilimAdı.Text = yonetici.Adi;
            txtProfilimSoyadı.Text = yonetici.Soyadi;
            txtProfilimKullaniciAdi.Text = FormSeferTasiGiris.kullanici.KullaniciAdi;
            txtProfilimParola.Text = FormSeferTasiGiris.kullanici.Parola;
            txtProfilimParolaTekrar.Text = FormSeferTasiGiris.kullanici.Parola;
            txtProfilimEMail.Text = FormSeferTasiGiris.kullanici.Email;
        }

        Firma seciliFirma;
        private void lstFirmalar_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstFirmalar.SelectedItem == null) return;
            seciliFirma = lstFirmalar.SelectedItem as Firma;
            firmaButonAyarla();
            lblFirmaAdı.Text = seciliFirma.FirmaAdi.ToString();
            lblPuan.Text = new SiparisRepo().HepsiniGetir().Where(x => x.FirmaId == seciliFirma.FirmaId).Average(x => x.Puan).ToString(); //Puan firmanın siparişlerinin puanlarının ortalaması sorgusu
            lblSiparisSayisi.Text = new SiparisRepo().HepsiniGetir().Count(x => x.FirmaId == seciliFirma.FirmaId).ToString();
            lblCirosu.Text = new FirmaRepo().FirmaminSiparisleri(seciliFirma.FirmaId).Sum(x => x.Toplam).ToString();
        }

        private void firmaButonAyarla()
        {
            seciliFirma = new FirmaRepo().IDYiGetir((lstFirmalar.SelectedItem as Firma).FirmaId);
            if (seciliFirma.FirmaAktifMi == true)
            {
                btnAskıyaAl.Enabled = true;
                btnOnayla.Enabled = false;
            }
            else
            {
                btnAskıyaAl.Enabled = false;
                btnOnayla.Enabled = true;
            }
        }

        private void btnAskıyaAl_Click(object sender, EventArgs e)
        {
            if (lstFirmalar.SelectedItem == null) return;
            seciliFirma = new FirmaRepo().IDYiGetir((lstFirmalar.SelectedItem as Firma).FirmaId);
            seciliFirma.FirmaAktifMi = false;
            Kullanici seciliKullanici = new KullaniciRepo().HepsiniGetir().Where(x => x.KullaniciId == seciliFirma.KullaniciId).FirstOrDefault();
            seciliKullanici.KullaniciAktifMi = false;
            new FirmaRepo().Guncelle();
            new KullaniciRepo().Guncelle();
            firmaButonAyarla();
            FirmaListesiGuncelle();

        }

        private void btnOnayla_Click(object sender, EventArgs e)
        {
            if (lstFirmalar.SelectedItem == null) return;
            seciliFirma = new FirmaRepo().IDYiGetir((lstFirmalar.SelectedItem as Firma).FirmaId);
            seciliFirma.FirmaAktifMi = true;
            Kullanici seciliKullanici = new KullaniciRepo().HepsiniGetir().Where(x => x.KullaniciId == seciliFirma.KullaniciId).FirstOrDefault();
            seciliKullanici.KullaniciAktifMi = true;
            new FirmaRepo().Guncelle();
            new KullaniciRepo().Guncelle();
            
            firmaButonAyarla();
            FirmaListesiGuncelle();
        }
        
        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            FormSeferTasiGiris.kullanici.KullaniciAdi = txtProfilimKullaniciAdi.Text;
            FormSeferTasiGiris.kullanici.Parola = txtProfilimParola.Text;
            FormSeferTasiGiris.kullanici.Email = txtProfilimEMail.Text;
            using (var tran=db.Database.BeginTransaction())
            {
                try
                {
                    new KullaniciRepo().Guncelle();
                    yonetici.Adi = txtProfilimAdı.Text;
                    yonetici.Soyadi = txtProfilimSoyadı.Text;
                    new YoneticiRepo().Guncelle();
                    MessageBox.Show("Güncelleme Başarılı");
                }
                catch (Exception)
                {

                    MessageBox.Show("Güncelleme Başarısız");;
                }
                finally
                {
                    formuTemizle(this.tabPage2);
                    profilimiDoldur();
                }
            }
            
        }
        
        private void btnUyelikSil_Click(object sender, EventArgs e)
        {
            try
                {
                    new YoneticiRepo().Sil(yonetici);
                    new KullaniciRepo().Sil(FormSeferTasiGiris.kullanici);
                    formuTemizle(this.tabPage2);
                    MessageBox.Show("Yönetici Sİlindi");
                    this.Close();
                    this.Dispose();
            }
                catch (Exception ex)
                {
                    MessageBox.Show("Yonetici Silinemedi !" + ex);
                }
            
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            
            
                Kullanici yeniKullanici = new Kullanici()
                {
                    KullaniciAdi = txtYoneticiKullaniciAdi.Text,
                    Parola = txtYoneticiParola.Text,
                    Email = txtYoneticiEMail.Text,
                    RolId = 1,
                    KullaniciAktifMi = true
                };
            using (var tran = db.Database.BeginTransaction())
            {
                try
                {
                    new KullaniciRepo().Ekle(yeniKullanici);

                    new YoneticiRepo().Ekle(new Yonetici()
                    {
                        Adi = txtYoneticiAdı.Text,
                        Soyadi = txtYoneticiSoyadı.Text,
                        YoneticiAktifMi = true,
                        KullaniciId = yeniKullanici.KullaniciId
                    });
                    formuTemizle(this.tabPage3);
                    MessageBox.Show("Kayıt Başarılı");
                }
                catch (Exception)
                {

                    MessageBox.Show("Kayıt Başarısız");
                }

            }
                
            
        }

        private void cmbFirmalar_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbFirmalar.SelectedItem == null) return;
            seciliFirma = cmbFirmalar.SelectedItem as Firma;
            cmbGrafikler.Enabled = true;
            cmbGrafikler.SelectedIndex = -1;
        }

        private void cmbGrafikler_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbGrafikler.SelectedItem == null) return;
            ChartYenile();
            foreach (Control item in this.tabPage4.Controls)
            {
                if (item is Chart)
                {
                    (item as Chart).Visible = false;
                    if ((item as Chart).Text == cmbGrafikler.SelectedItem.ToString())
                    {
                        (item as Chart).Visible = true;
                    }
                }
            }
        }
    }
}
