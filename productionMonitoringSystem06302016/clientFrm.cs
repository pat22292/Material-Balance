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
using DevComponents.DotNetBar.Controls;
using MetroFramework;

namespace productionMonitoringSystem06302016
{
    public partial class clientFrm :MetroForm 
    {
        public clientFrm()
        {
            InitializeComponent();
            displayClients();
            metroButton1.Enabled = false;
            slidePanel2.IsOpen = false;
            for (int i = 1; i <= 7; i++)
            {
                metroGrid2.Columns[i].ReadOnly = true;
            }
           
        }
        private void clientFrm_Load(object sender, EventArgs e)
        {
            statusCheck();
            
        }
        private void displayClients() //Display users list....
        {
            sqlcon userConnect = new sqlcon();
            userConnect.dbIn();
            SqlCommand client = new SqlCommand("[diplayClients]", sqlcon.con);
            SqlDataAdapter calculated = new SqlDataAdapter();
            calculated.SelectCommand = client;
            DataTable dataSet = new DataTable();
            calculated.Fill(dataSet);
            BindingSource nSource = new BindingSource();
            nSource.DataSource = dataSet;
            metroGrid2.DataSource = nSource;
            calculated.Update(dataSet);
            userConnect.dbOut();

        }
        private void disableMod()
        {
            if  ( metroTextBox1.Text == "" || metroTextBox2.Text == "" || metroTextBox8.Text == "" || metroTextBox14.Text == "" || metroTextBox15.Text == "" || metroTextBox16.Text == "" )
            {
                metroTextButton1.Enabled = false;
            }
            else
            {
                metroTextButton1.Enabled = true;
            }
        }
        private void metroButton1_Click(object sender, EventArgs e)
        {
            if (metroTextBox3.Text == "" || metroTextBox4.Text == "" || metroTextBox5.Text == "" || metroTextBox6.Text == "" || metroTextBox7.Text == "")
            {
                DesktopAlert.Show("Please check your inputs!");
            }
            else
            {
                 DialogResult dialogResult = MetroMessageBox.Show(this, "Are you sure you want to save this client? .", "Save Client", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                 if (dialogResult == DialogResult.Yes)addClient(); DesktopAlert.Show("New client has been added!"); ClearTextBoxes(this.Controls);
                
            }
            statusCheck();
        }

        private void ClearTextBoxes(Control.ControlCollection cc)
        {
            foreach (Control ctrl in cc)
            {
                TextBox tb = ctrl as TextBox;
                if (tb != null)
                    tb.Text = "";
                else
                    ClearTextBoxes(ctrl.Controls);
            }
        }
        private void addClient()
        {
            try {
                sqlcon userConnect = new sqlcon();
                userConnect.dbIn();
                SqlCommand addClient = new SqlCommand("[addClient]", sqlcon.con);
                addClient.CommandType = System.Data.CommandType.StoredProcedure;
                addClient.Parameters.AddWithValue("@Cname", metroTextBox3.Text);
                addClient.Parameters.AddWithValue("@FirstName", metroTextBox4.Text);
                addClient.Parameters.AddWithValue("@MiddleName", metroTextBox11.Text);
                addClient.Parameters.AddWithValue("@LastName", metroTextBox12.Text);
                addClient.Parameters.AddWithValue("@Description", metroTextBox13.Text);
                addClient.Parameters.AddWithValue("@Address", metroTextBox5.Text);
                addClient.Parameters.AddWithValue("@ContactNo", metroTextBox6.Text);
                addClient.Parameters.AddWithValue("@EmailAdd", metroTextBox7.Text);
                addClient.Parameters.AddWithValue("@addBy", globalVar.name.ToString());
                addClient.ExecuteNonQuery();
                userConnect.dbOut();
                displayClients();
            }
           catch{}
        }
        private void metroTextBox3_TextChanged(object sender, EventArgs e)
        {
            avoidBlanks();
        }
        private void avoidBlanks()
        {
            if (metroTextBox3.Text == "" || metroTextBox4.Text == "" || metroTextBox5.Text == "" 
                || metroTextBox6.Text == "" || metroTextBox7.Text == ""
                || metroTextBox11.Text == "" || metroTextBox12.Text == "" || metroTextBox13.Text == "")
            {
                metroButton1.Enabled = false;
            }
            else
            {
                metroButton1.Enabled = true;
            }
        
            
        }
        private void metroTextBox4_TextChanged(object sender, EventArgs e)
        {
            avoidBlanks();
        }
        private void metroTextBox5_TextChanged(object sender, EventArgs e)
        {
            avoidBlanks();
        }
        private void metroTextBox6_TextChanged(object sender, EventArgs e)
        {
            avoidBlanks();
        }
        private void metroTextBox7_TextChanged(object sender, EventArgs e)
        {
            avoidBlanks();
        }
        private void clientDetails() //Display User details in textboxes..in modifyTab
        {
            sqlcon userConnect = new sqlcon();
            userConnect.dbIn();
            SqlCommand cmd = new SqlCommand("[showClients] '" + metroLabel11.Text + "'", sqlcon.con);
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                metroTextBox9.Text = dr["Description"].ToString();
                metroTextBox1.Text = dr["Address"].ToString();
                metroTextBox8.Text = dr["ContactNoumber"].ToString();
                metroTextBox2.Text = dr["emailAdress"].ToString();
                metroTextBox16.Text = dr["FirstName"].ToString();
                metroTextBox15.Text = dr["MiddleName"].ToString();
                metroTextBox14.Text = dr["LastName"].ToString();
            }
            userConnect.dbOut();
        }
        private void metroGrid2_DoubleClick(object sender, EventArgs e)
        {
            slidePanel2.BringToFront();
            slidePanel2.AnimationTime = 1000;
            slidePanel2.SlideSide = eSlideSide.Left;
            slidePanel2.IsOpen = true;
            metroGrid2.Enabled = false;
            var rowsCount = metroGrid2.SelectedRows.Count;
            if (rowsCount == 0 || rowsCount > 1) return;
            metroLabel11.Text = metroGrid2.SelectedCells[2].Value.ToString();
            clientDetails();
            this.Opacity = 30;
            texiUsers tUsr = new texiUsers();
            tUsr.Opacity = 1;
        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            slidePanel2.SlideSide = eSlideSide.Right;
            slidePanel2.IsOpen = false;
            metroGrid2.Enabled = true;
        }

        private void metroTextBox10_KeyUp(object sender, KeyEventArgs e)
        {
            //metroTextBox10.Text = metroTextBox10.Text.Replace("\r\n", "");
        }

        private void metroTextBox9_KeyUp(object sender, KeyEventArgs e)
        {
            metroTextBox9.Text = metroTextBox9.Text.Replace("\r\n", " ");
        }

        private void metroTextBox1_Click(object sender, EventArgs e)
        {
            metroTextBox1.Text = metroTextBox1.Text.Replace("\r\n", "");
        }

        private void metroTextBox8_KeyUp(object sender, KeyEventArgs e)
        {
            metroTextBox8.Text = metroTextBox8.Text.Replace("\r\n", "");
        }

        private void metroTextBox2_KeyUp(object sender, KeyEventArgs e)
        {
            metroTextBox2.Text = metroTextBox2.Text.Replace("\r\n", "");
        }

        private void metroTextBox3_KeyUp(object sender, KeyEventArgs e)
        {
            metroTextBox3.Text = metroTextBox3.Text.Replace("\r\n", "");
        }

        private void metroTextBox4_KeyUp(object sender, KeyEventArgs e)
        {
            metroTextBox4.Text = metroTextBox4.Text.Replace("\r\n", " ");
        }

        private void metroTextBox5_KeyUp(object sender, KeyEventArgs e)
        {
            metroTextBox5.Text = metroTextBox5.Text.Replace("\r\n", "");
        }

        private void metroTextBox6_KeyUp(object sender, KeyEventArgs e)
        {
            metroTextBox6.Text = metroTextBox6.Text.Replace("\r\n", "");
        }

        private void metroTextBox7_Click(object sender, EventArgs e)
        {
            metroTextBox7.Text = metroTextBox7.Text.Replace("\r\n", "");
        }

        private void metroTextButton1_Click(object sender, EventArgs e)
        {
            sqlcon userConnect = new sqlcon();
            userConnect.dbIn();
            SqlCommand addClient = new SqlCommand("[modifyClient]", sqlcon.con);
            addClient.CommandType = System.Data.CommandType.StoredProcedure;
            addClient.Parameters.AddWithValue("@clientName", metroLabel11.Text);
            //addClient.Parameters.AddWithValue("@newClientName", metroTextBox10.Text);
            addClient.Parameters.AddWithValue("@description", metroTextBox9.Text);
            addClient.Parameters.AddWithValue("@Fname", metroTextBox16.Text);
            addClient.Parameters.AddWithValue("@Mname", metroTextBox15.Text);
            addClient.Parameters.AddWithValue("@Lname", metroTextBox14.Text);
            addClient.Parameters.AddWithValue("@address", metroTextBox1.Text);
            addClient.Parameters.AddWithValue("@contactNumber", metroTextBox8.Text);
            addClient.Parameters.AddWithValue("@emailAddress", metroTextBox2.Text);
            addClient.Parameters.AddWithValue("@user", globalVar.name.ToString());
            addClient.ExecuteNonQuery();
            userConnect.dbOut();
            displayClients();
            slidePanel2.SlideSide = eSlideSide.Right;
            slidePanel2.IsOpen = false;
            metroGrid2.Enabled = true;
            statusCheck();
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            
            try
            {
                foreach (DataGridViewRow item in metroGrid2.Rows)
                {
                    
                        sqlcon userConnect = new sqlcon();
                        userConnect.dbIn();
                        SqlCommand material = new SqlCommand("[voidClient]", sqlcon.calc);
                        material.CommandType = System.Data.CommandType.StoredProcedure;
                        material.Parameters.AddWithValue("@name", item.Cells[2].Value.ToString());
                        if (item.Cells[0].Value.Equals("Enable")) { material.Parameters.AddWithValue("@void", "N");}
                        else if (item.Cells[0].Value.Equals("Disable")) { material.Parameters.AddWithValue("@void", "Y");}
                        material.ExecuteNonQuery();
                        userConnect.dbOut();
                        
                }
                
            }
            catch { return; }
            displayClients();
            statusCheck();
        }
        private void statusCheck()
        {
            foreach (DataGridViewRow row in metroGrid2.Rows)
            {
                DataGridViewComboBoxCell chk = (DataGridViewComboBoxCell)row.Cells[0];
                if (row.Cells[9].Value.Equals("Y")) { chk.Value = "Disable"; }
                else if (row.Cells[9].Value.Equals("N")) { chk.Value = "Enable"; }
            }
        }

        private void metroTextBox7_TextChanged_1(object sender, EventArgs e)
        {
            avoidBlanks();
        }

        private void metroTextBox6_TextChanged_1(object sender, EventArgs e)
        {
            avoidBlanks();
        }

        private void metroTextBox5_TextChanged_1(object sender, EventArgs e)
        {
            avoidBlanks();
        }

        private void metroTextBox13_TextChanged(object sender, EventArgs e)
        {
            avoidBlanks();
        }

        private void metroTextBox12_TextChanged(object sender, EventArgs e)
        {
            avoidBlanks();
        }

        private void metroTextBox4_TextChanged_1(object sender, EventArgs e)
        {
            avoidBlanks();
        }

        private void metroTextBox11_TextChanged(object sender, EventArgs e)
        {
            avoidBlanks();
        }

        private void metroTextBox3_TextChanged_1(object sender, EventArgs e)
        {
            avoidBlanks();
        }

        private void metroTextBox7_Click_1(object sender, EventArgs e)
        {

        }

        private void metroTextBox16_TextChanged(object sender, EventArgs e)
        {
            disableMod();
        }

        private void metroTextBox15_TextChanged(object sender, EventArgs e)
        {
            disableMod();
        }

        private void metroTextBox14_Click(object sender, EventArgs e)
        {

        }

        private void metroTextBox14_TextChanged(object sender, EventArgs e)
        {
            disableMod();
        }

        private void metroTextBox9_TextChanged(object sender, EventArgs e)
        {
            disableMod();
        }

        private void metroTextBox1_TextChanged(object sender, EventArgs e)
        {
            disableMod();
        }

        private void metroTextBox8_TextChanged(object sender, EventArgs e)
        {
            disableMod();
        }

        private void metroTextBox2_TextChanged(object sender, EventArgs e)
        {
            disableMod();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void metroButton4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

     
    }
}
