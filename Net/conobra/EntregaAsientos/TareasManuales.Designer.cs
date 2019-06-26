namespace SmartQuickbook
{
    partial class TareasManuales
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
            this.pnlTareas = new System.Windows.Forms.Panel();
            this.pnlLog = new System.Windows.Forms.Panel();
            this.lblResultado = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pnlLog.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTareas
            // 
            this.pnlTareas.BackColor = System.Drawing.SystemColors.ControlLight;
            this.pnlTareas.Location = new System.Drawing.Point(28, 41);
            this.pnlTareas.Name = "pnlTareas";
            this.pnlTareas.Size = new System.Drawing.Size(227, 522);
            this.pnlTareas.TabIndex = 0;
            this.pnlTareas.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlTareas_Paint);
            // 
            // pnlLog
            // 
            this.pnlLog.BackColor = System.Drawing.SystemColors.ControlLight;
            this.pnlLog.Controls.Add(this.lblResultado);
            this.pnlLog.Location = new System.Drawing.Point(357, 41);
            this.pnlLog.Name = "pnlLog";
            this.pnlLog.Size = new System.Drawing.Size(690, 522);
            this.pnlLog.TabIndex = 1;
            this.pnlLog.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
            // 
            // lblResultado
            // 
            this.lblResultado.AutoSize = true;
            this.lblResultado.Location = new System.Drawing.Point(20, 26);
            this.lblResultado.Name = "lblResultado";
            this.lblResultado.Size = new System.Drawing.Size(0, 13);
            this.lblResultado.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(119, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = ".:PROCESO MANUAL:.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(357, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(152, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "RESULTADO DEL PROCESO";
            // 
            // TareasManuales
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1057, 586);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pnlLog);
            this.Controls.Add(this.pnlTareas);
            this.Name = "TareasManuales";
            this.Text = "TareasManuales";
            this.Load += new System.EventHandler(this.TareasManuales_Load);
            this.pnlLog.ResumeLayout(false);
            this.pnlLog.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlTareas;
        private System.Windows.Forms.Panel pnlLog;
        private System.Windows.Forms.Label lblResultado;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}