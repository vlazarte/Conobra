using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SmartQuickbook.Configuration;

namespace SmartQuickbook
{
    public partial class ProcessControl : UserControl
    {
        public Proceso proceso ;

        public ProcessControl( Proceso proceso )
        {
            InitializeComponent();
            this.proceso = proceso;
        }


        private void button1_Click(object sender, EventArgs e)
        {
         //   MessageBox.Show("Test");
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        public void InicioEjecucion()
        {
            imgLoading.Visible = true;
        }

        public void FinEjecucion()
        {
            imgLoading.Visible = false;
        }

        public void UltimaEjecucion()
        {
            if (proceso.ultimaEjecucion != null)
                lblUltimaEjecucion.Text = ((DateTime)proceso.ultimaEjecucion).ToString("yyyy-MM-dd hh:mm:ss");
        }

        public void ProximaEjecucion()
        {
            if ( proceso.siguienteEjecucion != null )
                lblProximaEjecucion.Text = ((DateTime)proceso.siguienteEjecucion).ToString("yyyy-MM-dd hh:mm:ss");
        }

        public void MostrarMensaje(string msg)
        {
            txtLog.Text = "[" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "] " + msg + Environment.NewLine + txtLog.Text;
        }

        public void ImagenLoading(bool visible)
        {
            imgLoading.Visible = visible;
        }

        private void ProcessControl_Load(object sender, EventArgs e)
        {

        }

        private void btnStart_Click(object sender, EventArgs e)
        {

        }
    }
}
