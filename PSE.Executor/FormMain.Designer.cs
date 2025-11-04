namespace PSE.Executor
{
    partial class FormMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            buttonBrowseSourceFiles = new Button();
            openFileDialog = new OpenFileDialog();
            listViewSourceFiles = new ListView();
            labelSourceFiles = new Label();
            buttonExtraction = new Button();
            tabControl = new TabControl();
            tabPageLog = new TabPage();
            treeViewLog = new TreeView();
            tabPageOutput = new TabPage();
            buttonCopy = new Button();
            textBoxJson = new TextBox();
            groupBoxExtractionOption = new GroupBox();
            checkBoxGenerateJson = new CheckBox();
            radioButtonDocx = new RadioButton();
            radioButtonPdf = new RadioButton();
            checkBoxGenerateFile = new CheckBox();
            tabControl.SuspendLayout();
            tabPageLog.SuspendLayout();
            tabPageOutput.SuspendLayout();
            groupBoxExtractionOption.SuspendLayout();
            SuspendLayout();
            // 
            // buttonBrowseSourceFiles
            // 
            buttonBrowseSourceFiles.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonBrowseSourceFiles.Location = new Point(481, 123);
            buttonBrowseSourceFiles.Name = "buttonBrowseSourceFiles";
            buttonBrowseSourceFiles.Size = new Size(75, 23);
            buttonBrowseSourceFiles.TabIndex = 0;
            buttonBrowseSourceFiles.Text = "Browse...";
            buttonBrowseSourceFiles.UseVisualStyleBackColor = true;
            buttonBrowseSourceFiles.Click += browseFiles_Click;
            // 
            // openFileDialog
            // 
            openFileDialog.DefaultExt = "*.txt";
            openFileDialog.Filter = "Text fles|*.txt";
            openFileDialog.Multiselect = true;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Title = "Source files selection...";
            // 
            // listViewSourceFiles
            // 
            listViewSourceFiles.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            listViewSourceFiles.FullRowSelect = true;
            listViewSourceFiles.Location = new Point(12, 33);
            listViewSourceFiles.Name = "listViewSourceFiles";
            listViewSourceFiles.Size = new Size(463, 113);
            listViewSourceFiles.TabIndex = 2;
            listViewSourceFiles.UseCompatibleStateImageBehavior = false;
            listViewSourceFiles.View = View.List;
            // 
            // labelSourceFiles
            // 
            labelSourceFiles.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            labelSourceFiles.Location = new Point(12, 9);
            labelSourceFiles.Name = "labelSourceFiles";
            labelSourceFiles.Size = new Size(463, 21);
            labelSourceFiles.TabIndex = 3;
            labelSourceFiles.Text = "Files to import:";
            labelSourceFiles.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // buttonExtraction
            // 
            buttonExtraction.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonExtraction.Enabled = false;
            buttonExtraction.Location = new Point(562, 123);
            buttonExtraction.Name = "buttonExtraction";
            buttonExtraction.Size = new Size(150, 23);
            buttonExtraction.TabIndex = 4;
            buttonExtraction.Text = "Start extraction...";
            buttonExtraction.UseVisualStyleBackColor = true;
            buttonExtraction.Click += buttonExtraction_Click;
            // 
            // tabControl
            // 
            tabControl.Alignment = TabAlignment.Left;
            tabControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabControl.Controls.Add(tabPageLog);
            tabControl.Controls.Add(tabPageOutput);
            tabControl.Location = new Point(12, 164);
            tabControl.Multiline = true;
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(700, 274);
            tabControl.TabIndex = 5;
            // 
            // tabPageLog
            // 
            tabPageLog.BackColor = SystemColors.Control;
            tabPageLog.Controls.Add(treeViewLog);
            tabPageLog.Location = new Point(27, 4);
            tabPageLog.Name = "tabPageLog";
            tabPageLog.Padding = new Padding(3);
            tabPageLog.Size = new Size(669, 266);
            tabPageLog.TabIndex = 0;
            tabPageLog.Text = "Log";
            // 
            // treeViewLog
            // 
            treeViewLog.Dock = DockStyle.Fill;
            treeViewLog.Location = new Point(3, 3);
            treeViewLog.Name = "treeViewLog";
            treeViewLog.Size = new Size(663, 260);
            treeViewLog.TabIndex = 0;
            // 
            // tabPageOutput
            // 
            tabPageOutput.BackColor = SystemColors.Control;
            tabPageOutput.Controls.Add(buttonCopy);
            tabPageOutput.Controls.Add(textBoxJson);
            tabPageOutput.Location = new Point(27, 4);
            tabPageOutput.Name = "tabPageOutput";
            tabPageOutput.Padding = new Padding(3);
            tabPageOutput.Size = new Size(669, 266);
            tabPageOutput.TabIndex = 1;
            tabPageOutput.Text = "Output";
            // 
            // buttonCopy
            // 
            buttonCopy.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCopy.Enabled = false;
            buttonCopy.Location = new Point(516, 237);
            buttonCopy.Name = "buttonCopy";
            buttonCopy.Size = new Size(150, 23);
            buttonCopy.TabIndex = 1;
            buttonCopy.Text = "Copy to clipboard";
            buttonCopy.UseVisualStyleBackColor = true;
            buttonCopy.Click += buttonCopy_Click;
            // 
            // textBoxJson
            // 
            textBoxJson.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            textBoxJson.BackColor = Color.White;
            textBoxJson.Location = new Point(3, 3);
            textBoxJson.Multiline = true;
            textBoxJson.Name = "textBoxJson";
            textBoxJson.ReadOnly = true;
            textBoxJson.ScrollBars = ScrollBars.Vertical;
            textBoxJson.Size = new Size(663, 228);
            textBoxJson.TabIndex = 0;
            // 
            // groupBoxExtractionOption
            // 
            groupBoxExtractionOption.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            groupBoxExtractionOption.Controls.Add(checkBoxGenerateJson);
            groupBoxExtractionOption.Controls.Add(radioButtonDocx);
            groupBoxExtractionOption.Controls.Add(radioButtonPdf);
            groupBoxExtractionOption.Controls.Add(checkBoxGenerateFile);
            groupBoxExtractionOption.Location = new Point(562, 12);
            groupBoxExtractionOption.Name = "groupBoxExtractionOption";
            groupBoxExtractionOption.Size = new Size(149, 105);
            groupBoxExtractionOption.TabIndex = 8;
            groupBoxExtractionOption.TabStop = false;
            groupBoxExtractionOption.Text = "Extraction options:";
            // 
            // checkBoxGenerateJson
            // 
            checkBoxGenerateJson.AutoSize = true;
            checkBoxGenerateJson.Checked = true;
            checkBoxGenerateJson.CheckState = CheckState.Checked;
            checkBoxGenerateJson.Location = new Point(6, 26);
            checkBoxGenerateJson.Name = "checkBoxGenerateJson";
            checkBoxGenerateJson.Size = new Size(98, 19);
            checkBoxGenerateJson.TabIndex = 11;
            checkBoxGenerateJson.Text = "Generate json";
            checkBoxGenerateJson.UseVisualStyleBackColor = true;
            checkBoxGenerateJson.CheckedChanged += checkBoxGenerateJson_CheckedChanged;
            // 
            // radioButtonDocx
            // 
            radioButtonDocx.AutoSize = true;
            radioButtonDocx.Enabled = false;
            radioButtonDocx.Location = new Point(68, 80);
            radioButtonDocx.Name = "radioButtonDocx";
            radioButtonDocx.Size = new Size(53, 19);
            radioButtonDocx.TabIndex = 10;
            radioButtonDocx.Text = ".docx";
            radioButtonDocx.UseVisualStyleBackColor = true;
            // 
            // radioButtonPdf
            // 
            radioButtonPdf.AutoSize = true;
            radioButtonPdf.Checked = true;
            radioButtonPdf.Enabled = false;
            radioButtonPdf.Location = new Point(7, 80);
            radioButtonPdf.Name = "radioButtonPdf";
            radioButtonPdf.Size = new Size(46, 19);
            radioButtonPdf.TabIndex = 9;
            radioButtonPdf.TabStop = true;
            radioButtonPdf.Text = ".pdf";
            radioButtonPdf.UseVisualStyleBackColor = true;
            // 
            // checkBoxGenerateFile
            // 
            checkBoxGenerateFile.AutoSize = true;
            checkBoxGenerateFile.Location = new Point(6, 55);
            checkBoxGenerateFile.Name = "checkBoxGenerateFile";
            checkBoxGenerateFile.Size = new Size(92, 19);
            checkBoxGenerateFile.TabIndex = 8;
            checkBoxGenerateFile.Text = "Generate file";
            checkBoxGenerateFile.UseVisualStyleBackColor = true;
            checkBoxGenerateFile.CheckedChanged += checkBoxGenerateFile_CheckedChanged;
            // 
            // FormMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(723, 450);
            Controls.Add(groupBoxExtractionOption);
            Controls.Add(tabControl);
            Controls.Add(buttonExtraction);
            Controls.Add(labelSourceFiles);
            Controls.Add(listViewSourceFiles);
            Controls.Add(buttonBrowseSourceFiles);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "FormMain";
            Text = "Portfolio Statement Extractor";
            Load += FormMain_Load;
            tabControl.ResumeLayout(false);
            tabPageLog.ResumeLayout(false);
            tabPageOutput.ResumeLayout(false);
            tabPageOutput.PerformLayout();
            groupBoxExtractionOption.ResumeLayout(false);
            groupBoxExtractionOption.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Button buttonBrowseSourceFiles;
        private OpenFileDialog openFileDialog;
        private ListView listViewSourceFiles;
        private Label labelSourceFiles;
        private Button buttonExtraction;
        private TabControl tabControl;
        private TabPage tabPageLog;
        private TabPage tabPageOutput;
        private TextBox textBoxJson;
        private TreeView treeViewLog;
        private Button buttonCopy;
        private GroupBox groupBoxExtractionOption;
        private RadioButton radioButtonDocx;
        private RadioButton radioButtonPdf;
        private CheckBox checkBoxGenerateFile;
        private CheckBox checkBoxGenerateJson;
    }
}