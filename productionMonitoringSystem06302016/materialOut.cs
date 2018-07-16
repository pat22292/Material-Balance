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
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using DevComponents.WinForms;
using DevComponents.DotNetBar.Controls;
using MetroFramework;



namespace productionMonitoringSystem06302016
{
    public partial class materialOut : DevComponents.DotNetBar.Metro.MetroForm
    {
        string count = "";
        int chkCNT = 0;

        public materialOut()
        {
            InitializeComponent();
            listOfFormulas();
            listOfClients();
            metroLabel10.Text = "Created by: "+globalVar.name;
            recentSolds();
            //addButton();
            reloadChkBox();
            productionCount();
            for (int i = 1; i <= 8; i++)
            {
                metroGrid2.Columns[i].ReadOnly = true;
            }

            
        }
        public void soldFinishGood()
        {
            try
            {
                foreach (DataGridViewRow item in metroGrid2.Rows)
                {
                    if (bool.Parse(item.Cells[0].Value.ToString()))
                    {

                        sqlcon userConnect = new sqlcon();
                        userConnect.dbIn();
                        SqlCommand soldFinishGood = new SqlCommand("[materialOut]", sqlcon.calc);
                        soldFinishGood.CommandType = System.Data.CommandType.StoredProcedure;
                        soldFinishGood.Parameters.AddWithValue("@userID", item.Cells[5].Value.ToString());
                        soldFinishGood.Parameters.AddWithValue("@fGcode", item.Cells[1].Value.ToString());
                        soldFinishGood.Parameters.AddWithValue("@addBy", globalVar.name.ToString());
                        soldFinishGood.ExecuteNonQuery();
                        userConnect.dbOut();
                        metroTextBox1.Text = "";
                       
                    }
                }
            }
            catch
            {
                return;
            }
            recentSolds();
            reloadChkBox();
        }

        //private void addButton()
        //{
        //    DataGridViewButtonXColumn dn = new DataGridViewButtonXColumn();
        //    metroGrid2.Columns.Add(dn);
        //    dn.HeaderText = "Done";
        //    dn.Text = "Done";
        //    dn.Name = "btn";
        //    dn.UseColumnTextForButtonValue = true;


        //}
        private void listOfFormulas() //provide the material list for combobox
        {
            sqlcon userConnect = new sqlcon();
            userConnect.dbIn();
            SqlDataAdapter da = new SqlDataAdapter("exec [listOfFormulas]", sqlcon.calc);
            DataTable dt = new DataTable();
            da.Fill(dt);
            metroComboBox2.DataSource = dt;
            metroComboBox2.DisplayMember = "FormulaName";
            userConnect.dbOut();
        }
        public void listOfClients() //provide the client list for combobox
        {
            sqlcon userConnect = new sqlcon();
            userConnect.dbIn();
            SqlDataAdapter da = new SqlDataAdapter("exec [clientList]", sqlcon.con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            metroComboBox1.DataSource = dt;
            metroComboBox1.DisplayMember = "ClientName";
            userConnect.dbOut();
        }
        private void calc()//Create a quotation of a particullar Finish Product/Formula
        {
            sqlcon userConnect = new sqlcon();
            userConnect.dbIn();
            SqlCommand recipe = new SqlCommand("[consumableMaterials] '" + metroTextBox1.Text + "','" + metroComboBox2.Text + "'", sqlcon.calc);
            SqlDataAdapter calculated = new SqlDataAdapter();
            calculated.SelectCommand = recipe;
            DataTable dataSet = new DataTable();
            calculated.Fill(dataSet);
            BindingSource nSource = new BindingSource();
            nSource.DataSource = dataSet;
            metroGrid1.DataSource = nSource;
            calculated.Update(dataSet);
            userConnect.dbOut();
        }
        private void metroTextBox1_TextChanged(object sender, EventArgs e)
        {
            calc();
            presyo();
            noOfBagsandTOtal();
            if (metroTextBox1.Text == "")
            {
                metroTextBox1.Text = "";
                metroTile3.Enabled= false;
            }
            else
            {
                metroTile3.Enabled = true;
                computePrice();
            }

            foreach (DataGridViewRow row in metroGrid1.Rows)
            {
                if (Convert.ToString(row.Cells["Result"].Value) == "INSUFFICIENT") { metroTile3.Enabled = false; }
            }
        }
        private void metroComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            calc();
            presyo();
            noOfBagsandTOtal();
            metroTextBox1.Text = "";
            //computePrice();
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
        private void presyo() //Display price in metrotab3
        {
            sqlcon userConnect = new sqlcon();
            userConnect.dbIn();
            SqlCommand cmd = new SqlCommand("[amountOfDelivery] '" + metroTextBox1.Text + "','" + metroComboBox2.Text + "'", sqlcon.calc);
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                metroLabel3.Text = dr["Price"].ToString();
                metroLabel16.Text = "Php. " + dr["Price"].ToString();
                metroLabel12.Text = dr["Bagging"].ToString();
            }
            userConnect.dbOut();
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
                else if (Convert.ToString(row.Cells["Result"].Value) != "INSUFFICIENT")
                {

                    row.DefaultCellStyle.ForeColor = Color.Black;
                }
            }
        }
        private void noOfBagsandTOtal() //Display no.OfSacks in metrotab3
        {
            sqlcon userConnect = new sqlcon();
            userConnect.dbIn();
            SqlCommand cmd = new SqlCommand("[totalOfAmountsandBags] '" + metroTextBox1.Text + "','" + metroComboBox2.Text + "','" + metroLabel12.Text + "'", sqlcon.calc);
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                metroLabel4.Text = dr["Total"].ToString();
                metroLabel8.Text = dr["noOfBags"].ToString();
            }
            userConnect.dbOut();
        }
        private void computePrice() //Display price in metrotab11
        {
            sqlcon userConnect = new sqlcon();
            userConnect.dbIn();
            SqlCommand cmd = new SqlCommand("exec [totalPriceFg] '" + metroTextBox1.Text + "','" + metroLabel3.Text + "','"
            + metroComboBox2.Text + "','" + metroLabel12.Text + "'", sqlcon.calc);
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                metroLabel11.Text = dr["Presyo"].ToString();
            }
            userConnect.dbOut();
        }

        private void tempsoldFinishGood()
        {
            sqlcon userConnect = new sqlcon();
            userConnect.dbIn();
            SqlCommand soldFinishGood = new SqlCommand("[TEMPmaterialOut]", sqlcon.calc);
            soldFinishGood.CommandType = System.Data.CommandType.StoredProcedure;
            soldFinishGood.Parameters.AddWithValue("@recipe", metroComboBox2.Text);
            soldFinishGood.Parameters.AddWithValue("@client", metroComboBox1.Text);
            soldFinishGood.Parameters.AddWithValue("@amount", metroTextBox1.Text);
            soldFinishGood.Parameters.AddWithValue("@employe", globalVar.name.ToString());
            soldFinishGood.Parameters.AddWithValue("@pricePerBag", metroLabel3.Text);
            soldFinishGood.Parameters.AddWithValue("@bagging", metroLabel12.Text);
            soldFinishGood.Parameters.AddWithValue("@userID", globalVar.x.ToString());
            soldFinishGood.Parameters.AddWithValue("@fGcode", metroLabel18.Text);
            soldFinishGood.ExecuteNonQuery();
            userConnect.dbOut();
            metroTextBox1.Text = "";
            DesktopAlert.AlertColor = eDesktopAlertColor.Green;
            DesktopAlert.Show("Item Has been added!");
            DesktopAlert.AutoCloseTimeOut = 3;
            reloadChkBox();

        }
        private void metroTile3_Click(object sender, EventArgs e)
        {
            if (metroTextBox1.Text == "")
            {
                MetroMessageBox.Show(this, "", "Please check your inputs!", MessageBoxButtons.OK, MessageBoxIcon.Question);
            }
            else
            {
                tempsoldFinishGood();
                recentSolds();
                Form1 fm = new Form1();
                fm.notifDsply();
            }
            reloadChkBox();
            productionCount();
            //allProduction();
        }
        private void recentSolds()
        {
            sqlcon userConnect = new sqlcon();
            userConnect.dbIn();
            SqlCommand recipe = new SqlCommand("exec materialOutView", sqlcon.calc);
            SqlDataAdapter calculated = new SqlDataAdapter();
            calculated.SelectCommand = recipe;
            DataTable dataSet = new DataTable();
            calculated.Fill(dataSet);
            BindingSource nSource = new BindingSource();
            nSource.DataSource = dataSet;
            metroGrid2.DataSource = nSource;
            calculated.Update(dataSet);
            userConnect.dbOut();
            reloadChkBox();
       
        }
        private void metroButton3_Click(object sender, EventArgs e)
        {
           
            //allProduction();
        }
        private void soldCounts() //Display price in metrotab3
        {

            sqlcon userConnect = new sqlcon();
            userConnect.dbIn();
            SqlCommand cmd = new SqlCommand("[countsOfsOLD]", sqlcon.calc);
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                count = dr["count"].ToString();
            }
            userConnect.dbOut();
        }
     
        private void materialID()
        {

            String sDate = DateTime.Now.ToString();
            DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));
            String yy = datevalue.Year.ToString();
          
            soldCounts();

            int cnt = cnt = Convert.ToInt32(count);
            metroLabel18.Text = "ORD" + datevalue.ToString("yy") + "-" + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + "00" + (cnt + 1);
            metroLabel18.Visible  = false;
        }
     
        private void metroTextBox1_Click(object sender, EventArgs e)
        {
            materialID();
        }
        private void reloadChkBox()
        {
            foreach (DataGridViewRow row in metroGrid2.Rows)
            {
                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells[0];
                chk.Value = false;
            }
        }

        private void materialOut_Load(object sender, EventArgs e)
        {
      
                        
            
            string screenWidth = Screen.PrimaryScreen.Bounds.Width.ToString();
            string screenHeight = Screen.PrimaryScreen.Bounds.Height.ToString();

            if (screenWidth == "1920" && screenHeight == "1080") { this.WindowState = FormWindowState.Normal; this.Width = 1366; this.Height = 768; this.CenterToScreen(); }
            metroTabControl2.SelectedTab = metroTabPage2;
            //allProduction();
            reloadChkBox();
        }

        private void metroGrid2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
            //if (metroGrid2.SelectedCells[10].Value != null) { metroLabel20.Text = metroGrid2.SelectedCells[10].Value.ToString(); }
            //else { metroLabel20.Text = "in"; }
            //metroLabel20.Text = metroGrid2.SelectedCells[2].Value.ToString();
            //var senderGrid = (DataGridView)sender;

            ////if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0 && metroLabel20.Text != "Pending")
            //if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn)
            //{
            //    globalVar.orderID = metroGrid2.SelectedCells[2].Value.ToString();
            //    globalVar.userID = metroGrid2.SelectedCells[10].Value.ToString();
            //    DialogResult dialogResult = MetroMessageBox.Show(this, "Production finished?.", "Ready for Pickup/ Delivery", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            //    if (dialogResult == DialogResult.Yes)
            //    {
            //        try
            //        {

            //            SaveFileDialog savefiledialog1 = new SaveFileDialog();
            //            savefiledialog1.FileName = (metroLabel20.Text) + " " + (DateTime.Now.ToLongDateString());
            //            savefiledialog1.Filter = "PDF Files|*.pdf";


            //            if (savefiledialog1.ShowDialog() == DialogResult.OK)
            //            {
            //                {
            //                    Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 42, 35);
            //                    PdfWriter wri = PdfWriter.GetInstance(doc, new FileStream(savefiledialog1.FileName, FileMode.Create));
            //                    wri.PageEvent = new PdfWriterEvents("T E X I C O N");


            //                    doc.Open();
            //                    iTextSharp.text.Image PNG = iTextSharp.text.Image.GetInstance("TexiconLogo.png");
            //                    PNG.ScaleAbsolute(250, 125);
            //                    PNG.SetAbsolutePosition(175, 630);
            //                    PNG.SpacingAfter = 80f;
            //                    PNG.SpacingBefore = 10f;
            //                    doc.Add(PNG);
            //                    Paragraph para1 = new Paragraph("Sales Invoice");
            //                    para1.Alignment = Element.ALIGN_CENTER;
            //                    para1.SpacingAfter = 5f;
            //                    para1.SpacingBefore = 140f;
            //                    para1.Font.Size = 15;
            //                    doc.Add(para1);

            //                    PdfPTable table = new PdfPTable(4);
            //                    //Paragraph paratest = new Paragraph(new Phrase(metroLabel20.Text, FontFactory.GetFont("Times New Roman", 8f, iTextSharp.text.Font.ITALIC, BaseColor.BLACK)));
            //                    var itFont = FontFactory.GetFont("Times New Roman", 12f, iTextSharp.text.Font.ITALIC, BaseColor.BLACK);
            //                    var boldFont = FontFactory.GetFont("Times New Roman", 12f, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
            //                    table.SpacingBefore = 10f;
                             
            //                    Phrase ListTitlePhrase = new Phrase();
            //                    ListTitlePhrase.Add(new Chunk("Order No.", boldFont));
            //                    ListTitlePhrase.Add(new Chunk(metroLabel20.Text, itFont));

            //                    PdfPCell cell0 = new PdfPCell(ListTitlePhrase);

            //                    //cell0.AddElement(new Paragraph(ListTitlePhrase));
                               
            //                    //cell0.AddElement(new Paragraph("Receipt: " + "Receipt: "));
            //                    //cell0.AddElement(new Paragraph("Receipt: "));
            //                    cell0.Colspan = 4;
            //                    cell0.HorizontalAlignment = 1;
                                
                               
            //                    var rowsCount1 = metroGrid2.SelectedRows.Count;
            //                    if (rowsCount1 == 0 || rowsCount1 > 1) return;
            //                    PdfPCell cell = new PdfPCell(new Phrase(metroGrid2.Columns[3].HeaderText, FontFactory.GetFont("Times New Roman", 10f, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
            //                    cell.HorizontalAlignment = 1;
            //                    cell.Colspan = 2;
            //                    cell.Padding = 5;
            //                    PdfPCell cell1 = new PdfPCell(new Phrase(metroGrid2.SelectedCells[3].Value.ToString(), FontFactory.GetFont("Times New Roman", 10f, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
            //                    cell1.Colspan = 2;
            //                    cell1.Padding = 5;
            //                    PdfPCell cell2 = new PdfPCell(new Phrase("Product Name", FontFactory.GetFont("Times New Roman", 10f, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
            //                    cell2.HorizontalAlignment = 1;
            //                    cell2.Colspan = 2;
            //                    cell2.Padding = 5;
            //                    PdfPCell cell3 = new PdfPCell(new Phrase(metroGrid2.SelectedCells[4].Value.ToString(), FontFactory.GetFont("Times New Roman", 10f, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
            //                    cell3.Colspan = 2;
            //                    cell3.Padding = 5;
            //                    PdfPCell cell4 = new PdfPCell(new Phrase(metroGrid2.Columns[5].HeaderText, FontFactory.GetFont("Times New Roman", 10f, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
            //                    cell4.HorizontalAlignment = 1;
            //                    cell4.Colspan = 2;
            //                    cell4.Padding = 5;
            //                    PdfPCell cell5 = new PdfPCell(new Phrase(metroGrid2.SelectedCells[5].Value.ToString(), FontFactory.GetFont("Times New Roman", 10f, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
            //                    cell5.Colspan = 2;
            //                    cell5.Padding = 5;
            //                    PdfPCell cell6 = new PdfPCell(new Phrase(metroGrid2.Columns[6].HeaderText, FontFactory.GetFont("Times New Roman", 10f, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
            //                    cell6.HorizontalAlignment = 1;
            //                    cell6.Colspan = 2;
            //                    cell6.Padding = 5;
            //                    PdfPCell cell7 = new PdfPCell(new Phrase(metroGrid2.SelectedCells[6].Value.ToString(), FontFactory.GetFont("Times New Roman", 10f, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
            //                    cell7.Colspan = 2;
            //                    cell7.Padding = 5;
            //                    PdfPCell cell8 = new PdfPCell(new Phrase(metroGrid2.Columns[7].HeaderText, FontFactory.GetFont("Times New Roman", 10f, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
            //                    cell8.HorizontalAlignment = 1;
            //                    cell8.Colspan = 2;
            //                    cell8.Padding = 5;
            //                    PdfPCell cell9 = new PdfPCell(new Phrase(metroGrid2.SelectedCells[7].Value.ToString(), FontFactory.GetFont("Times New Roman", 10f, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
            //                    cell9.Colspan = 2;
            //                    cell9.Padding = 5;
            //                    PdfPCell cell10 = new PdfPCell(new Phrase(metroGrid2.Columns[8].HeaderText, FontFactory.GetFont("Times New Roman", 10f, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
            //                    cell10.HorizontalAlignment = 1;
            //                    cell10.Colspan = 2;
            //                    cell10.Padding = 5;
            //                    PdfPCell cell11 = new PdfPCell(new Phrase(metroGrid2.SelectedCells[8].Value.ToString(), FontFactory.GetFont("Times New Roman", 10f, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
            //                    cell11.Colspan = 2;
            //                    cell11.Padding = 5;
            //                    PdfPCell cell12 = new PdfPCell(new Phrase(metroGrid2.Columns[9].HeaderText, FontFactory.GetFont("Times New Roman", 10f, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
            //                    cell12.HorizontalAlignment = 1;
            //                    cell12.Colspan = 2;
            //                    cell12.Padding = 5;
            //                    PdfPCell cell13 = new PdfPCell(new Phrase(metroGrid2.SelectedCells[9].Value.ToString(), FontFactory.GetFont("Times New Roman", 10f, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
            //                    cell13.Colspan = 2;
            //                    cell13.Padding = 5;
            //                    PdfPCell cell14 = new PdfPCell(new Phrase(metroGrid2.Columns[10].HeaderText, FontFactory.GetFont("Times New Roman", 10f, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
            //                    cell14.HorizontalAlignment = 1;
            //                    cell14.Colspan = 2;
            //                    cell14.Padding = 5;
            //                    PdfPCell cell15 = new PdfPCell(new Phrase(metroGrid2.SelectedCells[10].Value.ToString(), FontFactory.GetFont("Times New Roman", 10f, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
            //                    cell15.Colspan = 2;
            //                    cell15.Padding = 5;
            //                    table.AddCell(cell0);
            //                    table.AddCell(cell);
            //                    table.AddCell(cell1);
            //                    table.AddCell(cell2);
            //                    table.AddCell(cell3);
            //                    table.AddCell(cell4);
            //                    table.AddCell(cell5);
            //                    table.AddCell(cell6);
            //                    table.AddCell(cell7);
            //                    table.AddCell(cell8);
            //                    table.AddCell(cell9);
            //                    table.AddCell(cell10);
            //                    table.AddCell(cell11);
            //                    table.AddCell(cell12);
            //                    table.AddCell(cell13);
            //                    table.AddCell(cell14);
            //                    table.AddCell(cell15);
            //                    doc.Add(table);
            //                    doc.Close();
            //                     AddPageNumber(savefiledialog1.FileName, savefiledialog1.FileName);
            //                    System.Diagnostics.Process.Start(savefiledialog1.FileName);
            //                }
            //                soldFinishGood();
            //                DesktopAlert.Show("Transferred to finish goods!");
            //            }
            //            else if (savefiledialog1.ShowDialog() == DialogResult.Cancel)
            //            {
            //                DesktopAlert.Show("Export the receipt first!");
            //            }

            //        }
            //        catch
            //        {
            //            DesktopAlert.Show("The file is open!");
            //        }
                   
            //    }
                
            //}
          
            //var rowsCount = metroGrid2.SelectedRows.Count; 
            //if (rowsCount == 0 || rowsCount > 1) return;

            //allProduction();
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
                        ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_MIDDLE, new Phrase("     Received by:            __________________________________________                                                                           Date:____/___/_____", FontFactory.GetFont("Times New Roman", 8f, iTextSharp.text.Font.ITALIC, BaseColor.BLACK)), 5f, 60f, 0);
                        ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_MIDDLE, new Phrase("                                                       Signature over printed name ", FontFactory.GetFont("Times New Roman", 8f, iTextSharp.text.Font.ITALIC, BaseColor.BLACK)), 5f, 50f, 0);
                        ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_MIDDLE, new Phrase("     Generated by: " + globalVar.name.ToString() + "                                           " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToShortTimeString() + "                                      " + "Page " + i.ToString() + " of " + pages, FontFactory.GetFont("Times New Roman", 8f, iTextSharp.text.Font.ITALIC, BaseColor.BLACK)), 5f, 20f, 0);
                    }
                }
                bytes = stream.ToArray();
            }
            File.WriteAllBytes(fileOut, bytes);
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //private void metroButton3_Click_1(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        foreach (DataGridViewRow item in metroGrid2.Rows)
        //        {
        //            if (bool.Parse(item.Cells[0].Value.ToString()))
        //            {
        //                sqlcon userConnect = new sqlcon(); userConnect.dbIn();
        //                SqlCommand material = new SqlCommand("[voidProduction]", sqlcon.calc); material.CommandType = System.Data.CommandType.StoredProcedure;
        //                material.Parameters.AddWithValue("@orderID", item.Cells[0].Value.ToString());
        //                //material.Parameters.AddWithValue("@checkUserID", globalVar.x.ToString());
        //                material.ExecuteNonQuery();
        //                userConnect.dbOut();
        //            }
        //        }
        //    }
        //    catch
        //    {
        //        return;
        //    }
        //    recentSolds();
        //}

        private void metroTextButton2_Click(object sender, EventArgs e)
        {   
            DialogResult dialogResult = MetroMessageBox.Show(this, "Production Finished?", "Finish Goods", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
              
                SaveFileDialog savefiledialog1 = new SaveFileDialog();
                savefiledialog1.FileName = "Proof of Transaction" + " " + (DateTime.Now.ToLongDateString());
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
                                PNG.SetAbsolutePosition(175, 630);
                                PNG.SpacingAfter = 80f;
                                PNG.SpacingBefore = 10f;
                                doc.Add(PNG);
                                Paragraph para1 = new Paragraph("Proof of Transaction");
                                para1.Alignment = Element.ALIGN_CENTER;
                                para1.SpacingAfter = 5f;
                                para1.SpacingBefore = 140f;
                                para1.Font.Size = 15;
                                doc.Add(para1);
                                Paragraph para2 = new Paragraph("(for Delivery)");
                                para2.Alignment = Element.ALIGN_CENTER;
                                para2.SpacingAfter = 15f;
                                para2.SpacingBefore = .5f;
                                para2.Font.Size = 13;
                                doc.Add(para2);


                                
                                PdfPTable table = new PdfPTable(metroGrid2.ColumnCount - 1);
                                table.SpacingBefore = 15f;

                                        for (int j = 1; j < 9; j++)
                                    {
                                        PdfPCell cell = new PdfPCell(new Phrase(metroGrid2.Columns[j].HeaderText, FontFactory.GetFont("Times New Roman", 10f, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                                        cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.ColorTranslator.FromHtml("#C4C4C4"));
                                        cell.HorizontalAlignment = 1;
                                        table.AddCell(cell);
                                    }
                                    table.HeaderRows = 1;
                                    for (int i = 0; i < metroGrid2.Rows.Count; i++)
                                    {
                                        for (int k = 1; k < 9; k++)
                                        {
                                            if (metroGrid2.Rows[i].Cells[0].Value.Equals(true))
                                            {
                                                PdfPCell cell2 = new PdfPCell(new Phrase(metroGrid2[k, i].Value.ToString(), FontFactory.GetFont("Times New Roman", 9f, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                                if (i % 2 != 0)
                                                {
                                                    cell2.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.ColorTranslator.FromHtml("#f0f0f5"));
                                                }
                                                cell2.HorizontalAlignment = 1;
                                                table.AddCell(cell2);
                                               
                                            }
                                        }
                                    }


                                doc.Add(table);
                                doc.Close();
                                AddPageNumber(savefiledialog1.FileName, savefiledialog1.FileName);
                                System.Diagnostics.Process.Start(savefiledialog1.FileName);
                            
                        }

                            soldFinishGood();
                            productionCount();
                                }



                    }
                }
        private void metroTextButton4_Click(object sender, EventArgs e)
        {

            try
            {
                foreach (DataGridViewRow item in metroGrid2.Rows)
                {
                    if (bool.Parse(item.Cells[0].Value.ToString()))
                    {
                        sqlcon userConnect = new sqlcon(); userConnect.dbIn();
                        SqlCommand material = new SqlCommand("[voidProduction]", sqlcon.calc); material.CommandType = System.Data.CommandType.StoredProcedure;
                        material.Parameters.AddWithValue("@orderID", item.Cells[1].Value.ToString());
                        //material.Parameters.AddWithValue("@checkUserID", globalVar.x.ToString());
                        material.ExecuteNonQuery();
                        userConnect.dbOut();
                    }
                }
            }
            catch
            {
                return;
            }
            recentSolds();
            reloadChkBox();

        }

        private void metroGrid2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            int go = 0;
            try
            {
                bool isChecked = Convert.ToBoolean(metroGrid2.Rows[metroGrid2.CurrentCell.RowIndex].Cells[0].Value.ToString());

                if (isChecked)
                {
                    chkCNT += 1;
                }
                else
                {
                    chkCNT -= 1;
                }

                foreach (DataGridViewRow item in metroGrid2.Rows)
                {
                    if (bool.Parse(item.Cells[0].Value.ToString())) { go = 1; }
                }

                //metroLabel2.Text = "Selected Items: " + chkCNT;
                //metroLabel21.Text = go.ToString();
                if (go == 1) { metroTextButton2.Enabled = true; metroTextButton4.Enabled = true; } else { metroTextButton2.Enabled = false; metroTextButton4.Enabled = false; }
            }
            catch { }
        }

        private void metroGrid2_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (metroGrid2.IsCurrentCellDirty)
            {
                metroGrid2.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void metroTile1_Click(object sender, EventArgs e)
        {
            //foreach (DataGridViewRow dr in metroGrid2.Rows)
            //{
            //    if (dr.Cells[0].Value.ToString() == "False")
            //    {
            //        dr.Visible = false;
            //    }
            //}
        }

        private void metroLabel21_Click(object sender, EventArgs e)
        {

        }

        private void metroLabel22_Click(object sender, EventArgs e)
        {

        }

        private void productionCount() //Display price in metrotab3
        {

            sqlcon userConnect = new sqlcon();
            userConnect.dbIn();
            SqlCommand cmd = new SqlCommand("[TOTALproduction]", sqlcon.calc);
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                metroLabel22.Text = dr[0].ToString();
                metroLabel23.Text = dr[1].ToString();
            }
            userConnect.dbOut();
        }
    }
    
}
