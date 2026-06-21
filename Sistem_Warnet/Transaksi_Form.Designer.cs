namespace Sistem_Warnet
{
    partial class Transaksi_Form
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
            this.lblOperator = new System.Windows.Forms.Label();
            this.lblWaktu = new System.Windows.Forms.Label();
            this.cmbTier = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbNoPC = new System.Windows.Forms.ComboBox();
            this.lblTotalBayar = new System.Windows.Forms.Label();
            this.nudDurasiJam = new System.Windows.Forms.NumericUpDown();
            this.lblMenit = new System.Windows.Forms.Label();
            this.txtUangTunai = new System.Windows.Forms.TextBox();
            this.txtKembalian = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.labelkembalian = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudDurasiJam)).BeginInit();
            this.SuspendLayout();
            // 
            // lblOperator
            // 
            this.lblOperator.AutoSize = true;
            this.lblOperator.Location = new System.Drawing.Point(6, 42);
            this.lblOperator.Name = "lblOperator";
            this.lblOperator.Size = new System.Drawing.Size(66, 16);
            this.lblOperator.TabIndex = 0;
            this.lblOperator.Text = "Operator: ";
            // 
            // lblWaktu
            // 
            this.lblWaktu.AutoSize = true;
            this.lblWaktu.BackColor = System.Drawing.SystemColors.Control;
            this.lblWaktu.Location = new System.Drawing.Point(327, 9);
            this.lblWaktu.Name = "lblWaktu";
            this.lblWaktu.Size = new System.Drawing.Size(151, 16);
            this.lblWaktu.TabIndex = 1;
            this.lblWaktu.Text = "PEMBELIAN PAKET PC";
            this.lblWaktu.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblWaktu.Click += new System.EventHandler(this.lblWaktu_Click);
            // 
            // cmbTier
            // 
            this.cmbTier.FormattingEnabled = true;
            this.cmbTier.Location = new System.Drawing.Point(136, 144);
            this.cmbTier.Name = "cmbTier";
            this.cmbTier.Size = new System.Drawing.Size(121, 24);
            this.cmbTier.TabIndex = 10;
            this.cmbTier.SelectedIndexChanged += new System.EventHandler(this.cmbTier_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 194);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(118, 16);
            this.label3.TabIndex = 8;
            this.label3.Text = "Durasi Main (Jam):";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(55, 147);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 16);
            this.label2.TabIndex = 7;
            this.label2.Text = "Tier";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(55, 99);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 16);
            this.label1.TabIndex = 6;
            this.label1.Text = "No PC";
            // 
            // cmbNoPC
            // 
            this.cmbNoPC.FormattingEnabled = true;
            this.cmbNoPC.Location = new System.Drawing.Point(136, 96);
            this.cmbNoPC.Name = "cmbNoPC";
            this.cmbNoPC.Size = new System.Drawing.Size(121, 24);
            this.cmbNoPC.TabIndex = 12;
            this.cmbNoPC.SelectedIndexChanged += new System.EventHandler(this.cmbNoPC_SelectedIndexChanged);
            // 
            // lblTotalBayar
            // 
            this.lblTotalBayar.AutoSize = true;
            this.lblTotalBayar.Location = new System.Drawing.Point(18, 269);
            this.lblTotalBayar.Name = "lblTotalBayar";
            this.lblTotalBayar.Size = new System.Drawing.Size(122, 16);
            this.lblTotalBayar.TabIndex = 14;
            this.lblTotalBayar.Text = "Total Pembayaran:";
            // 
            // nudDurasiJam
            // 
            this.nudDurasiJam.Location = new System.Drawing.Point(137, 192);
            this.nudDurasiJam.Name = "nudDurasiJam";
            this.nudDurasiJam.Size = new System.Drawing.Size(120, 22);
            this.nudDurasiJam.TabIndex = 15;
            this.nudDurasiJam.ValueChanged += new System.EventHandler(this.nudDurasiJam_ValueChanged);
            // 
            // lblMenit
            // 
            this.lblMenit.AutoSize = true;
            this.lblMenit.Location = new System.Drawing.Point(272, 194);
            this.lblMenit.Name = "lblMenit";
            this.lblMenit.Size = new System.Drawing.Size(72, 16);
            this.lblMenit.TabIndex = 16;
            this.lblMenit.Text = "To Menit = ";
            // 
            // txtUangTunai
            // 
            this.txtUangTunai.Location = new System.Drawing.Point(137, 298);
            this.txtUangTunai.Name = "txtUangTunai";
            this.txtUangTunai.Size = new System.Drawing.Size(100, 22);
            this.txtUangTunai.TabIndex = 17;
            this.txtUangTunai.TextChanged += new System.EventHandler(this.txtUangTunai_TextChanged);
            // 
            // txtKembalian
            // 
            this.txtKembalian.Location = new System.Drawing.Point(137, 333);
            this.txtKembalian.Name = "txtKembalian";
            this.txtKembalian.Size = new System.Drawing.Size(100, 22);
            this.txtKembalian.TabIndex = 18;
            this.txtKembalian.TextChanged += new System.EventHandler(this.txtKembalian_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(18, 301);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 16);
            this.label4.TabIndex = 19;
            this.label4.Text = "Uang Tunai:";
            // 
            // labelkembalian
            // 
            this.labelkembalian.AutoSize = true;
            this.labelkembalian.Location = new System.Drawing.Point(18, 333);
            this.labelkembalian.Name = "labelkembalian";
            this.labelkembalian.Size = new System.Drawing.Size(110, 16);
            this.labelkembalian.TabIndex = 20;
            this.labelkembalian.Text = "Uang Kembalian:";
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.YellowGreen;
            this.button2.Location = new System.Drawing.Point(69, 375);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(140, 63);
            this.button2.TabIndex = 22;
            this.button2.Text = "Cetak Pembayaran";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.IndianRed;
            this.button3.Location = new System.Drawing.Point(673, 401);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(115, 37);
            this.button3.TabIndex = 23;
            this.button3.Text = "Batal";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // Transaksi_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.labelkembalian);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtKembalian);
            this.Controls.Add(this.txtUangTunai);
            this.Controls.Add(this.lblMenit);
            this.Controls.Add(this.nudDurasiJam);
            this.Controls.Add(this.lblTotalBayar);
            this.Controls.Add(this.cmbNoPC);
            this.Controls.Add(this.cmbTier);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblWaktu);
            this.Controls.Add(this.lblOperator);
            this.Name = "Transaksi_Form";
            this.Text = "Transaksi_Form";
            this.Load += new System.EventHandler(this.Transaksi_Form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudDurasiJam)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblOperator;
        private System.Windows.Forms.Label lblWaktu;
        private System.Windows.Forms.ComboBox cmbTier;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbNoPC;
        private System.Windows.Forms.Label lblTotalBayar;
        private System.Windows.Forms.NumericUpDown nudDurasiJam;
        private System.Windows.Forms.Label lblMenit;
        private System.Windows.Forms.TextBox txtUangTunai;
        private System.Windows.Forms.TextBox txtKembalian;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labelkembalian;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
    }
}