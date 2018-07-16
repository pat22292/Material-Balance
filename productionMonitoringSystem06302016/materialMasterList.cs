using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using MetroFramework.Forms;
using DevComponents.WinForms;
using DevComponents.DotNetBar.Controls;
using System.Data.SqlClient;
using MetroFramework;

namespace productionMonitoringSystem06302016
{
    public partial class materialMasterList : MetroForm
    {
        string count = "";
        
        public materialMasterList()
        {
            InitializeComponent();
            displayMaterialView();
            slidePanel1.IsOpen = false;
            for (int i = 1; i <= 5; i++)
            {
                metroGrid2.Columns[i].ReadOnly = true;
            }
        }
        private void materialID()
        {
            //string text = metroTextBox3.Text.ToUpper();
            //string firstLetters = "";

            //foreach (var part in text.Split(' '))
            //{
            //    firstLetters += part.Substring(0, 1);
            //}

            String sDate = DateTime.Now.ToString();
            DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));
            String yy = datevalue.Year.ToString();
            materialCount();
            int cnt = cnt = Convert.ToInt32(count);
            metroLabel6.Text = "MTRL" + yy + "-000" + (cnt + 1);
        }
        private void realoadChkBox()
        {
            foreach (DataGridViewRow row in metroGrid2.Rows)
            {
                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells[0];
                chk.Value = false;
            }
            foreach (DataGridViewRow row in metroGrid2.Rows)
            {
                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells[0];
                chk.Value = false;

                if (row.Cells[7].Value.Equals("Y"))
                {
                    chk.Value = true;
                }
                else if (row.Cells[7].Value.Equals("N"))
                {
                    chk.Value = false;
                }

            }
        }
        private void metroTextBox3_Leave(object sender, EventArgs e)
        {
            
            if (metroTextBox3.Text == "")
            {
                metroTextBox3.Text = "";
                metroLabel6.Text = "";
            }
            else
            {
                materialID();
                matDuplicateCheck();
            }
        }
        private void materialCount() //Display price in metrotab3
        {
          
            sqlcon userConnect = new sqlcon();
            userConnect.dbIn();
            SqlCommand cmd = new SqlCommand("[countsOfMaterials]", sqlcon.calc);
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                count = dr["count"].ToString();
            }
            userConnect.dbOut();
        }
        private void displayMaterialView()
        {
            sqlcon userConnect = new sqlcon();
            userConnect.dbIn();
            SqlCommand recipe = new SqlCommand("[listOfMaterials]", sqlcon.calc);
            SqlDataAdapter calculated = new SqlDataAdapter();
            calculated.SelectCommand = recipe;
            DataTable dataSet = new DataTable();
            calculated.Fill(dataSet);
            BindingSource nSource = new BindingSource();
            nSource.DataSource = dataSet;
            metroGrid2.DataSource = nSource;
            calculated.Update(dataSet);
            userConnect.dbOut();
        }
        private void addNewMaterial()
        {
            try
            {
                sqlcon userConnect = new sqlcon();
                userConnect.dbIn();
                SqlCommand newMat = new SqlCommand("[addItemMMML]", sqlcon.calc);
                newMat.CommandType = System.Data.CommandType.StoredProcedure;
                newMat.Parameters.AddWithValue("@matID", metroLabel6.Text);
                newMat.Parameters.AddWithValue("@matName", metroTextBox3.Text);
                newMat.Parameters.AddWithValue("@matDesc", metroTextBox4.Text);
                newMat.Parameters.AddWithValue("@minStock", metroTextBox5.Text);
                newMat.Parameters.AddWithValue("@employe", globalVar.name.ToString());
                newMat.Parameters.AddWithValue("@price", metroTextBox1.Text);
                newMat.Parameters.AddWithValue("@userID", globalVar.x.ToString());
                newMat.ExecuteNonQuery();
                userConnect.dbOut();
                DesktopAlert.Show("New material has been added.");
                Form1 updateList = new Form1();
                updateList.notifDsply();
                updateList.notifIcon();
                metroTextBox1.Text = "";
                metroTextBox3.Text = "";
                metroTextBox4.Text = "";
                metroTextBox5.Text = "";
                metroLabel6.Text = "";
                displayMaterialView();
            }
            catch 
            {
                DesktopAlert.Show("Please check your Inputs!");
            }
        }
        private void metroButton1_Click(object sender, EventArgs e)
        {
            if (metroTextBox1.Text == "" || metroTextBox3.Text == "" || metroTextBox4.Text == "" || metroTextBox5.Text == "")
            {
                MetroMessageBox.Show(this, "", "Please Check Your Inputs", MessageBoxButtons.OK, MessageBoxIcon.Question);
            }
            else
            {

                addNewMaterial();
            }
        }

        private void metroTextBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            decimal x;
            if (ch == (char)Keys.Back)
            {
                e.Handled = false;
            }
            else if (!char.IsDigit(ch) && ch != '.' || !Decimal.TryParse(metroTextBox5.Text + ch, out x))
            {
                e.Handled = true;
            }
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

        private void metroTextBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
           
            char ch = e.KeyChar;
            if (ch.Equals('\n'))
            {
                e.Handled = true;
                
            }
        }

        private void metroTextBox3_KeyUp(object sender, KeyEventArgs e)
        {
            metroTextBox3.Text = metroTextBox3.Text.Replace("\r\n", "");
          
        }

        private void metroGrid2_DoubleClick(object sender, EventArgs e)
        {
            slidePanel1.BringToFront();
            slidePanel1.AnimationTime = 1000;
            slidePanel1.SlideSide = eSlideSide.Left;
            slidePanel1.IsOpen = true;
            metroGrid2.Enabled = false;
            groupBox1.Enabled = false;
            var rowsCount = metroGrid2.SelectedRows.Count;
            if (rowsCount == 0 || rowsCount > 1) return;
            metroLabel8.Text = metroGrid2.SelectedCells[1].Value.ToString();
            //searchBox();
            matDetails();
            materialMasterList mml = new materialMasterList();
            
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            slidePanel1.SlideSide = eSlideSide.Right;
            slidePanel1.IsOpen = false;
            metroGrid2.Enabled = true;
            groupBox1.Enabled = true;
        }
        private void matDetails() //Display material details in textboxes..in slidepanel
        {
            sqlcon userConnect = new sqlcon();
            userConnect.dbIn();
            SqlCommand cmd = new SqlCommand("[materialInfo] '" + metroLabel8.Text + "'", sqlcon.con);
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                metroLabel14.Text = dr["MaterialName"].ToString();
                metroTextBox7.Text = dr["MaterialDescription"].ToString();
                metroTextBox8.Text = dr["MinimumAmount"].ToString();
                metroTextBox6.Text = dr["pricePerKilo"].ToString();
            }
            userConnect.dbOut();
        }
  

        private void metroTextBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (ch.Equals('\n'))
            {
                e.Handled = true;
            }
        }
        private void modifyAcct()
        {
            sqlcon userConnect = new sqlcon();
            userConnect.dbIn();
            SqlCommand modify = new SqlCommand("[modifyMaterial]", sqlcon.con);
            modify.CommandType = System.Data.CommandType.StoredProcedure;
            modify.Parameters.AddWithValue("@matName", metroLabel14.Text);
            modify.Parameters.AddWithValue("@descp", metroTextBox7.Text);
            modify.Parameters.AddWithValue("@minAmount", metroTextBox8.Text);
            modify.Parameters.AddWithValue("@price", metroTextBox6.Text);
            modify.Parameters.AddWithValue("@emp", globalVar.name.ToString());
            modify.Parameters.AddWithValue("@matlID", metroLabel8.Text);
            modify.ExecuteNonQuery();
            userConnect.dbOut();
            MetroMessageBox.Show(this, "", "Material has been modified!", MessageBoxButtons.OK, MessageBoxIcon.Question);
            displayMaterialView();
        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
           
            modifyAcct();
        }

        private void metroTextBox4_KeyUp(object sender, KeyEventArgs e)
        {
            metroTextBox4.Text = metroTextBox4.Text.Replace("\r\n", " ");
        }

        private void metroTextBox7_KeyUp(object sender, KeyEventArgs e)
        {
            metroTextBox7.Text = metroTextBox3.Text.Replace("\r\n", " ");
        }

        private void metroTextBox3_TextChanged(object sender, EventArgs e)
        {
  
        }

        private void matDuplicateCheck()
        {

            string fName = "";

            sqlcon userConnect = new sqlcon();

            userConnect.dbIn();
            SqlCommand cmd = new SqlCommand("[CheckMtrlDup_MML] '" + metroTextBox3.Text + "'", sqlcon.con);
            SqlDataReader dr = cmd.ExecuteReader();
            int user = 0;

            while (dr.Read())
            {
                user += 1;
                fName = dr["MaterialName"].ToString();
            }
            userConnect.dbOut();
            if (fName.ToUpper() == metroTextBox3.Text.ToUpper())
            {

                metroButton1.Enabled = false;
                DesktopAlert.Show("Material already Exist");
                metroLabel6.Text = "";

            }
            else
            {
                metroButton1.Enabled = true;
            }
        }

        private void materialMasterList_Load(object sender, EventArgs e)
        {
            realoadChkBox();
            foreach (DataGridViewRow row in metroGrid2.Rows)
            {
                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells[0];
                chk.Value = false;
            }
            realoadChkBox();
        }

        private void metroButton4_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (DataGridViewRow item in metroGrid2.Rows)
                {

                    sqlcon userConnect = new sqlcon();
                    userConnect.dbIn();
                    SqlCommand material = new SqlCommand("[voidRawMat]", sqlcon.calc);
                    material.CommandType = System.Data.CommandType.StoredProcedure;
                    material.Parameters.AddWithValue("@name", item.Cells[1].Value.ToString());
                    if (bool.Parse(item.Cells[0].Value.ToString()))
                    {
                        material.Parameters.AddWithValue("@void", "Y");
                    }
                    else
                    {
                        material.Parameters.AddWithValue("@void", "N");
                    }
                    material.ExecuteNonQuery();
                    userConnect.dbOut();
                   
                }
            }
            catch
            {
                return;
            }
            if (textBoxX1.Text == "" || textBoxX1.Text == null) { displayMaterialView(); }
            else { searchBox(); }
            realoadChkBox();
           
        }

        private void textBoxX1_TextChanged(object sender, EventArgs e)
        {
            if (textBoxX1.Text == null || textBoxX1.Text == "")
            {
                displayMaterialView();
            }

            else { searchBox(); }
            realoadChkBox();
        }

        private void searchBox() //filter users list....
        {
            sqlcon userConnect = new sqlcon();
            userConnect.dbIn();
            SqlCommand users = new SqlCommand("[searchMat] '" + textBoxX1.Text + "'", sqlcon.con);
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

        private void metroTextBox8_Click(object sender, EventArgs e)
        {

        }
        private void dissablebuttons()
        {
            if (metroTextBox6.Text == "" || metroTextBox8.Text == "")
            {
                metroButton3.Enabled = false;
            }
            else
                {
                    metroButton3.Enabled = true;
                }
            
        }

        private void metroButton3_TextChanged(object sender, EventArgs e)
        {
          
        }

        private void metroTextBox8_TextChanged(object sender, EventArgs e)
        {
            dissablebuttons();
        }

        private void metroTextBox6_TextChanged(object sender, EventArgs e)
        {
            dissablebuttons();
        }

        private void metroTextBox8_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            decimal x;
            if (ch == (char)Keys.Back)
            {
                e.Handled = false;
            }
            else if (!char.IsDigit(ch) && ch != '.' || !Decimal.TryParse(metroTextBox8.Text + ch, out x))
            {
                e.Handled = true;
            }
        }

        private void metroTextBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            decimal x;
            if (ch == (char)Keys.Back)
            {
                e.Handled = false;
            }
            else if (!char.IsDigit(ch) && ch != '.' || !Decimal.TryParse(metroTextBox8.Text + ch, out x))
            {
                e.Handled = true;
            }
        }

        private void metroTextBox8_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

     
    }
}