using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.IO;
using System.Windows.Forms;

namespace SmallPDF
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        public static void CompressPdf(string targetPath)
        {
            using (var stream = new MemoryStream(File.ReadAllBytes(targetPath)) { Position = 0 })
            using (var source = PdfReader.Open(stream, PdfDocumentOpenMode.Import))
            using (var document = new PdfDocument())
            {
                var options = document.Options;
                options.FlateEncodeMode = PdfFlateEncodeMode.BestCompression;
                options.UseFlateDecoderForJpegImages = PdfUseFlateDecoderForJpegImages.Automatic;
                options.CompressContentStreams = true;
                options.NoCompression = false;
                foreach (var page in source.Pages)
                {
                    document.AddPage(page);
                }

                document.Save(targetPath);
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.Title = "Select PDF file";
            this.openFileDialog1.Filter = "PDF Files (.pdf)|*.pdf|All Files (*.*)|*.*";

            DialogResult dr = this.openFileDialog1.ShowDialog();
            
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    label2.Text = openFileDialog1.FileName;
                    axAcroPDF1.LoadFile(openFileDialog1.FileName);
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnCompress_Click(object sender, EventArgs e)
        {
            try
            {
                CompressPdf(openFileDialog1.FileName);
                MessageBox.Show("Completed!", "SmallPDF", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
