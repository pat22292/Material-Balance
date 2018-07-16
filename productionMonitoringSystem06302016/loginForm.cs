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
using DevComponents.WinForms;
using DevComponents.DotNetBar.Controls;
using System.Data.SqlClient;
using MetroFramework;
namespace productionMonitoringSystem06302016
{
    public partial class loginForm : DevComponents.DotNetBar.Metro.MetroForm 
    {
        public static int wrongPassword = 1;
        private static int powerSwitch = 0; 
        public loginForm()
        {
            
            InitializeComponent();
            wrongPassword = 1;
        }
        private void loginForm_Load(object sender, EventArgs e)
        {
            
            slideAnimate(1, 800,eSlideSide.Left,false);
            this.Opacity = 1;
            //pictureBox1.Visible = false;
            //slidePanel2.IsOpen = false;
            if (Control.IsKeyLocked(Keys.CapsLock))
            {
                metroLabel1.Text = ("CapsLock is : ON!");
            }
            else
            {
                metroLabel1.Text = (" ");
            }
        }
        private void metroTextButton1_Click(object sender, EventArgs e)

        {

            pictureBox2.Enabled = false;
            logIN();
            
           
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            //if (progressBar1.Value != 100)
            //{
            //    pictureBox2.Visible = false;
            //    slidePanel1.SlideSide = eSlideSide.Right;
            //    slidePanel1.IsOpen = false;
            //    metroProgressSpinner1.Visible = true;
            //    progressBar1.Value += 5;
            //    metroTextButton2.Visible = false;
            //}
            //else
            //{
            //        wrongPassword = 1;
            //        this.Hide();
            //        timer1.Stop();
            //        Form1 nfrm = new Form1();
            //        nfrm.ShowDialog();
            //        globalVar.pass = "";
            //        powerSwitch = 0;
            //}
        }
        private void slideAnimate(int x, int y,eSlideSide side, Boolean a) 
        {
            slidePanel1.AnimationTime = x;
            slidePanel1.IsOpen = a;
            slidePanel1.SlideSide = side;
            slidePanel1.AnimationTime = y;
            slidePanel1.IsOpen = true;
            
        }
        private void logIN()
        {
            int user = 0;
            try
            {
                sqlcon userConnect = new sqlcon();
                userConnect.dbIn();
                //SqlCommand cmd = new SqlCommand("[userIDdetect] '" + metroTextBox1.Text + "'", sqlcon.con);
                SqlCommand cmd = new SqlCommand("[userIDdetect] '" + metroTextBox1.Text + "'", sqlcon.con);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    user += 1;
                    globalVar.name = dr["Name"].ToString();
                    globalVar.x = metroTextBox1.Text;
                    globalVar.pass = dr["passkey"].ToString();
                    globalVar.position = dr["position"].ToString();
                    globalVar.empStatus = dr["Employeestatus"].ToString();
                    globalVar.admin = dr["isAdmin"].ToString();
                    
                }
                userConnect.dbOut();
                if (user != 1)
                {
                    labelX1.Text = "Please enter a valid User ID";
                    labelX3.Text = "";
                    metroTextBox2.Text = "";
                     wrongPassword = 1;
           
                }
                else if ( user == 1 && globalVar.empStatus == "Deactivated")
                {
                    metroTextBox1.Text = "";
                    lockedUser lUsr = new lockedUser();
                    lUsr.ShowDialog();
                }
                else
                {
                    labelX1.Text = "Proceed";
                    if (metroTextBox2.Text != string.Empty)
                    {
                        if (metroTextBox2.Text == globalVar.pass && globalVar.empStatus != "Deactivated")
                        {
                            
                            
                            //globalVar.name = labelX1.Text.ToString();
                            sqlcon userOUT = new sqlcon();
                            userOUT.dbIn();
                            SqlCommand logIN = new SqlCommand("[timeLogIN] '" + globalVar.x.ToString() + "','" + globalVar.name.ToString() + "'", sqlcon.con);
                            SqlDataReader LI = logIN.ExecuteReader();
                            while (LI.Read())
                            {
                            }
                            userOUT.dbOut();
                            wrongPassword = 1;
                            this.Hide();
                            timer1.Stop();
                            Form1 nfrm = new Form1();
                            nfrm.ShowDialog();
                            globalVar.pass = "";
                            powerSwitch = 0;
                        }
                        else
                        {
                            pictureBox1.Visible = true;
                            //MessageBox.Show("Sorry" + " " + labelX1.Text);

                            if (wrongPassword == 5 && metroTextBox1.Text != "AGRI0001")
                            {
                                lockUser();
                                lockedUser lUsr = new lockedUser();
                                lUsr.ShowDialog();
                                wrongPassword = 1;
                            }
                            else
                            {
                                wrongPassword += 1;
                                metroTextBox2.Text = "";
                                globalVar.pass = "";
                            }
                        }
                    }
                    else
                    {
                    }
                }
            }
            catch
            {
                labelX3.ForeColor = Color.Red;
                labelX3.Text = "Database Unavailable...";
               
            }
        }
        private void metroTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (metroTextBox1.Text == "AGRI0001" && powerSwitch == 0) { metroTextBox2.Enabled = false; metroTextButton1.Enabled = false;
            DesktopAlert.AutoCloseTimeOut = 2; MetroMessageBox.Show(this, "You are not allowed to use this account!", "Acces Denied!", MessageBoxButtons.OK, MessageBoxIcon.Stop); DesktopAlert.AlertAnimationDuration = 100;
            }
            else { metroTextButton1.Enabled = true; metroTextBox2.Enabled = true; logIN(); }
            

        }
        private void timer2_Tick(object sender, EventArgs e)
        {
        }
            
        private void metroTextBox2_KeyDown(object sender, KeyEventArgs e)
        {
                if (e.KeyCode == Keys.Enter)
                {
                    if (metroTextBox1.Text == ""){e.SuppressKeyPress = true;DesktopAlert.Show("Please enter your User ID!");}
                    else{logIN();}
                }
                
            if (Control.IsKeyLocked(Keys.CapsLock)){metroLabel1.Text = ("CapsLock is : ON!");}
                else{metroLabel1.Text = (" ");}
        }
        private void metroTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter){e.SuppressKeyPress = true;metroTextBox2.Focus();}
            if (Control.IsKeyLocked(Keys.CapsLock)){metroLabel1.Text = ("CapsLock is : ON!");}
            else{metroLabel1.Text = (" ");}
        }
        private void loginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
           

        }
        private void metroTextButton2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void metroTextBox2_Click(object sender, EventArgs e)
        {
            pictureBox1.Visible = false;
        }
        private void lockUser()
        {
            Random r = new Random();
            sqlcon userConnect = new sqlcon();
            userConnect.dbIn();
            SqlCommand usrLock = new SqlCommand("[resetPasswords]", sqlcon.con);
            usrLock.CommandType = System.Data.CommandType.StoredProcedure;
            usrLock.Parameters.AddWithValue("@USERid", globalVar.x);
            usrLock.Parameters.AddWithValue("@pincode", (r.Next(1000, 9999).ToString()));
            usrLock.ExecuteNonQuery();
            userConnect.dbOut();
            metroTextBox1.Text = "";
            metroTextBox2.Text = "";
            pictureBox1.Visible = false;
        }
        private void loginForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (Control.IsKeyLocked(Keys.CapsLock)){metroLabel1.Text = ("CapsLock is : ON!");}
            else{metroLabel1.Text = (" ");}
        }
        private void metroTextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            metroTextBox1.Text = metroTextBox1.Text.Replace("\r\n", "");
            metroTextBox1.Text = metroTextBox1.Text.Replace("'", "");
            metroTextBox1.Text = metroTextBox1.Text.Replace(" ", "");
            //if (metroTextBox1.Text == "AGRI0001") { metroTextBox2.Enabled = false; metroTextButton1.Enabled = false; }
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            sqlCon Bc = new sqlCon();
            Bc.ShowDialog();
        }
        private void metroTextBox2_KeyUp(object sender, KeyEventArgs e)
        {
            metroTextBox2.Text = metroTextBox2.Text.Replace("\r\n", "");
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.Alt | Keys.Enter))
            {
                powerSwitch = 1;
                //DesktopAlert.Show("You can now access the SysAdmin!");
                pictureBox2.Visible = true;
                labelX1.Text = "Admin";
                metroTextButton1.Enabled = true;
                metroTextBox2.Enabled = true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void metroTextBox1_Click(object sender, EventArgs e)
        {
            metroTextBox2.Text = "";
        }

        private void loginForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void labelX2_Click(object sender, EventArgs e)
        {

        }

        private void labelX2_Click_1(object sender, EventArgs e)
        {

        }
    }
}
