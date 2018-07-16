﻿using System;
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
    public partial class UserFrm : DevComponents.DotNetBar.Metro.MetroForm
    {
        public UserFrm()
        {
            disablebutton();
            InitializeComponent();
        }

        private void UserFrm_Load(object sender, EventArgs e)
        {
            
            slidePanel1.IsOpen = false;
            analogClockControl1.AutomaticMode = true;
            ListOfFormulasTwo();
            labelX2.Text = ("Hi, " + globalVar.name + ", " + globalVar.position);
            globalVar.pass = "";
            timer1.Start();
            this.Opacity = 0.1;
        }
        private void metroTileItem49_Click(object sender, EventArgs e)
        {
            slidePanel1.IsOpen = true;
            slidePanel1.BringToFront();

        }

        private void metroTile1_Click(object sender, EventArgs e)
        {
            slidePanel1.IsOpen = false;
        }

        private void metroTileItem29_Click(object sender, EventArgs e)
        {
            reportForm rF = new reportForm();
            rF.ShowDialog();

        }

        private void metroTileItem38_Click(object sender, EventArgs e)
        {
            materialIn MI = new materialIn();
            MI.ShowDialog();
        }

        private void metroTileItem41_Click(object sender, EventArgs e)
        {
            materialMasterList MML = new materialMasterList();
            MML.ShowDialog();
        }

        private void metroTileItem40_Click(object sender, EventArgs e)
        {
            materialOut MO = new materialOut();
            MO.ShowDialog();
        }

        private void metroTileItem42_Click(object sender, EventArgs e)
        {
            matrlStock mS = new matrlStock();
            mS.ShowDialog();
        }
        private void metroTileItem44_Click(object sender, EventArgs e)
        {
            Sales Sls = new Sales();
            Sls.ShowDialog();
        }
        private void calcTwo()
        {
            sqlcon userConnect = new sqlcon();
            userConnect.dbIn();
            SqlCommand recipe = new SqlCommand("recipeCalculator '" + metroComboBox1.Text + "','" + metroTextBox1.Text + "'", sqlcon.calc);
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
        private void ListOfFormulasTwo()
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

        private void metroComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            calcTwo();
        }

        private void metroTextBox1_TextChanged(object sender, EventArgs e)
        {
            disablebutton();
            calcTwo();
        }
        private void disablebutton()
        {
            if (metroTextBox1.Text == null || metroTextBox1.Text == "")
            {
                metroTextButton1.Enabled = false;
            }
            else
            {
                metroTextButton1.Enabled = true;
            }
        }

        private void metroTileItem46_Click(object sender, EventArgs e)
        {
            msgBoxFrm mbx = new msgBoxFrm();
            mbx.ShowDialog();
            this.FormClosing -= new System.Windows.Forms.FormClosingEventHandler(this.UserFrm_FormClosing);
            this.Hide();
        }

        private void analogClockControl1_ValueChanged(object sender, EventArgs e)
        {
            labelX3.Text = (DateTime.Now.ToLongDateString() + " at " + DateTime.Now.ToLongTimeString());
            metroLabel3.Text = DateTime.Now.ToLongTimeString();
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

        private void UserFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }

        private void metroTextButton2_Click(object sender, EventArgs e)
        {
            if (metroTextBox1.Text == null || metroTextBox1.Text == "")
            {
                DesktopAlert.Show("No Item hass been inputed!");
            }
            else
            {

                uploadTOmaterialTemps();
                DesktopAlert.Show("Raw materials has been added!");
                metroTextBox1.Text = "";
                slidePanel1.IsOpen = false;
                materialIn matIn = new materialIn();
                matIn.ShowDialog();


            }
        }

        private void uploadTOmaterialTemps()
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
        private void metroTextButton1_Click(object sender, EventArgs e)
        {
            SaveFileDialog savefiledialog1 = new SaveFileDialog();
            savefiledialog1.FileName = "Quotation report " + (DateTime.Now.ToShortDateString()); 
            savefiledialog1.Filter = "PDF Files|*.pdf";

            if (savefiledialog1.ShowDialog() == DialogResult.OK)
            {
                {
                    Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 42, 35);
                    PdfWriter wri = PdfWriter.GetInstance(doc, new FileStream(savefiledialog1.FileName, FileMode.Create));
                    
                    doc.Open();
                    iTextSharp.text.Image PNG = iTextSharp.text.Image.GetInstance("TexiconLogo.png");
                    PNG.ScaleAbsolute(250, 125);
                    PNG.SetAbsolutePosition(175, 660);
                    PNG.SpacingAfter = 70f;
                    doc.Add(PNG);
                    Paragraph para2 = new Paragraph("Printed By:", FontFactory.GetFont("Segoe UI", 9f, iTextSharp.text.Font.ITALIC, BaseColor.BLACK));
                    para2.SpacingBefore = 70f;
                    para2.SpacingAfter = .50f;
                    para2.SpacingBefore = 45f;
                    para2.IndentationLeft = 230f;
                    doc.Add(para2);
                    Paragraph para4 = new Paragraph(globalVar.name + ", " + globalVar.position);
                    para4.SpacingAfter = .50f;
                    para4.IndentationLeft = 15f;
                    para4.Font.Size = 9;
                    doc.Add(para4);
                    Paragraph para5 = new Paragraph(DateTime.Now.ToLongDateString());
                    para5.SpacingAfter = 10f;
                    para5.IndentationLeft = 15f;
                    para5.Font.Size = 9;
                    doc.Add(para5);
                    Paragraph para3 = new Paragraph("Quotation Report");
                    para3.SpacingAfter = 3f;
                    para3.IndentationLeft = 200f;
                    para3.Font.Size = 13;
                    doc.Add(para3);


                    PdfPTable table = new PdfPTable(metroGrid1.Columns.Count);
                   
                    for (int j = 0; j < metroGrid1.Columns.Count; j++)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(metroGrid1.Columns[j].HeaderText, FontFactory.GetFont("Times New Roman", 8f, iTextSharp.text.Font.BOLD, BaseColor.WHITE)));
                        cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.ColorTranslator.FromHtml("#9DA2A3"));
                        cell.HorizontalAlignment = 1;
                        table.TotalWidth = 340f;
                        table.LockedWidth = true;
                        table.AddCell(cell);
                    }
                    table.HeaderRows = 1;
                    for (int i = 0; i < metroGrid1.Rows.Count; i++)
                    {
                        for (int k = 0; k < metroGrid1.Columns.Count; k++)
                        {
                            PdfPCell cell2 = new PdfPCell(new Phrase(metroGrid1[k, i].Value.ToString(), FontFactory.GetFont("Times New Roman", 7f, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));

                            if (i % 2 != 0)
                            {
                                cell2.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.ColorTranslator.FromHtml("#C4C4C4"));
                            }

                            cell2.HorizontalAlignment = 1;
                            table.TotalWidth = 340f;
                           table.LockedWidth = true;
                            table.AddCell(cell2);
                        }
                    }
                    doc.Add(table);
                    doc.Close();
                    AddPageNumber(savefiledialog1.FileName, savefiledialog1.FileName);
                    System.Diagnostics.Process.Start(savefiledialog1.FileName);
                }
            }
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
                        ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_RIGHT, new Phrase(i.ToString(), FontFactory.GetFont("Times New Roman", 7f, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)), 568f, 15f, 0);
                    }
                }
                bytes = stream.ToArray();
            }
            File.WriteAllBytes(fileOut, bytes);
        }

    }
}
