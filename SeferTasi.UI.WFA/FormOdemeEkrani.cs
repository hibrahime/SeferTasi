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
    public partial class FormOdemeEkrani : FormBaseTasarim
    {
        public FormOdemeEkrani()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 4)
                this.textBox2.Focus();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text.Length == 4)
                this.textBox3.Focus();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (textBox3.Text.Length == 4)
                this.textBox4.Focus();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex == -1) return;
            this.comboBox3.Focus();
        }

        private void FormOdemeEkrani_Load(object sender, EventArgs e)
        {
            cmbOdemeTurleri.DataSource = new OdemeRepo().HepsiniGetir();
            cmbOdemeTurleri.DisplayMember = "OdemeCesidiAdi";
            txtSepetTutari.Text = this.Text;

        }

        private void cmbOdemeTurleri_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbOdemeTurleri.SelectedIndex == 0)
            {
                   
                
            }
            if (cmbOdemeTurleri.SelectedIndex == 1)
            {
                
            }
            if (cmbOdemeTurleri.SelectedIndex == 2)
            {
                
            }
        }

        private void cbKayitliAdres_CheckedChanged(object sender, EventArgs e)
        {
            if (cbKayitliAdres.Checked)
            {
                txtSiparisAdres.Text = new MusteriRepo().MusteriyiGetir(FormSeferTasiGiris.kullanici.KullaniciId).Adres;
            }
            else
            {
               
            }
        
        }

        private void txtSiparisAdres_TextChanged(object sender, EventArgs e)
        {
            if(txtSiparisAdres.Text!= new MusteriRepo().MusteriyiGetir(FormSeferTasiGiris.kullanici.KullaniciId).Adres)
            {
                cbKayitliAdres.Checked = false;
            }
        }

        private void btnKapıdaOde_Click(object sender, EventArgs e)
        {
            
        }
    }
}
