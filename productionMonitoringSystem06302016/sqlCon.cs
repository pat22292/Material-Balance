using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//be sure to add this shits
using System.Data.SqlClient;
using MetroFramework.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using MetroFramework;

using System.IO;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System.Diagnostics;

namespace productionMonitoringSystem06302016
{
    public partial class sqlCon : MetroForm
    {
        public sqlCon()
        {
            InitializeComponent();
                    }
        //private void runGrid()
        //{ 
  
        //    sqlcon userConnect = new sqlcon();
        //    userConnect.dbIn();
        //    SqlCommand users = new SqlCommand("[DisplayFG]", sqlcon.calc);
        //    SqlDataAdapter calculated = new SqlDataAdapter();
        //    calculated.SelectCommand = users;
        //    DataTable dataSet = new DataTable();
        //    calculated.Fill(dataSet);
        //    BindingSource nSource = new BindingSource();
        //    nSource.DataSource = dataSet;
        //    metroGrid1.DataSource = nSource;
        //    calculated.Update(dataSet);
        //    userConnect.dbOut();

        //}

   

        private void button1_Click(object sender, EventArgs e)
        {
            //Properties.Settings.Default.Database.ToString();
            //Properties.Settings.Default.userName.ToString();
            //Properties.Settings.Default.pw.ToString();
            Properties.Settings.Default.server = metroTextBox4.Text;
            Properties.Settings.Default.Database = metroTextBox4.Text;
            Properties.Settings.Default.userName = metroTextBox2.Text;
            Properties.Settings.Default.pw = metroTextBox3.Text;
            Properties.Settings.Default.Save();
            MessageBox.Show("Connection has been updated!");
            //label1.Text = Properties.Settings.Default.server.ToString() + " " + Properties.Settings.Default.Database.ToString() + " " + Properties.Settings.Default.userName.ToString() + " " + Properties.Settings.Default.pw.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //string sqlConnectionString = ("Server = " + Properties.Settings.Default.server.ToString() + ";Database = master; User Id = " + Properties.Settings.Default.userName.ToString() + "; Password = " + Properties.Settings.Default.pw.ToString() + "");
            //FileInfo file = new FileInfo("C:\\productionScripts.sql");
            //string script = file.OpenText().ReadToEnd();
            //SqlConnection conn = new SqlConnection(sqlConnectionString);
            //Server server = new Server(new ServerConnection(conn));
            //server.ConnectionContext.ExecuteNonQuery(script);
            
                      //Process process = new Process();
                      //process.StartInfo.UseShellExecute = false;
                      //process.StartInfo.WorkingDirectory = scriptDir;
                      //process.StartInfo.RedirectStandardOutput = true;
                      //process.StartInfo.FileName = "sqlplus";
                      //process.StartInfo.Arguments = string.Format("{0} @{1}", credentials, scriptFilename);
                      //process.StartInfo.CreateNoWindow = true;

                      //process.Start();
                      //string output = process.StandardOutput.ReadToEnd();
                      //process.WaitForExit();

                      //return output;

        }

        private void metroLabel2_Click(object sender, EventArgs e)
        {

        }

        private void metroTextButton1_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.server = metroTextBox4.Text;
            Properties.Settings.Default.Database = metroTextBox4.Text;
            Properties.Settings.Default.userName = metroTextBox2.Text;
            Properties.Settings.Default.pw = metroTextBox3.Text;
            Properties.Settings.Default.Save();
            DesktopAlert.Show("Database has been Updated");
        }


        private void metroTextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            metroTextBox1.Text = metroTextBox1.Text.Replace("\r\n", " ");
        }

        private void metroTextBox2_KeyUp(object sender, KeyEventArgs e)
        {
            metroTextBox2.Text = metroTextBox2.Text.Replace("\r\n", " ");
        }

        private void metroTextBox3_KeyUp(object sender, KeyEventArgs e)
        {
            metroTextBox3.Text = metroTextBox3.Text.Replace("\r\n", " ");
        }

        private void dissablebutton()
        {
            if (metroTextBox1.Text == "" || metroTextBox2.Text == "" || metroTextBox3.Text == "" || metroTextBox4.Text == "")
            {
                metroButton1.Enabled = false;
            }

            else
            {
                metroButton1.Enabled = true;
            }

        }

        private void metroTextBox4_KeyUp(object sender, KeyEventArgs e)
        {
            metroTextBox4.Text = metroTextBox4.Text.Replace("\r\n", " ");
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.server = metroTextBox1.Text;
            Properties.Settings.Default.Database = metroTextBox4.Text;
            Properties.Settings.Default.userName = metroTextBox2.Text;
            Properties.Settings.Default.pw = metroTextBox3.Text;
            Properties.Settings.Default.Save();
            MetroMessageBox.Show(this, "", "Datebase has been updated", MessageBoxButtons.OK, MessageBoxIcon.Question);
        }

        private void metroTextBox1_TextChanged(object sender, EventArgs e)
        {
            dissablebutton();
        }

        private void metroTextBox2_TextChanged(object sender, EventArgs e)
        {
            dissablebutton();
        }

        private void metroTextBox3_TextChanged(object sender, EventArgs e)
        {
            dissablebutton();
        }

        private void metroTextBox4_TextChanged(object sender, EventArgs e)
        {
            dissablebutton();
        }

        private void sqlCon_Load(object sender, EventArgs e)
        {
            dissablebutton();
        }
    }
}
