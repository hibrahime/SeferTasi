using SeferTasiBLL.Repository;
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
    public partial class FormSeferTasiGiris : FormBaseTasarim
    {
        public FormSeferTasiGiris()
        {
            InitializeComponent();
        }

        public static List<Kullanici> kullanicilar = new List<Kullanici>();
        public static List<Yonetici> yoneticiler = new List<Yonetici>();
        public static List<Firma> firmalar = new List<Firma>();
        public static List<Musteri> musteriler = new List<Musteri>();
        public static Kullanici kullanici = new Kullanici();
        public static Yonetici yonetici = new Yonetici();
        public static Firma firma = new Firma();
        public static Musteri musteri = new Musteri();

        private void btnGiris_Click(object sender, EventArgs e)
        {


            try
            {
                kullanici = new KullaniciRepo().KullaniciGetir(txtKullaniciAdi.Text, txtParola.Text);

                if (new RolRepo().IDYiGetir((int)kullanici.RolId).RolAdi == "Yönetici" && kullanici.KullaniciAktifMi != false)
                {
                    FormYonetici formYonetici = new FormYonetici();
                    yonetici = new YoneticiRepo().YoneticiyiGetir(kullanici.KullaniciId);
                    formYonetici.ShowDialog();
                }
                else if (new RolRepo().IDYiGetir((int)kullanici.RolId).RolAdi == "Firma" && kullanici.KullaniciAktifMi != false)
                {
                    FormFirma formFirma = new FormFirma();
                    firma = new FirmaRepo().FirmayiGetir(kullanici.KullaniciId);
                    formFirma.ShowDialog();
                }
                else if (new RolRepo().IDYiGetir((int)kullanici.RolId).RolAdi == "Müsteri" && kullanici.KullaniciAktifMi != false)
                {
                    FormMüsteri formMusteri = new FormMüsteri();
                    musteri = new MusteriRepo().MusteriyiGetir(kullanici.KullaniciId);
                    formMusteri.ShowDialog();
                }
                else
                {
                    throw new Exception("Geçersiz Kullanıcı! Üye iseniz lütfen geçerli bir kullanıcı adı ve parola ile giriş yapınız..");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                txtKullaniciAdi.Clear();
                txtParola.Clear();
            }
        }

        private void FormSeferTasiGiris_Load(object sender, EventArgs e)
        {
            kullanicilar = new KullaniciRepo().HepsiniGetir();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FormKullaniciEkle formKullaniciEkle = new FormKullaniciEkle();
            formKullaniciEkle.ShowDialog();
            formKullaniciEkle.Activate();
        }
    }
}
