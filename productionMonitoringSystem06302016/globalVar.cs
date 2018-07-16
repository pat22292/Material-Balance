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

namespace productionMonitoringSystem06302016
{
    class globalVar
    {
            public static string x = "".ToString();
            public static string pass = "".ToString();
            public static string name = "".ToString();
            public static string position = "".ToString();
            public static string empStatus = "".ToString();
            public static string pinCode = "".ToString();
            public static string admin = "".ToString();
            public static string userID = "".ToString();
            public static string orderID = "".ToString();
            public static int MatInID = 0;
          
            
        }
        public class sqlcon
        {
            public static SqlConnection con = null;
            public static SqlConnection calc = null;
            public void dbIn()
            {
                //con = new SqlConnection("Data Source=PATRICK-2016\\DEV;Initial Catalog=PRODUCTION3;Integrated Security=True");
                //calc = new SqlConnection("Data Source=PATRICK-2016\\DEV;Initial Catalog=PRODUCTION3;Integrated Security=True");
                //////Properties.Settings.Default.Database.ToString();

                con = new SqlConnection("Server = " + Properties.Settings.Default.server.ToString() + ";Database = " + Properties.Settings.Default.Database.ToString() + "; User Id = " + Properties.Settings.Default.userName.ToString() +"; Password = " + Properties.Settings.Default.pw.ToString() + "");

                calc = new SqlConnection("Server = " + Properties.Settings.Default.server.ToString() +
                    "; Database = " + Properties.Settings.Default.Database.ToString() +
                    "; User Id = " + Properties.Settings.Default.userName.ToString() +
                    "; Password = " + Properties.Settings.Default.pw.ToString() + "");
             
                con.Open();
                calc.Open();
            }
            public void dbOut()
            {
                con.Close();
                calc.Close();
            }

    }
       

}
