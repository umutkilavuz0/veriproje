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
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace firinprojesi
{
    public partial class FormSatis : Form
    {
        Dictionary<int, string> urunler = new Dictionary<int, string>();
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        private void Surukle(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
        public FormSatis()
        {
            InitializeComponent();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void FormSatis_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None;
            Veritabani.BaglantiAc();
            SqlCommand komut = new SqlCommand("SELECT urId, urUrunAd FROM Urunler", Veritabani.conn);
            SqlDataReader dr = komut.ExecuteReader();
            while (dr.Read())
            {
                urunler.Add(Convert.ToInt32(dr["urId"]), dr["urUrunAd"].ToString());
            }
            dr.Close();
            cmbSatisUrun.DataSource = new BindingSource(urunler, null);
            cmbSatisUrun.DisplayMember = "Value";
            cmbSatisUrun.ValueMember = "Key";
            Veritabani.BaglantiKapat();
            this.Paint += new PaintEventHandler(FormCalisanYonetim_Paint);


            UrunleriListele();
            Button[] butonlar = { btnSatisYap,};

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
        private void UrunleriListele()
        {
            Veritabani.BaglantiAc();
            SqlDataAdapter da = new SqlDataAdapter("SELECT urId, urUrunAd, urUrunMiktar, urUrunFiyat FROM Urunler", Veritabani.conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dgvUrunler.DataSource = dt;
            Veritabani.BaglantiKapat();

            // 🔽 Sütun başlıklarını kullanıcı dostu yap
            dgvUrunler.Columns["urId"].HeaderText = "Ürün Kodu";
            dgvUrunler.Columns["urUrunAd"].HeaderText = "Ürün Adı";
            dgvUrunler.Columns["urUrunMiktar"].HeaderText = "Miktar";
            dgvUrunler.Columns["urUrunFiyat"].HeaderText = "Fiyat";
        }

        private void cmbSatisUrun_SelectedIndexChanged(object sender, EventArgs e)
        {

            int urId = ((KeyValuePair<int, string>)cmbSatisUrun.SelectedItem).Key;
            Veritabani.BaglantiAc();
            SqlCommand komut = new SqlCommand("SELECT urUrunFiyat FROM Urunler WHERE urId = @urId", Veritabani.conn);
            komut.Parameters.AddWithValue("@urId", urId);
            object fiyatObj = komut.ExecuteScalar();
            Veritabani.BaglantiKapat();

            if (fiyatObj != DBNull.Value && fiyatObj != null)
            {
                txtFiyat.Text = Convert.ToDecimal(fiyatObj).ToString("0.00");
                HesaplaToplam();
            }
            else
            {
                txtFiyat.Text = "0.00";
                txtToplamFiyat.Text = "0.00";
            }
        }
        

        private void nudSatisAdet_ValueChanged(object sender, EventArgs e)
        {
            HesaplaToplam();
        }
        private void HesaplaToplam()
        {
            if (decimal.TryParse(txtFiyat.Text, out decimal birimFiyat))
            {
                int adet = (int)nudSatisAdet.Value;
                decimal toplam = birimFiyat * adet;
                txtToplamFiyat.Text = toplam.ToString("0.00");
            }
        }

        private void btnSatisYap_Click(object sender, EventArgs e)
        {
            int urId = ((KeyValuePair<int, string>)cmbSatisUrun.SelectedItem).Key;
            int adet = (int)nudSatisAdet.Value;

            Veritabani.BaglantiAc();
            SqlCommand stokSorgu = new SqlCommand("SELECT urUrunMiktar FROM Urunler WHERE urId = @urId", Veritabani.conn);
            stokSorgu.Parameters.AddWithValue("@urId", urId);
            int mevcutStok = Convert.ToInt32(stokSorgu.ExecuteScalar());

            if (mevcutStok < adet)
            {
                Veritabani.BaglantiKapat();
                MessageBox.Show("Yetersiz stok!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SqlCommand satisKomut = new SqlCommand("UPDATE Urunler SET urUrunMiktar = urUrunMiktar - @adet WHERE urId = @urId", Veritabani.conn);
            satisKomut.Parameters.AddWithValue("@adet", adet);
            satisKomut.Parameters.AddWithValue("@urId", urId);
            satisKomut.ExecuteNonQuery();
            SqlCommand log = new SqlCommand(@"
    INSERT INTO IslemKayit (kullaniciAd, islemTipi, tabloAdi, aciklama)
    VALUES (@kulAd, @tip, @tablo, @aciklama)", Veritabani.conn);

            log.Parameters.AddWithValue("@kulAd", Oturum.KullaniciAdi + " (" + Oturum.KullaniciRol + ")"); log.Parameters.AddWithValue("@tip", "Satış");
            log.Parameters.AddWithValue("@tablo", "Satislar");
            log.Parameters.AddWithValue("@aciklama", $"{cmbSatisUrun.Text} ürününden {adet} adet satıldı.");
            log.ExecuteNonQuery();

            Veritabani.BaglantiKapat();
            MessageBox.Show("Satış tamamlandı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

            UrunleriListele();

        }
        private void FormCalisanYonetim_Paint(object sender, PaintEventArgs e)
        {

            int borderWidth = 6;
            Color borderColor = Color.Black;

            ControlPaint.DrawBorder(e.Graphics, this.ClientRectangle,
                borderColor, borderWidth, ButtonBorderStyle.Solid,
                borderColor, borderWidth, ButtonBorderStyle.Solid,
                borderColor, borderWidth, ButtonBorderStyle.Solid,
                borderColor, borderWidth, ButtonBorderStyle.Solid);
        }

        private void FormSatis_MouseDown(object sender, MouseEventArgs e)
        {
            Surukle(e);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtToplamFiyat_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
