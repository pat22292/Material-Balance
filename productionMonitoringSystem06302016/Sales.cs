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
using DevComponents.DotNetBar;
using DevComponents.WinForms;
using System.Windows.Forms.DataVisualization.Charting;
using DevComponents.DotNetBar.Charts;


namespace productionMonitoringSystem06302016
{
    public partial class Sales : DevComponents.DotNetBar.Metro.MetroForm
    {
        private static double  T1 = 0.00;
        private static double T2 = 0.00;

       

        public Sales()
        {
            InitializeComponent();


            
            //grandSalesReport();
            metroDateTime1.Value = metroDateTime1.MinDate;
            //labelX3.Text = ("Date from : " + metroDateTime1.Value.ToLongDateString() + " to " + metroDateTime2.Value.ToLongDateString());
            //metroLabel20.Text = metroLabel6.Text;
            //metroLabel19.Text = metroLabel3.Text;
            //groupBox1.Text = "As of " + metroDateTime2.Value.ToLongDateString();

            todayEvent();
        }
        private void purchasesGrandTotal() //Display purchases grandTotal
        {
            sqlcon userConnect = new sqlcon();
            userConnect.dbIn();
            SqlCommand cmd1 = new SqlCommand("[purchasesGrandTotal] '" + metroDateTime1.Value + "','" + metroDateTime2.Value + "'", sqlcon.calc);
            SqlDataReader dr1 = cmd1.ExecuteReader();
            while (dr1.Read())
            {
                metroLabel25.Text = dr1["gTotal"].ToString();
                if (dr1["T2"] == null) { T2 = 0; }
                else { T2 = Convert.ToDouble(dr1["T2"]); }
            }
            userConnect.dbOut();
        }
        private void salesGrandTotal() //Display sales grandTotal
        {

            sqlcon userConnect = new sqlcon();
            userConnect.dbIn();
            SqlCommand cmd2 = new SqlCommand("[salesGrandTotal] '" + metroDateTime1.Value + "','" + metroDateTime2.Value + "'", sqlcon.calc);
            SqlDataReader dr2 = cmd2.ExecuteReader();
            while (dr2.Read())
            {
                metroLabel26.Text = dr2["grandTotal"].ToString();
                if (dr2["T1"] == null) { T1 = 0; }
                else { T1 = Convert.ToDouble(dr2["T1"]); }
                
            }
            userConnect.dbOut();
        }
        private void grandSalesReport() //Display OverAll 
        {
            //sqlcon userConnect = new sqlcon();
            //userConnect.dbIn();
            //SqlCommand cmd2 = new SqlCommand("[salesNet]", sqlcon.calc);
            //SqlDataReader dr2 = cmd2.ExecuteReader();
            //while (dr2.Read())
            //{
            //    metroLabel22.Text = dr2["grandTotal"].ToString();
            //}
            //userConnect.dbOut();
        }
        private void SalesChart(Chart chart, string sp) //Charts
        {
            
            //chart.ChartAreas[0].AxisY.LabelStyle.Enabled = false;
            sqlcon userConnect = new sqlcon();
            string sqlStr = sp + " '" + metroDateTime1.Value + "','" + metroDateTime2.Value + "'";
            SqlDataAdapter myCommand = new SqlDataAdapter(sqlStr, sqlcon.calc);
            DataSet ds = new DataSet();
            myCommand.Fill(ds);
            DataView source = new DataView(ds.Tables[0]);
            chart.DataSource = source;
            chart.Series[0].XValueMember = "Material Name";
            chart.Series[0].YValueMembers = "Total Price";
            chart.DataBind();
            chart.Series[0]["PieLabelStyle"] = "Disabled";
            
        }

        private void metroDateTime1_ValueChanged(object sender, EventArgs e)
        {
            labelX3.Text = ("Date from : " + metroDateTime1.Value.ToLongDateString() + " to " + metroDateTime2.Value.ToLongDateString());
            salesGrandTotal();
            purchasesGrandTotal();
            computeNet();
            SalesChart(chart2, "[diplayChartMaterialOut]");
            SalesChart(chart1, "[diplayChartPurchases]");
            SalesChart(chart3, "[diplayChartTopClients]");
            SalesChart(chart5, "[diplayChartTopProducts]");
            SalesChart(chart4, "[diplayChartTopSellers]");
            SalesGraph();
            //SalesChart(chart6, "[diplayChartMonthSales]");   
        }
        private void metroDateTime2_ValueChanged(object sender, EventArgs e)
        {
            labelX3.Text = ("Date from : " + metroDateTime1.Value.ToLongDateString() + " to " + metroDateTime2.Value.ToLongDateString());
            groupBox5.Text = "As of " + metroDateTime2.Value.ToLongDateString();
            salesGrandTotal();
            purchasesGrandTotal();
            computeNet();
            SalesChart(chart2, "[diplayChartMaterialOut]");
            SalesChart(chart1, "[diplayChartPurchases]");
            SalesChart(chart3, "[diplayChartTopClients]");
            SalesChart(chart5, "[diplayChartTopProducts]");
            SalesChart(chart4, "[diplayChartTopSellers]");
            SalesGraph();
            label1.Text = metroDateTime2.Value.Year.ToString();
            //SalesChart(chart6, "[diplayChartMonthSales]");
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void chart6_GetToolTipText(object sender, ToolTipEventArgs e)
        {
           
            // Check selected chart element and set tooltip text for it
            switch (e.HitTestResult.ChartElementType)

            {
                case ChartElementType.DataPoint:
                    var dataPoint = e.HitTestResult.Series.Points[e.HitTestResult.PointIndex];
                    e.Text = string.Format("Month:\t{0}\nSales: Php {1}", dataPoint.AxisLabel, dataPoint.YValues[0].ToString("N2"));
                    break;
            }
        }

        private void SalesGraph()
        {
           
            sqlcon userConnect = new sqlcon();
            string sqlStr = "[diplayChartMonthSales] '" + metroDateTime1.Value + "','" + metroDateTime2.Value + "'";
            SqlDataAdapter myCommand = new SqlDataAdapter(sqlStr, sqlcon.calc);
            DataSet ds = new DataSet();
            myCommand.Fill(ds);
            DataView source = new DataView(ds.Tables[0]);
            chart6.DataSource = source;
            chart6.Series[0].XValueMember = "Material Name";
            chart6.ChartAreas[0].AxisY.LabelStyle.Format = "₱ {0,00}";
            string x = Convert.ToString("Total Price");
            chart6.Series[0].YValueMembers = x;
            chart6.DataBind();
            chart6.Series[0]["PieLabelStyle"] = "Disabled";
        }
        private void computeNet()
        {
            double Sum = T1 - T2;
            try { metroLabel22.Text = "Php. " + Sum.ToString("N2"); }
            catch { T1 = 0; T2 = 0; }
        }

        private void Sales_Load(object sender, EventArgs e)
        {


            string screenWidth = Screen.PrimaryScreen.Bounds.Width.ToString();
            string screenHeight = Screen.PrimaryScreen.Bounds.Height.ToString();

            if (screenWidth == "1920" && screenHeight == "1080") { this.WindowState = FormWindowState.Normal; this.Width = 1366; this.Height = 768; this.CenterToScreen(); }

            label1.Text = metroDateTime2.Value.Year.ToString();
        }

        private void chart1_GetToolTipText(object sender, ToolTipEventArgs e)
        {
            switch (e.HitTestResult.ChartElementType)
            {
                case ChartElementType.DataPoint:
                    var dataPoint = e.HitTestResult.Series.Points[e.HitTestResult.PointIndex];
                    e.Text = string.Format("Material:\t{0}\nSales: Php {1}", dataPoint.AxisLabel, dataPoint.YValues[0].ToString("N2"));
                    break;
            }
        }

        private void chart2_GetToolTipText(object sender, ToolTipEventArgs e)
        {
            switch (e.HitTestResult.ChartElementType)
            {
                case ChartElementType.DataPoint:
                    var dataPoint = e.HitTestResult.Series.Points[e.HitTestResult.PointIndex];
                    e.Text = string.Format("Material:\t{0}\nSales: Php {1}", dataPoint.AxisLabel, dataPoint.YValues[0].ToString("N2"));
                    break;
            }
        }

        private void chart3_GetToolTipText(object sender, ToolTipEventArgs e)
        {
            switch (e.HitTestResult.ChartElementType)
            {
                case ChartElementType.DataPoint:
                    var dataPoint = e.HitTestResult.Series.Points[e.HitTestResult.PointIndex];
                    e.Text = string.Format("Material:\t{0}\nSales: Php {1}", dataPoint.AxisLabel, dataPoint.YValues[0].ToString("N2"));
                    break;
            }
        }

        private void chart4_GetToolTipText(object sender, ToolTipEventArgs e)
        {
            switch (e.HitTestResult.ChartElementType)
            {
                case ChartElementType.DataPoint:
                    var dataPoint = e.HitTestResult.Series.Points[e.HitTestResult.PointIndex];
                    e.Text = string.Format("Material:\t{0}\nSales: Php {1}", dataPoint.AxisLabel, dataPoint.YValues[0].ToString("N2"));
                    break;
            }
        }

        private void chart5_GetToolTipText(object sender, ToolTipEventArgs e)
        {
            switch (e.HitTestResult.ChartElementType)
            {
                case ChartElementType.DataPoint:
                    var dataPoint = e.HitTestResult.Series.Points[e.HitTestResult.PointIndex];
                    e.Text = string.Format("Material:\t{0}\nSales: Php {1}", dataPoint.AxisLabel, dataPoint.YValues[0].ToString("N2"));
                    break;
            }
        }

        private void metroLabel7_Click(object sender, EventArgs e)
        {

        }

        private void todayEvent()
        {
            sqlcon userConnect = new sqlcon();
            userConnect.dbIn();
            SqlCommand cmd = new SqlCommand("[todaysEvent]", sqlcon.con);
            SqlDataReader dr = cmd.ExecuteReader();
         

            while (dr.Read())
            {
                metroLabel20.Text = "Php. " + dr["sales"].ToString();
                metroLabel19.Text = "Php. " +  dr["Purchases"].ToString();
                metroLabel16.Text = "Php. " + dr["equals"].ToString();
            }
            userConnect.dbOut();

            sqlcon userConnect2 = new sqlcon();
            userConnect2.dbIn();
            SqlCommand cmd2 = new SqlCommand("[weekEvent]", sqlcon.con);
            SqlDataReader dr2 = cmd2.ExecuteReader();

           
            while (dr2.Read())
            {
                metroLabel6.Text = "Php. " + dr2["sales"].ToString();
                metroLabel3.Text = "Php. " + dr2["Purchases"].ToString();
                metroLabel7.Text = "Php. " + dr2["equals"].ToString();
            }
            userConnect2.dbOut();
           
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

    }
}
