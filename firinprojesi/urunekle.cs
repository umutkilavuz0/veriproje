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
    public partial class urunekle : Form
    {
        public urunekle()
        {
            InitializeComponent();
        }
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
        private void urunekle_Load(object sender, EventArgs e)
        {
            this.Paint += new PaintEventHandler(urunekle_Paint);
            this.FormBorderStyle = FormBorderStyle.None;

            Dictionary<int, string> malzemeler = new Dictionary<int, string>();

            Veritabani.BaglantiAc();

            SqlCommand komut = new SqlCommand("SELECT uId, uUrunAd FROM Urun", Veritabani.conn);
            SqlDataReader dr = komut.ExecuteReader();
            while (dr.Read())
            {
                malzemeler.Add((int)dr["uId"], dr["uUrunAd"].ToString());
            }
            Veritabani.BaglantiKapat();

            // Malzeme (ComboBox) sütunu
            DataGridViewComboBoxColumn combo = new DataGridViewComboBoxColumn();
            combo.HeaderText = "Malzeme";
            combo.Name = "uId";
            combo.DataSource = new BindingSource(malzemeler, null);
            combo.DisplayMember = "Value";
            combo.ValueMember = "Key";
            dgvRecete.Columns.Add(combo);

            // Miktar (Textbox) sütunu
            DataGridViewTextBoxColumn miktar = new DataGridViewTextBoxColumn();
            miktar.HeaderText = "Miktar";
            miktar.Name = "miktar";
            dgvRecete.Columns.Add(miktar);
            UrunleriYukle();
            Button[] butonlar = { btnUrunEkle, button1 };

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

        private void btnUrunEkle_Click(object sender, EventArgs e)
        {
            {
                string urunAd = txtUrunAd.Text.Trim();
                string urunKod = txtUrunKod.Text.Trim();
                int urunId, miktar, kritik, dId, fiyat;

                if (!int.TryParse(txtUrunId.Text, out urunId)
                    || string.IsNullOrEmpty(urunAd)
                    || string.IsNullOrEmpty(urunKod)
                    || !int.TryParse(nudMiktar.Text, out miktar)
                    || !int.TryParse(nudKritikSeviye.Text, out kritik)
                    || !int.TryParse(txtDId.Text, out dId)
                    || !int.TryParse(txtFiyat.Text, out fiyat))
                {
                    MessageBox.Show("Tüm alanları geçerli şekilde doldurun.");
                    return;
                }

                Veritabani.BaglantiAc();

                SqlCommand komut = new SqlCommand(@"
    INSERT INTO Urunler 
    (urId, urUrunAd, urUrunKod, urUrunMiktar, urKritikSeviye, dId, urUrunFiyat) 
    VALUES 
    (@id, @ad, @kod, @miktar, @kritik, @depo, @fiyat)", Veritabani.conn);

                komut.Parameters.AddWithValue("@id", urunId);
                komut.Parameters.AddWithValue("@ad", urunAd);
                komut.Parameters.AddWithValue("@kod", urunKod);
                komut.Parameters.AddWithValue("@miktar", miktar);
                komut.Parameters.AddWithValue("@kritik", kritik);
                komut.Parameters.AddWithValue("@depo", dId);
                komut.Parameters.AddWithValue("@fiyat", fiyat);

                try
                {
                    komut.ExecuteNonQuery();

                    // 🔽🔽🔽 Reçete Kayıtları Burada Başlıyor 🔽🔽🔽
                    foreach (DataGridViewRow row in dgvRecete.Rows)
                    {
                        if (row.Cells["uId"].Value == null || row.Cells["miktar"].Value == null)
                            continue;

                        int malzemeId = Convert.ToInt32(row.Cells["uId"].Value);
                        int malzemeMiktar = Convert.ToInt32(row.Cells["miktar"].Value);

                        SqlCommand receteEkle = new SqlCommand(@"
                INSERT INTO UrunRecetesi (urId, uId, miktar) 
                VALUES (@urId, @uId, @miktar)", Veritabani.conn);

                        receteEkle.Parameters.AddWithValue("@urId", urunId);
                        receteEkle.Parameters.AddWithValue("@uId", malzemeId);
                        receteEkle.Parameters.AddWithValue("@miktar", malzemeMiktar);
                        receteEkle.ExecuteNonQuery();
                    }
                    // 🔼🔼🔼 Reçete Kayıtları Bitti 🔼🔼🔼

                    MessageBox.Show("Ürün ve reçetesi başarıyla eklendi.");
                    UrunleriYukle();
                   

                }
                catch (SqlException ex)
                {
                    MessageBox.Show("HATA: " + ex.Message);
                }

                Veritabani.BaglantiKapat();
            }
            Veritabani.BaglantiAc();
            Veritabani.BaglantiKapat();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (cmbSilinecekUrun.SelectedItem == null)
            {
                MessageBox.Show("Silinecek ürünü seçin.");
                return;
            }

            var secilen = (KeyValuePair<int, string>)cmbSilinecekUrun.SelectedItem;
            int urunId = secilen.Key;
            string urunAd = secilen.Value;

            DialogResult cevap = MessageBox.Show($"{urunAd} adlı ürünü silmek istiyor musun?", "Silme Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (cevap != DialogResult.Yes)
                return;

            Veritabani.BaglantiAc();

            try
            {
                SqlCommand receteSil = new SqlCommand("DELETE FROM UrunRecetesi WHERE urId = @urId", Veritabani.conn);
                receteSil.Parameters.AddWithValue("@urId", urunId);
                receteSil.ExecuteNonQuery();

                SqlCommand urunSil = new SqlCommand("DELETE FROM Urunler WHERE urId = @urId", Veritabani.conn);
                urunSil.Parameters.AddWithValue("@urId", urunId);
                urunSil.ExecuteNonQuery();

                MessageBox.Show("Ürün başarıyla silindi.");
                UrunleriYukle(); // silme sonrası listeyi yenile
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
            finally
            {
                Veritabani.BaglantiKapat();
            }
        }
        

        private void pictureBox2_Click(object sender, EventArgs e)
        {

            this.Close();



        }

        private void cmbUrunler_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void UrunleriYukle()
        {
            cmbSilinecekUrun.DropDownStyle = ComboBoxStyle.DropDownList;

            Veritabani.BaglantiAc();
            SqlCommand komut = new SqlCommand("SELECT urId, urUrunAd FROM Urunler", Veritabani.conn);
            SqlDataReader dr = komut.ExecuteReader();

            Dictionary<int, string> urunler = new Dictionary<int, string>();
            while (dr.Read())
            {
                urunler.Add(Convert.ToInt32(dr["urId"]), dr["urUrunAd"].ToString());
            }

            dr.Close();
            Veritabani.BaglantiKapat();

            cmbSilinecekUrun.DataSource = new BindingSource(urunler, null);
            cmbSilinecekUrun.DisplayMember = "Value";
            cmbSilinecekUrun.ValueMember = "Key";
        }
        private void urunekle_Paint(object sender, PaintEventArgs e)
        {

            int borderWidth = 6;
            Color borderColor = Color.Black;

            ControlPaint.DrawBorder(e.Graphics, this.ClientRectangle,
                borderColor, borderWidth, ButtonBorderStyle.Solid,
                borderColor, borderWidth, ButtonBorderStyle.Solid,
                borderColor, borderWidth, ButtonBorderStyle.Solid,
                borderColor, borderWidth, ButtonBorderStyle.Solid);
        }

        private void urunekle_MouseDown(object sender, MouseEventArgs e)
        {
            Surukle(e);
        }
    }
}

