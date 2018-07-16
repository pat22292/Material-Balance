using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar.Controls;
using MetroFramework.Forms;
using System.Data.SqlClient;


namespace productionMonitoringSystem06302016
{
    public partial class lockedUser : MetroForm
    {
        public lockedUser()
        {
            InitializeComponent();
            metroTextButton2.Enabled = false;
            slidePanel2.IsOpen = false;
            metroLabel2.Text = globalVar.x;
            metroLabel3.Text = globalVar.x;
            ableToReset();
        }
        private void lockedUser_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }
        private void lockedUser_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode.ToString() == "R")
            {
                DesktopAlert.Show("Beng");
                //slidePanel1.IsOpen = true;
            }
        }
        private void metroTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode.ToString() == "R")
            {
                DesktopAlert.Show("Beng");
                //slidePanel1.IsOpen = true;
            }
        }
        private void metroTextButton1_Click(object sender, EventArgs e)
        {
            pinCheck();
        }
        private void metroTextButton1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode.ToString() == "R")
            {
                DesktopAlert.Show("Beng");
                //slidePanel1.IsOpen = true;
            }
        }
        private void pinCheck()
        {
            int user = 0;
            sqlcon userConnect = new sqlcon();
            userConnect.dbIn();
            SqlCommand cmd = new SqlCommand("[resetPasswords2] '" + metroLabel2.Text + "'", sqlcon.con);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                user += 1;
                globalVar.pinCode = dr["pinCode"].ToString();
                
            }
            userConnect.dbOut();
            if (globalVar.pinCode == metroTextBox1.Text)
            {
                slidePanel2.IsOpen = true;
                slidePanel1.IsOpen = false;
            }
            else
            {
                DesktopAlert.Show("PIN code does not match");
            }
            
        }
        private void changePW()
        {
            sqlcon userConnect = new sqlcon();
            userConnect.dbIn();
            SqlCommand recipe = new SqlCommand("[resetPasswords3]", sqlcon.con);
            recipe.CommandType = System.Data.CommandType.StoredProcedure;
            recipe.Parameters.AddWithValue("@userID", metroLabel2.Text.ToString());
            recipe.Parameters.AddWithValue("@passkey", metroTextBox2.Text.ToString());
            recipe.Parameters.AddWithValue("@pinCode", globalVar.pinCode.ToString());
            recipe.ExecuteNonQuery();
            userConnect.dbOut();
            DesktopAlert.Show("You're new password has been set!");
            this.FormClosing -= new System.Windows.Forms.FormClosingEventHandler(this.lockedUser_FormClosing);
            this.Close();
        }
        private void metroTextButton2_Click(object sender, EventArgs e)
        {
            changePW();
        }
        private void metroTextBox2_TextChanged(object sender, EventArgs e)
        {
            if (metroTextBox3.Text == metroTextBox2.Text)
            {
                metroTextButton2.Enabled = true;
            }
        }
        private void metroTextButton3_Click(object sender, EventArgs e)
        {
            this.FormClosing -= new System.Windows.Forms.FormClosingEventHandler(this.lockedUser_FormClosing);
            this.Close();
        }

        private void metroTextBox3_KeyUp(object sender, KeyEventArgs e)
        {
            metroTextBox3.Text = metroTextBox3.Text.Replace("\r\n", "");
        }

        private void metroTextBox2_KeyUp(object sender, KeyEventArgs e)
        {
            metroTextBox2.Text = metroTextBox2.Text.Replace("\r\n", "");
        }
        private void ableToReset()
        {
            int user = 0;
            sqlcon userConnect = new sqlcon();
            userConnect.dbIn();
            SqlCommand cmd = new SqlCommand("[resetPasswords2] '" + metroLabel2.Text + "'", sqlcon.con);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read()){user += 1;}
            userConnect.dbOut();
            if (user != 1) { slidePanel1.IsOpen = false; slidePanel2.IsOpen = false; metroLabel1.Visible = false; metroLabel4.Visible = true; }

        }
    }
}
