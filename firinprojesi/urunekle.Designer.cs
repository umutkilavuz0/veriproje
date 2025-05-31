namespace firinprojesi
{
    partial class urunekle
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(urunekle));
            this.btnUrunEkle = new System.Windows.Forms.Button();
            this.dgvRecete = new System.Windows.Forms.DataGridView();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.txtUrugAd = new System.Windows.Forms.Label();
            this.txtUrunAd = new System.Windows.Forms.TextBox();
            this.txtUrunKod = new System.Windows.Forms.TextBox();
            this.nudMiktar = new System.Windows.Forms.TextBox();
            this.nudKritikSeviye = new System.Windows.Forms.TextBox();
            this.txtDId = new System.Windows.Forms.TextBox();
            this.txtFiyat = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtUrunId = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.cmbSilinecekUrun = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecete)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // btnUrunEkle
            // 
            this.btnUrunEkle.BackColor = System.Drawing.Color.Silver;
            this.btnUrunEkle.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnUrunEkle.FlatAppearance.BorderSize = 2;
            this.btnUrunEkle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUrunEkle.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnUrunEkle.ForeColor = System.Drawing.Color.Black;
            this.btnUrunEkle.Location = new System.Drawing.Point(155, 479);
            this.btnUrunEkle.Name = "btnUrunEkle";
            this.btnUrunEkle.Size = new System.Drawing.Size(94, 35);
            this.btnUrunEkle.TabIndex = 33;
            this.btnUrunEkle.Text = "Ürün Ekle";
            this.btnUrunEkle.UseVisualStyleBackColor = false;
            this.btnUrunEkle.Click += new System.EventHandler(this.btnUrunEkle_Click);
            // 
            // dgvRecete
            // 
            this.dgvRecete.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvRecete.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRecete.Location = new System.Drawing.Point(5, 5);
            this.dgvRecete.Name = "dgvRecete";
            this.dgvRecete.RowHeadersWidth = 51;
            this.dgvRecete.Size = new System.Drawing.Size(821, 260);
            this.dgvRecete.TabIndex = 34;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(775, 527);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(31, 27);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 39;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Click += new System.EventHandler(this.pictureBox2_Click);
            // 
            // txtUrugAd
            // 
            this.txtUrugAd.AutoSize = true;
            this.txtUrugAd.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold);
            this.txtUrugAd.Location = new System.Drawing.Point(45, 442);
            this.txtUrugAd.Name = "txtUrugAd";
            this.txtUrugAd.Size = new System.Drawing.Size(66, 16);
            this.txtUrugAd.TabIndex = 40;
            this.txtUrugAd.Text = "Ürün Adı:";
            // 
            // txtUrunAd
            // 
            this.txtUrunAd.Location = new System.Drawing.Point(135, 442);
            this.txtUrunAd.Name = "txtUrunAd";
            this.txtUrunAd.Size = new System.Drawing.Size(114, 20);
            this.txtUrunAd.TabIndex = 41;
            // 
            // txtUrunKod
            // 
            this.txtUrunKod.Location = new System.Drawing.Point(135, 416);
            this.txtUrunKod.Name = "txtUrunKod";
            this.txtUrunKod.Size = new System.Drawing.Size(114, 20);
            this.txtUrunKod.TabIndex = 42;
            // 
            // nudMiktar
            // 
            this.nudMiktar.Location = new System.Drawing.Point(135, 309);
            this.nudMiktar.Name = "nudMiktar";
            this.nudMiktar.Size = new System.Drawing.Size(114, 20);
            this.nudMiktar.TabIndex = 43;
            // 
            // nudKritikSeviye
            // 
            this.nudKritikSeviye.Location = new System.Drawing.Point(135, 338);
            this.nudKritikSeviye.Name = "nudKritikSeviye";
            this.nudKritikSeviye.Size = new System.Drawing.Size(114, 20);
            this.nudKritikSeviye.TabIndex = 44;
            // 
            // txtDId
            // 
            this.txtDId.Location = new System.Drawing.Point(135, 364);
            this.txtDId.Name = "txtDId";
            this.txtDId.Size = new System.Drawing.Size(114, 20);
            this.txtDId.TabIndex = 45;
            // 
            // txtFiyat
            // 
            this.txtFiyat.Location = new System.Drawing.Point(135, 390);
            this.txtFiyat.Name = "txtFiyat";
            this.txtFiyat.Size = new System.Drawing.Size(114, 20);
            this.txtFiyat.TabIndex = 46;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(34, 416);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 16);
            this.label2.TabIndex = 48;
            this.label2.Text = "Ürün Kodu:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(29, 310);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 16);
            this.label3.TabIndex = 49;
            this.label3.Text = "Ürün Miktarı:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold);
            this.label4.Location = new System.Drawing.Point(29, 338);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 16);
            this.label4.TabIndex = 50;
            this.label4.Text = "Kritik Seviyesi:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold);
            this.label5.Location = new System.Drawing.Point(50, 364);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(62, 16);
            this.label5.TabIndex = 51;
            this.label5.Text = "Depo ID:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold);
            this.label6.Location = new System.Drawing.Point(69, 390);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(42, 16);
            this.label6.TabIndex = 52;
            this.label6.Text = "Fiyat:";
            // 
            // txtUrunId
            // 
            this.txtUrunId.Location = new System.Drawing.Point(135, 283);
            this.txtUrunId.Name = "txtUrunId";
            this.txtUrunId.Size = new System.Drawing.Size(114, 20);
            this.txtUrunId.TabIndex = 53;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(55, 283);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 16);
            this.label1.TabIndex = 54;
            this.label1.Text = "Ürün ID:";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Silver;
            this.button1.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.button1.FlatAppearance.BorderSize = 2;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold);
            this.button1.ForeColor = System.Drawing.Color.Black;
            this.button1.Location = new System.Drawing.Point(507, 479);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(94, 35);
            this.button1.TabIndex = 55;
            this.button1.Text = "Ürünü Sil:";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // cmbSilinecekUrun
            // 
            this.cmbSilinecekUrun.FormattingEnabled = true;
            this.cmbSilinecekUrun.Location = new System.Drawing.Point(480, 437);
            this.cmbSilinecekUrun.Name = "cmbSilinecekUrun";
            this.cmbSilinecekUrun.Size = new System.Drawing.Size(121, 21);
            this.cmbSilinecekUrun.TabIndex = 57;
            this.cmbSilinecekUrun.SelectedIndexChanged += new System.EventHandler(this.cmbUrunler_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold);
            this.label7.Location = new System.Drawing.Point(408, 438);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(67, 16);
            this.label7.TabIndex = 58;
            this.label7.Text = "Ürün Seç:";
            // 
            // urunekle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(830, 561);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.cmbSilinecekUrun);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtUrunId);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtFiyat);
            this.Controls.Add(this.txtDId);
            this.Controls.Add(this.nudKritikSeviye);
            this.Controls.Add(this.nudMiktar);
            this.Controls.Add(this.txtUrunKod);
            this.Controls.Add(this.txtUrunAd);
            this.Controls.Add(this.txtUrugAd);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.dgvRecete);
            this.Controls.Add(this.btnUrunEkle);
            this.Name = "urunekle";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "urunekle";
            this.Load += new System.EventHandler(this.urunekle_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.urunekle_MouseDown);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecete)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnUrunEkle;
        private System.Windows.Forms.DataGridView dgvRecete;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label txtUrugAd;
        private System.Windows.Forms.TextBox txtUrunAd;
        private System.Windows.Forms.TextBox txtUrunKod;
        private System.Windows.Forms.TextBox nudMiktar;
        private System.Windows.Forms.TextBox nudKritikSeviye;
        private System.Windows.Forms.TextBox txtDId;
        private System.Windows.Forms.TextBox txtFiyat;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtUrunId;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox cmbSilinecekUrun;
        private System.Windows.Forms.Label label7;
    }
}