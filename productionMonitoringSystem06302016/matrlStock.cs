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
namespace productionMonitoringSystem06302016
{
    public partial class matrlStock : MetroForm
    {
        public matrlStock()
        {
            InitializeComponent();
        }
        private void stocksComputation()//display stocks
        {
            sqlcon userConnect = new sqlcon();
            userConnect.dbIn();
            SqlCommand stocks = new SqlCommand("[STOCKONHAND]", sqlcon.calc);
            SqlDataAdapter stcks = new SqlDataAdapter();
            stcks.SelectCommand = stocks;
            DataTable dataSet = new DataTable();
            stcks.Fill(dataSet);
            BindingSource nSource = new BindingSource();
            nSource.DataSource = dataSet;
            metroGrid3.DataSource = nSource;
            stcks.Update(dataSet);


            SqlCommand totalIN = new SqlCommand("[totaOfOrderIN]", sqlcon.calc);
            SqlDataAdapter ttlIN = new SqlDataAdapter();
            ttlIN.SelectCommand = totalIN;
            DataTable dataSet2 = new DataTable();
            ttlIN.Fill(dataSet2);
            BindingSource nSource2 = new BindingSource();
            nSource2.DataSource = dataSet2;
            metroGrid1.DataSource = nSource2;
            ttlIN.Update(dataSet2);

            SqlCommand totalOut = new SqlCommand("[totaOfOrderOUT]", sqlcon.calc);
            SqlDataAdapter ttlOut = new SqlDataAdapter();
            ttlOut.SelectCommand = totalOut;
            DataTable dataSet3 = new DataTable();
            ttlOut.Fill(dataSet3);
            BindingSource nSource3 = new BindingSource();
            nSource3.DataSource = dataSet3;
            metroGrid2.DataSource = nSource3;
            ttlOut.Update(dataSet3);
            userConnect.dbOut();
        }

        private void matrlStock_Load(object sender, EventArgs e)
        {
            stocksComputation();
        }
    }
}
