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
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;

namespace productionMonitoringSystem06302016
{
    public partial class Form1 : DevComponents.DotNetBar.Metro.MetroForm
    {
        int tileCount = 12;
        int time = 1;
        public Form1()
        {

            InitializeComponent();
            disablebtton();
            slidePanel3.IsOpen = false;
            tileController();
            if (tileCount < 8 && metroTileItem32.Visible != true) { metroTilePanel1.ItemSpacing = 80; itemContainer3.LayoutOrientation = eOrientation.Vertical; }
            else if (tileCount < 8 && metroTileItem38.Visible != true && metroTileItem40.Visible != true) { metroTilePanel1.ItemSpacing = 120; }
            string screenWidth = Screen.PrimaryScreen.Bounds.Width.ToString();
            string screenHeight = Screen.PrimaryScreen.Bounds.Height.ToString();
            if (screenWidth == "1366" && screenHeight == "768") { slidePanel2.Height = 768; }
            else if (screenWidth == "1920" && screenHeight == "1080") { slidePanel2.Height = 1080; metroTilePanel1.Width = 2000; metroTilePanel1.Height = 450; }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            disablebtton();
            //label1.Text = Properties.Settings.Default.Database.ToString() + " " + Properties.Settings.Default.userName.ToString() + " " + Properties.Settings.Default.pw.ToString();
            globalVar.pass = "";
            slidePanel1.IsOpen = false;
            analogClockControl1.AutomaticMode = true;
            labelX2.Text = ("Hi, " + globalVar.name + ", " + globalVar.position);
            this.Opacity = 0.1;
            timer1.Start();
            listOfFormulas();
            clientNAme();
            slidePanel2.IsOpen = false;
            notifIcon();
            notifDsply();
            label2.Text = "Materials State as of" + "\r\n" + (DateTime.Now.ToLongDateString());
            clientID();



        }
        public void notifIcon()
        {
            foreach (DataGridViewRow row in metroGrid2.Rows)
            {
                if (Convert.ToString(row.Cells["Status"].Value) == "INSUFFICIENT!" || Convert.ToString(row.Cells["Status"].Value) == "CAUTION!" || Convert.ToString(row.Cells["Status"].Value) == "CRITICAL!")
                {

                    metroTileItem48.NotificationMarkText = "!";
                    row.DefaultCellStyle.ForeColor = Color.Red;
                    if (row.Selected)
                    {
                        row.DefaultCellStyle.SelectionBackColor = Color.Yellow;
                        row.DefaultCellStyle.SelectionForeColor = Color.Red;
                    }
                }

            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (this.Opacity <= 1.0)
            {
                this.Opacity += 0.5;
            }
            else
            {
                timer1.Stop();
            }
        }
        private void analogClockControl1_ValueChanged(object sender, EventArgs e)
        {
            labelX3.Text = (DateTime.Now.ToLongDateString() + " at " + DateTime.Now.ToLongTimeString());
            //metroLabel3.Text = DateTime.Now.ToLongTimeString();
            //notifDsply();
            time += 1;
            if (time == 10)
            {
                notifDsply();
                time = 1;
            }
            notifIcon();
            if (metroGrid2.Rows.Count == 0)
            {
                metroTileItem48.NotificationMarkText = null;
                metroTileItem48.Visible = false;
            }
            else
            {
                metroTileItem48.Visible = true;
            }
        }
        private void metroTileItem31_Click(object sender, EventArgs e)
        {
            texiUsers tU = new texiUsers();
            //tU.tabSelect(0);
            tU.ShowDialog();

        }
        private void metroTileItem32_Click(object sender, EventArgs e)
        {
            slidePanel3.SlideSide = eSlideSide.Bottom;
            slidePanel3.IsOpen = true;
            slidePanel3.BringToFront();
        }
        private void metroTileItem38_Click(object sender, EventArgs e)
        {
            materialIn mI = new materialIn();
            mI.ShowDialog();
        }
        private void metroTileItem40_Click(object sender, EventArgs e)
        {
            materialOut mO = new materialOut();
            mO.ShowDialog();
        }
        private void metroTileItem29_Click(object sender, EventArgs e)
        {
            reportForm rF = new reportForm();
            rF.ShowDialog();
        }
        private void metroTileItem49_Click(object sender, EventArgs e)
        {
            slidePanel1.SlideSide = eSlideSide.Left;
            slidePanel1.BringToFront();
            slidePanel1.IsOpen = true;
        }
        private void metroTile1_Click(object sender, EventArgs e)
        {
            slidePanel1.SlideSide = eSlideSide.Right;
            slidePanel1.IsOpen = false;
            metroTextBox1.Text = "";
        }
        private void metroTileItem44_Click(object sender, EventArgs e)
        {
            Sales sF = new Sales();
            sF.ShowDialog();
        }
        private void metroTileItem42_Click(object sender, EventArgs e)
        {
            matrlStock mS = new matrlStock();
            mS.ShowDialog();
            //metroTileItem42.NotificationMarkText = "!";
        }
        private void metroTileItem33_Click(object sender, EventArgs e)
        {
            formulaFrm fF = new formulaFrm();
            fF.ShowDialog();
        }
        private void calc()//Create a quotation of a particullar Finish Product/Formula
        {
            sqlcon userConnect = new sqlcon();
            userConnect.dbIn();
            SqlCommand recipe = new SqlCommand("recipeCalculator '" + metroComboBox1.Text + "','" + metroTextBox1.Text + "'", sqlcon.calc);
            SqlDataAdapter calculated = new SqlDataAdapter();
            calculated.SelectCommand = recipe;
            DataTable dataSet = new DataTable();

            DataColumn col1 = new DataColumn();

            col1.ColumnName = "No.";
            col1.AutoIncrement = true;
            col1.AutoIncrementSeed = 1;
            col1.AutoIncrementStep = 1;


            dataSet.Columns.Add(col1);

            col1.SetOrdinal(0);

            for (int i = 0; i < dataSet.Rows.Count; i++)
            {
                dataSet.Rows[i]["No."] = i + 1;

            }

            calculated.Fill(dataSet);
            BindingSource nSource = new BindingSource();
            nSource.DataSource = dataSet;
            metroGrid1.DataSource = nSource;
            calculated.Update(dataSet);
            userConnect.dbOut();
            this.metroGrid1.Columns["No."].Width = 20;
        }
        private void listOfFormulas() //provide the material list for combobox
        {
            sqlcon userConnect = new sqlcon();
            userConnect.dbIn();
            SqlDataAdapter da = new SqlDataAdapter("exec [listOfFormulas]", sqlcon.calc);
            DataTable dt = new DataTable();
            da.Fill(dt);
            metroComboBox1.DataSource = dt;
            metroComboBox1.DisplayMember = "FormulaName";
            userConnect.dbOut();
        }
        private void clientNAme()
        {
            sqlcon userConnect = new sqlcon();
            userConnect.dbIn();
            SqlDataAdapter da = new SqlDataAdapter("exec [clientList]", sqlcon.calc);
            DataTable dt = new DataTable();
            da.Fill(dt);
            metroComboBox2.DataSource = dt;
            metroComboBox2.DisplayMember = "ClientName";
            userConnect.dbOut();
        }
        private void metroTextBox1_TextChanged(object sender, EventArgs e)
        {
            calc();
            presyo();
            computePrice();
            noOfBagsandTOtal();
            disablebtton();
        }
        private void metroComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            calc();
            presyo();
        }
        private void metroGrid1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            foreach (DataGridViewRow row in metroGrid1.Rows)
            {
                if (Convert.ToString(row.Cells["Result"].Value) == "INSUFFICIENT")
                {
                    row.DefaultCellStyle.ForeColor = Color.Red;
                    if (row.Selected)
                    {
                        row.DefaultCellStyle.SelectionBackColor = Color.Yellow;
                        row.DefaultCellStyle.SelectionForeColor = Color.Red;
                    }
                }
                else
                {
                    row.DefaultCellStyle.ForeColor = Color.Black;
                }
            }
        }
        private void metroTileItem41_Click(object sender, EventArgs e)
        {
            materialMasterList mml = new materialMasterList();
            mml.ShowDialog();
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
        private void metroTileItem46_Click(object sender, EventArgs e)
        {
            LOmessagaForm mB = new LOmessagaForm();
            mB.ShowDialog();
            this.FormClosing -= new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Hide();
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }
        //private void metroLabel3_TextChanged(object sender, EventArgs e)
        //{
        //    string time = "";
        //    time = metroTextBox3.Text + ":" + metroTextBox2.Text + ":00 " + metroComboBox2.Text;
        //    metroLabel4.Text = time;
        //    if (metroLabel3.Text == metroLabel4.Text)
        //    {
        //        metroTileItem46.RaiseClick();
        //    }
        //}

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            notifDsply();
            slidePanel2.IsOpen = true;
            slidePanel2.BringToFront();

        }
        private void Form1_MouseHover(object sender, EventArgs e)
        {
            slidePanel2.IsOpen = false;
        }
        private void metroTilePanel1_MouseHover(object sender, EventArgs e)
        {
            slidePanel2.IsOpen = false;

        }
        public void notifDsply()
        {
            sqlcon userConnect = new sqlcon();
            userConnect.dbIn();
            SqlCommand users = new SqlCommand("exec [notifDisplay]", sqlcon.con);
            SqlDataAdapter sqlDA = new SqlDataAdapter();
            sqlDA.SelectCommand = users;
            DataTable dataSet = new DataTable();
            sqlDA.Fill(dataSet);
            BindingSource nSource = new BindingSource();
            nSource.DataSource = dataSet;
            metroGrid2.DataSource = nSource;
            sqlDA.Update(dataSet);
            userConnect.dbOut();
        }
        private void metroGrid2_CellFormatting_1(object sender, DataGridViewCellFormattingEventArgs e)
        {
            notifIcon();
        }
        private void metroTextButton1_Click(object sender, EventArgs e)
        {


            try
            {
                SaveFileDialog savefiledialog1 = new SaveFileDialog();
                savefiledialog1.FileName = "Raw material request " + (DateTime.Now.ToLongDateString());
                savefiledialog1.Filter = "PDF Files|*.pdf";


                if (savefiledialog1.ShowDialog() == DialogResult.OK)
                {

                    {
                        Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 42, 35);

                        PdfWriter wri = PdfWriter.GetInstance(doc, new FileStream(savefiledialog1.FileName, FileMode.Create));
                        wri.PageEvent = new PdfWriterEvents("T E X I C O N");
                        doc.Open();
                        iTextSharp.text.Image PNG = iTextSharp.text.Image.GetInstance("TexiconLogo.png");
                        PNG.ScaleAbsolute(250, 125);
                        PNG.SetAbsolutePosition(175, 600);
                        PNG.SpacingAfter = 30f;
                        doc.Add(PNG);
                        Paragraph para3 = new Paragraph("RAW MATERIALS REQUEST");
                        para3.SpacingBefore = 130f;
                        para3.SpacingAfter = .5f;
                        para3.Alignment = Element.ALIGN_CENTER;
                        para3.Font.Size = 13;
                        doc.Add(para3);

                        Paragraph para9 = new Paragraph("( " + metroComboBox1.Text + " )");
                        para9.SpacingBefore = 3f;
                        para9.SpacingAfter = .5f;
                        para9.Font.Size = 11;
                        para9.Alignment = Element.ALIGN_CENTER;
                        doc.Add(para9);

                        PdfPTable table = new PdfPTable(3);
                        table.SpacingBefore = 20f;
                        for (int j = 0; j < 3; j++)
                        {
                            PdfPCell cell = new PdfPCell(new Phrase(metroGrid1.Columns[j].HeaderText, FontFactory.GetFont("Times New Roman", 12f, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                            cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.ColorTranslator.FromHtml("#C4C4C4"));
                            cell.HorizontalAlignment = 1;
                            table.TotalWidth = 340f;
                            table.LockedWidth = true;
                            table.AddCell(cell);
                        }
                        table.HeaderRows = 1;
                        for (int i = 0; i < metroGrid1.Rows.Count; i++)
                        {
                            for (int k = 0; k < 3; k++)
                            {
                                PdfPCell cell2 = new PdfPCell(new Phrase(metroGrid1[k, i].Value.ToString(), FontFactory.GetFont("Times New Roman", 10f, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));

                                if (i % 2 != 0)
                                {
                                    cell2.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.ColorTranslator.FromHtml("#f0f0f5"));
                                }

                                cell2.HorizontalAlignment = 1;
                                table.TotalWidth = 340f;
                                table.LockedWidth = true;
                                table.AddCell(cell2);
                            }


                        }
                        doc.Add(table);
                        Paragraph items = new Paragraph("Total of Items: " + metroGrid1.RowCount.ToString() + "                                           Total Price :" + (metroLabel6.Text));
                        items.Alignment = Element.ALIGN_LEFT;
                        items.IndentationLeft = 133f;
                        items.Font.Size = 10;
                        items.SpacingBefore = .5f;
                        doc.Add(items);
                        Paragraph para8 = new Paragraph("Packaging in Kg. :" + (metroLabel7.Text));
                        para8.SpacingBefore = 5f;
                        para8.IndentationLeft = 133f;
                        para8.Alignment = Element.ALIGN_LEFT;
                        para8.Font.Size = 10;
                        doc.Add(para8);
                        Paragraph para6 = new Paragraph("Theoretical output in bags :" + (metroLabel10.Text));
                        para6.SpacingBefore = 5f;
                        para6.IndentationLeft = 133f;
                        para6.Font.Size = 10;
                        para6.Alignment = Element.ALIGN_LEFT;
                        doc.Add(para6);

                        doc.Close();
                        AddPageNumber(savefiledialog1.FileName, savefiledialog1.FileName);
                        System.Diagnostics.Process.Start(savefiledialog1.FileName);
                    }
                }
            }
            catch
            {
                DesktopAlert.Show("Invalid or the file is open!");
            }

        }
        private void disablebtton()
        {
            if (metroTextBox1.Text == "" || metroTextBox1.Text == null || Convert.ToInt32(metroTextBox1.Text) < 0)
            { metroTextButton1.Enabled = false; metroTextButton2.Enabled = false; metroTextButton4.Enabled = false; }
            else
            { metroTextButton1.Enabled = true; metroTextButton2.Enabled = true; metroTextButton4.Enabled = true; }

        }
        private void AddPageNumber(string fileIn, string fileOut)
        {
            byte[] bytes = File.ReadAllBytes(fileIn);

            using (MemoryStream stream = new MemoryStream())
            {
                PdfReader reader = new PdfReader(bytes);
                using (PdfStamper stamper = new PdfStamper(reader, stream))
                {
                    int pages = reader.NumberOfPages;
                    for (int i = 1; i <= pages; i++)
                    {
                        ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_MIDDLE, new Phrase("     Requested by: " + globalVar.name.ToString() + "            Received By:__________________________________________                         Date:____/___/_____", FontFactory.GetFont("Times New Roman", 9f, iTextSharp.text.Font.ITALIC, BaseColor.BLACK)), 5f, 70f, 0);
                        ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_MIDDLE, new Phrase("                             " + globalVar.position.ToString() + "                                                               Signature over printed name ", FontFactory.GetFont("Times New Roman", 9f, iTextSharp.text.Font.ITALIC, BaseColor.BLACK)), 5f, 60f, 0);
                        ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_MIDDLE, new Phrase("     Generated by: " + globalVar.name.ToString() + "                                           " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToShortTimeString() + "                                      " + "Page " + i.ToString() + " of " + pages, FontFactory.GetFont("Times New Roman", 9f, iTextSharp.text.Font.ITALIC, BaseColor.BLACK)), 5f, 15f, 0);
                    }
                }
                bytes = stream.ToArray();
            }

            File.WriteAllBytes(fileOut, bytes);
        }

        private void metroTextButton2_Click(object sender, EventArgs e)
        {
            if (metroTextBox1.Text == null || metroTextBox1.Text == "")
            {
                DesktopAlert.Show("No Item hass been inputed!");
            }
            else
            {

                uploadTOmaterialTemp();
                DesktopAlert.Show("Raw materials has been added!");
                metroTextBox1.Text = "";
                slidePanel1.IsOpen = false;
                materialIn matIn = new materialIn();
                matIn.ShowDialog();
            }

        }
        private void uploadTOmaterialTemp()
        {
            sqlcon userConnect = new sqlcon();
            userConnect.dbIn();
            SqlCommand material = new SqlCommand("[materialINBunch]", sqlcon.calc);
            material.CommandType = System.Data.CommandType.StoredProcedure;
            material.Parameters.AddWithValue("@amount", metroTextBox1.Text);
            material.Parameters.AddWithValue("@employeeNAme", globalVar.name.ToString());
            material.Parameters.AddWithValue("@recipe", metroComboBox1.Text);
            material.Parameters.AddWithValue("@userID", globalVar.x.ToString());
            material.ExecuteNonQuery();
            userConnect.dbOut();
        }
        private void metroTileItem37_Click(object sender, EventArgs e)
        {
            clientFrm uF = new clientFrm();
            uF.ShowDialog();
        }
        private void presyo() //Display price in metrotab3
        {
            sqlcon userConnect = new sqlcon();
            userConnect.dbIn();
            SqlCommand cmd = new SqlCommand("[amountOfDelivery] '" + metroTextBox1.Text + "','" + metroComboBox1.Text + "'", sqlcon.calc);
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                metroLabel5.Text = dr["Price"].ToString();
                metroLabel7.Text = dr["Bagging"].ToString();
            }
            userConnect.dbOut();
        }
        private void computePrice() //Display price in metrotab11
        {

            try
            {
                sqlcon userConnect = new sqlcon();
                userConnect.dbIn();
                SqlCommand cmd = new SqlCommand("exec [totalPriceFg] '" + metroTextBox1.Text + "','" + metroLabel5.Text + "','"
                + metroComboBox1.Text + "','" + metroLabel7.Text + "'", sqlcon.calc);
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    metroLabel6.Text = dr["Presyo"].ToString();
                }
                userConnect.dbOut();
            }
            catch
            {
                DesktopAlert.Show("No Item yet!");
            }

        }
        private void tileController()
        {
            string T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14 = "";

            sqlcon userConnect = new sqlcon();
            userConnect.dbIn();
            SqlCommand cmd = new SqlCommand("[tileManager] '" + globalVar.position.ToString() + "'", sqlcon.con);
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                T1 = dr["Addusertile"].ToString();
                T2 = dr["UserRoletile"].ToString();
                T3 = dr["ClientTile"].ToString();
                T4 = dr["ReportTile"].ToString();
                T5 = dr["MMLTile"].ToString();
                T6 = dr["SalesTile"].ToString();
                T7 = dr["materialStocksTile"].ToString();
                T8 = dr["FormulaTile"].ToString();
                T9 = dr["QuotationTile"].ToString();
                T10 = dr["expensesTile"].ToString();
                T11 = dr["GoodSoldsTile"].ToString();
                T12 = dr["BNRTile"].ToString();
                T13 = dr["PRODUCTION"].ToString();
                T14 = dr["FGOODS"].ToString();


                if (T1 == "0") { metroTileItem31.Visible = false; tileCount -= 1; }
                if (T2 == "0") { metroTileItem32.Visible = false; tileCount -= 1; }
                if (T3 == "0") { metroTileItem37.Visible = false; tileCount -= 1; }
                if (T4 == "0") { metroTileItem29.Visible = false; tileCount -= 1; }
                if (T5 == "0") { metroTileItem41.Visible = false; tileCount -= 1; }
                if (T6 == "0") { metroTileItem44.Visible = false; tileCount -= 1; }
                if (T7 == "0") { metroTileItem42.Visible = false; tileCount -= 1; }
                if (T8 == "0") { metroTileItem33.Visible = false; tileCount -= 1; }
                if (T9 == "0") { metroTileItem49.Visible = false; tileCount -= 1; }
                if (T10 == "0") { metroTileItem38.Visible = false; tileCount -= 1; }
                if (T11 == "0") { metroTileItem40.Visible = false; tileCount -= 1; }
                if (T12 == "0") { metroTileItem43.Visible = false; tileCount -= 1; }
                if (T13 == "0") { metroTileItem47.Visible = false; tileCount -= 1; }
                if (T14 == "0") { metroTileItem45.Visible = false; tileCount -= 1; }

            }
            userConnect.dbOut();
        }

        private void resetIcon()
        {
            if (metroGrid2.Rows[0] == null)
            {
                metroTileItem48.NotificationMarkText = null;

            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            slidePanel3.SlideSide = eSlideSide.Bottom;
            slidePanel3.IsOpen = true;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            slidePanel3.SlideSide = eSlideSide.Bottom;
            slidePanel3.IsOpen = false;
        }

        private void metroTextButton3_Click(object sender, EventArgs e)
        {
            changePass();

        }
        private void metroTextBox6_TextChanged(object sender, EventArgs e)
        {
            searchUser();

            if (metroTextBox5.Text == metroTextBox6.Text && metroTextBox4.Text != "" && metroTextBox4.Text != null
                 && metroTextBox5.Text != "" && metroTextBox5.Text != null && metroTextBox4.Text == globalVar.pinCode)
            {
                metroTextButton3.Enabled = true;
            }
            else { metroTextButton3.Enabled = false; }
        }

        private void metroTextBox5_TextChanged(object sender, EventArgs e)
        {
            searchUser();
            if (metroTextBox5.Text == metroTextBox6.Text && metroTextBox4.Text != "" && metroTextBox4.Text != null
                && metroTextBox5.Text != "" && metroTextBox5.Text != null && metroTextBox4.Text == globalVar.pinCode)
            {
                metroTextButton3.Enabled = true;
            }
            else { metroTextButton3.Enabled = false; }
        }

        private void metroTextBox4_TextChanged(object sender, EventArgs e)
        {
            searchUser();
            if (metroTextBox5.Text == metroTextBox6.Text && metroTextBox4.Text != "" && metroTextBox4.Text != null
                 && metroTextBox5.Text != "" && metroTextBox5.Text != null && metroTextBox4.Text == globalVar.pinCode)
            {
                metroTextButton3.Enabled = true;
            }
            else { metroTextButton3.Enabled = false; }
        }
        private void changePass()
        {
            sqlcon userConnect = new sqlcon();
            userConnect.dbIn();
            SqlCommand modify = new SqlCommand("[ChangePW]", sqlcon.con);
            modify.CommandType = System.Data.CommandType.StoredProcedure;
            modify.Parameters.AddWithValue("@passkey", metroTextBox4.Text);
            modify.Parameters.AddWithValue("@newpass", metroTextBox5.Text);
            modify.Parameters.AddWithValue("@userID", globalVar.x.ToString());
            modify.Parameters.AddWithValue("@name", globalVar.name.ToString());
            modify.ExecuteNonQuery();
            userConnect.dbOut();
            DesktopAlert.Show("Password has been changed, please Log IN");
            this.FormClosing -= new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Close();
            loginForm lF = new loginForm();
            lF.ShowDialog();
        }
        private void searchUser() //detect Pincode
        {
            sqlcon userConnect = new sqlcon();
            userConnect.dbIn();
            SqlCommand cmd = new SqlCommand("[findUser] '" + globalVar.x + "'", sqlcon.con);
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                globalVar.pinCode = dr["passkey"].ToString();
            }
            userConnect.dbOut();
        }

        //private void metroTileItem48_Click(object sender, EventArgs e)
        //{
        //    slidePanel2.IsOpen = true;
        //    slidePanel2.BringToFront();
        //}

        private void metroTileItem45_Click(object sender, EventArgs e)
        {
            FinishedGoods fg = new FinishedGoods();
            fg.ShowDialog();

        }
        private void noOfBagsandTOtal() //Display no.OfSacks in metrotab3
        {
            sqlcon userConnect = new sqlcon();
            userConnect.dbIn();
            SqlCommand cmd = new SqlCommand("[totalOfAmountsandBags] '" + metroTextBox1.Text + "','" + metroComboBox1.Text + "','" + metroLabel7.Text + "'", sqlcon.calc);
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                metroLabel10.Text = dr["noOfBags"].ToString();
            }
            userConnect.dbOut();
        }

        private void metroTileItem48_Click(object sender, EventArgs e)
        {
            slidePanel2.IsOpen = true;
            slidePanel2.BringToFront();
        }

        private void metroTileItem47_Click(object sender, EventArgs e)
        {
            SoldGoods sG = new SoldGoods();
            sG.ShowDialog();

        }

        private void metroTileItem43_Click(object sender, EventArgs e)
        {
            BackUpandRestore bNR = new BackUpandRestore();
            bNR.ShowDialog();
        }

        private void metroGrid1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void metroTextBox1_Click(object sender, EventArgs e)
        {

        }

        private void metroTextButton4_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog savefiledialog1 = new SaveFileDialog();
                savefiledialog1.FileName = "Client Quotation " + (DateTime.Now.ToLongDateString());
                savefiledialog1.Filter = "PDF Files|*.pdf";


                if (savefiledialog1.ShowDialog() == DialogResult.OK)
                {

                    {
                        Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 42, 35);

                        PdfWriter wri = PdfWriter.GetInstance(doc, new FileStream(savefiledialog1.FileName, FileMode.Create));
                        wri.PageEvent = new PdfWriterEvents("T E X I C O N");
                        doc.Open();
                        iTextSharp.text.Image PNG = iTextSharp.text.Image.GetInstance("TexiconLogo.png");
                        PNG.ScaleAbsolute(250, 125);
                        PNG.SetAbsolutePosition(175, 600);
                        PNG.SpacingAfter = 70f;
                        doc.Add(PNG);
                        Paragraph para3 = new Paragraph("Client ID No: " +metroLabel12.Text +"                                     " + DateTime.Today.ToLongDateString());
                        para3.SpacingBefore = 130f;
                        para3.SpacingAfter = .5f;
                      para3.IndentationLeft = 120f;
                        para3.Alignment = Element.ALIGN_LEFT;
                        para3.Font.Size = 11;
                        doc.Add(para3);
                        Paragraph para9 = new Paragraph("Client Name: " + metroComboBox2.Text);
                        para9.SpacingBefore = 3f;
                        para9.SpacingAfter = .5f;
                        para9.IndentationLeft = 120f;
                        para9.Font.Size = 11;
                        para9.Alignment = Element.ALIGN_LEFT;
                        doc.Add(para9);
                        Paragraph para1 = new Paragraph("Quotation for");
                        para1.SpacingBefore = 10f;
                        para1.SpacingAfter = .5f;
                        para1.Alignment = Element.ALIGN_CENTER;
                        para1.Font.Size = 11;
                        doc.Add(para1);
                        Paragraph para2 = new Paragraph(metroComboBox1.Text);
                        para2.SpacingBefore = .5f;
                        para2.SpacingAfter = 20f;
                        para2.Alignment = Element.ALIGN_CENTER;
                        para2.Font.Size = 11;
                        doc.Add(para2);
                        PdfPTable table = new PdfPTable(4);
                        PdfPCell cell = new PdfPCell(new Phrase("Price", FontFactory.GetFont("Times New Roman", 10f, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                        cell.HorizontalAlignment = 1;
                        cell.Colspan = 2;
                        cell.Padding = 5;
                        PdfPCell cell1 = new PdfPCell(new Phrase(metroLabel6.Text, FontFactory.GetFont("Times New Roman", 10f, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                        cell1.HorizontalAlignment = 1;
                        cell1.Colspan = 2;
                        cell1.Padding = 5;
                        PdfPCell cell2 = new PdfPCell(new Phrase("Quantity (Ton/s)", FontFactory.GetFont("Times New Roman", 10f, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                        cell2.HorizontalAlignment = 1;
                        cell2.Colspan = 2;
                        cell2.Padding = 5;
                        PdfPCell cell3 = new PdfPCell(new Phrase(metroTextBox1.Text, FontFactory.GetFont("Times New Roman", 10f, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                        cell3.HorizontalAlignment = 1;
                        cell3.Colspan = 2;
                        cell3.Padding = 5;
                        PdfPCell cell4 = new PdfPCell(new Phrase("Kilogram per Bag", FontFactory.GetFont("Times New Roman", 10f, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                        cell4.HorizontalAlignment = 1;
                        cell4.Colspan = 2;
                        cell4.Padding = 5;
                        PdfPCell cell5 = new PdfPCell(new Phrase("50", FontFactory.GetFont("Times New Roman", 10f, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                        cell5.HorizontalAlignment = 1;
                        cell5.Colspan = 2;
                        cell5.Padding = 5;
                        PdfPCell cell6 = new PdfPCell(new Phrase("Theoretrical Output In Bags", FontFactory.GetFont("Times New Roman", 10f, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                        cell6.HorizontalAlignment = 1;
                        cell6.Colspan = 2;
                        cell6.Padding = 5;
                        PdfPCell cell7 = new PdfPCell(new Phrase(metroLabel10.Text, FontFactory.GetFont("Times New Roman", 10f, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                        cell7.HorizontalAlignment = 1;
                        cell7.Colspan = 2;
                        cell7.Padding = 5;
                       
                        table.AddCell(cell2);
                        table.AddCell(cell3);
                        table.AddCell(cell4);
                        table.AddCell(cell5);
                        table.AddCell(cell6);
                        table.AddCell(cell7);
                        table.AddCell(cell);
                        table.AddCell(cell1);
                        doc.Add(table);
                        Paragraph nothing = new Paragraph("********************************* NOTHING FOLLOWS *********************************");
                        nothing.Alignment = Element.ALIGN_CENTER;
                        nothing.SpacingBefore = .5f;
                        doc.Add(nothing);
                     
                        doc.Close();
                        AddPageNumber(savefiledialog1.FileName, savefiledialog1.FileName);
                        System.Diagnostics.Process.Start(savefiledialog1.FileName);
                    }
                }
            }
            catch
            {
                DesktopAlert.Show("Invalid or the file is open!");
            }

        }

   
        private void metroComboBox2_SelectionChangeCommitted(object sender, EventArgs e)
        {
            clientID();
        }

        private void clientID()
        {
            sqlcon userConnect = new sqlcon();
            userConnect.dbIn();
            SqlCommand cmd = new SqlCommand("[searchCLIENT_ID] '" + metroComboBox2.Text + "'", sqlcon.calc);
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                metroLabel12.Text = dr["clientID"].ToString();
            }
            userConnect.dbOut();
        }
    }
}


        
  

