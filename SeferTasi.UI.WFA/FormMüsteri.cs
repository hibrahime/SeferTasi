using SeferTasiBLL.Repository;
using SeferTasiDAL;
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

namespace SeferTasi.UI.WFA
{
    public partial class FormMüsteri : FormBaseTasarim
    {
        List<MusterininSepetiViewModel> SepettekiUrunler = new List<MusterininSepetiViewModel>();
        Firma seciliFirma = new Firma();
       
        public FormMüsteri()
        {
            InitializeComponent();
        }

        
        private void tabPage2_Click(object sender, EventArgs e)
        {

        }
        
        private void formTemizle(GroupBox groupbox)
        {
            foreach (Control item in groupbox.Controls)
            {
                SepettekiUrunler.Clear();
                if (item is TextBox && (item.Name != "txtMüsteriMail" || item.Name != "txtMüsteriKullanıcıAdi"))
                    (item as TextBox).Text = string.Empty;
                else if(item.Text == "lstUrunler")
                {
                    (item as ListBox).Items.Clear();
                    SepeteEkle();
                }
                else if (item is NumericUpDown)
                    (item as NumericUpDown).Value = 1;
                else if (item is RichTextBox)
                    (item as RichTextBox).Text = string.Empty;
                else if (item is PictureBox)
                    (item as PictureBox).Image = null;
                else if (item is CheckBox)
                    (item as CheckBox).Checked = false;
                else if (item is DateTimePicker)
                    (item as DateTimePicker).Value = DateTime.Now;
                lblFiyat.Text = "0,00 ₺";
                cmbOdemeTurleri.SelectedIndex = 0;
                txtSiparisAdres.Text = string.Empty;
                btnSepetiSifirla.PerformClick();
                rbIleriTarihSiparis.Checked = false;
                dtpTarih.MinDate = new DateTime(1990, 8, 7);


            }
        }
        private void FormMüsteri_Load(object sender, EventArgs e)
        {
            lstFirmalar.DataSource = new FirmaRepo().HepsiniGetir().Where(x => x.FirmaAktifMi == true).ToList();
            lstFirmalar.DisplayMember = "FirmaAdi";
            profilimiDoldur();
            dtpTarih.Value.AddMinutes(2);
            cmbOdemeTurleri.DataSource = new OdemeRepo().HepsiniGetir().ToList();
            cmbOdemeTurleri.DisplayMember = "OdemeCesidiAdi";
            //lstEskiSiparisler.DisplayMember = "";
            //lstEskiSiparisler.DataSource = new MusteriRepo().EskiSiparislerim(musteri.MusteriId);
            foreach (var item in new MusteriRepo().EskiSiparislerim(musteri.MusteriId))
            {
                lstEskiSiparisler.Items.Add(item);
            }


        }

        private void txtArama_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtArama.Text.Length == 0)
                {
                    lstUrunler.DataSource = new FirmaRepo().FirmaninUrunleri(seciliFirma.FirmaId);
                    return;
                }
                string aranacak = txtArama.Text.ToLower();
                var sorgu = new FirmaRepo().FirmaninUrunleri(seciliFirma.FirmaId).Where(x => x.UrunAdi.ToLower().Contains(aranacak));
                lstUrunler.DataSource = sorgu.ToList();
            }
            catch (Exception ex)
            {

                MessageBox.Show("Hatalı Arama  " + ex.Message);
            }
        }
        MyContext db = new MyContext();
        private void rbIleriTarihSiparis_CheckedChanged(object sender, EventArgs e)
        {
            if (lstSepet.Items.Count >= 1 && rbAnlikSiparis.Checked == true)
                btnSiparisOnay.Enabled = true;
            dtpTarih.MinDate = DateTime.Now.AddMinutes(Convert.ToDouble(seciliFirma.MinTeslimSuresi));
            if (rbIleriTarihSiparis.Checked==true)
            {
                dtpTarih.Visible = true;
            }
            else
            {
                dtpTarih.Visible = false;
            }
            btnSiparisOnay.Enabled = true;
               
        }
        Form FormOdemeEkrani;
        DateTime tarih;
        
        private void btnSiparisOnay_Click(object sender, EventArgs e)
        {
            seciliFirma = lstFirmalar.SelectedItem as Firma;
            
            if (lstSepet.Items.Count == 0)
            { MessageBox.Show("Sepetinizde hiç ürün yok..");
                return;
            }
            try
            {
                
               if(rbAnlikSiparis.Checked==true)
                { tarih = DateTime.Now; }
               if(rbIleriTarihSiparis.Checked==true)
                { tarih = dtpTarih.Value; }
                var sonkayit = new Siparis()
                {
                    Adres = txtSiparisAdres.Text,
                    MusteriId = musteri.MusteriId,
                    FirmaId = seciliFirma.FirmaId,
                    OdemeId = (cmbOdemeTurleri.SelectedItem as Odeme).OdemeId,

                    SiparisIstendigiTarih = tarih,
                    SiparisVerildigiTarih = DateTime.Now
                    
                   
                };

                using (var tran = db.Database.BeginTransaction())
                {
                    new SiparisRepo().Ekle(sonkayit);

                    foreach (var item in SepettekiUrunler)
                    {
                        var sonnkayit = new Siparis_Detay()
                        {
                            SiparisId = sonkayit.SiparisId,
                            Adet = item.Adet,
                            BirimFiyat = item.BirimFiyat,
                            UrunId = item.UrunId

                        };
                        new SiparisDetayRepo().Ekle(sonnkayit);
                    }
                    MessageBox.Show($"Siparişiniz ilgili firmaya iletilmiştir. \nSiparişin tahmini teslim süresi {seciliFirma.MinTeslimSuresi} dakikadır.");
                   
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Sipariş Alınamadı  "+ ex.Message);
            }
            formTemizle(groupBox1);
            formTemizle(groupBox2);
            formTemizle(groupBox3);
            formTemizle(groupBox4);
            formTemizle(groupBox5);
        }

        private void lstUrunler_SelectedIndexChanged(object sender, EventArgs e)
        {
            Urun seciliurun = lstUrunler.SelectedItem as Urun;

            pbFoto.Image = new Bitmap(new MemoryStream(seciliurun.Fotograf));
            
            //pbFoto.Image = new Bitmap(new UrunRepo().FotoDonustur(seciliurun.Fotograf));
            
        }

        private void btnSepeteEkle_Click(object sender, EventArgs e)
        {
            if (lstUrunler.SelectedItem == null) return;
            Urun seciliUrun = lstUrunler.SelectedItem as Urun;
            bool sepetteMi = false;
            foreach (var urun in SepettekiUrunler)
            {
                if (urun.UrunId == seciliUrun.UrunId)
                    sepetteMi = true;
            }
            if (sepetteMi)
            {
                SepettekiUrunler.Where(x => x.UrunId == seciliUrun.UrunId).FirstOrDefault().Adet += Convert.ToInt32(numAdet.Value);
            }
            else
            {
                try
                {
                    SepettekiUrunler.Add(new MusterininSepetiViewModel()
                    {
                        Adet = (int)numAdet.Value,
                        BirimFiyat = new UrunDetayRepo().HepsiniGetir().Where(x => x.UrunId == seciliUrun.UrunId).FirstOrDefault().BirimFiyat,
                        UrunId = seciliUrun.UrunId,
                        UrunAdi = seciliUrun.UrunAdi
                    });
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
            }
            SepeteEkle();
            numAdet.Value = 1;
        }
        private void SepeteEkle()
        {
            lstSepet.Items.Clear();
            decimal sepetTutari = 0;
            foreach (var item in SepettekiUrunler)
            {
                lstSepet.Items.Add(item);
                sepetTutari += item.BirimFiyat * item.Adet;
            }
            lblFiyat.Text = $"{sepetTutari:c2}";
        }

        private void btnBilgiGuncelle_Click(object sender, EventArgs e)
        {
          
            try
            {
                
                musteri.Adi = txtMüsteriKullanıcıAdi.Text;
                musteri.Kullanici.Parola = txtMüsteriParola.Text;
                musteri.Kullanici.Email = txtMüsteriMail.Text;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            profilimiDoldur();
        }
        Musteri musteri = new MusteriRepo().HepsiniGetir().Where(x => x.KullaniciId == FormSeferTasiGiris.kullanici.KullaniciId).FirstOrDefault();
        private void lstFirmalar_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstFirmalar.SelectedItem == null) return;
            seciliFirma = lstFirmalar.SelectedItem as Firma;

            lstUrunler.DisplayMember = "UrunAdi";
            lstUrunler.DataSource = new FirmaRepo().FirmaninUrunleri(seciliFirma.FirmaId);
        }
        private void profilimiDoldur()
        {
           
            txtMüsteriKullanıcıAdi.Text = musteri.Adi;
            txtMüsteriMail.Text = musteri.Kullanici.Email;
            
        }

        private void btnUyelikIptal_Click(object sender, EventArgs e)
        {
            Musteri seciliMusteri = new MusteriRepo().HepsiniGetir().Where(x => x.MusteriId == musteri.MusteriId).FirstOrDefault();
            
            seciliMusteri.MusteriAktifMi = false;
            using (var tran = db.Database.BeginTransaction())
            {
                new MusteriRepo().Guncelle();
                Kullanici seciliKullanici = new KullaniciRepo().HepsiniGetir().Where(x => x.KullaniciId == seciliMusteri.KullaniciId).FirstOrDefault();
                seciliKullanici.KullaniciAktifMi = false;
                new KullaniciRepo().Guncelle();
                MessageBox.Show("Üyeliğiniz İptal Edilmiştir !");
                this.Dispose();
            }
                
            
            this.Dispose();
        }

        private void dtpTarih_ValueChanged(object sender, EventArgs e)
        {
            
        }

        private void rbAnlikSiparis_CheckedChanged(object sender, EventArgs e)
        {
            if (lstSepet.Items.Count >= 1 && rbAnlikSiparis.Checked == true)
                btnSiparisOnay.Enabled = true;
        }

       

        private void cmbOdemeTurleri_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void cbKayitliAdres_CheckedChanged(object sender, EventArgs e)
        {
            if (cbKayitliAdres.Checked)
            {
                txtSiparisAdres.Text = new MusteriRepo().MusteriyiGetir(FormSeferTasiGiris.kullanici.KullaniciId).Adres;
            }
        }

        private void txtSiparisAdres_TextChanged(object sender, EventArgs e)
        {
            if (txtSiparisAdres.Text != new MusteriRepo().MusteriyiGetir(FormSeferTasiGiris.kullanici.KullaniciId).Adres)
            {
                cbKayitliAdres.Checked = false;
            }
        }
        
        private void btnOyla_Click(object sender, EventArgs e)
        {
            try
            {
                var sorgu = lstEskiSiparisler.SelectedItem as MusterininEskiSiparisleriViewModel;
                Siparis seciliSiparis = new SiparisRepo().IDYiGetir(sorgu.SiparisId);//.SiparisGetir(sorgu.SiparisId);______ID yi getir yerine bunu yazdım aynı işi görüyor

                foreach (Control item in groupBox5.Controls)
                {
                    if (item is RadioButton)
                    {
                        if ((item as RadioButton).Checked == true)
                        {
                            seciliSiparis.Puan = Convert.ToInt32(item.Text);
                            new SiparisRepo().Guncelle();
                            MessageBox.Show($"Değerlendirme başarılı. \nBu siparişe {item.Text} puan verdiniz");
                            lstEskiSiparisler.Items.Clear();
                            foreach (var item2 in new MusteriRepo().EskiSiparislerim(musteri.MusteriId))
                            {
                                lstEskiSiparisler.Items.Add(item2);
                            }

                        }

                    }
                }
                
            }
            catch (Exception ex)
            {

                MessageBox.Show("Puanlama Başarısız. \nLütfen bir sipariş seçiniz");
            }
           
        }

        private void btnSepetiSifirla_Click(object sender, EventArgs e)
        {
            SepettekiUrunler.Clear();
            SepeteEkle();
            rbAnlikSiparis.Checked = false;
            rbIleriTarihSiparis.Checked = false;
            btnSiparisOnay.Enabled = false;
            
        }

        private void btnSeciliUrunuCikar_Click(object sender, EventArgs e)
        {
            foreach (MusterininSepetiViewModel item in lstSepet.SelectedItems)
            {
                SepettekiUrunler.Remove(item);
            }
            SepeteEkle();
        }
    }
}
