using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices; // En üste ekle

namespace firinprojesi
{
    public partial class FormStok : Form
    {

        // Windows API'lerinden yardım alacağız
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HTCAPTION = 0x2;

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        public FormStok()
        {
            InitializeComponent();
        }
        private void Listele()
        {
            
                SqlDataAdapter da = new SqlDataAdapter(@"
    SELECT 
        uId AS [ID],
        uUrunAd AS [Malzeme Adı],
        uUrunKod AS [Kod],
        uUrunMiktar AS [Miktar],
        uKritikSeviye AS [Kritik Seviye],
        dId AS [Depo ID]
    FROM Urun", Veritabani.conn);

                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvUrunler.DataSource = dt;

                Veritabani.BaglantiKapat();
            
        }

        private void FormStok_Load(object sender, EventArgs e)
        {
            Listele();
            this.FormBorderStyle = FormBorderStyle.None;
            this.Paint += new PaintEventHandler(FormStok_Paint);
            Button[] butonlar = { btnEkle, btnGuncelle, btnSil, btnTemizle };

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
        private void FormStok_Paint(object sender, PaintEventArgs e)
        {
            // Kenarlık çiz (siyah, kalınlık: 2 piksel)
            ControlPaint.DrawBorder(e.Graphics, this.ClientRectangle,
                Color.Black, 6, ButtonBorderStyle.Solid,  // Sol
                Color.Black, 6, ButtonBorderStyle.Solid,  // Üst
                Color.Black, 6, ButtonBorderStyle.Solid,  // Sağ
                Color.Black, 6, ButtonBorderStyle.Solid); // Alt
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }
        private void Temizle()
        {
            txtUrunID.Clear();
            txtUrunAd.Clear();
            txtUrunKod.Clear();
            nudMiktar.Value = 0;
            txtKritik.Clear();
            txtDID.Clear();
        }


        private void dgvUrunler_CellClick(object sender, DataGridViewCellEventArgs e)
        {
           
            if (e.RowIndex >= 0 && !dgvUrunler.Rows[e.RowIndex].IsNewRow)
            {
                DataGridViewRow row = dgvUrunler.Rows[e.RowIndex];

                txtUrunID.Text = row.Cells["ID"].Value?.ToString();
                txtUrunAd.Text = row.Cells["Malzeme Adı"].Value?.ToString();
                txtUrunKod.Text = row.Cells["Kod"].Value?.ToString();

                // Güvenli dönüşüm
                object miktarDegeri = row.Cells["Miktar"].Value;
                nudMiktar.Value = miktarDegeri == DBNull.Value ? 0 : Convert.ToDecimal(miktarDegeri);

                txtKritik.Text = row.Cells["Kritik Seviye"].Value?.ToString();
                txtDID.Text = row.Cells["Depo ID"].Value?.ToString();
            
        }


        }


        private void btnEkle_Click(object sender, EventArgs e)
        {
            try
            {
                Veritabani.BaglantiAc();
                SqlCommand komut = new SqlCommand("INSERT INTO Urun (uId, uUrunAd, uUrunKod,uUrunMiktar, uKritikSeviye, dId) VALUES (@uId, @ad, @kod,@miktar, @kritik, @did)", Veritabani.conn);
                komut.Parameters.AddWithValue("@uId", Convert.ToInt32(txtUrunID.Text));
                komut.Parameters.AddWithValue("@ad", txtUrunAd.Text);
                komut.Parameters.AddWithValue("@kod", txtUrunKod.Text);
                komut.Parameters.AddWithValue("@miktar", nudMiktar.Value);
                komut.Parameters.AddWithValue("@kritik", Convert.ToInt32(txtKritik.Text));
                komut.Parameters.AddWithValue("@did", Convert.ToInt32(txtDID.Text));
                komut.ExecuteNonQuery();
                SqlCommand log = new SqlCommand(@"
    INSERT INTO IslemKayit (kullaniciAd, islemTipi, tabloAdi, aciklama)
    VALUES (@kulAd, @tip, @tablo, @aciklama)", Veritabani.conn);

                log.Parameters.AddWithValue("@kulAd", Oturum.KullaniciAdi + " (" + Oturum.KullaniciRol + ")"); log.Parameters.AddWithValue("@tip", "Ekleme");
                log.Parameters.AddWithValue("@tablo", "Stok");
                log.Parameters.AddWithValue("@aciklama", $"{txtUrunAd.Text} adlı malzeme eklendi.");
                log.ExecuteNonQuery();
                MessageBox.Show("Malzemeniz başarılı bir şekilde eklendi.");
                Veritabani.BaglantiKapat();
                Listele();
                Temizle();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
            
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            try
            {
                Veritabani.BaglantiAc();
                SqlCommand komut = new SqlCommand("UPDATE Urun SET uUrunAd=@ad, uUrunKod=@kod,uUrunMiktar=@miktar, uKritikSeviye=@kritik, dId=@did WHERE uId=@id", Veritabani.conn);
                komut.Parameters.AddWithValue("@id", Convert.ToInt32(txtUrunID.Text));
                komut.Parameters.AddWithValue("@ad", txtUrunAd.Text);
                komut.Parameters.AddWithValue("@kod", txtUrunKod.Text);

                komut.Parameters.AddWithValue("@miktar", nudMiktar.Value);
                komut.Parameters.AddWithValue("@kritik", Convert.ToInt32(txtKritik.Text));
                komut.Parameters.AddWithValue("@did", Convert.ToInt32(txtDID.Text));
                komut.ExecuteNonQuery();
                SqlCommand log = new SqlCommand(@"
    INSERT INTO IslemKayit (kullaniciAd, islemTipi, tabloAdi, aciklama)
    VALUES (@kulAd, @tip, @tablo, @aciklama)", Veritabani.conn);

                log.Parameters.AddWithValue("@kulAd", "admin");
                log.Parameters.AddWithValue("@tip", "Güncelleme");
                log.Parameters.AddWithValue("@tablo", "Stok");
                log.Parameters.AddWithValue("@aciklama", $"{txtUrunAd.Text} adlı malzeme güncellendi.");
                log.ExecuteNonQuery();
                MessageBox.Show("Malzemeniz başarılı bir şekilde güncellendi.");
                Veritabani.BaglantiKapat();
                Listele();
                Temizle();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
            
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            try
            {
                Veritabani.BaglantiAc();
                SqlCommand komut = new SqlCommand("DELETE FROM Urun WHERE uId=@id", Veritabani.conn);
                komut.Parameters.AddWithValue("@id", Convert.ToInt32(txtUrunID.Text));
                komut.ExecuteNonQuery();
                SqlCommand log = new SqlCommand(@"
    INSERT INTO IslemKayit (kullaniciAd, islemTipi, tabloAdi, aciklama)
    VALUES (@kulAd, @tip, @tablo, @aciklama)", Veritabani.conn);

                log.Parameters.AddWithValue("@kulAd", "admin");
                log.Parameters.AddWithValue("@tip", "Silme");
                log.Parameters.AddWithValue("@tablo", "Stok");
                log.Parameters.AddWithValue("@aciklama", $"{txtUrunAd.Text} adlı malzeme silindi.");
                log.ExecuteNonQuery();
                MessageBox.Show("Malzemeniz başarılı bir şekilde silindi.");
                Veritabani.BaglantiKapat();
                Listele();
                Temizle();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
            

        }

        private void btnTemizle_Click(object sender, EventArgs e)
        {
            Temizle();

        }

        private void FormStok_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
        }

        private void dgvUrunler_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
