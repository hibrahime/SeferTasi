
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

namespace SeferTasi.UI.WFA
{
    public partial class FormKullaniciEkle : FormBaseTasarim
    {
        MyContext db = new MyContext();
        public FormKullaniciEkle()
        {
            InitializeComponent();
        }

        private void FormKullaniciEkle_Load(object sender, EventArgs e)
        {
           
        }

        private void FormKullaniciEkle_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }

        private void formuTemizle(GroupBox tabPage)
        {
            foreach (Control item in tabPage.Controls)
            {
                if (item is TextBox)
                    (item as TextBox).Text = string.Empty;
                else if (item is ListBox)
                    (item as ListBox).SelectedIndex = 0;
                if (item is RichTextBox)
                    (item as RichTextBox).Text = string.Empty;
            }
        }

        private void btnFirmaKaydet_Click(object sender, EventArgs e)
        {
            
                Kullanici yeniKullanici = new Kullanici()
                {
                    KullaniciAdi = txtFirmaKullaniciAdi.Text,
                    Parola = txtFirmaParola.Text,
                    Email = txtFirmaMail.Text,
                    KullaniciAktifMi = false,
                    RolId = 2
                };



            using (var tran = db.Database.BeginTransaction())
            {
                try
                {
                    new KullaniciRepo().Ekle(yeniKullanici);
                    Kullanici sonKullanici = new KullaniciRepo().HepsiniGetir().LastOrDefault();
                    new FirmaRepo().Ekle(new Firma()
                    {
                        FirmaAdi = txtFirmaAdi.Text,
                        FirmaAktifMi = false,
                        Adres = txtFirmaAdres.Text,
                        Telefon = txtFirmaTelefon.Text,
                        MinTeslimSuresi = 30,
                        KullaniciId = sonKullanici.KullaniciId
                    });

                    MessageBox.Show("Kayıt Başarılı");
                }
                catch (Exception)
                {

                    MessageBox.Show("Kayıt Başarısız");
                }
                finally
                {
                    formuTemizle(groupBox1);
                    formuTemizle(groupBox2);
                }
                
            }
        }

        private void btnMüsteriKaydet_Click(object sender, EventArgs e)
        {
            
                Kullanici yeniKullanici = new Kullanici()
                {
                    KullaniciAdi = txtMüsteriKullanıcıAdi.Text,
                    Parola = txtMüsteriParola.Text,
                    Email = txtMüsteriMail.Text,
                    KullaniciAktifMi = true,
                    RolId = 3
                };
                using (var tran = db.Database.BeginTransaction())
                {
                try
                {
                    new KullaniciRepo().Ekle(yeniKullanici);

                    new MusteriRepo().Ekle(new Musteri()
                    {
                        MusteriAktifMi = true,
                        Adi = txtMüsteriAdi.Text,
                        Soyadi = txtMüsteriSoyadi.Text,
                        Telefon = txtMüsteriTelefon.Text,
                        Adres = txtMüsteriAdres.Text,
                        KullaniciId = yeniKullanici.KullaniciId
                    });
                    MessageBox.Show("Kayıt Başarılı");
                }
                catch (Exception)
                {

                    MessageBox.Show("Kayıt Başarısız");
                }
                finally
                {
                    formuTemizle(groupBox4);
                    formuTemizle(groupBox3);
                }
               
            }
          
        }
    }
}
