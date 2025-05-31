using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace firinprojesi
{
    public partial class FormUretim : Form
    {
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HTCAPTION = 0x2;
        public FormUretim()
        {
            InitializeComponent();

        }

        private void FormUretim_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.Paint += new PaintEventHandler(FormUretim_Paint);

            Veritabani.BaglantiAc();


            SqlCommand komut = new SqlCommand("SELECT urId, urUrunAd FROM Urunler", Veritabani.conn);
            SqlDataReader dr = komut.ExecuteReader();

            Dictionary<int, string> urunler = new Dictionary<int, string>();

            while (dr.Read())
            {
                urunler.Add(Convert.ToInt32(dr["urId"]), dr["urUrunAd"].ToString());
            }

            dr.Close(); // 🔴 reader'ı kapatmayı unutma

            cmbUrun.DataSource = new BindingSource(urunler, null);
            cmbUrun.DisplayMember = "Value"; // görünen: ürün adı
            cmbUrun.ValueMember = "Key";     // arka planda: urId

            Veritabani.BaglantiKapat();
            Button[] butonlar = { btnUret, btnUrunEkle };

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

        private void cmbUrun_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgvRecete.Columns.Clear();
            dgvRecete.Rows.Clear();

            dgvRecete.Columns.Add("urUrunAd", "Ürün Adı");
            dgvRecete.Columns.Add("uId", "Malzeme ID");
            dgvRecete.Columns.Add("uUrunAd", "Malzeme Adı");
            dgvRecete.Columns.Add("miktar", "Gerekli Miktar (1 Adet)");
            dgvRecete.Columns.Add("stok", "Mevcut Stok");

            int urId = ((KeyValuePair<int, string>)cmbUrun.SelectedItem).Key;
            string urunAdi = ((KeyValuePair<int, string>)cmbUrun.SelectedItem).Value;

            Veritabani.BaglantiAc();

            SqlCommand komut = new SqlCommand(@"
        SELECT r.uId, u.uUrunAd, r.miktar, u.uUrunMiktar
        FROM UrunRecetesi r
        JOIN Urun u ON u.uId = r.uId
        WHERE r.urId = @urId", Veritabani.conn);
            
            komut.Parameters.AddWithValue("@urId", urId);

            SqlDataReader dr = komut.ExecuteReader();
            while (dr.Read())
            {
                dgvRecete.Rows.Add(
                    urunAdi,
                    dr["uId"].ToString(),
                    dr["uUrunAd"].ToString(),
                    dr["miktar"].ToString(),
                    dr["uUrunMiktar"].ToString());
            }

            dr.Close();
            Veritabani.BaglantiKapat();
        }

        private void btnUret_Click(object sender, EventArgs e)
        {

        }

        private void btnUret_Click_1(object sender, EventArgs e)
        {
            int adet = Convert.ToInt32(nudAdet.Value);
            List<string> yetersizMalzemeler = new List<string>();

            Veritabani.BaglantiAc();

            foreach (DataGridViewRow row in dgvRecete.Rows)
            {
                if (row.Cells["uId"].Value == null) continue;

                int uId = Convert.ToInt32(row.Cells["uId"].Value);
                int receteMiktar = Convert.ToInt32(row.Cells["miktar"].Value);
                int toplamGerekli = receteMiktar * adet;

                SqlCommand stokSorgu = new SqlCommand("SELECT uUrunMiktar FROM Urun WHERE uId = @uId", Veritabani.conn);
                stokSorgu.Parameters.AddWithValue("@uId", uId);
                int mevcutStok = Convert.ToInt32(stokSorgu.ExecuteScalar());

                if (mevcutStok < toplamGerekli)
                {
                    string ad = row.Cells["uUrunAd"].Value.ToString();
                    yetersizMalzemeler.Add($"{ad} (stok: {mevcutStok}, gerek: {toplamGerekli})");
                }
            }

            if (yetersizMalzemeler.Count > 0)
            {
                Veritabani.BaglantiKapat();
                MessageBox.Show("Yetersiz stok bulunan malzemeler:\n\n" + string.Join("\n", yetersizMalzemeler), "Üretim Hatası", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Stokları düş
            foreach (DataGridViewRow row in dgvRecete.Rows)
            {
                if (row.Cells["uId"].Value == null) continue;

                int uId = Convert.ToInt32(row.Cells["uId"].Value);
                int receteMiktar = Convert.ToInt32(row.Cells["miktar"].Value);
                int toplamKullanilacak = receteMiktar * adet;

                SqlCommand komut = new SqlCommand(
                    "UPDATE Urun SET uUrunMiktar = uUrunMiktar - @kullanilan WHERE uId = @uId", Veritabani.conn);
                komut.Parameters.AddWithValue("@kullanilan", toplamKullanilacak);
                komut.Parameters.AddWithValue("@uId", uId);
                komut.ExecuteNonQuery();
            }

            Veritabani.BaglantiKapat();
            Veritabani.BaglantiAc();

            MessageBox.Show("Üretim başarıyla tamamlandı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            int urId = ((KeyValuePair<int, string>)cmbUrun.SelectedItem).Key;
            int adet1 = Convert.ToInt32(nudAdet.Value);

            SqlCommand komut2 = new SqlCommand("UPDATE Urunler SET urUrunMiktar = urUrunMiktar + @adet WHERE urId = @urId", Veritabani.conn);
            komut2.Parameters.AddWithValue("@adet", adet1);
            komut2.Parameters.AddWithValue("@urId", urId);
            komut2.ExecuteNonQuery();
            SqlCommand log = new SqlCommand(@"
    INSERT INTO IslemKayit (kullaniciAd, islemTipi, tabloAdi, aciklama)
    VALUES (@kulAd, @tip, @tablo, @aciklama)", Veritabani.conn);

            log.Parameters.AddWithValue("@kulAd", Oturum.KullaniciAdi + " (" + Oturum.KullaniciRol + ")");
            log.Parameters.AddWithValue("@tip", "Üretim");
            log.Parameters.AddWithValue("@tablo", "Uretim");
            log.Parameters.AddWithValue("@aciklama", $"{cmbUrun.Text} ürününden {nudAdet.Value} adet üretildi.");
            log.ExecuteNonQuery();
        }

        private void FormUretim_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
        }
        private void FormUretim_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, this.ClientRectangle,
                Color.Black, 6, ButtonBorderStyle.Solid,  // Sol
                Color.Black, 6, ButtonBorderStyle.Solid,  // Üst
                Color.Black, 6, ButtonBorderStyle.Solid,  // Sağ
                Color.Black, 6, ButtonBorderStyle.Solid); // Alt
            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnUrunEkle_Click(object sender, EventArgs e)
        {
            urunekle form = new urunekle();
            form.ShowDialog();
           
        }


    }

}
