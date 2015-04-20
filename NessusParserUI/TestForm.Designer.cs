namespace NessusParserUI
{
    partial class TestForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.FilePathTextBox = new System.Windows.Forms.TextBox();
            this.BrowseForFileButton = new System.Windows.Forms.Button();
            this.VTXTestButton = new System.Windows.Forms.Button();
            this.LinqTestButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.testFormBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.testFormBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // FilePathTextBox
            // 
            this.FilePathTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.testFormBindingSource, "FilePath", true));
            this.FilePathTextBox.Location = new System.Drawing.Point(15, 16);
            this.FilePathTextBox.Name = "FilePathTextBox";
            this.FilePathTextBox.Size = new System.Drawing.Size(234, 20);
            this.FilePathTextBox.TabIndex = 0;
            this.FilePathTextBox.TabStop = false;
            // 
            // BrowseForFileButton
            // 
            this.BrowseForFileButton.Location = new System.Drawing.Point(255, 17);
            this.BrowseForFileButton.Name = "BrowseForFileButton";
            this.BrowseForFileButton.Size = new System.Drawing.Size(25, 19);
            this.BrowseForFileButton.TabIndex = 1;
            this.BrowseForFileButton.Text = "...";
            this.BrowseForFileButton.UseVisualStyleBackColor = true;
            this.BrowseForFileButton.Click += new System.EventHandler(this.BrowseForFileButton_Click);
            // 
            // VTXTestButton
            // 
            this.VTXTestButton.Location = new System.Drawing.Point(20, 46);
            this.VTXTestButton.Name = "VTXTestButton";
            this.VTXTestButton.Size = new System.Drawing.Size(83, 25);
            this.VTXTestButton.TabIndex = 2;
            this.VTXTestButton.Text = "vtx.FileUtilities";
            this.VTXTestButton.UseVisualStyleBackColor = true;
            this.VTXTestButton.Click += new System.EventHandler(this.VTXTestButton_Click);
            // 
            // LinqTestButton
            // 
            this.LinqTestButton.Location = new System.Drawing.Point(109, 46);
            this.LinqTestButton.Name = "LinqTestButton";
            this.LinqTestButton.Size = new System.Drawing.Size(83, 25);
            this.LinqTestButton.TabIndex = 3;
            this.LinqTestButton.Text = "XMLReader";
            this.LinqTestButton.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(120, 228);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.testFormBindingSource, "FilePath", true));
            this.label1.Location = new System.Drawing.Point(12, 97);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "label1";
            // 
            // testFormBindingSource
            // 
            this.testFormBindingSource.DataSource = typeof(NessusParserUI.TestForm);
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.LinqTestButton);
            this.Controls.Add(this.VTXTestButton);
            this.Controls.Add(this.BrowseForFileButton);
            this.Controls.Add(this.FilePathTextBox);
            this.Name = "TestForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Text = "TestForm";
            this.Load += new System.EventHandler(this.TestForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.testFormBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox FilePathTextBox;
        private System.Windows.Forms.Button BrowseForFileButton;
        private System.Windows.Forms.Button VTXTestButton;
        private System.Windows.Forms.Button LinqTestButton;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.BindingSource testFormBindingSource;
        private System.Windows.Forms.Label label1;
    }
}

