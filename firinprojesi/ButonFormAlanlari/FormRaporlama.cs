using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace firinprojesi
{
    public partial class FormRaporlama : Form
    {
        // 🔧 Windows API fonksiyonları
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        public FormRaporlama()
        {
            InitializeComponent();
        }

        private void FormRaporlama_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None;
            
            Veritabani.BaglantiAc();
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM IslemKayit ORDER BY tarih DESC", Veritabani.conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dgvRaporlar.DataSource = dt;
            Veritabani.BaglantiKapat();
            dgvRaporlar.Columns["islemId"].HeaderText = "ID";
            dgvRaporlar.Columns["kullaniciAd"].HeaderText = "Kullanıcı";
            dgvRaporlar.Columns["islemTipi"].HeaderText = "İşlem Türü";
            dgvRaporlar.Columns["tabloAdi"].HeaderText = "Hedef Tablo";
            dgvRaporlar.Columns["aciklama"].HeaderText = "Açıklama";
            dgvRaporlar.Columns["tarih"].HeaderText = "Tarih";
            cmbIslemTuru.Items.Clear();
            cmbIslemTuru.Items.AddRange(new string[] { "Tümü", "Ekleme", "Güncelleme", "Silme", "Üretim", "Satış" });
            cmbIslemTuru.SelectedIndex = 0;

            // DateTimePicker başlangıç ve bitiş tarihlerini bugüne ayarla
            dtBaslangic.Value = DateTime.Today;
            dtBitis.Value = DateTime.Today;

            // Form açıldığında tüm kayıtları göster
            TumKayitlariGetir();
            this.Paint += new PaintEventHandler(FormRaporlama_Paint);
            Button[] butonlar = { btnUret, button1,};

            foreach (Button btn in butonlar)
            {
                // Normal görünüm
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderColor = Color.Black;
                btn.FlatAppearance.BorderSize = 2;
                btn.BackColor = Color.Gainsboro;
                btn.ForeColor = Color.Black;
                btn.Font = new Font("Century Gothic", 10, FontStyle.Bold);

                // Hover efekti - patron stilinde
                btn.MouseEnter += (s, ev) =>
                {
                    btn.BackColor = Color.FromArgb(64, 64, 64); // koyu gri
                    btn.ForeColor = Color.White;
                };

                btn.MouseLeave += (s, ev) =>
                {
                    btn.BackColor = Color.Gainsboro;
                    btn.ForeColor = Color.Black;
                };
            }
        }
        private void FormRaporlama_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, this.ClientRectangle,
                Color.Black, 6, ButtonBorderStyle.Solid,
                Color.Black, 6, ButtonBorderStyle.Solid,
                Color.Black, 6, ButtonBorderStyle.Solid,
                Color.Black, 6, ButtonBorderStyle.Solid);
        }


        private void TumKayitlariGetir()
        {
            Veritabani.BaglantiAc();
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM IslemKayit ORDER BY tarih DESC", Veritabani.conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dgvRaporlar.DataSource = dt;
            Veritabani.BaglantiKapat();
        }




        private void txtAciklamaAra_TextChanged(object sender, EventArgs e)
        {

        }

        
        
        private void FormRaporlama_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void dtBaslangic_ValueChanged(object sender, EventArgs e)
        {
            if (dtBaslangic.Checked)
            {
                DateTime seciliTarih = dtBaslangic.Value; // Kullanıcı tarih seçmiş
            }
            else
            {
                // Tarih seçilmemiş gibi davran
            }
        }

        

        private void btnTemizle_Click(object sender, EventArgs e)
        {
            dtBaslangic.Value = DateTime.Today;
            dtBitis.Value = DateTime.Today;
            cmbIslemTuru.SelectedIndex = 0;
            txtAciklamaAra.Clear();

            // 🔁 Tüm kayıtları tekrar getir
            TumKayitlariGetir();
        }

        private void btnUret_Click(object sender, EventArgs e)
        {
            Veritabani.BaglantiAc();

            string sorgu = "SELECT * FROM IslemKayit WHERE 1=1";

            // 🔹 Tarih filtresi
            sorgu += " AND tarih BETWEEN @t1 AND @t2";

            // 🔹 İşlem tipi
            if (cmbIslemTuru.Text != "Tümü")
                sorgu += " AND islemTipi = @tip";

            // 🔹 Açıklama araması
            if (!string.IsNullOrWhiteSpace(txtAciklamaAra.Text))
                sorgu += " AND aciklama LIKE @aciklama";

            // 🔹 ID filtre
            if (!string.IsNullOrWhiteSpace(txtIdAra.Text))
                sorgu += " AND islemId = @id";

            // 🔹 Kullanıcı filtre
            if (!string.IsNullOrWhiteSpace(txtKullaniciAra.Text))
                sorgu += " AND kullaniciAd LIKE @kullanici";

            sorgu += " ORDER BY tarih DESC";

            SqlCommand komut = new SqlCommand(sorgu, Veritabani.conn);
            komut.Parameters.AddWithValue("@t1", dtBaslangic.Value.Date);
            komut.Parameters.AddWithValue("@t2", dtBitis.Value.Date.AddDays(1).AddTicks(-1));

            if (cmbIslemTuru.Text != "Tümü")
                komut.Parameters.AddWithValue("@tip", cmbIslemTuru.Text);

            if (!string.IsNullOrWhiteSpace(txtAciklamaAra.Text))
                komut.Parameters.AddWithValue("@aciklama", "%" + txtAciklamaAra.Text + "%");

            if (!string.IsNullOrWhiteSpace(txtIdAra.Text))
                komut.Parameters.AddWithValue("@id", Convert.ToInt32(txtIdAra.Text));

            if (!string.IsNullOrWhiteSpace(txtKullaniciAra.Text))
                komut.Parameters.AddWithValue("@kullanici", "%" + txtKullaniciAra.Text + "%");

            SqlDataAdapter da = new SqlDataAdapter(komut);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dgvRaporlar.DataSource = dt;

            Veritabani.BaglantiKapat();
        }
        

        private void button1_Click(object sender, EventArgs e)
        {
            dtBaslangic.Value = DateTime.Today;
            dtBitis.Value = DateTime.Today;
            cmbIslemTuru.SelectedIndex = 0;
            txtAciklamaAra.Clear();
            txtIdAra.Clear();
            txtKullaniciAra.Clear();
            // 🔁 Tüm kayıtları tekrar getir
            TumKayitlariGetir();

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvRaporlar_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dtBitis_ValueChanged(object sender, EventArgs e)
        {
            if (dtBitis.Checked)
            {
                DateTime seciliTarih = dtBitis.Value; // Kullanıcı tarih seçmiş
            }
            else
            {
                // Tarih seçilmemiş gibi davran
            }
        }
    }
    }

