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
            Quickbook.Config.IsProduction = true;//(Properties.Settings.Default.qbook_production == "b4f16ca3cd7d");
            var qbook = new Connector(Properties.Settings.Default.qbook_app_name, Properties.Settings.Default.qbook_file);
            if (qbook.Connect())
            {
                

                string xmlResponse = qbook.sendRequest(textBox1.Text);
                textBox2.Text = xmlResponse;
                qbook.Disconnect();

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var ws = new wClient.WService();
            string err = "";
            string resp = ws.doQuery(textBox3.Text, null, out err);
            textBox4.Text = resp;
        }
    }
}
