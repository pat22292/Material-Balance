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
    public partial class materialIn : MetroForm 
    {
        private static double amount = 0.00;
        private static double price = 0.00;
        int chkCNT = 0;
        public materialIn()
        {
            InitializeComponent();

            autoSuggest();
            //circularProgress1.Visible = false;
            tempTableOrderIN();
            
            checkinput();

            for (int i = 1; i <7; i++)
            {
                metroGrid1.Columns[i].ReadOnly = true;
            }
            chkCNT = metroGrid1.RowCount;
            //metroLabel2.Text = chkCNT.ToString();
      
        }
        private void addButton()
        {
            DataGridViewButtonXColumn dn = new DataGridViewButtonXColumn();
            metroGrid1.Columns.Add(dn);
            dn.HeaderText = "Received";
            dn.Text = "Received?";
            dn.Name = "btn";
            dn.UseColumnTextForButtonValue = true;


        }
        private void presyo() //Display price in metrotab3
        {

            if (metroTextBox1.Text == null || metroTextBox1.Text == "") { amount = 0.00; } else { amount = Convert.ToDouble(metroTextBox1.Text); }
            if (metroTextBox2.Text == null || metroTextBox2.Text == "") { price = 0.00; } else { price = Convert.ToDouble(metroTextBox2.Text); }
            
            double eq = amount * price;
            metroLabel1.Text = eq.ToString("N");
        }
        private void metroTextBox1_TextChanged(object sender, EventArgs e)
        {
            presyo();
            checkinput();
        }
        public void reviewList()
        {
            sqlcon userConnect = new sqlcon();
            userConnect.dbIn();
            SqlCommand material = new SqlCommand("[materialINListSample]", sqlcon.calc);
            material.CommandType = System.Data.CommandType.StoredProcedure;
            material.Parameters.AddWithValue("@materialName", textBoxX1.Text);
            material.Parameters.AddWithValue("@pricePerKilogram", metroTextBox2.Text);
            material.Parameters.AddWithValue("@amount", metroTextBox1.Text);
            material.Parameters.AddWithValue("@employeeNAme", globalVar.name.ToString());
            material.Parameters.AddWithValue("@userID", globalVar.x.ToString());
            material.ExecuteNonQuery();
            userConnect.dbOut();
            DesktopAlert.Show("Raw material has been added!");
            pictureBox1.Visible = false;
            metroLabel1.Text = "";
            metroTextBox2.Text = "";
        }
        private void metroTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            decimal x;
            if (ch == (char)Keys.Back)
            {
                e.Handled = false;
            }
            else if (!char.IsDigit(ch) && ch != '.' || !Decimal.TryParse(metroTextBox1.Text + ch, out x))
            {
                e.Handled = true;
            }
        }
        private void materialIn_Load(object sender, EventArgs e)
        {
            reloadChkBox();
            
        }
        private void reloadChkBox()
        {
            foreach (DataGridViewRow row in metroGrid1.Rows)
            {
                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells[0];
                chk.Value = false;
            }
        }
        private void textBoxX1_TextChanged(object sender, EventArgs e)
        {
            //presyo();
            if ((textBoxX1.Text.Length) == 1)
            {
                textBoxX1.Text = textBoxX1.Text[0].ToString().ToUpper();
                textBoxX1.Select(2, 1);
            }
            int mat = 0;
                sqlcon userConnect = new sqlcon();
                userConnect.dbIn();
                SqlCommand cmd = new SqlCommand("[materialIDdetect] '" + textBoxX1.Text + "'", sqlcon.calc);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    mat += 1;
                }
                userConnect.dbOut();
                if (textBoxX1.Text != "")
                {
                    if (mat != 1)
                    {
                        metroButton1.Enabled = false;
                        pictureBox2.Visible = true;
                    }
                    else if (mat == 1)
                    {
                        metroButton1.Enabled = true;
                        pictureBox2.Visible = false;
                    }
                }
                else
                {
                    pictureBox2.Visible = false;
                }

                checkinput();
              }
        private void autoSuggest()
        {
            
            AutoCompleteStringCollection namesCollection = new AutoCompleteStringCollection();
            SqlDataReader dReader;
            sqlcon userConnect = new sqlcon();
            userConnect.dbIn();
            SqlCommand cmd = new SqlCommand("[materialsListCombo]", sqlcon.calc);
            cmd.CommandType = CommandType.Text;
            userConnect.dbIn();
            dReader = cmd.ExecuteReader();
            if (dReader.HasRows == true)
            {
                while (dReader.Read())
                {
                    namesCollection.Add(dReader["MaterialName"].ToString());
                }
            }
            else
            {
                DesktopAlert.Show("Data not found");
            }
            
            dReader.Close();
            userConnect.dbOut();
            textBoxX1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textBoxX1.AutoCompleteSource = AutoCompleteSource.CustomSource;
            textBoxX1.AutoCompleteCustomSource = namesCollection;


            textBoxX2.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textBoxX2.AutoCompleteSource = AutoCompleteSource.CustomSource;
            textBoxX2.AutoCompleteCustomSource = namesCollection;
        }
        private void tempTableOrderIN()
        {
            sqlcon userConnect = new sqlcon();
            userConnect.dbIn();
            SqlCommand recipe = new SqlCommand("exec [tempTableOrderIN] ", sqlcon.calc);
            SqlDataAdapter calculated = new SqlDataAdapter();
            calculated.SelectCommand = recipe;
            DataTable dataSet = new DataTable();
            calculated.Fill(dataSet);
            BindingSource nSource = new BindingSource();
            nSource.DataSource = dataSet;
            metroGrid1.DataSource = nSource;
            calculated.Update(dataSet);
            textBoxX1.Text = "";
            metroTextBox1.Text = "";
            userConnect.dbOut();
        }

        private void searchBox() //filter users list....
        {
            sqlcon userConnect = new sqlcon();
            userConnect.dbIn();
            SqlCommand users = new SqlCommand("[searchMaterialIN] '" + textBoxX2.Text + "'", sqlcon.con);
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
      
        private void metroTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter)
            //{
            //    if (textBoxX1.Text == null || metroTextBox1.Text == "")
            //    {
            //        pictureBox1.Visible = true;
            //    }
            //    else
            //    {
            //        reviewList();
            //        tempTableOrderIN();
            //        DesktopAlert.Show("Raw material has been added!");
            //        metroLabel1.Text = "";
            //    }
            //    reloadChkBox();
            //}
        }
        private void metroButton1_Click(object sender, EventArgs e)
        {

            if (textBoxX1.Text == null || metroTextBox1.Text == "")
            {
                pictureBox1.Visible = true;
            }
            else
            {

                DialogResult dialogResult = MetroMessageBox.Show(this, "", "Add item for purchases?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    reviewList();
                    tempTableOrderIN();

                }
               
            }
            reloadChkBox();
            
        }
        private void metroButton2_Click(object sender, EventArgs e)
        {
      
            try
            {
                DialogResult dialogResult = MetroMessageBox.Show(this, "Materials Received?", "Purchased Material", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {

                foreach (DataGridViewRow item2 in metroGrid1.Rows)
                {
                    if (bool.Parse(item2.Cells[0].Value.ToString()))
                    {

                            sqlcon userConnect = new sqlcon();
                            userConnect.dbIn();
                            SqlCommand material = new SqlCommand("[materialINChckBox]", sqlcon.calc);
                            material.CommandType = System.Data.CommandType.StoredProcedure;
                            material.Parameters.AddWithValue("@employee", globalVar.name.ToString());
                            material.Parameters.AddWithValue("@id", item2.Cells[1].Value.ToString());
                            material.ExecuteNonQuery();
                            userConnect.dbOut();
                        }
                    }
                }
            }
            catch
            {
                return;
            }
            tempTableOrderIN();
            reloadChkBox();


        }
        private void disable()
        {
             if (metroGrid1.Rows.Count > 0)
             { checkBox1.Visible = true; metroButton2.Visible = true; }
              else
             { checkBox1.Visible = true; metroButton2.Visible = false; }
        }
        private void uploadToDB()
        {
            sqlcon userConnect = new sqlcon();
            userConnect.dbIn();
            SqlCommand material = new SqlCommand("[materialIN]", sqlcon.calc);
            material.CommandType = System.Data.CommandType.StoredProcedure;
            material.Parameters.AddWithValue("@employee", globalVar.name.ToString());
         
            material.ExecuteNonQuery();
            userConnect.dbOut();
        }

        private void uploadToDB2()
        {
            sqlcon userConnect = new sqlcon();
            userConnect.dbIn();
            SqlCommand material = new SqlCommand("[materialIN2]", sqlcon.calc);
            material.CommandType = System.Data.CommandType.StoredProcedure;
            material.Parameters.AddWithValue("@ID", globalVar.MatInID);
            material.Parameters.AddWithValue("@employee", globalVar.name.ToString());
            material.ExecuteNonQuery();
            userConnect.dbOut();
        }
        
        
        private void materialIn_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (circularProgress1.IsRunning == true)
            //{
            //    e.Cancel = true;
            //}
            //else
            //{
            //    e.Cancel = false;
            //}
            //circularProgress1.IsRunning = false;
           
        }
        private void timer1_Tick(object sender, EventArgs e)
        {

            //if (progressBar1.Value != 100)
            //{
            //    circularProgress1.Visible = true;
            //    circularProgress1.IsRunning = true;
            //    progressBar1.Value++;
            //}
            //else
            //{
            //    timer1.Stop();
            //    uploadToDB();
            //    DesktopAlert.Show("The list has been added to purchased materials list.");
            //    Form1 updateNotif = new Form1();
            //    updateNotif.notifDsply();
            //    updateNotif.notifIcon();
            //    textBoxX1.Text = "";
            //    metroTextBox1.Text = "";
            //    tempTableOrderIN();
            //    circularProgress1.Visible = false;
            //    circularProgress1.IsRunning = false;
            //}
        }
        private void metroButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void metroGrid1_Click(object sender, EventArgs e)
        {
            var rowsCount = metroGrid1.SelectedRows.Count;
            if (rowsCount == 0 || rowsCount > 1) return;


            //metroLabel8.Text = metroGrid1.SelectedCells[1].Value.ToString();
        }
        private void metroButton3_Click_1(object sender, EventArgs e)
        {
            //bool.Parse(item.Cells[0].Value.ToString()) == true
            
            try
            {
                foreach (DataGridViewRow item in metroGrid1.Rows)
                {
                    if (bool.Parse(item.Cells[0].Value.ToString()))
                    {
                        sqlcon userConnect = new sqlcon();
                        userConnect.dbIn();
                        SqlCommand material = new SqlCommand("[delTempMaterials]", sqlcon.calc);
                        material.CommandType = System.Data.CommandType.StoredProcedure;
                        material.Parameters.AddWithValue("@id", item.Cells[1].Value.ToString());
                        material.ExecuteNonQuery();
                        userConnect.dbOut();
                    }
                }
            }
            catch 
            {
                return;
            }
            tempTableOrderIN();
                reloadChkBox();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            
            if (checkBox1.CheckState == CheckState.Checked)
            {

                foreach (DataGridViewRow row in metroGrid1.Rows)
                {
                    DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells[0];
                    chk.Value = true;
                }
            }
            else
            {

                //foreach (DataGridViewRow row in metroGrid1.Rows)
                //{
                //    DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells[0];
                //    chk.Value = false;
                    
                //}
                reloadChkBox();
            }
        }
        private void textBoxX1_KeyUp(object sender, KeyEventArgs e)
        {
            textBoxX1.Text = textBoxX1.Text.Replace("\r\n", "");
        }

        private void metroTextBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            decimal x;
            if (ch == (char)Keys.Back)
            {
                e.Handled = false;
            }
            else if (!char.IsDigit(ch) && ch != '.' || !Decimal.TryParse(metroTextBox2.Text + ch, out x))
            {
                e.Handled = true;
            }
        }

        private void metroTextBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (textBoxX1.Text == null || metroTextBox2.Text == "")
                {
                    pictureBox1.Visible = true;
                }
                else
                {
                    //reviewList();
                    //tempTableOrderIN();
                    //DesktopAlert.Show("Raw material has been added!");
                    //metroTextBox2.Text = "";
                }
                //reloadChkBox();
            }
        }

        private void metroTextBox2_TextChanged(object sender, EventArgs e)
        {
            presyo();
            checkinput();
        }
        private void metroGrid1_DataSourceChanged(object sender, EventArgs e)
        {
            disable();
        }
        private void checkinput()
        {
            if (metroTextBox2.Text == null || metroTextBox2.Text == "" || metroTextBox1.Text == null || metroTextBox1.Text == ""
                || textBoxX1.Text == null || textBoxX1.Text == "") { metroButton1.Enabled = false; }
            else { metroButton1.Enabled = true; }
        }

        private void metroGrid1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            try
            {

                var senderGrid = (DataGridView)sender;
                globalVar.MatInID = Convert.ToInt32(metroGrid1.SelectedCells[2].Value);
                if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn)
                {
                    DialogResult dialogResult = MetroMessageBox.Show(this, "Already Recieved ?", "Purchased Material", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        var rowsCount = metroGrid1.SelectedRows.Count;
                        uploadToDB2();
                        DesktopAlert.Show("The list has been added to purchased materials list.");
                        Form1 updateNotif = new Form1();
                        updateNotif.notifDsply();
                        updateNotif.notifIcon();
                        textBoxX1.Text = "";
                        metroTextBox1.Text = "";
                        tempTableOrderIN();
                    }
                }
            }
            catch {}
        }

        private void metroGrid1_RowLeave(object sender, DataGridViewCellEventArgs e)
        {

     
        }

        private void metroGrid1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
           
            //foreach (DataGridViewRow item in metroGrid1.Rows)
            //{
            //    if (bool.Parse(item.Cells[0].Value.ToString())) { chkCNT = 1; }
            //    else { chkCNT = 2; }
            //}
            //if (chkCNT == 1)
            //{
            //    metroButton2.Enabled = true;
            //}
            //else { metroButton2.Enabled = false; }
        }

        private void metroGrid1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            int go = 0;
            try
            {
                bool isChecked = Convert.ToBoolean(metroGrid1.Rows[metroGrid1.CurrentCell.RowIndex].Cells[0].Value.ToString());

                if (isChecked)
                {
                    chkCNT += 1;
                }
                else
                {
                    chkCNT -= 1;
                }

                foreach (DataGridViewRow item in metroGrid1.Rows)
                {
                    if (bool.Parse(item.Cells[0].Value.ToString())) { go = 1; }
                }

                //metroLabel2.Text = "Selected Items: " + chkCNT;
                metroLabel5.Text = go.ToString();
                if (go == 1) { metroButton2.Enabled = true; } else { metroButton2.Enabled = false; }
            }
            catch { }

          
        }

        private void metroGrid1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (metroGrid1.IsCurrentCellDirty)
            {
                metroGrid1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }

        }

        private void textBoxX2_TextChanged(object sender, EventArgs e)
        {
            if (textBoxX2.Text == null || textBoxX2.Text == "") { tempTableOrderIN(); }
            else { searchBox(); }
        }

     

        
    }
}
