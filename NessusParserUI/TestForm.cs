using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.Diagnostics;
using VTX.Nessus;


namespace NessusParserUI
{
    public partial class TestForm : Form
    {
        private string _filePath;
        public NessusClientDataV2 _NessusFile;
        public event PropertyChangedEventHandler PropertyChanged;
        public TestForm()
        {
            InitializeComponent();
        }

        private void TestForm_Load(object sender, EventArgs e)
        {
        }

        public string FilePath
        {
            get { return _filePath; }
            set {
                if (value != _filePath)
                {
                    if (File.Exists(value) == false) { throw new FileNotFoundException("File Not Found", value); }
                    _filePath = value;
                    PropertyChangedEventHandler handler = PropertyChanged;
                    if (handler != null) handler(this, new PropertyChangedEventArgs(_filePath));
                }
            }
        }

        private void BrowseForFileButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Nessus files (*.nessus)|*.nessus|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try{
                    this.FilePath = openFileDialog1.FileName;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not find file: " + ex.Message);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this._filePath);
        }

        private void VTXTestButton_Click(object sender, EventArgs e)
        {
            if (this._filePath != null)
            {
                try{
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    _NessusFile = new NessusClientDataV2(this._filePath);
                    sw.Stop();
                    TimeSpan ts = sw.Elapsed;

                    label1.Text = _NessusFile.ReportName + " |  " + ts.ToString();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Parsing Error: " + ex.Message);
                }
            }
        }

        private void LinqTestButton_Click(object sender, EventArgs e)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            if (File.Exists(_filePath) == false) { throw new FileNotFoundException("File Not Found", _filePath); }

            XElement NessusClientData_v2 = XElement.Load(_filePath);
            XNamespace aw = "http://www.nessus.org/cm";
            string reportName =
                (from el in NessusClientData_v2.Descendants("Report")
                 select el).First().Attribute("name").Value;

            ConcurrentDictionary<string, XElement> hostDictionary = new ConcurrentDictionary<string, XElement>();
            IEnumerable<XElement> reportHosts =
                from el in NessusClientData_v2.Descendants("ReportHost")
                select el;
            foreach(XElement reportHost in reportHosts)
            {
                if (hostDictionary.TryAdd(reportHost.Attribute("name").Value, reportHost))
                {

                }
                else
                {

                }
            }
            sw.Stop();
            TimeSpan ts = sw.Elapsed;

            label1.Text = reportName + " |  " + ts.ToString();
            
        }
    }
}
