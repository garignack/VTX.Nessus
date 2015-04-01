using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NessusParserUI
{
    public partial class TestForm : Form
    {
        private string _filePath;
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

    }
}
