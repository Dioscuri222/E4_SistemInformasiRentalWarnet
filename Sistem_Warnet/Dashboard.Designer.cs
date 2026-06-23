namespace Sistem_Warnet
{
    partial class Dashboard_Form
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.chartPendapatan = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.lblTotalPendapatan = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.chartPendapatan)).BeginInit();
            this.SuspendLayout();
            // 
            // chartPendapatan
            // 
            chartArea2.Name = "ChartArea1";
            this.chartPendapatan.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.chartPendapatan.Legends.Add(legend2);
            this.chartPendapatan.Location = new System.Drawing.Point(52, 48);
            this.chartPendapatan.Name = "chartPendapatan";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.chartPendapatan.Series.Add(series2);
            this.chartPendapatan.Size = new System.Drawing.Size(675, 365);
            this.chartPendapatan.TabIndex = 0;
            this.chartPendapatan.Text = "chart1";
            // 
            // lblTotalPendapatan
            // 
            this.lblTotalPendapatan.AutoSize = true;
            this.lblTotalPendapatan.Location = new System.Drawing.Point(49, 428);
            this.lblTotalPendapatan.Name = "lblTotalPendapatan";
            this.lblTotalPendapatan.Size = new System.Drawing.Size(59, 13);
            this.lblTotalPendapatan.TabIndex = 1;
            this.lblTotalPendapatan.Text = "NOMINAL:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(333, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "PENDAPATAN CHART";
            // 
            // Dashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblTotalPendapatan);
            this.Controls.Add(this.chartPendapatan);
            this.Name = "Dashboard";
            this.Text = "Dashboard";
            this.Load += new System.EventHandler(this.Dashboard_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chartPendapatan)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chartPendapatan;
        private System.Windows.Forms.Label lblTotalPendapatan;
        private System.Windows.Forms.Label label1;
    }
}