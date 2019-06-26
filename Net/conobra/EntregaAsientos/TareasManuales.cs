using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SmartQuickbook.Configuration;
using SmartQuickbook.Helper;

namespace SmartQuickbook
{
    public partial class TareasManuales : Form
    {
        public Processor processor = null;
        public List<Button> Buttons = null;
        public TareasManuales()
        {
            InitializeComponent();
        }

        private void TareasManuales_Load(object sender, EventArgs e)
        {
            processor = new Processor();
            Buttons = new List<Button>();

            processor.load();
            this.Text = Properties.Settings.Default.qbook_Compania;
            CrearButtonsProcesos();
            
             
        }
        public void CrearButtonsProcesos()
        {
            Button btnReference=null;
            for (int i = 0; i < processor.procesos.Count; i++)
            {
                Proceso P = processor.procesos[i];
                if (P.tipoEjecucion == "manual")
                {

                     CrearButtonProceso(P,ref btnReference);
                   
                }
            }
        }
        public void CrearButtonProceso(Proceso proceso,ref Button btnReference )
        {
            System.Windows.Forms.Button taskbutton = new Button();

            Buttons.Add(taskbutton);
            pnlTareas.Controls.Add(taskbutton);
            int positionY = 26;
            if (btnReference != null)
            {

                int position = btnReference.Location.Y;
                int espacio = 45;
                positionY = positionY + position + espacio;
            }
          
            taskbutton.Location = new System.Drawing.Point(18, positionY);
            taskbutton.Name = proceso.id;
            taskbutton.Text = proceso.nombre;
            taskbutton.Padding = new System.Windows.Forms.Padding(3);
            taskbutton.Size = new System.Drawing.Size(173, 46);
            taskbutton.UseVisualStyleBackColor = true;
            btnReference = taskbutton;
            taskbutton.Click += new System.EventHandler(this.btnClick_Click);
            
            ProcessControl PC = new ProcessControl(proceso);
            proceso.controlUI = PC;
            pnlLog.Controls.Add(PC);
        }

        private void pnlTareas_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnClick_Click(object sender, EventArgs e)
        {
            Button btn= (Button)sender;

            Proceso proceso = processor.procesos.Find(p => p.id == btn.Name && p.tipoEjecucion == "manual");

            EjecutarProceso(proceso);
           

        }
        private void ImportandoProveedores(string idProceso) {
            string err = string.Empty;
            string cvs = string.Empty;

            Proceso proceso = processor.procesos.Find(p => p.id == idProceso && p.tipoEjecucion == "manual");

            EjecutarProceso(proceso);


            //if (HelperTask.ImportToQuickBaseVendedoresFromQuickBookCVS(ref err, ref cvs))
            //{
            //    //exito
            //    MessageBox.Show("importo Proveedores de "+Properties.Settings.Default.qbook_CompaniaBD + "a Quickbase con exito!");

            //}
            //else
            //{
            //    //error
            //    MessageBox.Show(err);
            //}
        }
        private void ImportandoClases(string idProceso)
        {
            Proceso proceso = processor.procesos.Find(p => p.id == idProceso && p.tipoEjecucion == "manual");

           // EjecutarProceso(idProceso);
            string err = string.Empty;
            string cvs = string.Empty;
            if (HelperTask.ImportToQuickBaseClassFromQuickBookCVS(ref err, ref cvs))
            {
                //exito
                MessageBox.Show("Importo Proyecto clases a Quickbase con exito");

            }
            else
            {
                //error
                MessageBox.Show(err);
            }
        }


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
                            proceso.controlUI.MostrarMensaje("Buscando datos de Quickbooks");
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

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }


    }
}
