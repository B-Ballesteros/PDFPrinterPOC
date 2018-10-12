using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PdfiumViewer;

namespace PrinterPOC
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            FillCombo();
        }

        private List<string> GetPrinters()
        {
            var printers = new List<string>();
            foreach(string printer in PrinterSettings.InstalledPrinters)
            {
                printers.Add(printer);
            }
            return printers;
        }

        private void FillCombo()
        {
            comboPrinters.DataSource = GetPrinters();
            comboPrinters.SelectedIndex = 0;
        }

        private void print(string filename)
        {
            try
            {
                var printerSettings = new PrinterSettings
                {
                    PrinterName = comboPrinters.SelectedValue.ToString(),
                    Copies = 1,
                };
                var pageSettings = new PageSettings(printerSettings)
                {
                    Margins = new Margins(0, 0, 0, 0),
                };
                foreach (PaperSize paperSize in printerSettings.PaperSizes)
                {
                    if (paperSize.PaperName == "A4")
                    {
                        pageSettings.PaperSize = paperSize;
                        break;
                    }
                }
                using (var document = PdfDocument.Load(filename))
                {
                    using (var printDocument = document.CreatePrintDocument())
                    {
                        
                        printDocument.PrinterSettings = printerSettings;
                        printDocument.DefaultPageSettings = pageSettings;
                        printDocument.PrintController = new StandardPrintController();
                        printDocument.Print();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SelectFile()
        {
            if(odf.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = odf.FileName;
            }
        }

        private void OnSearchClick(object sender, EventArgs e)
        {
            SelectFile();
        }

        private void onPrintClick(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(textBox1.Text))
            {
                print(textBox1.Text);
            }
        }
    }
}
