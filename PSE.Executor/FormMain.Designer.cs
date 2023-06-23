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
        private void InitializeComponent()
        {
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
            pictureBoxLogo = new PictureBox();
            tabControl.SuspendLayout();
            tabPageLog.SuspendLayout();
            tabPageOutput.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxLogo).BeginInit();
            SuspendLayout();
            // 
            // buttonBrowseSourceFiles
            // 
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
            labelSourceFiles.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            labelSourceFiles.Location = new Point(12, 9);
            labelSourceFiles.Name = "labelSourceFiles";
            labelSourceFiles.Size = new Size(463, 21);
            labelSourceFiles.TabIndex = 3;
            labelSourceFiles.Text = "Files to import:";
            labelSourceFiles.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // buttonExtraction
            // 
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
            // pictureBoxLogo
            // 
            pictureBoxLogo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pictureBoxLogo.BorderStyle = BorderStyle.FixedSingle;
            pictureBoxLogo.Image = Properties.Resources.LogoApsoft;
            pictureBoxLogo.Location = new Point(606, 12);
            pictureBoxLogo.Name = "pictureBoxLogo";
            pictureBoxLogo.Size = new Size(106, 62);
            pictureBoxLogo.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBoxLogo.TabIndex = 6;
            pictureBoxLogo.TabStop = false;
            pictureBoxLogo.Visible = false;
            // 
            // FormMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(723, 450);
            Controls.Add(pictureBoxLogo);
            Controls.Add(tabControl);
            Controls.Add(buttonExtraction);
            Controls.Add(labelSourceFiles);
            Controls.Add(listViewSourceFiles);
            Controls.Add(buttonBrowseSourceFiles);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "FormMain";
            Text = "Portfolio Statement Extractor";
            tabControl.ResumeLayout(false);
            tabPageLog.ResumeLayout(false);
            tabPageOutput.ResumeLayout(false);
            tabPageOutput.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxLogo).EndInit();
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
        private PictureBox pictureBoxLogo;
        private Button buttonCopy;
    }
}