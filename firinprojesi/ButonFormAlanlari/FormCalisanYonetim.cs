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
using System.Data.SqlClient;
using System.Runtime.InteropServices;

namespace firinprojesi
{
    public partial class FormCalisanYonetim : Form
    {
        public static int  sayi = 1;
        private static string connectionString = @"Data Source=Umut\SQLEXPRESS;Initial Catalog=firinprojesi;Integrated Security=True;TrustServerCertificate=True";
        public static SqlConnection conn = new SqlConnection(connectionString);
        // Formu sürüklemek için gerekli WinAPI fonksiyonları
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        // Sürükleme işlemini gerçekleştiren metod
        private void Surukle(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        public FormCalisanYonetim()
        {
            InitializeComponent();
            this.Load += FormCalisanYonetim_Load;
        }

        private void FormCalisanYonetim_Load(object sender, EventArgs e)
        {
            ListeleDtBasTarihiIle();
            Listele();
            cmbGorev.Items.AddRange(new string[] { "Patron", "Usta", "Çırak", "Kasiyer" });
            this.FormBorderStyle = FormBorderStyle.None;
            this.Paint += new PaintEventHandler(FormCalisanYonetim_Paint);
            dtBasTarihi.Value = DateTime.Now; // Bugünün tam saatiyle birlikte ayarlanır

            // Eğer sadece tarih kısmı (saat 00:00:00) olsun istersen:
            // dtBasTarihi.Value = DateTime.Today;

            // isteğe bağlı, açılır açılmaz listeleme yapsın
            dtBasTarihi.Checked = true;

            dgvCalisanlar.EditMode = DataGridViewEditMode.EditProgrammatically;


            Button[] butonlar = { btnListele, btnKaydet, btnGuncelle, btnSil, btnTemizle };

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
            if (dgvCalisanlar.Rows.Count > 0)
            {
                dgvCalisanlar.ClearSelection();
                dgvCalisanlar.Rows[0].Selected = true;
            }

           

        }



        void Listele()
        {


            SqlDataAdapter da = new SqlDataAdapter(@"
        SELECT 
            cId AS [ID],
            cIsim AS [İsim],
            cSoyisim AS [Soyisim],
            cTlfNo AS [Telefon],
            cTcNo AS [TC Kimlik No],
            cSifre AS [Şifre],
            cGorev AS [Görev],
            cBasTarihi AS [Başlangıç Tarihi],
            fId AS [Fırın ID]
        FROM calisanlar", Veritabani.conn);

            DataTable dt = new DataTable();
            da.Fill(dt);
            dgvCalisanlar.DataSource = dt;




        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            SqlCommand komut = new SqlCommand("INSERT INTO calisanlar (cId,cIsim, cSoyisim, cTlfNo, cTcNo, cSifre, cGorev, cBasTarihi, fId) VALUES (@id,@isim, @soyisim, @tel, @tc, @sifre, @gorev, @tarih, @fid)", Veritabani.conn);
            komut.Parameters.AddWithValue("@id", txtId.Text);
            komut.Parameters.AddWithValue("@isim", txtIsim.Text);
            komut.Parameters.AddWithValue("@soyisim", txtSoyisim.Text);
            komut.Parameters.AddWithValue("@tel", txtTelefon.Text);
            komut.Parameters.AddWithValue("@tc", txtTC.Text);
            komut.Parameters.AddWithValue("@sifre", txtSifre.Text);
            komut.Parameters.AddWithValue("@gorev", cmbGorev.Text);                 
            komut.Parameters.AddWithValue("@tarih", dtBasTarihi.Value.ToString("yyyy-MM-dd"));
            komut.Parameters.AddWithValue("@fid", txtFid.Text);
            MessageBox.Show("Çalışan başarılı bir şekilde kaydedildi.");
            Veritabani.BaglantiAc();
            komut.ExecuteNonQuery();
            SqlCommand logKomut = new SqlCommand(@"
    INSERT INTO IslemKayit (kullaniciAd, islemTipi, tabloAdi, aciklama)
    VALUES (@kulAd, @tip, @tablo, @aciklama)", Veritabani.conn);

            logKomut.Parameters.AddWithValue("@kulAd", Oturum.KullaniciAdi + " (" + Oturum.KullaniciRol + ")"); logKomut.Parameters.AddWithValue("@tip", "Ekleme");
            logKomut.Parameters.AddWithValue("@tablo", "Calisanlar");
            logKomut.Parameters.AddWithValue("@aciklama", $"{txtIsim.Text} {txtSoyisim.Text} adlı çalışan eklendi.");

            logKomut.ExecuteNonQuery();
            Veritabani.BaglantiKapat();
            Listele();
            Temizle();
            
        }


        void Temizle()
        {
            txtId.Clear();
            txtIsim.Clear();
            txtSoyisim.Clear();
            txtTelefon.Clear();
            txtTC.Clear();
            txtSifre.Clear();
            cmbGorev.SelectedIndex = -1;
            dtBasTarihi.Value = DateTime.Now;
            txtFid.Clear();
        }


        private void FormCalisanYonetim_MouseDown(object sender, MouseEventArgs e)
        {
            Surukle(e);
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

       

      

        private void dgvCalisanlar_CellBorderStyleChanged(object sender, EventArgs e)
        {

        }

        private void dgvCalisanlar_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvCalisanlar.Rows[e.RowIndex];

                txtId.Text = row.Cells["ID"].Value.ToString();
                txtIsim.Text = row.Cells["İsim"].Value.ToString();
                txtSoyisim.Text = row.Cells["Soyisim"].Value.ToString();
                txtTelefon.Text = row.Cells["Telefon"].Value.ToString();
                txtTC.Text = row.Cells["TC Kimlik No"].Value.ToString();
                txtSifre.Text = row.Cells["Şifre"].Value.ToString();
                cmbGorev.Text = row.Cells["Görev"].Value.ToString();
                dtBasTarihi.Text = row.Cells["Başlangıç Tarihi"].Value.ToString();
                txtFid.Text = row.Cells["Fırın ID"].Value.ToString();
            }
        




        }

        private void ListeleDtBasTarihiIle()
        {
            Veritabani.BaglantiAc();

            List<string> filtreler = new List<string>();
            SqlCommand komut = new SqlCommand();
            komut.Connection = Veritabani.conn;

            string sorgu = @"
    SELECT 
        cId AS [ID],
        cIsim AS [İsim],
        cSoyisim AS [Soyisim],
        cTlfNo AS [Telefon],
        cTcNo AS [TC Kimlik No],
        cSifre AS [Şifre],
        cGorev AS [Görev],
        cBasTarihi AS [Başlangıç Tarihi],
        fId AS [Fırın ID]
    FROM calisanlar
    WHERE 1=1
    ";

            // Başlangıç Tarihi
            if (dtBasTarihi.Checked)
            {
                filtreler.Add("cBasTarihi = @tarih");
                komut.Parameters.AddWithValue("@tarih", dtBasTarihi.Value.Date);
            }

            // Diğer filtreler
            if (!string.IsNullOrWhiteSpace(txtId.Text))
            {
                filtreler.Add("cId = @id");
                komut.Parameters.AddWithValue("@id", txtId.Text.Trim());
            }

            if (!string.IsNullOrWhiteSpace(txtIsim.Text))
            {
                filtreler.Add("cIsim LIKE @isim");
                komut.Parameters.AddWithValue("@isim", "%" + txtIsim.Text.Trim() + "%");
            }

            if (!string.IsNullOrWhiteSpace(txtSoyisim.Text))
            {
                filtreler.Add("cSoyisim LIKE @soyisim");
                komut.Parameters.AddWithValue("@soyisim", "%" + txtSoyisim.Text.Trim() + "%");
            }

            if (!string.IsNullOrWhiteSpace(txtTelefon.Text))
            {
                filtreler.Add("cTlfNo LIKE @telefon");
                komut.Parameters.AddWithValue("@telefon", "%" + txtTelefon.Text.Trim() + "%");
            }

            if (!string.IsNullOrWhiteSpace(txtTC.Text))
            {
                filtreler.Add("cTcNo LIKE @tc");
                komut.Parameters.AddWithValue("@tc", "%" + txtTC.Text.Trim() + "%");
            }

            if (!string.IsNullOrWhiteSpace(txtSifre.Text))
            {
                filtreler.Add("cSifre LIKE @sifre");
                komut.Parameters.AddWithValue("@sifre", "%" + txtSifre.Text.Trim() + "%");
            }

            if (!string.IsNullOrWhiteSpace(txtFid.Text))
            {
                filtreler.Add("fId = @fid");
                komut.Parameters.AddWithValue("@fid", txtFid.Text.Trim());
            }

            if (!string.IsNullOrWhiteSpace(cmbGorev.Text))
            {
                filtreler.Add("cGorev = @gorev");
                komut.Parameters.AddWithValue("@gorev", cmbGorev.Text);
            }

            // Filtreleri birleştir
            if (filtreler.Count > 0)
            {
                sorgu += " AND " + string.Join(" AND ", filtreler);
            }

            // Sıralama
            sorgu += " ORDER BY cBasTarihi DESC";

            // Sorguyu ata ve çalıştır
            komut.CommandText = sorgu;

            SqlDataAdapter da = new SqlDataAdapter(komut);
            DataTable dt = new DataTable();
            da.Fill(dt);

            // DataGridView'e veriyi bağla
            dgvCalisanlar.DataSource = dt;
            dgvCalisanlar.AutoGenerateColumns = true;
            dgvCalisanlar.Refresh();
            dgvCalisanlar.Invalidate();

            // İlk satırı seç ve görünümü temizle
            dgvCalisanlar.ClearSelection();

            if (dgvCalisanlar.Rows.Count > 0)
            {
                dgvCalisanlar.CurrentCell = dgvCalisanlar.Rows[0].Cells[0];
                dgvCalisanlar.Rows[0].Selected = true;
            }

            Veritabani.BaglantiKapat();
        }



        //    }



        private void txtId_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            
            ListeleDtBasTarihiIle();
            

        }

        private void btnGuncelle_Click_1(object sender, EventArgs e)
        {
            SqlCommand komut = new SqlCommand("UPDATE calisanlar SET cIsim=@isim, cSoyisim=@soyisim, cTlfNo=@tel, cTcNo=@tc, cSifre=@sifre, cGorev=@gorev, cBasTarihi=@tarih, fId=@fid WHERE cId=@id", Veritabani.conn);
            komut.Parameters.AddWithValue("@id", txtId.Text);
            komut.Parameters.AddWithValue("@isim", txtIsim.Text);
            komut.Parameters.AddWithValue("@soyisim", txtSoyisim.Text);
            komut.Parameters.AddWithValue("@tel", txtTelefon.Text);
            komut.Parameters.AddWithValue("@tc", txtTC.Text);
            komut.Parameters.AddWithValue("@sifre", txtSifre.Text);
            komut.Parameters.AddWithValue("@gorev", cmbGorev.Text);
            komut.Parameters.AddWithValue("@tarih", dtBasTarihi.Value.ToString("yyyy-MM-dd"));
            komut.Parameters.AddWithValue("@fid", txtFid.Text);
            MessageBox.Show("Çalışan başarılı bir şekilde güncellendi.");
            Veritabani.BaglantiAc();
            komut.ExecuteNonQuery();
            Veritabani.BaglantiKapat();
            Listele();
            Temizle();
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            SqlCommand komut = new SqlCommand("DELETE FROM calisanlar WHERE cId=@id", Veritabani.conn);
            komut.Parameters.AddWithValue("@id", txtId.Text);
            Veritabani.BaglantiAc();
            komut.ExecuteNonQuery();
            Veritabani.BaglantiKapat();
            MessageBox.Show("Çalışan başarılı bir şekilde silindi.");
            Listele();
            Temizle();
        }

        private void btnTemizle_Click(object sender, EventArgs e)
        {
            Temizle();
        }

        

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtFid_TextChanged(object sender, EventArgs e)
        {

        }
    }

}
