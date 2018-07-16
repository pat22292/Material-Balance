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
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using MetroFramework;

namespace productionMonitoringSystem06302016
{
    public partial class FinishedGoods : MetroForm
    {
        
        string imgloc = "";
       

        public FinishedGoods()
        {
            InitializeComponent();
           
        }

        private void listOfUser() //provide the client list for combobox
        {
            metroComboBox2.Items.Insert(0, "all");
            metroComboBox2.Items.Add("All");
            sqlcon userConnect = new sqlcon();
            userConnect.dbIn();
            SqlDataAdapter da = new SqlDataAdapter("exec [userList]", sqlcon.con);
            DataTable dt = new DataTable();
            da.Fill(dt);

            dt.Columns.Add("ID", typeof(int));
            metroComboBox2.DisplayMember = "NAMES";
            metroComboBox2.ValueMember = "ID";
            metroComboBox2.DataSource = dt;

            DataRow dr = dt.NewRow();
            dr["NAMES"] = "All";
            dr["ID"] = 0;

            dt.Rows.InsertAt(dr, 0);
            metroComboBox2.SelectedIndex = 0;
            userConnect.dbOut();

        }
        private void listOfProduct()
        {
            sqlcon userConnect = new sqlcon();
            userConnect.dbIn();
            SqlDataAdapter da = new SqlDataAdapter("exec [productList]", sqlcon.con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            metroComboBox1.DataSource = dt;
            //metroComboBox3.DisplayMember = "ClientName";

            dt.Columns.Add("ID", typeof(int));
            metroComboBox1.DisplayMember = "Products";
            metroComboBox1.ValueMember = "ID";
            metroComboBox1.DataSource = dt;

            DataRow dr = dt.NewRow();
            dr["Products"] = "All";
           

            dt.Rows.InsertAt(dr, 0);
            metroComboBox1.SelectedIndex = 0;

            userConnect.dbOut();
        }
        public void listOfClients() //provide the client list for combobox
        {
            sqlcon userConnect = new sqlcon();
            userConnect.dbIn();
            SqlDataAdapter da = new SqlDataAdapter("exec [clientList]", sqlcon.con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            metroComboBox3.DataSource = dt;
            //metroComboBox3.DisplayMember = "ClientName";

            dt.Columns.Add("ID", typeof(int));
            metroComboBox3.DisplayMember = "ClientName";
            metroComboBox3.ValueMember = "ID";
            metroComboBox3.DataSource = dt;

            DataRow dr = dt.NewRow();
            dr["ClientName"] = "All";
            dr["ID"] = 0;

            dt.Rows.InsertAt(dr, 0);
            metroComboBox3.SelectedIndex = 0;

            userConnect.dbOut();
        }
        private void FinishedGoods_Load(object sender, EventArgs e)
        {
           
            slidePanel1.IsOpen = false;
            //finishGoods();
            
            listOfUser();
            listOfProduct();
            listOfClients();

            displayReports();
            addButton();
            displayReports();
        }
        private void displayReports()
        {
            
            string eName = metroComboBox2.Text;
            string CName = metroComboBox3.Text;
            string Pname = metroComboBox1.Text;
            sqlcon userConnect = new sqlcon();
            userConnect.dbIn();
            SqlCommand recipe = new SqlCommand("exec [soldProductsGoods] '" + metroDateTime1.Value + "','" + metroDateTime2.Value + "','" + eName + "','" + CName + "','" + Pname + "'", sqlcon.calc);
            SqlDataAdapter calculated = new SqlDataAdapter();
            calculated.SelectCommand = recipe;
            DataTable dataSet = new DataTable();
            calculated.Fill(dataSet);
            BindingSource nSource = new BindingSource();
            nSource.DataSource = dataSet;
            metroGrid1.DataSource = nSource;
            calculated.Update(dataSet);
            userConnect.dbOut();
           
        }//Heart of the string
        private void addButton()
        {
            DataGridViewButtonXColumn dn = new DataGridViewButtonXColumn();
            metroGrid1.Columns.Add(dn);
            dn.HeaderText = "Delivered?";
            dn.Text = "Upload img";
            dn.Name = "btn";
            dn.UseColumnTextForButtonValue = true;


        }
    
        private void metroDateTime2_ValueChanged(object sender, EventArgs e)
        {
            if (metroDateTime1.Value > metroDateTime2.Value)
            {
                //MessageBox.Show("The date you entered is INVALID !!");
                DesktopAlert.Show("The date you entered is INVALID !!");
            }
            else
            {
                displayReports();
            }
        }

        private void metroTabPage1_Click(object sender, EventArgs e)
        {

        }

        private void metroDateTime1_ValueChanged(object sender, EventArgs e)
        {
            if (metroDateTime1.Value > metroDateTime2.Value)
            {
                //MessageBox.Show("The date you entered is INVALID !!");
                DesktopAlert.Show("The date you entered is INVALID !!");
            }
            else
            {
                displayReports();
            }
        }

        private void metroComboBox2_SelectionChangeCommitted(object sender, EventArgs e)
        {
            displayReports();
        }

        private void metroComboBox3_SelectionChangeCommitted(object sender, EventArgs e)
        {
            displayReports();
        }

        private void metroComboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            displayReports();
        }
        private void uploadToDB2()
        {
            //sqlcon userConnect = new sqlcon();
            //userConnect.dbIn();
            //SqlCommand material = new SqlCommand("[materialIN2]", sqlcon.calc);
            //material.CommandType = System.Data.CommandType.StoredProcedure;
            //material.Parameters.AddWithValue("@employee", globalVar.name.ToString());
            //material.Parameters.AddWithValue("@ID", globalVar.MatInID);
            //material.ExecuteNonQuery();
            //userConnect.dbOut();
        }

        private void metroGrid1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
             var senderGrid = (DataGridView)sender;
            //globalVar.MatInID = Convert.ToInt32(metroGrid1.SelectedCells[4].Value);
            //metroLabel2.Text = metroGrid1.SelectedCells[4].Value.ToString();
             metroLabel6.Text = metroGrid1.SelectedCells[1].Value.ToString();
            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn)
            {
                DialogResult dialogResult = MetroMessageBox.Show(this, " ", "Delivered already?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    metroTabControl1.Enabled = false;
                    slidePanel1.SlideSide = eSlideSide.Left;
                    slidePanel1.IsOpen = true;
                    var rowsCount = metroGrid1.SelectedRows.Count;
                    if (rowsCount == 0 || rowsCount > 1) return;
                    metroLabel6.Text = metroGrid1.SelectedCells[1].Value.ToString();
                    metroLabel8.Text  = metroGrid1.SelectedCells[4].Value.ToString();
                    metroLabel9.Text  = metroGrid1.SelectedCells[5].Value.ToString();
                    metroLabel11.Text  = metroGrid1.SelectedCells[6].Value.ToString();
                    metroLabel15.Text  = metroGrid1.SelectedCells[7].Value.ToString();
                    metroLabel13.Text  = metroGrid1.SelectedCells[8].Value.ToString();
                    metroLabel17.Text = metroGrid1.SelectedCells[9].Value.ToString();
                }
            }
            if (imgloc == "") { metroButton3.Enabled = false; } 
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            slidePanel1.SlideSide = eSlideSide.Right;
            slidePanel1.IsOpen = false;
            metroTabControl1.Enabled = true;
            imgloc = "";
            pictureBox1.Image = null;
           
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            imgloc = "";
            pictureBox1.Image = null;

            try
            {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Filter = "JPG Files (*.jpg)|*.jpg|GIF Files(*.gif)|*.gif|All files(*.*)|*.*";
                dlg.Title = "Select Receiving form for this customer";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    imgloc = dlg.FileName;
                    pictureBox1.ImageLocation = imgloc;
                    textBox1.Text = dlg.FileName;
                }
                if (imgloc != "") { metroButton3.Enabled = true; }
            }

            catch (Exception ex)
            {
                DesktopAlert.Show(ex.Message);
            }
        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            uploadwithImage();
        }
        private void uploadwithImage()
        {

            
            string Con = Properties.Settings.Default.Database.ToString() + ".[dbo].Final_Sold_Goods ";
            string con3 = Con.Replace(" ", "");

            sqlcon userConnect1 = new sqlcon();
            userConnect1.dbIn();
            SqlCommand soldFinishGood = new SqlCommand("[receivedGoods]", sqlcon.calc);
            soldFinishGood.CommandType = System.Data.CommandType.StoredProcedure;
            soldFinishGood.Parameters.AddWithValue("@userID", globalVar.x.ToString());
            soldFinishGood.Parameters.AddWithValue("@emp2", globalVar.name.ToString());
            soldFinishGood.Parameters.AddWithValue("@emp", metroLabel13.Text);
            soldFinishGood.Parameters.AddWithValue("@fGcode", metroLabel6.Text);
            soldFinishGood.ExecuteNonQuery();
            userConnect1.dbOut();

            sqlcon userConnect = new sqlcon();
            userConnect.dbIn();
            //SqlCommand cmd = new SqlCommand("[userIDdetect] '" + metroTextBox1.Text + "'", sqlcon.con);
            SqlCommand cmd = new SqlCommand("UPDATE " + con3.ToString() + " SET [image] = (SELECT BulkColumn FROM Openrowset( Bulk '" + imgloc.ToString() + "', Single_Blob) as img) WHERE FGCode = '" + metroLabel6.Text + "'", sqlcon.calc);
            SqlDataReader dr = cmd.ExecuteReader();
            userConnect.dbOut();
            displayReports();
            slidePanel1.IsOpen = false;
            DesktopAlert.Show("Item has been sold");
            metroTabControl1.Enabled = true;
            pictureBox1.Image = null;
        }

        private void pictureBox1_BackgroundImageChanged(object sender, EventArgs e)
        {
            //try
            //{
            //    sqlcon userConnect = new sqlcon();
            //    userConnect.dbIn();
            //    //SqlCommand cmd = new SqlCommand("[userIDdetect] '" + metroTextBox1.Text + "'", sqlcon.con);
            //    SqlCommand cmd = new SqlCommand("SELECT BulkColumn FROM Openrowset( Bulk '" + imgloc.ToString() + "', Single_Blob) as img", sqlcon.calc);
            //    SqlDataReader dr = cmd.ExecuteReader();
            //    userConnect.dbOut();
            //}

            //catch (Exception Ex)
            //{
            //    DesktopAlert.Show(Ex.ToString());
            //}
        }

        private void pictureBox1_LoadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                sqlcon userConnect = new sqlcon();
                userConnect.dbIn();
                //SqlCommand cmd = new SqlCommand("[userIDdetect] '" + metroTextBox1.Text + "'", sqlcon.con);
                SqlCommand cmd = new SqlCommand("SELECT BulkColumn FROM Openrowset( Bulk '" + imgloc.ToString() + "', Single_Blob) as img", sqlcon.calc);
                SqlDataReader dr = cmd.ExecuteReader();
                userConnect.dbOut();
            }

            catch 
            {
                DesktopAlert.Show("Image Location is invalid!");
                metroButton3.Enabled = false;
            }
        }

    }
}
