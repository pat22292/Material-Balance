using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Forms;
using System.Data.SqlClient;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using DevComponents.WinForms;
using DevComponents.DotNetBar.Controls;
using MetroFramework;


namespace productionMonitoringSystem06302016
{
    public partial class BackUpandRestore : MetroForm
    {
        private SqlConnection conn;
        private SqlCommand command;
        private SqlDataReader reader;
        string sql = "";
        string connectionString = "";

        public BackUpandRestore()
        {
            InitializeComponent();
        }


        private void BackUpandRestore_Load(object sender, EventArgs e)
        {
            metroTabControl1.SelectTab(metroTabPage1);
            BAndR();
            metroLabel1.Text = "Backup Destination : " + Properties.Settings.Default.server.ToString();
            metroButton1.Enabled = false;
            metroComboBox2.Enabled = false;
            
            //metroComboBox1.Enabled = false;
            //metroButton3.Enabled = false;
        }

        private void BAndR()
        {
            try
            {
                sqlcon userConnect = new sqlcon();
                userConnect.dbIn();
                SqlDataAdapter da = new SqlDataAdapter("exec [DBBASE]", sqlcon.con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                metroComboBox1.DataSource = dt;
                metroComboBox1.DisplayMember = "name";
                DataTable mod = new DataTable();
     
                userConnect.dbOut();


                metroButton3.Enabled = true;

                metroComboBox1.Enabled = true;

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                DesktopAlert.Show(ex.Message);
            }
        }
       

        private void metroButton1_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    connectionString = "Data Source =" + Properties.Settings.Default.server.ToString() + "; User ID =" + Properties.Settings.Default.userName.ToString() + "; Password =" + Properties.Settings.Default.pw.ToString() + "";
            //    //Data Source = USER - PC; User ID = sa; Password = ***********
            //    conn = new SqlConnection(connectionString);
            //    conn.Open();
            //    sql = "EXEC sp_databases";
            //    command = new SqlCommand(sql, conn);
            //    reader = command.ExecuteReader();
            //    metroComboBox1.Items.Clear();
            //    while (reader.Read())
            //    {
            //        metroComboBox1.Items.Add(reader[0].ToString());
            //    }

            //    yow.Enabled = false;

            //    metroButton3.Enabled = true;

            //    metroComboBox1.Enabled = true;

            //}
            //catch (Exception ex)
            //{
            //    //MessageBox.Show(ex.Message);
            //    DesktopAlert.Show(ex.Message);
            //}


        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
           
            metroComboBox1.Enabled = false;
            metroButton3.Enabled = false;

            
        }
        private void sqlAuthen()
        {
            try
            {
                if (metroComboBox1.Text.CompareTo("") == 0)
                {
                    DesktopAlert.Show("Please Select Database.");
                    return;
                }
                conn = new SqlConnection(connectionString);
                conn.Open();
                sql = "BACKUP DATABASE " + metroComboBox1.Text + " TO DISK = '" + "D:\'" + "\\" + metroComboBox1.Text + "-" + DateTime.Now.Ticks.ToString() + ".bak'";
                command = new SqlCommand(sql, conn);
                command.ExecuteNonQuery();
                //DesktopAlert.Show("Database Backup Completed");
                MessageBox.Show("YOLO men");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            try
            {
                if (metroComboBox1.Text.CompareTo("") == 0)
                {
                    MetroMessageBox.Show(this, "", "Please Select Database", MessageBoxButtons.OK, MessageBoxIcon.Question);
                    return;
                }
                sqlcon userConnect = new sqlcon();
                userConnect.dbIn();
                sql = "BACKUP DATABASE " + metroComboBox1.Text + " TO DISK = '" + "C:" + "\\" + metroComboBox1.Text + "-" + DateTime.Now.Ticks.ToString() + ".bak'";
                command = new SqlCommand(sql, sqlcon.con);
                command.ExecuteNonQuery();
                DesktopAlert.Show("Database Backup Successfully");
                userConnect.dbOut();
               
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        private void metroButton4_Click(object sender, EventArgs e)
        {
           
        }

        private void metroButton6_Click(object sender, EventArgs e)
        {

        }

        private void metroTextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            
          
        }

        private void metroTextBox2_KeyUp(object sender, KeyEventArgs e)
        {
            
           
        }

        private void metroTextBox3_KeyUp(object sender, KeyEventArgs e)
        {
           
        }

        private void metroButton5_Click(object sender, EventArgs e)
        {

        }

        private void metroTextBox3_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter)
            //{
            //    if (metroTextBox3.Text == "" && metroTextBox2.Text == "" && metroTextBox1.Text == "" ) 
            //    { 
            //        e.SuppressKeyPress = true; DesktopAlert.Show("Please enter valid database authentication!");
            //        metroTextBox1.Text = ""; metroTextBox2.Text = ""; metroTextBox3.Text = "";
            //    }
            //    else
            //    {
            //        sqlAuthen();
            //    }
            //}
        }

        private void metroButton1_Click_1(object sender, EventArgs e)
        {

        }

        private void metroButton2_Click_1(object sender, EventArgs e)
        {
            try
            {
                connectionString = "Data Source =" + metroTextBox3.Text + "; User ID =" + metroTextBox1.Text + "; Password =" + metroTextBox2.Text + "";
                //Data Source = USER - PC; User ID = sa; Password = ***********
                conn = new SqlConnection(connectionString);
                conn.Open();
                sql = "EXEC sp_databases";
                command = new SqlCommand(sql, conn);
                reader = command.ExecuteReader();
                metroComboBox1.Items.Clear();
                DesktopAlert.Show("Database Authentication Success!");
                metroButton1.Enabled = true;
                conn.Close();
                conn.Dispose();
               
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                DesktopAlert.Show(ex.Message);
                metroTextBox1.Text = "";
                metroTextBox2.Text = "";
                metroTextBox3.Text = "";
            }
        }

        private void metroButton1_Click_2(object sender, EventArgs e)
        {
            metroTextBox1.Text = "";
            metroTextBox2.Text = "";
            metroTextBox3.Text = "";

            try
            {
                if (metroComboBox2.Text.CompareTo("")==0)
                {
                    DesktopAlert.Show("Please select a database !");
                    return;
                }
                conn = new SqlConnection(connectionString);
                conn.Open();
                //sql = "Alter Database " + metroComboBox2.Text + " Set SINGLE_USER WITH ROLLBACK IMMEDIATE ";
                sql += "Restore Database " + metroComboBox2.Text + "FROM Disk = '" + metroTextBox4.Text + "' WITH REPLACE;";
                command = new SqlCommand("Alter Database " + metroComboBox2.Text + " Set SINGLE_USER WITH ROLLBACK IMMEDIATE Restore Database " + metroComboBox2.Text + " FROM Disk = '" + metroTextBox4.Text + "' WITH REPLACE; ", conn);
                command.ExecuteNonQuery();
                
               
                DesktopAlert.Show("Successfully restore database!");
                conn.Close();
                conn.Dispose();
            }
            catch
            {
                DesktopAlert.Show("Please check your credentials! \nNote: This machine may not be capable of using this feature.\n Please contact your administrator");
            }
        }

        private void metroTabPage2_Click(object sender, EventArgs e)
        {

        }

        private void metroComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void metroButton2_Click_2(object sender, EventArgs e)
        {
            try
            {
                connectionString = "Data Source =" + metroTextBox3.Text + "; User ID =" + metroTextBox1.Text + "; Password =" + metroTextBox2.Text + "";
                //Data Source = USER - PC; User ID = sa; Password = ***********
                conn = new SqlConnection(connectionString);
                conn.Open();
                sql = "EXEC sp_databases";
                command = new SqlCommand(sql, conn);
                reader = command.ExecuteReader();
                metroComboBox2.Items.Clear();
                DesktopAlert.Show("You can now select database");
                metroButton1.Enabled = true;
                while (reader.Read())
                {
                    metroComboBox2.Items.Add(reader[0].ToString());
                }
                metroComboBox2.Enabled = true;

                

            }
            catch 
            {
                //MessageBox.Show(ex.Message);
                
               MetroMessageBox.Show(this, "", "Please check your Inputs!", MessageBoxButtons.OK, MessageBoxIcon.Question);
                metroTextBox1.Text = "";
                metroTextBox2.Text = "";
                metroTextBox3.Text = "";
            }
        }

        private void metroComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void metroButton5_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Backup Files(*.bak)|*.bak|All Files(*.*)|*.*";
            dlg.FilterIndex = 0;
            if (dlg.ShowDialog() == DialogResult.OK) 
            {
                metroTextBox4.Text = dlg.FileName;
            }
        }

       

    }
}