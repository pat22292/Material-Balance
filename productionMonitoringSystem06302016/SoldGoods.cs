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
using System.IO;
//using iTextSharp.text;
//using iTextSharp.text.pdf;
//using iTextSharp.text.html.simpleparser;

namespace productionMonitoringSystem06302016
{
    public partial class SoldGoods : DevComponents.DotNetBar.Metro.MetroForm
    {
        public SoldGoods()
        {
            InitializeComponent();

        }

        private void SoldGoods_Load(object sender, EventArgs e)
        {
            string screenWidth = Screen.PrimaryScreen.Bounds.Width.ToString();
            string screenHeight = Screen.PrimaryScreen.Bounds.Height.ToString();
            if (screenWidth == "1920" && screenHeight == "1080") { this.WindowState = FormWindowState.Normal; this.Width = 1366; this.Height = 768; this.CenterToScreen(); }
            allPaid();
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void searchBox() //filter Products list....
        {
            sqlcon userConnect = new sqlcon();
            userConnect.dbIn();
            SqlCommand users = new SqlCommand("[materialDetect] '" + textBoxX1.Text + "'", sqlcon.con);
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
        private void allPaid()
        {
            sqlcon userConnect = new sqlcon();
            userConnect.dbIn();
            SqlCommand recipe = new SqlCommand("exec [paidGoods]", sqlcon.calc);
            SqlDataAdapter calculated = new SqlDataAdapter();
            calculated.SelectCommand = recipe;
            DataTable dataSet = new DataTable();
            calculated.Fill(dataSet);
            BindingSource nSource = new BindingSource();
            nSource.DataSource = dataSet;
            metroGrid1.DataSource = nSource;
            calculated.Update(dataSet);
            userConnect.dbOut();
            //reloadChkBox();
        }

        private void metroGrid1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var rowsCount = metroGrid1.SelectedRows.Count;
            if (rowsCount == 0 || rowsCount > 1) return;
            metroLabel1.Text = metroGrid1.SelectedCells[0].Value.ToString();

            sqlcon userConnect = new sqlcon();
            userConnect.dbIn();
            SqlCommand cmd = new SqlCommand("[imageReceipt] '" + metroLabel1.Text + "'", sqlcon.con);
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {

                byte[] img = (byte[])(dr[0]);
                MemoryStream ms = new MemoryStream(img);
                pictureBox1.Image = Image.FromStream(ms);
            }

            userConnect.dbOut();

        }

        private void metroGrid1_SelectionChanged(object sender, EventArgs e)
        {
            var rowsCount = metroGrid1.SelectedRows.Count;
            if (rowsCount == 0 || rowsCount > 1) return;
            metroLabel1.Text = metroGrid1.SelectedCells[0].Value.ToString();

            sqlcon userConnect = new sqlcon();
            userConnect.dbIn();
            SqlCommand cmd = new SqlCommand("[imageReceipt] '" + metroLabel1.Text + "'", sqlcon.con);
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {

                byte[] img = (byte[])(dr[0]);
                MemoryStream ms = new MemoryStream(img);
                pictureBox1.Image = Image.FromStream(ms);
            }

            userConnect.dbOut();
        }

        private void textBoxX1_TextChanged(object sender, EventArgs e)
        {
            if (textBoxX1.Text == null || textBoxX1.Text == "") { allPaid(); }
            else { searchBox(); }
        }

    }
}
