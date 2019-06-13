namespace QbookSync
{
    partial class Dashboard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Dashboard));
            this.tab111 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.imgLoadCotizacion = new System.Windows.Forms.PictureBox();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnCotizacionManual = new System.Windows.Forms.Button();
            this.txtRefNumber = new System.Windows.Forms.TextBox();
            this.lblCotizacionPrev = new System.Windows.Forms.Label();
            this.lblCotizacionNext = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.button8 = new System.Windows.Forms.Button();
            this.logCotizaciones = new System.Windows.Forms.TextBox();
            this.workerCotizaciones = new System.ComponentModel.BackgroundWorker();
            this.tab111.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgLoadCotizacion)).BeginInit();
            this.SuspendLayout();
            // 
            // tab111
            // 
            this.tab111.Controls.Add(this.tabPage1);
            this.tab111.Location = new System.Drawing.Point(12, 12);
            this.tab111.Multiline = true;
            this.tab111.Name = "tab111";
            this.tab111.SelectedIndex = 0;
            this.tab111.Size = new System.Drawing.Size(675, 537);
            this.tab111.TabIndex = 23;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.Transparent;
            this.tabPage1.Controls.Add(this.imgLoadCotizacion);
            this.tabPage1.Controls.Add(this.btnStop);
            this.tabPage1.Controls.Add(this.btnStart);
            this.tabPage1.Controls.Add(this.btnCotizacionManual);
            this.tabPage1.Controls.Add(this.txtRefNumber);
            this.tabPage1.Controls.Add(this.lblCotizacionPrev);
            this.tabPage1.Controls.Add(this.lblCotizacionNext);
            this.tabPage1.Controls.Add(this.label9);
            this.tabPage1.Controls.Add(this.label10);
            this.tabPage1.Controls.Add(this.button8);
            this.tabPage1.Controls.Add(this.logCotizaciones);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(667, 511);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Customers";
            // 
            // imgLoadCotizacion
            // 
            this.imgLoadCotizacion.Image = ((System.Drawing.Image)(resources.GetObject("imgLoadCotizacion.Image")));
            this.imgLoadCotizacion.Location = new System.Drawing.Point(591, 72);
            this.imgLoadCotizacion.Name = "imgLoadCotizacion";
            this.imgLoadCotizacion.Size = new System.Drawing.Size(20, 20);
            this.imgLoadCotizacion.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.imgLoadCotizacion.TabIndex = 41;
            this.imgLoadCotizacion.TabStop = false;
            this.imgLoadCotizacion.Visible = false;
            // 
            // btnStop
            // 
            this.btnStop.BackColor = System.Drawing.Color.Transparent;
            this.btnStop.Enabled = false;
            this.btnStop.FlatAppearance.BorderSize = 0;
            this.btnStop.Location = new System.Drawing.Point(559, 43);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(87, 23);
            this.btnStop.TabIndex = 20;
            this.btnStop.Text = "Detener";
            this.btnStop.UseVisualStyleBackColor = false;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.Color.Transparent;
            this.btnStart.FlatAppearance.BorderSize = 0;
            this.btnStart.Location = new System.Drawing.Point(559, 14);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(87, 23);
            this.btnStart.TabIndex = 13;
            this.btnStart.Text = "Iniciar";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnCotizacionManual
            // 
            this.btnCotizacionManual.BackColor = System.Drawing.Color.Transparent;
            this.btnCotizacionManual.FlatAppearance.BorderSize = 0;
            this.btnCotizacionManual.Location = new System.Drawing.Point(559, 182);
            this.btnCotizacionManual.Name = "btnCotizacionManual";
            this.btnCotizacionManual.Size = new System.Drawing.Size(87, 39);
            this.btnCotizacionManual.TabIndex = 36;
            this.btnCotizacionManual.Text = "Ejecución Manual";
            this.btnCotizacionManual.UseVisualStyleBackColor = false;
            this.btnCotizacionManual.Click += new System.EventHandler(this.btnCotizacionManual_Click);
            // 
            // txtRefNumber
            // 
            this.txtRefNumber.Location = new System.Drawing.Point(559, 156);
            this.txtRefNumber.Name = "txtRefNumber";
            this.txtRefNumber.Size = new System.Drawing.Size(87, 20);
            this.txtRefNumber.TabIndex = 42;
            // 
            // lblCotizacionPrev
            // 
            this.lblCotizacionPrev.ForeColor = System.Drawing.Color.Blue;
            this.lblCotizacionPrev.Location = new System.Drawing.Point(324, 435);
            this.lblCotizacionPrev.Name = "lblCotizacionPrev";
            this.lblCotizacionPrev.Size = new System.Drawing.Size(108, 27);
            this.lblCotizacionPrev.TabIndex = 40;
            this.lblCotizacionPrev.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCotizacionNext
            // 
            this.lblCotizacionNext.ForeColor = System.Drawing.Color.Green;
            this.lblCotizacionNext.Location = new System.Drawing.Point(109, 436);
            this.lblCotizacionNext.Name = "lblCotizacionNext";
            this.lblCotizacionNext.Size = new System.Drawing.Size(108, 27);
            this.lblCotizacionNext.TabIndex = 39;
            this.lblCotizacionNext.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label9
            // 
            this.label9.Font = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.Blue;
            this.label9.Location = new System.Drawing.Point(223, 442);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(109, 17);
            this.label9.TabIndex = 38;
            this.label9.Text = "Última Ejecución";
            this.label9.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label10
            // 
            this.label10.Font = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.Green;
            this.label10.Location = new System.Drawing.Point(6, 441);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(109, 17);
            this.label10.TabIndex = 37;
            this.label10.Text = "Próxima Ejecución";
            this.label10.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(559, 409);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(87, 23);
            this.button8.TabIndex = 21;
            this.button8.Text = "Vaciar Log";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // logCotizaciones
            // 
            this.logCotizaciones.BackColor = System.Drawing.SystemColors.ControlLight;
            this.logCotizaciones.Location = new System.Drawing.Point(9, 18);
            this.logCotizaciones.Multiline = true;
            this.logCotizaciones.Name = "logCotizaciones";
            this.logCotizaciones.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.logCotizaciones.Size = new System.Drawing.Size(535, 414);
            this.logCotizaciones.TabIndex = 0;
            // 
            // workerCotizaciones
            // 
            this.workerCotizaciones.WorkerReportsProgress = true;
            this.workerCotizaciones.WorkerSupportsCancellation = true;
            this.workerCotizaciones.DoWork += new System.ComponentModel.DoWorkEventHandler(this.workerCotizaciones_DoWork);
            // 
            // Dashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(699, 561);
            this.Controls.Add(this.tab111);
            this.Name = "Dashboard";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tab111.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgLoadCotizacion)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tab111;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.PictureBox imgLoadCotizacion;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnCotizacionManual;
        private System.Windows.Forms.TextBox txtRefNumber;
        private System.Windows.Forms.Label lblCotizacionPrev;
        private System.Windows.Forms.Label lblCotizacionNext;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.TextBox logCotizaciones;
        private System.ComponentModel.BackgroundWorker workerCotizaciones;
    }
}

