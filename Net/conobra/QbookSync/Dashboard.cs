using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using Quickbook;

namespace QbookSync
{
    public partial class Dashboard : Form
    {
        private bool RunCotizaciones = true;
        private bool RunCotizacionesStop = false;

        private Task taskCotizaciones;

        public Dashboard()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void __StartCustomers()
        {
            RunCotizaciones = true;
            RunCotizacionesStop = false;
            btnStart.Enabled = false;
            btnStop.Enabled = true;
            btnCotizacionManual.Enabled = false;

            if (!workerCotizaciones.IsBusy)
                workerCotizaciones.RunWorkerAsync();
        }

        private void __StopCustomers()
        {
            RunCotizaciones = false;
            RunCotizacionesStop = true;
            if (workerCotizaciones.IsBusy)
            {
                workerCotizaciones.CancelAsync();
                workerCotizaciones.Dispose();
            }

            btnStart.Enabled = true;
            btnStop.Enabled = false;
            btnCotizacionManual.Enabled = true;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            __StartCustomers();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            __StopCustomers();
        }

        private void SyncCustomers()
        {

            //Connector qbook = new Connector(Properties.Settings.Default.qbook_app_name, Properties.Settings.Default.qbook_file);
            //if (qbook.Connect())
            //{
            //    Quickbook.Config.qbook = qbook;

            //    qbook.Disconnect();
            //}

        }

    

        private void workerCotizaciones_DoWork(object sender, DoWorkEventArgs e)
        {
            if (RunCotizaciones == false)
                return;

            taskCotizaciones = Task.Factory.StartNew(() =>
            {
                BeginInvoke((Action)(() =>
                {
                    imgLoadCotizacion.Visible = true;
                }));

                SyncCustomers();

                DateTime current = DateTime.Now;
                BeginInvoke((Action)(() =>
                {
                    lblCotizacionPrev.Text = current.ToString("dd/MM/yyyy H:mm:ss");
                }));
            });

            taskCotizaciones.ContinueWith((Success) =>
            {
                DateTime current = DateTime.Now;
                current = current.AddSeconds(3 * 60);
                BeginInvoke((Action)(() =>
                {
                    if (RunCotizacionesStop == true)
                        lblCotizacionNext.Text = "";
                    else
                        lblCotizacionNext.Text = current.ToString("dd/MM/yyyy H:mm:ss");
                    imgLoadCotizacion.Visible = false;
                }));
                System.Threading.Thread.Sleep(3 * 60 * 1000);
                if (RunCotizaciones == true)
                {
                    workerCotizaciones.RunWorkerAsync();
                }
                // callback when task is complete.
            }, TaskContinuationOptions.NotOnFaulted);
            taskCotizaciones.ContinueWith((Fail) =>
            {
                //log the exception i.e.: Fail.Exception.InnerException);
            }, TaskContinuationOptions.OnlyOnFaulted);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            BeginInvoke((Action)(() =>
            {
                logCotizaciones.Text = "";
            }));  
        }

        private void btnCotizacionManual_Click(object sender, EventArgs e)
        {

        }
    }
}
