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
    public partial class Form1 : Form

    {
       

        

        int orijinalGenislik;
        int orijinalYukseklik;
        Timer butonAnimasyonTimer = new Timer();
        bool buyuyor = true;

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();

        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(IntPtr hWnd, int wMsg, int wParam, int lPara);
        public Form1()
        {
            InitializeComponent();
            this.AcceptButton = btnGiris;
            this.FormBorderStyle = FormBorderStyle.None;
            this.MouseDown += Form1_MouseDown;
            this.Paint += new PaintEventHandler(Form1_Paint);

        }

        private void Form1_Load(object sender, EventArgs e)
        {


            this.AcceptButton = btnGiris;
            Label lblCapsLockUyari = new Label();
            lblCapsLockUyari.Text = "⚠ Caps Lock açık!";
            lblCapsLockUyari.ForeColor = Color.Red;
            lblCapsLockUyari.AutoSize = true;
            lblCapsLockUyari.Location = new Point(txtSifre.Left, txtSifre.Bottom + 25);
            lblCapsLockUyari.Visible = false;
            this.Controls.Add(lblCapsLockUyari);

            panel1.BackColor = Color.Black; // veya istediğin çizgi rengi
            panel1.Width = 3;              // çizgi kalınlığı (ince bir çizgi için 1)

            
        

        }




        
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
            btnGiris.MouseEnter += btnGiris_MouseEnter;
            btnGiris.MouseLeave += btnGiris_MouseLeave;

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void btnm1_Load(object sender, EventArgs e)
        {


        }
        private void btnGiris_MouseEnter(object sender, EventArgs e)
        {
            orijinalGenislik = btnGiris.Width;
            orijinalYukseklik = btnGiris.Height;
            buyuyor = true;
            butonAnimasyonTimer.Interval = 10;
            butonAnimasyonTimer.Tick += ButonAnimasyonTimer_Tick;
            butonAnimasyonTimer.Start();
        }

        private void btnGiris_MouseLeave(object sender, EventArgs e)
        {
            buyuyor = false;
            butonAnimasyonTimer.Start();
        }
        private void ButonAnimasyonTimer_Tick(object sender, EventArgs e)
        {
            if (buyuyor)
            {
                if (btnGiris.Width < orijinalGenislik + 10)
                {
                    btnGiris.Width += 2;
                    btnGiris.Height += 1;
                    btnGiris.Left -= 1;
                    btnGiris.Top -= 1;
                }
                else
                {
                    butonAnimasyonTimer.Stop();
                }
            }
            else
            {
                if (btnGiris.Width > orijinalGenislik)
                {
                    btnGiris.Width -= 2;
                    btnGiris.Height -= 1;
                    btnGiris.Left += 1;
                    btnGiris.Top += 1;
                }
                else
                {
                    butonAnimasyonTimer.Stop();
                    butonAnimasyonTimer.Tick -= ButonAnimasyonTimer_Tick;
                }
            }
        }

        private void btnGiris_Click(object sender, EventArgs e)
        {
            {

                string tc = txtTC.Text;
                string sifre = txtSifre.Text.Trim();

                SqlCommand cmd = new SqlCommand("SELECT * FROM calisanlar WHERE cTcNo = @tc AND cSifre = @sifre", Veritabani.conn);
                cmd.Parameters.AddWithValue("@tc", tc);
                cmd.Parameters.AddWithValue("@sifre", sifre);

                try
                {
                    Veritabani.BaglantiAc();
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        string gorev = dr["cGorev"].ToString().ToLower();
                        MessageBox.Show("Giriş Başarılı. Görev : " + gorev);
                        Oturum.KullaniciAdi = dr["cIsim"].ToString();         // Kullanıcı adı
                        Oturum.KullaniciRol = dr["cGorev"].ToString();        // Görev (Patron, Usta...)

                        // Göreve göre form yönlendirme
                        switch (gorev)
                        {
                            case "patron":
                                new FormPatron().Show(); break;
                            case "usta":
                                new FormUsta().Show(); break;
                            case "çırak":
                                new FormCırak().Show(); break;
                            case "kasiyer":
                                new FormKasiyer().Show(); break;
                            default:
                                MessageBox.Show("Tanımsız görev!"); break;
                        }

                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Hatalı TC veya Şifre Girdiniz.");
                    }

                    Veritabani.BaglantiKapat();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message);
                    Veritabani.BaglantiKapat();
                }
               

               
                

            }



        }
       
        


        private void txtSifre_TextChanged(object sender, EventArgs e)
        {

        }
        private void chkSifreGoster_CheckedChanged(object sender, EventArgs e)
        {
            txtSifre.UseSystemPasswordChar = !chkSifreGoster.Checked;
        }

        private void chkSifreGoster_CheckedChanged_1(object sender, EventArgs e)
        {
            txtSifre.UseSystemPasswordChar = !chkSifreGoster.Checked;
        }



        private void btnLogin_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void txtTC_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
           
        
            int borderWidth = 6;
            Color borderColor = Color.Black;

            ControlPaint.DrawBorder(e.Graphics, this.ClientRectangle,
                borderColor, borderWidth, ButtonBorderStyle.Solid,
                borderColor, borderWidth, ButtonBorderStyle.Solid,
                borderColor, borderWidth, ButtonBorderStyle.Solid,
                borderColor, borderWidth, ButtonBorderStyle.Solid);
        

    }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();

        }

       
    }
}




