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
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using System.Data.SqlClient;


namespace productionMonitoringSystem06302016
{
    public partial class LOmessagaForm : DevComponents.DotNetBar.Metro.MetroForm 
    {
        public LOmessagaForm()
        {
            InitializeComponent();
        }

        private void metroTile1_Click(object sender, EventArgs e)
        {
            Form1 fm1 = new Form1();
            fm1.Hide();
            sqlcon userOUT = new sqlcon();
            userOUT.dbIn();
            SqlCommand logOut = new SqlCommand("[timeLogoUT] '" + globalVar.x + "','"+globalVar.name+"'", sqlcon.con);
            SqlDataReader lO = logOut.ExecuteReader();
            while (lO.Read())
            {
            }
            this.Hide();
            userOUT.dbOut();
            fm1.Close();
            Application.Exit();
            loginForm lF = new loginForm();
            lF.Show();
        }

        private void metroTile2_Click(object sender, EventArgs e)
        {
            this.Close();
            Form1 fm1 = new Form1();
            fm1.Show();
        }

        private void LOmessagaForm_Load(object sender, EventArgs e)
        {
            string screenWidth = Screen.PrimaryScreen.Bounds.Width.ToString();
            string screenHeight = Screen.PrimaryScreen.Bounds.Height.ToString();
            if (screenWidth == "1366" && screenHeight == "768"){this.Width = 1366;}
            else if (screenWidth == "1920" && screenHeight == "1080"){this.Width = 1920;}
            metroLabel2.Text = "Hi, " + globalVar.name;
            this.CenterToScreen();
        }
    }
}
