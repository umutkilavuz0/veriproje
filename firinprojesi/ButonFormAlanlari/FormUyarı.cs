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
    public partial class FormUyarı : Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        public FormUyarı()
        {
            InitializeComponent();
        }

        private void FormUyarı_Load(object sender, EventArgs e)
        {
            UyariListele();
            this.FormBorderStyle = FormBorderStyle.None;
            this.Paint += new PaintEventHandler(FormUyarı_Paint);
            Button[] butonlar = { btnYenile,};

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
        private void UyariListele()
        {
            try
            {
                Veritabani.BaglantiAc();

                string sorgu = @"
            SELECT 
                'Malzeme' AS tur,
                uUrunAd AS ad,
                uUrunMiktar AS miktar,
                uKritikSeviye AS kritikSeviye
            FROM Urun
            WHERE uUrunMiktar < uKritikSeviye

            UNION

            SELECT 
                'Ürün' AS tur,
                urUrunAd AS ad,
                urUrunMiktar AS miktar,
                urKritikSeviye AS kritikSeviye
            FROM Urunler
            WHERE urUrunMiktar < urKritikSeviye";

                SqlDataAdapter da = new SqlDataAdapter(sorgu, Veritabani.conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvUyarilar.DataSource = dt;

                Veritabani.BaglantiKapat();
                dgvUyarilar.Columns["tur"].HeaderText = "Tür";
                dgvUyarilar.Columns["ad"].HeaderText = "Adı";
                dgvUyarilar.Columns["miktar"].HeaderText = "Mevcut";
                dgvUyarilar.Columns["kritikSeviye"].HeaderText = "Kritik Seviye";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Uyarılar alınırken hata: " + ex.Message);
            }
        }

        private void btnYenile_Click(object sender, EventArgs e)
        {
            UyariListele();
        }

        private void FormUyarı_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }

        }
        private void FormUyarı_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, this.ClientRectangle,
                Color.Black, 6, ButtonBorderStyle.Solid,
                Color.Black, 6, ButtonBorderStyle.Solid,
                Color.Black, 6, ButtonBorderStyle.Solid,
                Color.Black, 6, ButtonBorderStyle.Solid);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
