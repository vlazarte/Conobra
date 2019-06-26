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
    public partial class ExecuteXML : Form
    {
        public ExecuteXML()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                Quickbook.Config.IsProduction = true;//(Properties.Settings.Default.qbook_production == "b4f16ca3cd7d");
                //  Properties.Settings.Default.qbook_file = txtConection.Text;
                var qbook = new Connector(Properties.Settings.Default.qbook_app_name, Properties.Settings.Default.qbook_file);
                label1.Text = "Conectando";
                if (qbook.Connect())
                {
                    label1.Text = "Conecto con exito";

                    if (textBox1.Text != string.Empty)
                    {
                        string xmlResponse = qbook.sendRequest(textBox1.Text);
                        textBox2.Text = xmlResponse;
                    }
                    qbook.Disconnect();
                    label1.Text += "Desconecto!";
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error no se pudo conectar a: " + Properties.Settings.Default.qbook_file + ex.Message);
                
            }
           
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void ExecuteXML_Load(object sender, EventArgs e)
        {

        }

     

       
    }
}
