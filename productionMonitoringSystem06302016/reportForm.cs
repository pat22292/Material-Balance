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
    public partial class reportForm : MetroForm 
    {
        public reportForm()
        {
           // disablebtton();
            InitializeComponent();   
        }
        private void reportForm_Load(object sender, EventArgs e)
        {

            listOfReports();
            listOfUser();
            listOfClients();
            listOfProduct();
        }
        private void putcounts()
        {

            //int rowNumber = 1;
            //if (metroGrid1.Rows.Count > 0)
            //{
            //    foreach (DataRow row in metroGrid1.Rows)
            //    {
            //        row[0] = rowNumber;
            //        rowNumber++;
            //    }
            //}
        }
          private void displayReports()
          {

              try
              {

                  string eName = metroComboBox2.Text;
                  string CName = metroComboBox3.Text;
                  string PName = metroComboBox4.Text;
                  sqlcon userConnect = new sqlcon();
                  userConnect.dbIn();
                  SqlCommand recipe = new SqlCommand("exec [" + metroComboBox1.Text + "] '" + metroDateTime1.Value + "','" + metroDateTime2.Value + "','"
                      + eName + "','" + CName + "','" + PName + "'", sqlcon.calc);
                  SqlDataAdapter calculated = new SqlDataAdapter();
                  calculated.SelectCommand = recipe;
                  DataTable dataSet = new DataTable();



                  metroGrid1.DataSource = dataSet;
                  DataColumn col1 = new DataColumn();

                  BindingSource nSource = new BindingSource();
                  nSource.DataSource = dataSet;

                  metroGrid1.DataSource = nSource;
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
                  calculated.Update(dataSet);
                  calculated.Fill(dataSet);

                  userConnect.dbOut();

                  disablebtton();
                  this.metroGrid1.Columns["No."].Width = 50;



              }
              catch { }
            
            
          }//Heart of the string
   
          private void metroDateTime1_ValueChanged(object sender, EventArgs e)
          {
             
              displayReports();
              disablebtton();
          }
          private void metroDateTime2_ValueChanged(object sender, EventArgs e)
          {
              if (metroDateTime1.Value > metroDateTime2.Value)
              {
                  
                  DesktopAlert.Show("The date you entered is INVALID !!");
              }
              else
              {
                  displayReports();
                  disablebtton();
                  
              }
          }
              
          
          private void listOfUser() //provide the client list for combobox
          {
              try
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
              catch { }

          }
          private void listOfReports() //provide the client list for combobox
          {
              try
              {
                  sqlcon userConnect = new sqlcon();
                  userConnect.dbIn();
                  SqlDataAdapter da = new SqlDataAdapter("exec [reportsList]", sqlcon.calc);
                  DataTable dt = new DataTable();
                  da.Fill(dt);
                  metroComboBox1.DataSource = dt;
                  metroComboBox1.DisplayMember = "name".ToString();
                  userConnect.dbOut();
              }
              catch { }
          }
      
  
          private void metroButton1_Click(object sender, EventArgs e)
          {
              try
              {

                  SaveFileDialog savefiledialog1 = new SaveFileDialog();
                  savefiledialog1.FileName = (metroComboBox1.Text) + " " + (DateTime.Now.ToLongDateString());
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
                 
                          //else
                          //{
                              Paragraph para3 = new Paragraph(metroComboBox1.Text.ToUpper());
                              para3.Alignment = Element.ALIGN_CENTER;
                              para3.SpacingAfter = .5f;
                              para3.SpacingBefore = 110F;
                              para3.Font.Size = 12;
                              doc.Add(para3);
                         // }
                              if (metroComboBox1.Text == "Puchased Materials Report")
                              {
                                  Paragraph para7 = new Paragraph("(Material IN)");
                                  para7.Alignment = Element.ALIGN_CENTER;
                                  para7.SpacingAfter = .5f;
                                  para7.SpacingBefore = .5F;
                                  para7.Font.Size = 10;
                                  doc.Add(para7);
                              }

                          Paragraph para1 = new Paragraph("(" + (metroDateTime1.Value.ToLongDateString()) + "-" + (metroDateTime2.Value.ToLongDateString()) + ")");
                          para1.Alignment = Element.ALIGN_CENTER;
                          para1.SpacingAfter = 5f;
                          para1.SpacingBefore = .5f;
                          para1.Font.Size = 8;
                          doc.Add(para1);

                          PdfPTable table = new PdfPTable(metroGrid1.Columns.Count);
                          table.SpacingBefore = 15f;
                          for (int j = 0; j < metroGrid1.Columns.Count; j++)
                          {
                              PdfPCell cell = new PdfPCell(new Phrase(metroGrid1.Columns[j].HeaderText, FontFactory.GetFont("Times New Roman", 10f, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                              cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.ColorTranslator.FromHtml("#C4C4C4"));
                              cell.HorizontalAlignment = 1;
                              table.AddCell(cell);
                          }
                          table.HeaderRows = 1;
                          for (int i = 0; i < metroGrid1.Rows.Count; i++)
                          {
                              for (int k = 0; k < metroGrid1.Columns.Count; k++)
                              {
                                  PdfPCell cell2 = new PdfPCell(new Phrase(metroGrid1[k, i].Value.ToString(), FontFactory.GetFont("Times New Roman", 9f, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));

                                  if (i % 2 != 0)
                                  {
                                      cell2.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.ColorTranslator.FromHtml("#f0f0f5"));
                                  }

                                  cell2.HorizontalAlignment = 1;
                                  table.AddCell(cell2);
                              }
                          }
                          doc.Add(table);
                          Paragraph items = new Paragraph("Total of Items: "+metroGrid1.RowCount.ToString());
                          items.Alignment = Element.ALIGN_LEFT;
                          items.IndentationLeft = 133f;
                          items.SpacingBefore = .5f;
                          doc.Add(items);

                          Paragraph nothing = new Paragraph("********************************* NOTHING FOLLOWS *********************************");
                          nothing.Alignment = Element.ALIGN_CENTER;
                          nothing.SpacingBefore = .5f;
                          doc.Add(nothing);
                          doc.Close();
                          doc.Close();
                          AddPageNumber(savefiledialog1.FileName, savefiledialog1.FileName);
                          System.Diagnostics.Process.Start(savefiledialog1.FileName);
                      }
                  }
                  sqlcon userConnect = new sqlcon();
                  userConnect.dbIn();
                  SqlCommand material = new SqlCommand("[AuditTrailRpt]", sqlcon.calc);
                  material.CommandType = System.Data.CommandType.StoredProcedure;
                  material.Parameters.AddWithValue("@addBy", globalVar.name.ToString());
                  material.Parameters.AddWithValue("@Report", metroComboBox1.Text);
                  material.ExecuteNonQuery();
                  userConnect.dbOut();

              }
              catch
              {
                  DesktopAlert.Show("The file is open!");
                   //.Show("Files is open");
              }
          }
          public void listOfClients() //provide the client list for combobox
          {
              try
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
              catch { }
             
          }
          private void disablebtton()
          {
              if (metroGrid1.Rows.Count <= 0)
              { metroButton1.Enabled =false; }
              else
              { metroButton1.Enabled = true; }

          }
          private void metroComboBox2_SelectedValueChanged(object sender, EventArgs e) 
          {
             
              displayReports();
              disablebtton();
              
          }

          private void metroComboBox3_SelectedValueChanged(object sender, EventArgs e)
          {
              displayReports();
              disablebtton();
              
          }

          private void metroComboBox1_SelectedValueChanged(object sender, EventArgs e)
          {

              if (metroComboBox1.Text != "Sold Goods Report") { metroComboBox3.SelectedValue = 0; metroComboBox4.Visible = false; metroLabel6.Visible = false; metroComboBox3.Visible = false; metroLabel5.Visible = false; }
              else { metroComboBox3.Visible = true; metroLabel5.Visible = true; metroComboBox4.Visible = true; metroLabel6.Visible = true; }
              disablebtton();
          }
          private void listOfProduct()
          {
              try
              {
                  sqlcon userConnect = new sqlcon();
                  userConnect.dbIn();
                  SqlDataAdapter da = new SqlDataAdapter("exec [productList]", sqlcon.con);
                  DataTable dt = new DataTable();
                  da.Fill(dt);
                  metroComboBox4.DataSource = dt;
                  //metroComboBox3.DisplayMember = "ClientName";

                  dt.Columns.Add("ID", typeof(int));
                  metroComboBox4.DisplayMember = "Products";
                  metroComboBox4.ValueMember = "ID";
                  metroComboBox4.DataSource = dt;

                  DataRow dr = dt.NewRow();
                  dr["Products"] = "All";


                  dt.Rows.InsertAt(dr, 0);
                  metroComboBox4.SelectedIndex = 0;

                  userConnect.dbOut();
              }
              catch { }
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
                          ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_MIDDLE, new Phrase("     Printed by: " + globalVar.name.ToString() + "                                           " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToShortTimeString() + "                                      " + "Page " + i.ToString() + " of " + pages, FontFactory.GetFont("Times New Roman", 8f, iTextSharp.text.Font.ITALIC, BaseColor.BLACK)), 5f, 15f, 0);
                          // ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_MIDDLE, new Phrase("   Printed by: " + globalVar.name.ToString() + "                                           " + "Page " + i.ToString() + " of " + pages + "                                                              " + DateTime.Now.ToLongDateString(), FontFactory.GetFont("Times New Roman", 9f, iTextSharp.text.Font.ITALIC, BaseColor.BLACK)), 5f, 15f, 0);
                      }
                  }
                  bytes = stream.ToArray();
              }
              File.WriteAllBytes(fileOut, bytes);
          }

          private void metroComboBox1_SelectionChangeCommitted(object sender, EventArgs e)
          {
              metroComboBox4.SelectedIndex = 0;

              if (metroComboBox1.Text == "Audit Trail Report" && globalVar.x != "AGRI0001") {
                  DialogResult dialogResult = MetroMessageBox.Show(this, "You are not allowed to view this report", "Invalid Report", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                  if (dialogResult == DialogResult.OK)
                  {
                      metroComboBox1.SelectedIndex = 0;
                  }
              }
              else{displayReports();}
              
              disablebtton();
          }

          private void metroComboBox4_SelectedIndexChanged(object sender, EventArgs e)
          {
              
              displayReports();
              disablebtton();
              
          }

          private void metroDateTime1_Leave(object sender, EventArgs e)
          {
              disablebtton();
          }

          private void metroDateTime2_Leave(object sender, EventArgs e)
          {
              disablebtton();
          }

          private void metroGrid1_MouseHover(object sender, EventArgs e)
          {
              disablebtton();
          }

          private void metroButton2_Click(object sender, EventArgs e)
          {
              this.Close();
          }

       


         
    }

}

