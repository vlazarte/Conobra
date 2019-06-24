using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

using Quickbook;
using System.Collections;
using System.IO;
using System.Threading.Tasks;
using System.Globalization;
using System.Threading;
using System.Net;
using System.Collections.Specialized;
using System.Web.Script.Serialization;
using SmartQuickbook.Configuration;
using System.Reflection;
using SmartQuickbook.Helper;
using SmartQuickbook.Data;


namespace SmartQuickbook
{
    public partial class Dashboard : Form
    {

        public Processor processor = null;
        public List<TabPage> pages = null;

        public Dashboard()
        {
            InitializeComponent();

            processor = new Processor();
            pages = new List<TabPage>();

            processor.load();
            CrearTabProcesos();
            this.Text = Properties.Settings.Default.qbook_Compania;
        }

        #region "Configuracion"

        public void CrearTabProceso(Proceso proceso, ref TabControl tab)
        {
            System.Windows.Forms.TabPage tabPage = new TabPage();
            pages.Add(tabPage);
            tab.Controls.Add(tabPage);
            tabPage.Location = new System.Drawing.Point(4, 22);
            tabPage.Name = "p" + proceso.id;
            tabPage.Padding = new System.Windows.Forms.Padding(3);
            tabPage.Size = new System.Drawing.Size(671, 490);
            tabPage.TabIndex = 1;
            tabPage.Text = proceso.nombre;
            tabPage.UseVisualStyleBackColor = true;

            ProcessControl PC = new ProcessControl(proceso);
            proceso.controlUI = PC;
            tabPage.Controls.Add(PC);
        }

        public void CrearTabProcesos()
        {
            for (int i = 0; i < processor.procesos.Count; i++)
            {
                Proceso P = processor.procesos[i];
                if (P.tipoEjecucion == "intervalo") {
                    CrearTabProceso(P, ref tabControl1);
                }
            }
        }

        public void CargarListaProcesos()
        {
            dgListProcess.Rows.Clear();

            for (int i = 0; i < processor.procesos.Count; i++)
            {
                Proceso P = processor.procesos[i];
                string accion = "";
                foreach (ProcesoAccion A in P.acciones)
                {
                    accion += "De " + P.entrada.tipo + " a " + A.nombre + Environment.NewLine;
                }

                accion = accion.Trim(Environment.NewLine.ToCharArray());

                string ultimaEjecucion = "";
                if (P.ultimaEjecucion != null)
                    ultimaEjecucion = ((DateTime)P.ultimaEjecucion).ToString("yyyy-MM-dd hh:mm:ss");
                string siguienteEjecucion = "";
                if (P.siguienteEjecucion != null)
                    siguienteEjecucion = ((DateTime)P.siguienteEjecucion).ToString("yyyy-MM-dd hh:mm:ss");

                string datosEjecucion = " Cada " + P.tipoIntervaloValor + " " + P.tipoIntervalo;
                dgListProcess.Rows.Add(P.id,
                    P.nombre,
                    accion,
                    datosEjecucion,
                    P.estado,
                    ultimaEjecucion,
                    siguienteEjecucion
                );
            }
        }

        #endregion


        private void EjecutarProceso(Proceso proceso)
        {
            if (proceso.entrada.tipo == "quickbase")
            {
                BeginInvoke((Action)(() =>
                {
                    proceso.controlUI.MostrarMensaje("Buscando datos de Quickbase");
                }));
                string mensajes = HelperProcesor.ProcesoEjecutarToQuickBase(proceso);

                BeginInvoke((Action)(() =>
                {
                    proceso.controlUI.MostrarMensaje(mensajes);
                }));

                BeginInvoke((Action)(() =>
                {
                    proceso.controlUI.MostrarMensaje("Finalizo Proceso");
                }));

            }
            else
            {

                if (proceso.entrada.tipo == "quickbook")
                {
                    BeginInvoke((Action)(() =>
                    {
                        proceso.controlUI.MostrarMensaje("Buscando datos de Quickbook");
                    }));
                    string mensajes = HelperProcesor.ProcesoEjecutarToQuickBook(proceso);

                    BeginInvoke((Action)(() =>
                    {
                        proceso.controlUI.MostrarMensaje(mensajes);
                    }));

                    BeginInvoke((Action)(() =>
                    {
                        proceso.controlUI.MostrarMensaje("Finalizo Proceso");
                    }));
                }
            }
        }


        public void AsyncOperation_DoWork(object sender, DoWorkEventArgs e)
        {
            Proceso proceso = (Proceso)e.Argument;

            proceso.estado = "En ejecucion...";
            proceso.enEjecucion = true;
            proceso.ultimaEjecucion = DateTime.Now;

            BeginInvoke((Action)(() =>
            {
                proceso.controlUI.UltimaEjecucion();
                proceso.controlUI.ImagenLoading(true);
            }));

            BeginInvoke((Action)(() =>
            {
                CargarListaProcesos();
            }));

            Task taskAsync = null;
            BackgroundWorker worker = (BackgroundWorker)sender;

            taskAsync = Task.Factory.StartNew(() =>
            {
                EjecutarProceso(proceso);
            });

            taskAsync.ContinueWith((Success) =>
            {
                proceso.estado = "Terminado";
                BeginInvoke((Action)(() =>
                {
                    CargarListaProcesos();
                }));

                if (proceso.tipoEjecucion == Proceso.TIPO_EJECUCION_INTERVALO)
                {
                    int segundos = 0;
                    if (proceso.tipoIntervalo == Proceso.TIPO_INTERVALO_SEGUNDOS)
                    {
                        segundos = Int32.Parse(proceso.tipoIntervaloValor);
                        proceso.siguienteEjecucion = DateTime.Now.AddSeconds(double.Parse(proceso.tipoIntervaloValor));
                    }
                    else if (proceso.tipoIntervalo == Proceso.TIPO_INTERVALO_MINUTOS)
                    {
                        segundos = Int32.Parse(proceso.tipoIntervaloValor) * 60;
                        proceso.siguienteEjecucion = DateTime.Now.AddMinutes(double.Parse(proceso.tipoIntervaloValor));
                    }
                    else if (proceso.tipoIntervalo == Proceso.TIPO_INTERVALO_HORAS)
                    {
                        segundos = Int32.Parse(proceso.tipoIntervaloValor) * 3600;
                        proceso.siguienteEjecucion = DateTime.Now.AddHours(double.Parse(proceso.tipoIntervaloValor));
                    }
                    else if (proceso.tipoIntervalo == Proceso.TIPO_INTERVALO_DIAS)
                    {
                        segundos = Int32.Parse(proceso.tipoIntervaloValor) * 3600 * 24;
                        proceso.siguienteEjecucion = DateTime.Now.AddDays(double.Parse(proceso.tipoIntervaloValor));
                    }
                    else
                    {
                        segundos = -1;
                    }

                    if (segundos > 0)
                    {
                        BeginInvoke((Action)(() =>
                        {
                            proceso.controlUI.ProximaEjecucion();
                            proceso.controlUI.ImagenLoading(false);
                        }));

                        proceso.enEjecucion = false;
                        System.Threading.Thread.Sleep(segundos * 1000);
                        worker.RunWorkerAsync(argument: proceso);
                    }
                }




            }, TaskContinuationOptions.NotOnFaulted);

            taskAsync.ContinueWith((Fail) =>
            {
                //log the exception i.e.: Fail.Exception.InnerException);
            }, TaskContinuationOptions.OnlyOnFaulted);
        }
        public void AsyncOperation_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.label1.Text = e.UserState + " " + e.ProgressPercentage.ToString() + "% complete";
        }
        public void AsyncOperation_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //this.label1.Text = "The answer is: " + e.Result.ToString();
        }
        public BackgroundWorker AsyncOperation(Proceso proceso)
        {
            System.ComponentModel.BackgroundWorker bgWorker = new BackgroundWorker();

            bgWorker.WorkerReportsProgress = true;
            bgWorker.WorkerSupportsCancellation = true;
            bgWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.AsyncOperation_DoWork);
            bgWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.AsyncOperation_ProgressChanged);
            bgWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.AsyncOperation_RunWorkerCompleted);

            if (!bgWorker.IsBusy)
                bgWorker.RunWorkerAsync(argument: proceso);

            return bgWorker;
        }

        private void IniciarEjecucion()
        {
            CargarListaProcesos();

            foreach (Proceso proceso in processor.procesos)
            {
                if (proceso.tipoEjecucion == "intervalo")
                {
                    var result = AsyncOperation(proceso);
                }
            }
        }
        private void Dashboard_Load(object sender, EventArgs e)
        {

            // IniciarEjecucion();

        }




        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExecuteXML x = new ExecuteXML();
            x.ShowDialog();
        }

        private void iniciarProcesoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IniciarEjecucion();
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void dgListProcess_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void ClasesProyectosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TareasManuales tarea = new TareasManuales();
            tarea.ShowDialog();
        }

      
    }

    public class Par
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }


}
