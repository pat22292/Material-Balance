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

namespace productionMonitoringSystem06302016
{
    public partial class msgBoxFrm : MetroForm
    {
       
        public msgBoxFrm()
        {

            
            InitializeComponent();
    
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //private void metroButton2_Click(object sender, EventArgs e)
        //{
        //    materialOut proc = new materialOut();
           
        //    //materialIn txu = new materialIn();

        //    //if (txu.Enabled == true)
        //    //{
        //    //    txu.reviewList();
        //    //    this.Close();
        //    //}
        //    //else if(proc.Enabled == true)
        //    //{
        //        proc.soldFinishGood();
        //        this.Close();
        //    //}
        //}
    }
}
