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
using MetroFramework;

namespace productionMonitoringSystem06302016
{
    public partial class texiUsers : MetroForm
    {
        public texiUsers()
        {
            InitializeComponent();
             
            metroTabControl1.SelectTab(addUser);
            //listOfusers();
            displayUsers();
            blockedUsr();
            listOfpositions();
            displayUsers2();
            slidePanel2.IsOpen = false;
            slidePanel4.IsOpen = false;
            for (int i = 1; i <= 8; i++)
            {
                metroGrid1.Columns[i].ReadOnly = true;
            }
            for (int i = 1; i <= 4; i++)
            {
                metroGrid3.Columns[i].ReadOnly = true;
            }   
        }
        private void reloadChkBox()
        {
            foreach (DataGridViewRow row in metroGrid1.Rows)
            {
                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells[0];
                chk.Value = false;
            }

            foreach (DataGridViewRow row in metroGrid3.Rows)
            {
                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells[0];
                chk.Value = false;
            }
       

        }
        private void dis()
        {
            if (metroTextBox6.Text == "" || metroTextBox7.Text == "")
            {
                metroTextButton3.Enabled = false;
            }
            else
            {
                metroTextButton3.Enabled = true;
            }
        }
        private void searchBox() //filter users list....
        {
            sqlcon userConnect = new sqlcon();
            userConnect.dbIn();
            SqlCommand users = new SqlCommand("[searchUser] '" + textBoxX1.Text+ "'", sqlcon.con);
            SqlDataAdapter calculated = new SqlDataAdapter();
            calculated.SelectCommand = users;
            DataTable dataSet = new DataTable();
            calculated.Fill(dataSet);
            BindingSource nSource = new BindingSource();
            nSource.DataSource = dataSet;
            metroGrid1.DataSource = nSource;
            calculated.Update(dataSet);
            userConnect.dbOut();
        }
        private void displayUsers() //Display users list....
        {
            sqlcon userConnect = new sqlcon();
            userConnect.dbIn();
            SqlCommand users = new SqlCommand("[diplayUsers]", sqlcon.con);
            SqlDataAdapter calculated = new SqlDataAdapter();
            calculated.SelectCommand = users;
            DataTable dataSet = new DataTable();
            calculated.Fill(dataSet);
            BindingSource nSource = new BindingSource();
            nSource.DataSource = dataSet;
            metroGrid1.DataSource = nSource;
            calculated.Update(dataSet);
            userConnect.dbOut();
        }
        private void listOfpositions() //provide the client list for combobox
        {
            sqlcon userConnect = new sqlcon();
            userConnect.dbIn();
            SqlDataAdapter da = new SqlDataAdapter("exec [joblist]", sqlcon.con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            metroComboBox2.DataSource = dt;
            metroComboBox2.DisplayMember = "pos";
            DataTable mod = new DataTable();
            da.Fill(mod);
            metroComboBox6.DataSource = mod;
            metroComboBox6.DisplayMember = "pos";
            userConnect.dbOut();
        }
        private void metroButton1_Click(object sender, EventArgs e)
        {
            if (metroTextBox1.Text == "" || metroTextBox1.Text == "" || metroTextBox3.Text == "" || metroComboBox1.Text == "")
            {
                MetroMessageBox.Show(this, "", "Please check your inputs!", MessageBoxButtons.OK, MessageBoxIcon.Question);
            }
            else 
            {
                AddUser();
                DesktopAlert.Show("User has been added.");
                displayUsers();
                metroTextBox1.Text = "";
                metroTextBox2.Text = "";
                metroTextBox3.Text = "";
            }
        }
        private void userExistCheck() //Check for  user existence....
        {
            string fName = "";
            string lName = "";
            sqlcon userConnect = new sqlcon();
            userConnect.dbIn();
            SqlCommand cmd = new SqlCommand("[CheckDuplicates] '" + metroTextBox1.Text + "'", sqlcon.con);
            SqlDataReader dr = cmd.ExecuteReader();
            int user = 0;

            while (dr.Read())
            {
                user += 1;
                fName = dr["FirstName"].ToString();
                lName = dr["LastName"].ToString();
            }
            userConnect.dbOut();
            if (fName.ToUpper() == metroTextBox1.Text.ToUpper() && lName.ToUpper() == metroTextBox2.Text.ToUpper() &&
                metroTextBox1.Text != string.Empty && metroTextBox2.Text != string.Empty)
            {
                metroButton1.Enabled = false;
                DesktopAlert.Show("Already Exist");
            }
            else
            {
                metroButton1.Enabled = true;
            }
        }
        private void metroTextBox1_TextChanged(object sender, EventArgs e)
        {
            userExistCheck();
        }
        private void metroTextBox2_TextChanged(object sender, EventArgs e)
        {
            userExistCheck();
        }
        private void metroGrid1_DoubleClick(object sender, EventArgs e)
        {
            var rowsCntChck = metroGrid1.SelectedRows.Count;
            if ( rowsCntChck == 0 || rowsCntChck > 1 ) 

            {
            DesktopAlert.Show("Input a user first!");
            metroButton3.Enabled = false;
            }
            else
            {
            slidePanel2.BringToFront();
            metroTextBox11.Select();
            slidePanel2.AnimationTime = 1000;
            slidePanel2.SlideSide = eSlideSide.Left;
            slidePanel2.IsOpen = true;
            slidePanel1.Enabled = false;
            metroGrid1.Enabled = false;
            var rowsCount = metroGrid1.SelectedRows.Count;
            if (rowsCount == 0 || rowsCount > 1) return;
            metroLabel12.Text = metroGrid1.SelectedCells[1].Value.ToString();
            searchUser();
            texiUsers tUsr = new texiUsers();
            }
        }
        private void metroButton2_Click(object sender, EventArgs e)
        {
            slidePanel1.Enabled = true;
            slidePanel2.SlideSide = eSlideSide.Right;
            slidePanel2.IsOpen = false;
            metroGrid1.Enabled = true;
        }
        private void searchUser() //Display User details in textboxes..in slidepanel
        {
            sqlcon userConnect = new sqlcon();
            userConnect.dbIn();
            SqlCommand cmd = new SqlCommand("[findUser] '" + metroLabel12.Text + "'", sqlcon.con);
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                metroTextBox11.Text = dr["FirstName"].ToString();
                metroTextBox10.Text = dr["LastName"].ToString();
                metroTextBox9.Text = dr["passkey"].ToString();
                metroComboBox6.Text = dr["position"].ToString();
                metroComboBox5.Text = dr["Gender"].ToString();
                if (dr["Employeestatus"].ToString() == "Activated")
                {
                    metroCheckBox1.CheckState = CheckState.Checked;
                }
                else
                {
                    metroCheckBox1.CheckState = CheckState.Unchecked;
                }
            }
            userConnect.dbOut();
        }
        private void metroComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (metroComboBox2.Text == "System Admin" && globalVar.position != "System Admin")
            {
                DesktopAlert.Show("You are not allowed to use or change this Position!");
            }
        }
        private void textBoxX1_TextChanged(object sender, EventArgs e)
        {
            if (textBoxX1.Text == null || textBoxX1.Text == "") { displayUsers(); }
            else {searchBox();}
        }
        private void switchButton1_ValueChanged(object sender, EventArgs e)
        {
            if (switchButton1.Value == true){slidePanel1.Enabled = true;}
            else{slidePanel1.Enabled = false;}
        }
        private void AddUser()
        {
            sqlcon userConnect = new sqlcon();
            userConnect.dbIn();
            SqlCommand material = new SqlCommand("[AddUser]", sqlcon.con);
            material.CommandType = System.Data.CommandType.StoredProcedure;
            material.Parameters.AddWithValue("@FirstName", metroTextBox1.Text);
            material.Parameters.AddWithValue("@LastName", metroTextBox2.Text);
            material.Parameters.AddWithValue("@passkey", metroTextBox3.Text);
            material.Parameters.AddWithValue("@position", metroComboBox2.Text);
            material.Parameters.AddWithValue("@Gender", metroComboBox1.Text);
            material.Parameters.AddWithValue("@user", globalVar.name.ToString());
            material.ExecuteNonQuery();
            userConnect.dbOut();
        }
        private void textBoxX1_Click(object sender, EventArgs e)
        {
            switchButton1.Value = false;
        }
        private void metroTextButton1_Click(object sender, EventArgs e)
        {
            
            if (metroTextBox11.Text == "" || metroTextBox10.Text == "" || metroTextBox9.Text == "")
            {
                MetroMessageBox.Show(this, "", "Please Check Your Inputs", MessageBoxButtons.OK, MessageBoxIcon.Question);
            }
            else
            {
                modifyAcct();
                MetroMessageBox.Show(this, "", "User profile has been modified!", MessageBoxButtons.OK, MessageBoxIcon.Question);
                searchBox();
            }
        }
        private void modifyAcct()
        {
            string stat = "";
            if (metroCheckBox1.CheckState == CheckState.Checked)
            {
                stat = "Activated";
            }
            else
            {
                stat = "Deactivated";
            }
            sqlcon userConnect = new sqlcon();
            userConnect.dbIn();
            SqlCommand modify = new SqlCommand("modifyAcct", sqlcon.con);
            modify.CommandType = System.Data.CommandType.StoredProcedure;
            modify.Parameters.AddWithValue("@FirstName", metroTextBox11.Text);
            modify.Parameters.AddWithValue("@LastName", metroTextBox10.Text);
            modify.Parameters.AddWithValue("@passkey", metroTextBox9.Text);
            modify.Parameters.AddWithValue("@position", metroComboBox6.Text);
            modify.Parameters.AddWithValue("@Gender", metroComboBox5.Text);
            modify.Parameters.AddWithValue("@status", stat);
            modify.Parameters.AddWithValue("@userID", metroLabel12.Text);
            modify.ExecuteNonQuery();
            userConnect.dbOut();
        }

        private void metroTextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            metroTextBox1.Text = metroTextBox1.Text.Replace("\r\n", "");
        }

        private void metroTextBox3_KeyUp(object sender, KeyEventArgs e)
        {
            metroTextBox3.Text = metroTextBox3.Text.Replace("\r\n", "");
        }

        private void metroTextBox2_KeyUp(object sender, KeyEventArgs e)
        {
            metroTextBox2.Text = metroTextBox2.Text.Replace("\r\n", "");
        }

        private void textBoxX1_KeyUp(object sender, KeyEventArgs e)
        {
            textBoxX1.Text = textBoxX1.Text.Replace("\r\n", "");
        }
        private void blockedUsr() //filter users list....
        {
            sqlcon userConnect = new sqlcon();
            userConnect.dbIn();
            SqlCommand users = new SqlCommand("[diplayBlockedAccts]", sqlcon.con);
            SqlDataAdapter calculated = new SqlDataAdapter();
            calculated.SelectCommand = users;
            DataTable dataSet = new DataTable();
            calculated.Fill(dataSet);
            BindingSource nSource = new BindingSource();
            nSource.DataSource = dataSet;
            metroGrid2.DataSource = nSource;
            calculated.Update(dataSet);
            userConnect.dbOut();
        }
        private void texiUsers_Load(object sender, EventArgs e)
        {
            slidePanel1.Enabled = false;
            metroTextButton3.Enabled = false;
            reloadChkBox();
     
        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            checkDetect();
            try
            {
                foreach (DataGridViewRow item in metroGrid1.Rows)
                {
                    if (bool.Parse(item.Cells[0].Value.ToString()))
                    {
                        sqlcon userConnect = new sqlcon();
                        userConnect.dbIn();
                        SqlCommand material = new SqlCommand("[multiUserDel]", sqlcon.calc);
                        material.CommandType = System.Data.CommandType.StoredProcedure;
                        material.Parameters.AddWithValue("@id", item.Cells[1].Value.ToString());
                        material.ExecuteNonQuery();
                        userConnect.dbOut();
                    }
                }
            }
            catch{return;}
            displayUsers();
            reloadChkBox();
            
        }
        private void checkDetect() //filter users list....
        {

            foreach (DataGridViewRow row in metroGrid1.Rows)
            {
                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells[0];
                if (chk.Value.Equals(false)) { metroButton3.Enabled = false; }
                else if (chk.Value.Equals(true)) { metroButton3.Enabled = true; }
            }
        }

        private void metroGrid1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            metroButton3.Enabled = true;
        }

        private void metroTextButton3_Click(object sender, EventArgs e)
        {
            addNewPositon();
            metroTextBox7.Text = "";
            metroTextBox6.Text = "";
            foreach (Control ctrl in groupBox1.Controls )
            {
                if (ctrl is CheckBox)
                {
                    CheckBox checkBox = (CheckBox)ctrl;
                    checkBox.Checked = false;
                }
            }
        }

        private void metroTextButton2_Click(object sender, EventArgs e)
        {
            if (metroTextBox5.Text == "")
            {
                DesktopAlert.Show("Please Check your Inputs!");
            }
            else
            {
                modifyPosition();
            }
        }
        private void metroButton5_Click(object sender, EventArgs e)
        {
            try
            {
                sqlcon userConnect = new sqlcon();
                userConnect.dbIn();
                foreach (DataGridViewRow item in metroGrid3.Rows)
                {
                   
                    if (bool.Parse(item.Cells[0].Value.ToString()))
                    {
                   
                        SqlCommand material = new SqlCommand("[deletePosition]", sqlcon.calc);
                        material.CommandType = System.Data.CommandType.StoredProcedure;
                        material.Parameters.AddWithValue("@position", item.Cells[1].Value.ToString());
                        material.Parameters.AddWithValue("@stat", "Y");
                        material.Parameters.AddWithValue("@addBy", globalVar.name.ToString());
                        material.ExecuteNonQuery();
                       
                    }
                    //else
                    //{

                    //    SqlCommand material = new SqlCommand("[deletePosition]", sqlcon.calc);
                    //    material.CommandType = System.Data.CommandType.StoredProcedure;
                    //    material.Parameters.AddWithValue("@position", item.Cells[1].Value.ToString());
                    //    material.Parameters.AddWithValue("@stat", "N");
                    //    material.ExecuteNonQuery();

                    //}
                    

                   
                }
                userConnect.dbOut();
            }
            catch { return; }
            displayUsers2();
            reloadChkBox();
            DesktopAlert.Show("Position has been updated!");

        }
        private void displayUsers2() //Display users list....
        {
            sqlcon userConnect = new sqlcon();
            userConnect.dbIn();
            SqlCommand users = new SqlCommand("[diplayOrg]", sqlcon.con);
            SqlDataAdapter calculated = new SqlDataAdapter();
            calculated.SelectCommand = users;
            DataTable dataSet = new DataTable();
            calculated.Fill(dataSet);
            BindingSource nSource = new BindingSource();
            nSource.DataSource = dataSet;
            metroGrid3.DataSource = nSource;
            calculated.Update(dataSet);
            userConnect.dbOut();
        }
        private void addNewPositon()
        {
            //string privilage = "";
            string T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14 = "";
            sqlcon userConnect = new sqlcon();
            userConnect.dbIn();
            SqlCommand recipe = new SqlCommand("[AddNewPostion]", sqlcon.con);
            recipe.CommandType = System.Data.CommandType.StoredProcedure;
            recipe.Parameters.AddWithValue("@Position", metroTextBox7.Text);
            recipe.Parameters.AddWithValue("@Description", metroTextBox6.Text);
            recipe.Parameters.AddWithValue("@user", globalVar.name.ToString());
            //if (metroCheckBox1.CheckState == CheckState.Checked){privilage = "Y";}else{privilage = "N";}
            //recipe.Parameters.AddWithValue("@isAdmin", privilage);

            if (CHK1.CheckState == CheckState.Checked) { T1 = "1"; } else { T1 = "0"; }
            if (CHK2.CheckState == CheckState.Checked) { T2 = "1"; } else { T2 = "0"; }
            if (CHK3.CheckState == CheckState.Checked) { T3 = "1"; } else { T3 = "0"; }
            if (CHK4.CheckState == CheckState.Checked) { T4 = "1"; } else { T4 = "0"; }
            if (CHK5.CheckState == CheckState.Checked) { T5 = "1"; } else { T5 = "0"; }
            if (CHK6.CheckState == CheckState.Checked) { T6 = "1"; } else { T6 = "0"; }
            if (CHK7.CheckState == CheckState.Checked) { T7 = "1"; } else { T7 = "0"; }
            if (CHK8.CheckState == CheckState.Checked) { T8 = "1"; } else { T8 = "0"; }
            if (CHK9.CheckState == CheckState.Checked) { T9 = "1"; } else { T9 = "0"; }
            if (CHK10.CheckState == CheckState.Checked) { T10 = "1"; } else { T10 = "0"; }
            if (CHK27.CheckState == CheckState.Checked) { T11 = "1"; } else { T11 = "0"; }
            if (CHK12.CheckState == CheckState.Checked) { T12 = "1"; } else { T12 = "0"; }
            if (CHK28.CheckState == CheckState.Checked) { T13 = "1"; } else { T13 = "0"; }
            if (CHK11.CheckState == CheckState.Checked) { T14 = "1"; } else { T14 = "0"; }
            recipe.Parameters.AddWithValue("@T1", T1);
            recipe.Parameters.AddWithValue("@T2", T2);
            recipe.Parameters.AddWithValue("@T3", T3);
            recipe.Parameters.AddWithValue("@T4", T4);
            recipe.Parameters.AddWithValue("@T5", T5);
            recipe.Parameters.AddWithValue("@T6", T6);
            recipe.Parameters.AddWithValue("@T7", T7);
            recipe.Parameters.AddWithValue("@T8", T8);
            recipe.Parameters.AddWithValue("@T9", T9);
            recipe.Parameters.AddWithValue("@T10", T10);
            recipe.Parameters.AddWithValue("@T11", T11);
            recipe.Parameters.AddWithValue("@T12", T12);
            recipe.Parameters.AddWithValue("@T13", T13);
            recipe.Parameters.AddWithValue("@T14", T14);
            recipe.ExecuteNonQuery();
            userConnect.dbOut();
            displayUsers2();

        }
        private void searchPosition() //Display User details in textboxes..in slidepanel
        {
            sqlcon userConnect = new sqlcon();
            userConnect.dbIn();
            SqlCommand cmd = new SqlCommand("[findOrg] '" + label1.Text + "'", sqlcon.con);
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                metroLabel8.Text = dr["Position"].ToString();
                //metroTextBox4.Text = dr["Position"].ToString();
                metroTextBox5.Text = dr["Description"].ToString();
                if (dr["IsAdmin"].ToString() == "Yes") { metroCheckBox2.CheckState = CheckState.Checked; } else { metroCheckBox2.CheckState = CheckState.Unchecked; }
                if (dr["Addusertile"].ToString() == "1") { CHK13.CheckState = CheckState.Checked; } else { CHK13.CheckState = CheckState.Unchecked; }
                if (dr["UserRoletile"].ToString() == "1") { CHK14.CheckState = CheckState.Checked; } else { CHK14.CheckState = CheckState.Unchecked; }
                if (dr["ClientTile"].ToString() == "1") { CHK15.CheckState = CheckState.Checked; } else { CHK15.CheckState = CheckState.Unchecked; }
                if (dr["ReportTile"].ToString() == "1") { CHK16.CheckState = CheckState.Checked; } else { CHK16.CheckState = CheckState.Unchecked; }
                if (dr["MMLTile"].ToString() == "1") { CHK17.CheckState = CheckState.Checked; } else { CHK17.CheckState = CheckState.Unchecked; }
                if (dr["SalesTile"].ToString() == "1") { CHK18.CheckState = CheckState.Checked; } else { CHK18.CheckState = CheckState.Unchecked; }
                if (dr["materialStocksTile"].ToString() == "1") { CHK19.CheckState = CheckState.Checked; } else { CHK19.CheckState = CheckState.Unchecked; }
                if (dr["FormulaTile"].ToString() == "1") { CHK20.CheckState = CheckState.Checked; } else { CHK20.CheckState = CheckState.Unchecked; }
                if (dr["QuotationTile"].ToString() == "1") { CHK21.CheckState = CheckState.Checked; } else { CHK21.CheckState = CheckState.Unchecked; }
                if (dr["expensesTile"].ToString() == "1") { CHK22.CheckState = CheckState.Checked; } else { CHK22.CheckState = CheckState.Unchecked; }
                if (dr["GoodSoldsTile"].ToString() == "1") { CHK26.CheckState = CheckState.Checked; } else { CHK26.CheckState = CheckState.Unchecked; }
                if (dr["BNRTile"].ToString() == "1") { CHK24.CheckState = CheckState.Checked; } else { CHK24.CheckState = CheckState.Unchecked; }
                if (dr["PRODUCTION"].ToString() == "1") { CHK25.CheckState = CheckState.Checked; } else { CHK25.CheckState = CheckState.Unchecked; }
                if (dr["FGOODS"].ToString() == "1") { CHK23.CheckState = CheckState.Checked; } else { CHK23.CheckState = CheckState.Unchecked; }
            }

            userConnect.dbOut();
        }
        private void modifyPosition() //modify users details....
        {
            string privilage = "";
            string T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14= "";
            sqlcon userConnect = new sqlcon();
            userConnect.dbIn();
            SqlCommand recipe = new SqlCommand("[modifyPosition]", sqlcon.con);
            recipe.CommandType = System.Data.CommandType.StoredProcedure;

            recipe.Parameters.AddWithValue("@Position", metroLabel8.Text);
            recipe.Parameters.AddWithValue("@Description", metroTextBox5.Text);
            recipe.Parameters.AddWithValue("@user", globalVar.name.ToString());
            //recipe.Parameters.AddWithValue("@position2", metroLabel8.Text);
            if (metroCheckBox2.CheckState == CheckState.Checked) { privilage = "Y"; } else { privilage = "N"; }
            recipe.Parameters.AddWithValue("@isAdmin", privilage);

            if (CHK13.CheckState == CheckState.Checked) { T1 = "1"; } else { T1 = "0"; }
            if (CHK14.CheckState == CheckState.Checked) { T2 = "1"; } else { T2 = "0"; }
            if (CHK15.CheckState == CheckState.Checked) { T3 = "1"; } else { T3 = "0"; }
            if (CHK16.CheckState == CheckState.Checked) { T4 = "1"; } else { T4 = "0"; }
            if (CHK17.CheckState == CheckState.Checked) { T5 = "1"; } else { T5 = "0"; }
            if (CHK18.CheckState == CheckState.Checked) { T6 = "1"; } else { T6 = "0"; }
            if (CHK19.CheckState == CheckState.Checked) { T7 = "1"; } else { T7 = "0"; }
            if (CHK20.CheckState == CheckState.Checked) { T8 = "1"; } else { T8 = "0"; }
            if (CHK21.CheckState == CheckState.Checked) { T9 = "1"; } else { T9 = "0"; }
            if (CHK22.CheckState == CheckState.Checked) { T10 = "1"; } else { T10 = "0"; }
            if (CHK26.CheckState == CheckState.Checked) { T11 = "1"; } else { T11 = "0"; }
            if (CHK24.CheckState == CheckState.Checked) { T12 = "1"; } else { T12 = "0"; }
            if (CHK25.CheckState == CheckState.Checked) { T13 = "1"; } else { T13 = "0"; }
            if (CHK23.CheckState == CheckState.Checked) { T14 = "1"; } else { T14 = "0"; }
            recipe.Parameters.AddWithValue("@T1", T1);
            recipe.Parameters.AddWithValue("@T2", T2);
            recipe.Parameters.AddWithValue("@T3", T3);
            recipe.Parameters.AddWithValue("@T4", T4);
            recipe.Parameters.AddWithValue("@T5", T5);
            recipe.Parameters.AddWithValue("@T6", T6);
            recipe.Parameters.AddWithValue("@T7", T7);
            recipe.Parameters.AddWithValue("@T8", T8);
            recipe.Parameters.AddWithValue("@T9", T9);
            recipe.Parameters.AddWithValue("@T10", T10);
            recipe.Parameters.AddWithValue("@T11", T11);
            recipe.Parameters.AddWithValue("@T12", T12);
            recipe.Parameters.AddWithValue("@T13", T13);
            recipe.Parameters.AddWithValue("@T14", T14);
            recipe.ExecuteNonQuery();
            userConnect.dbOut();
            displayUsers2();
            DesktopAlert.Show("Positon has been modified!");
        }

        private void detectPositionExist() //filter position list....
        {

            sqlcon userConnect = new sqlcon();
            userConnect.dbIn();
            SqlCommand cmd = new SqlCommand("[positionID] '" + metroTextBox7.Text + "'", sqlcon.con);
            SqlDataReader dr = cmd.ExecuteReader();
            int user = 0;

            while (dr.Read())
            {
                user += 1;
            }
            userConnect.dbOut();
            if (user > 0) { pictureBox1.Visible = true; metroTextButton3.Enabled = false; }
            else { pictureBox1.Visible = false; metroTextButton3.Enabled = true; }
        }

        private void metroGrid3_DoubleClick(object sender, EventArgs e)
        {
            slidePanel4.BringToFront();
            metroTextBox5.Select();
            slidePanel4.AnimationTime = 1000;
            slidePanel4.SlideSide = eSlideSide.Left;
            slidePanel4.IsOpen = true;
            metroGrid3.Enabled = false;
            var rowsCount = metroGrid1.SelectedRows.Count;
            if (rowsCount == 0 || rowsCount > 1) return;
            label1.Text = metroGrid3.SelectedCells[1].Value.ToString();
            searchPosition();
        }

        private void metroButton4_Click(object sender, EventArgs e)
        {
            slidePanel3.Enabled = true;
            slidePanel4.SlideSide = eSlideSide.Right;
            slidePanel4.IsOpen = false;
            metroGrid3.Enabled = true;
        }

        private void metroTextBox4_KeyUp(object sender, KeyEventArgs e)
        {
            //metroTextBox4.Text = metroTextBox4.Text.Replace("\r\n", "");
        }

        private void metroTextBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            metroTextBox5.Text = metroTextBox5.Text.Replace("\r\n", " ");
        }

        private void metroTextBox7_KeyUp(object sender, KeyEventArgs e)
        {
            metroTextBox7.Text = metroTextBox7.Text.Replace("\r\n", "");
        }

        private void metroTextBox6_KeyUp(object sender, KeyEventArgs e)
        {
            metroTextBox6.Text = metroTextBox6.Text.Replace("\r\n", " ");
        }
       
        private void metroTextBox7_TextChanged(object sender, EventArgs e)
        {
            dis();
            detectPositionExist();
        }

        private void metroButton6_Click(object sender, EventArgs e)
        {

        }

        private void metroButton6_Click_1(object sender, EventArgs e)
        {
            try
            {
                sqlcon userConnect = new sqlcon();
                userConnect.dbIn();
                foreach (DataGridViewRow item in metroGrid3.Rows)
                {

                    if (bool.Parse(item.Cells[0].Value.ToString()))
                    {

                        SqlCommand material = new SqlCommand("[deletePosition]", sqlcon.calc);
                        material.CommandType = System.Data.CommandType.StoredProcedure;
                        material.Parameters.AddWithValue("@position", item.Cells[1].Value.ToString());
                        material.Parameters.AddWithValue("@stat", "N");
                        material.Parameters.AddWithValue("@addBy", globalVar.name.ToString());
                        material.ExecuteNonQuery();

                    }
 

                }
                userConnect.dbOut();
            }
            catch { return; }
            displayUsers2();
            reloadChkBox();
            DesktopAlert.Show("Position has been updated!");
        }

        private void metroTextBox6_TextChanged(object sender, EventArgs e)
        {
            dis();
        }
        //private void listOfusers() //provide the client list for combobox
        //{
        //    sqlcon userConnect = new sqlcon();
        //    userConnect.dbIn();
        //    SqlDataAdapter da = new SqlDataAdapter("exec [userList]", sqlcon.con);
        //    DataTable dt = new DataTable();
        //    da.Fill(dt);
        //    metroComboBox4.DataSource = dt;
        //    metroComboBox4.DisplayMember = "NAMES";
        //    DataTable mod = new DataTable();
        //    da.Fill(mod);
        //    metroComboBox6.DataSource = mod;
        //    metroComboBox6.DisplayMember = "NAMES";
        //    userConnect.dbOut();
        //}
    }
}
