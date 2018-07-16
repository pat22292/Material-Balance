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
using System.Text.RegularExpressions;
using MetroFramework;
namespace productionMonitoringSystem06302016
{
    public partial class formulaFrm : MetroForm
    {
        public formulaFrm()
        {
            InitializeComponent();
            displayFormula();
            for (int i = 1; i <= 4; i++)
            {
               metroGrid1.Columns[i].ReadOnly = true;
            }
        }
        private void reloadChkBox()
        {
            foreach (DataGridViewRow row in metroGrid1.Rows)
            {
                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells[0];
                chk.Value = false;
            }
            foreach (DataGridViewRow row in metroGrid1.Rows)
            {
                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells[0];
                chk.Value = false;

                if (row.Cells[5].Value.Equals("Y"))
                {
                    chk.Value = true;
                }
                else if (row.Cells[5].Value.Equals("N"))
                {
                    chk.Value = false;
                }

            }
        }
        private void formulaFrm_Load(object sender, EventArgs e)
        {
            slidePanel1.AnimationTime = 100;
            slidePanel1.IsOpen = false;
            //metroLabel11.Text = globalVar.name;
            groupBox1.Enabled = false;
            displayFormula();
            metroLabel4.Text = globalVar.name;
            autoSuggest();
            if (metroLabel22.Text == "1000")
            {
                groupBox2.Enabled = false;
            }
            reloadChkBox();

 
           
        }
        private void displayFormula() //Display FORMULA list....
        {
            sqlcon userConnect = new sqlcon();
            userConnect.dbIn();
            SqlCommand users = new SqlCommand("[DisplayFG]", sqlcon.calc);
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
        private void metroGrid1_DoubleClick(object sender, EventArgs e)
        {
            groupBox3.Visible = false;
            groupBox2.Enabled = true;
            slidePanel1.BringToFront();
            slidePanel1.AnimationTime = 1000;
            slidePanel1.SlideSide = eSlideSide.Left;
            slidePanel1.IsOpen = true;
            metroGrid1.Enabled = false;
            var rowsCount = metroGrid1.SelectedRows.Count;
            if (rowsCount == 0 || rowsCount > 1) return;
            metroLabel6.Text = metroGrid1.SelectedCells[1].Value.ToString();
            prodcutTxtBoxView();
            displayFormula2();
            formulaTotalCheck();
            if (metroLabel22.Text == "") { metroLabel22.Text = "0"; groupBox2.Enabled = true; }
            slidePanel2.Visible = false;
        }
        private void AddNewProduct()
        {
            sqlcon userConnect = new sqlcon();
            userConnect.dbIn();
            SqlCommand material = new SqlCommand("[addNewTexiProduct]", sqlcon.calc);
            material.CommandType = System.Data.CommandType.StoredProcedure;
            material.Parameters.AddWithValue("@productName", metroTextBox2.Text);
            material.Parameters.AddWithValue("@desc", metroTextBox1.Text);
            material.Parameters.AddWithValue("@price", metroTextBox3.Text);
            material.Parameters.AddWithValue("@employe", metroLabel4.Text);
            material.Parameters.AddWithValue("@Bagging", metroTextBox4.Text);
            material.Parameters.AddWithValue("@userID", globalVar.x.ToString());
            material.ExecuteNonQuery();
            userConnect.dbOut();
        }
        private void metroButton1_Click(object sender, EventArgs e)
        {
            if (metroTextBox1.Text == "" || metroTextBox2.Text == "" || metroTextBox3.Text == "" || metroTextBox4.Text == "")
            {
                MetroMessageBox.Show(this, "", "Please check your inputs!", MessageBoxButtons.OK, MessageBoxIcon.Question);
            }
            else
            {
                AddNewProduct();
                DesktopAlert.Show("New product has been Added!");
                metroTextBox1.Text = "";
                metroTextBox2.Text = "";
                metroTextBox3.Text = "";
                metroTextBox4.Text = "";
                displayFormula();
            }
        }
        private void productExistCheck() //Check for product existence....
        {
            string pName = "";
            sqlcon userConnect = new sqlcon();
            userConnect.dbIn();
            SqlCommand cmd = new SqlCommand("[chkFGProductsDup] '" + metroTextBox2.Text + "'", sqlcon.calc);
            SqlDataReader dr = cmd.ExecuteReader();
            int user = 0;
            while (dr.Read())
            {
                user += 1;
                pName = dr["productName"].ToString();
            }
            userConnect.dbOut();
            if (pName.ToUpper() == metroTextBox2.Text.ToUpper()  &&
                metroTextBox2.Text != string.Empty)
            {
                metroButton1.Enabled = false;
                DesktopAlert.Show("Already Exist");
            }
            else
            {
                metroButton1.Enabled = true;
            }
        }
        private void metroTextBox2_TextChanged(object sender, EventArgs e)
        {
            productExistCheck();
        }
        private void metroTextBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            decimal x;
            if (ch == (char)Keys.Back)
            {
                e.Handled = false;
            }
            else if (!char.IsDigit(ch) && ch != '.' || !Decimal.TryParse(metroTextBox3.Text + ch, out x))
            {
                e.Handled = true;
            }
            if (Regex.IsMatch(metroTextBox3.Text, @"\.\d\d") && e.KeyChar != 8) { e.Handled = true; }
        }
        private void metroTextBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
        private void metroButton2_Click(object sender, EventArgs e)
        {  
            if (metroLabel22.Text == "0" || metroLabel22.Text == "1000")
            {
                slidePanel1.SlideSide = eSlideSide.Right;
                slidePanel1.IsOpen = false;
                metroGrid1.Enabled = true;
                slidePanel2.Visible = true;
                metroLabel19.Text = "";
                metroTextBox12.Text = "";
            }
            else
            {
                MetroMessageBox.Show(this, "", "The formulation is not equal to 1 Ton./ 1000 Kg.", MessageBoxButtons.OK, MessageBoxIcon.Question);
                //groupBox2.Enabled = false;
                switchButton1.Value = false;
            }
        }
        private void prodcutTxtBoxView() //Display User details in textboxes..in modifyTab
        {
            sqlcon userConnect = new sqlcon();
            userConnect.dbIn();
            SqlCommand cmd = new SqlCommand("[productTxtView] '" + metroLabel6.Text + "'", sqlcon.calc);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                //metroTextBox5.Text = dr["productName"].ToString();
                metroTextBox6.Text = dr["description"].ToString();
                metroTextBox7.Text = dr["productPrice"].ToString();
                metroTextBox8.Text = dr["Bagging"].ToString();

            }
            userConnect.dbOut();
        }
        private void displayFormula2() //Display users list....
        {
            sqlcon userConnect = new sqlcon();
            userConnect.dbIn();
            SqlCommand users = new SqlCommand("[filteredFormula] '" + metroLabel6.Text+ "'", sqlcon.calc);
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
        private void switchButton1_ValueChanged(object sender, EventArgs e)
        {
            if (switchButton1.Value == false)
            {
                groupBox1.Enabled = false;
                if (metroLabel22.Text == "1000")
                {
                    groupBox2.Enabled = false;
                }
                else
                {
                    groupBox2.Enabled = true;
                }
                groupBox3.Enabled = true;
            }
            else
            {
                groupBox1.Enabled = true;
                groupBox2.Enabled = false;
                groupBox3.Enabled = false;
            }
        }
        
            
        
        private void metroGrid3_Click(object sender, EventArgs e)
        {
            switchButton1.Value = false;
            groupBox3.Visible = true;
            var rowsCount = metroGrid3.SelectedRows.Count;
            if (rowsCount == 0 || rowsCount > 1) return;
            metroLabel19.Text = metroGrid3.SelectedCells[0].Value.ToString();
            metroTextBox12.Text = metroGrid3.SelectedCells[1].Value.ToString();
        }
        private void metroTextButton2_Click(object sender, EventArgs e)
        {
            if (textBoxX1.Text != "" && metroTextBox11.Text != "") { addMatToRecipe(); }
            else { MetroMessageBox.Show(this, "", "Please check your inputs!", MessageBoxButtons.OK, MessageBoxIcon.Question); }
        }
        private void formulaFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (slidePanel1.IsOpen == true)
            {
                e.Cancel = true;
            }
            else
            {
                e.Cancel = false;
            }
        }
        private void formulaTotalCheck() //Check if the formulation weight is correct....
        {
            sqlcon userConnect = new sqlcon();
            userConnect.dbIn();
            SqlCommand cmd = new SqlCommand("[displayTotalRecipe] '" + metroLabel6.Text + "'", sqlcon.calc);
            SqlDataReader dr = cmd.ExecuteReader();
 
            while (dr.Read())
            {
                        metroLabel22.Text = dr["total"].ToString();
            }
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
        }
        private void textBoxX1_TextChanged_1(object sender, EventArgs e)
        {
           
            if ((textBoxX1.Text.Length) == 1)
            {
                textBoxX1.Text = textBoxX1.Text[0].ToString().ToUpper();
                textBoxX1.Select(2, 1);
            }
            switchButton1.Value = false;
            metroTextButton2.Enabled = true;
            groupBox3.Visible = false;
        }
        private void addMatToRecipe()
        {
            try
            {
                sqlcon userConnect = new sqlcon();
                userConnect.dbIn();
                SqlCommand material = new SqlCommand("[addMatToRecipe]", sqlcon.calc);
                material.CommandType = System.Data.CommandType.StoredProcedure;
                material.Parameters.AddWithValue("@FG", metroLabel6.Text);
                material.Parameters.AddWithValue("@matName", textBoxX1.Text);
                material.Parameters.AddWithValue("@amt", metroTextBox11.Text);
                material.Parameters.AddWithValue("@Emp", metroLabel4.Text);
                material.Parameters.AddWithValue("@userID", globalVar.x.ToString());
                material.ExecuteNonQuery();
                userConnect.dbOut();
                displayFormula2();
                formulaTotalCheck();
                textBoxX1.Text = null; metroTextBox11.Text = null; DesktopAlert.Show("Finish good has been updated!");
            }
            catch
            {
                DesktopAlert.Show("Please check your inputs!");
            }
        }
        private void metroTextBox11_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) { addMatToRecipe(); }
        }
        private void metroTextButton3_Click(object sender, EventArgs e)
        {

            if (Convert.ToInt16(metroTextBox12.Text) <= 0)
            {
                DesktopAlert.Show("Sorry Invalid Amount");
                
            }
            else
            {
                MetroMessageBox.Show(this, "", "Ingredients has been modified.", MessageBoxButtons.OK, MessageBoxIcon.Question);
                modifyFormula();
            }
        }
        private void modifyFormula()
        {
            sqlcon userConnect = new sqlcon();
            userConnect.dbIn();
            SqlCommand material = new SqlCommand("[modifyFormula] '" + metroLabel19.Text + "','" + metroLabel6.Text + "','" + metroTextBox12.Text + "','"+globalVar.name + "'", sqlcon.calc);
            SqlDataReader dr = material.ExecuteReader();
            userConnect.dbOut();
            displayFormula2();
            formulaTotalCheck();
        }
        private void metroTextBox11_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            decimal x;
            if (ch == (char)Keys.Back)
            {
                e.Handled = false;
            }
            else if (!char.IsDigit(ch) && ch != '.' || !Decimal.TryParse(metroTextBox11.Text + ch, out x))
            {
                e.Handled = true;
            }
        }
        private void metroTextBox12_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            decimal x;
            if (ch == (char)Keys.Back)
            {
                e.Handled = false;
            }
            else if (!char.IsDigit(ch) && ch != '.' || !Decimal.TryParse(metroTextBox12.Text + ch, out x))
            {
                e.Handled = true;
            }
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
        private void metroTextBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            decimal x;
            if (ch == (char)Keys.Back)
            {
                e.Handled = false;
            }
            else if (!char.IsDigit(ch) && ch != '.' || !Decimal.TryParse(metroTextBox7.Text + ch, out x))
            {
                e.Handled = true;
            }
        }
        private void metroTextBox12_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) { modifyFormula(); }
        }
        private void metroTextBox6_KeyUp(object sender, KeyEventArgs e)
        {
            metroTextBox6.Text = metroTextBox6.Text.Replace("\r\n", "");
        }
        private void metroTextBox5_KeyUp(object sender, KeyEventArgs e)
        {
            //metroTextBox5.Text = metroTextBox5.Text.Replace("\r\n", "");
        }
        private void metroTextBox2_KeyUp(object sender, KeyEventArgs e)
        {
            metroTextBox2.Text = metroTextBox2.Text.Replace("\r\n", "");
        }
        private void metroTextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            metroTextBox1.Text = metroTextBox1.Text.Replace("\r\n", "");
        }
        private void textBoxX1_KeyUp(object sender, KeyEventArgs e)
        {
            textBoxX1.Text = textBoxX1.Text.Replace("\r\n", "");
        }
        private void matExistCheck() //Check for material existence....
        {
            string matMame = "";
            string formula = "";

            sqlcon userConnect = new sqlcon();

            userConnect.dbIn();
            SqlCommand cmd = new SqlCommand("[CheckMtrlDup_formula] '" + textBoxX1.Text + "','" + metroLabel6.Text + "'", sqlcon.con);
            SqlDataReader dr = cmd.ExecuteReader();
            int user = 0;

            while (dr.Read())
            {
                user += 1;
                matMame = dr["Material"].ToString();
                formula = dr["formulaName"].ToString();
            }
            userConnect.dbOut();
            if (user >= 1)
            {
                metroTextButton2.Enabled = false;
                DesktopAlert.Show("Already Exist");
            }
            else
            {
                metroTextButton2.Enabled = true;
            }

        }
        private void metroTextBox11_Click(object sender, EventArgs e)
        {
            matExistCheck();
        }

        private void metroButton3_Click(object sender, EventArgs e)
        {

            try
            {
                foreach (DataGridViewRow item in metroGrid1.Rows)
                {
                  
                        sqlcon userConnect = new sqlcon();
                        userConnect.dbIn();
                        SqlCommand material = new SqlCommand("[voidFormula]", sqlcon.calc);
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
            displayFormula();
            reloadChkBox();
        }

        private void metroTextButton1_Click(object sender, EventArgs e)
        {
            try
            {
                sqlcon userConnect = new sqlcon();
                userConnect.dbIn();
                SqlCommand material = new SqlCommand("[modifyFormulaDetails]", sqlcon.calc);
                material.CommandType = System.Data.CommandType.StoredProcedure;
                //material.Parameters.AddWithValue("@matName", metroTextBox5.Text);
                material.Parameters.AddWithValue("@DESC", metroTextBox6.Text); 
                material.Parameters.AddWithValue("@amt", metroTextBox7.Text);
                material.Parameters.AddWithValue("@PKG", metroTextBox8.Text);
                material.Parameters.AddWithValue("@user", globalVar.name.ToString());
                material.Parameters.AddWithValue("@fglook", metroLabel6.Text);
                material.ExecuteNonQuery();
                userConnect.dbOut();
                displayFormula();
                DesktopAlert.AutoCloseTimeOut = 3;
                textBoxX1.Text = null; metroTextBox11.Text = null; MetroMessageBox.Show(this, "", "Details has been updated!", MessageBoxButtons.OK, MessageBoxIcon.Question);
            }
            catch
            {
                MetroMessageBox.Show(this, "", "Please check your inputs!", MessageBoxButtons.OK, MessageBoxIcon.Question);
            }
        }

        private void metroTextButton4_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow item in metroGrid1.Rows)
            {

                sqlcon userConnect = new sqlcon();
                userConnect.dbIn();
                SqlCommand material = new SqlCommand("[modifyFormulaDetailsDel]", sqlcon.calc);
                material.CommandType = System.Data.CommandType.StoredProcedure;
                material.Parameters.AddWithValue("@requiredMat", metroLabel19.Text);
                material.Parameters.AddWithValue("@FGname", metroLabel6.Text);
                material.ExecuteNonQuery();
                userConnect.dbOut();
                displayFormula2();
                metroTextBox12.Text = "";
                metroLabel19.Text = "";
            }

        }

        private void metroGrid3_DataSourceChanged(object sender, EventArgs e)
        {
            formulaTotalCheck();
        }

        private void metroTextBox3_Click(object sender, EventArgs e)
        {

        }

        private void metroTextBox12_Click(object sender, EventArgs e)
        {

        }

        private void metroTextBox12_TextChanged(object sender, EventArgs e)
        {
            if (metroTextBox12.Text == "")
            {
                metroTextButton3.Enabled = false;
            }
            else if (metroTextBox12.Text == "0")
            {
                metroTextButton3.Enabled = false;
            }
               else
            {
                metroTextButton3.Enabled = true;
            }
                
            
        }
    }
}
