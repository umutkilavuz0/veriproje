using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace firinprojesi
{
    public partial class FormUsta : Form
    {

        // Windows API fonksiyonları
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HTCAPTION = 0x2;

        public FormUsta()
        {
            InitializeComponent();
            this.btnCalisanYonetim.MouseEnter += new System.EventHandler(this.btnCalisanYonetim_MouseEnter);
            this.btnCalisanYonetim.MouseLeave += new System.EventHandler(this.btnCalisanYonetim_MouseLeave);

            this.btnUretim.MouseEnter += new System.EventHandler(this.btnUretim_MouseEnter);
            this.btnUretim.MouseLeave += new System.EventHandler(this.btnUretim_MouseLeave);

            this.btnStok.MouseEnter += new System.EventHandler(this.btnStok_MouseEnter);
            this.btnStok.MouseLeave += new System.EventHandler(this.btnStok_MouseLeave);

            this.btnSatis.MouseEnter += new System.EventHandler(this.btnSatis_MouseEnter);
            this.btnSatis.MouseLeave += new System.EventHandler(this.btnSatis_MouseLeave);

            this.btnRapor.MouseEnter += new System.EventHandler(this.btnRapor_MouseEnter);
            this.btnRapor.MouseLeave += new System.EventHandler(this.btnRapor_MouseLeave);

            this.btnMesajlar.MouseEnter += new System.EventHandler(this.btnMesajlar_MouseEnter);
            this.btnMesajlar.MouseLeave += new System.EventHandler(this.btnMesajlar_MouseLeave);
            this.Paint += new PaintEventHandler(FormUsta_Paint);

        }

        

        private void FormUsta_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None;


        }

        private void btnCalisanYonetim_MouseEnter(object sender, EventArgs e)
        {
            btnCalisanYonetim.Size = new Size(400, 55); // büyüt
        }

        private void btnCalisanYonetim_MouseLeave(object sender, EventArgs e)
        {
            btnCalisanYonetim.Size = new Size(390, 47); // eski haline dön
        }



        private void btnUretim_MouseEnter(object sender, EventArgs e)
        {
            btnUretim.Size = new Size(400, 55); // büyüt
        }

        private void btnUretim_MouseLeave(object sender, EventArgs e)
        {
            btnUretim.Size = new Size(390, 47); // eski haline dön
        }


        private void btnStok_MouseEnter(object sender, EventArgs e)
        {
            btnStok.Size = new Size(400, 55);// büyüt
        }

        private void btnStok_MouseLeave(object sender, EventArgs e)
        {
            btnStok.Size = new Size(390, 47); // eski haline dön
        }

        private void btnSatis_MouseEnter(object sender, EventArgs e)
        {
            btnSatis.Size = new Size(400, 55); ; // büyüt
        }

        private void btnSatis_MouseLeave(object sender, EventArgs e)
        {
            btnSatis.Size = new Size(390, 47); // eski haline dön
        }


        private void btnRapor_MouseEnter(object sender, EventArgs e)
        {
            btnRapor.Size = new Size(400, 55); // büyüt
        }

        private void btnRapor_MouseLeave(object sender, EventArgs e)
        {
            btnRapor.Size = new Size(390, 47); // eski haline dön
        }

        private void btnMesajlar_MouseEnter(object sender, EventArgs e)
        {
            btnMesajlar.Size = new Size(400, 55); // büyüt
        }

        private void btnMesajlar_MouseLeave(object sender, EventArgs e)
        {
            btnMesajlar.Size = new Size(390, 47); // eski haline dön
        }

        private void FormUsta_Paint(object sender, PaintEventArgs e)
        {

            int borderWidth = 6;
            Color borderColor = Color.Black;

            ControlPaint.DrawBorder(e.Graphics, this.ClientRectangle,
                borderColor, borderWidth, ButtonBorderStyle.Solid,
                borderColor, borderWidth, ButtonBorderStyle.Solid,
                borderColor, borderWidth, ButtonBorderStyle.Solid,
                borderColor, borderWidth, ButtonBorderStyle.Solid);
        }

       







       

       

        

        

        

        private void btnCalisanYonetim_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show("Giriş Yetkiniz Bulunmamaktadır.");
        }

        private void btnUretim_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            FormUretim frm = new FormUretim();
            frm.FormClosed += (s, args) => this.Show();
            frm.Show();

        }

        private void btnStok_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            FormStok frm = new FormStok();
            frm.FormClosed += (s, args) => this.Show();
            frm.Show();
        }

        private void btnSatis_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show("Giriş Yetkiniz Bulunmamaktadır.");

        }

        private void btnRapor_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show("Giriş Yetkiniz Bulunmamaktadır.");

        }

        private void btnMesajlar_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            FormUyarı frm = new FormUyarı();
            frm.FormClosed += (s, args) => this.Show();
            frm.Show();
        }

        private void FormUsta_MouseDown_1(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 frm = new Form1();
            frm.FormClosed += (s, args) => this.Show();
            frm.Show();

        }
    }

}
