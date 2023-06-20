using System.Reflection;
using System.Diagnostics;
using PSE.Model.Input.Interfaces;
using PSE.Model.Output.Interfaces;
using static PSE.Model.Common.Enumerations;

namespace PSE.Executor
{
    public partial class FormMain : Form
    {

        private readonly Extractor.Extractor _extractor;
        private readonly Builder.Builder _builder;

        public FormMain()
        {
            InitializeComponent();
            Assembly _assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo _fvi = FileVersionInfo.GetVersionInfo(_assembly.Location);
            this.Text += " - Ver. " + _fvi.FileVersion;
            _extractor = new Extractor.Extractor();
            _builder = new Builder.Builder();
        }

        private void browseFiles_Click(object sender, EventArgs e)
        {
            DialogResult _drFiles = this.openFileDialog.ShowDialog();
            if (_drFiles == DialogResult.OK)
            {
                this.listViewSourceFiles.Items.Clear();
                foreach (string _fileName in this.openFileDialog.FileNames)
                {
                    this.listViewSourceFiles.Items.Add(_fileName);
                }
            }
            this.buttonExtraction.Enabled = this.listViewSourceFiles.Items.Count > 0;
        }

        private void buttonExtraction_Click(object sender, EventArgs e)
        {
            if (this.listViewSourceFiles.Items.Count > 0)
            {
                string _tmpNodeKey;
                TreeNode _fileLogs;
                this.treeViewLog.Nodes.Clear();
                for (int _f = 0; _f < this.listViewSourceFiles.Items.Count; _f++)
                {
                    _fileLogs = new TreeNode("File to extract: '" + this.listViewSourceFiles.Items[_f].Text + "'");
                    IExtractedData _extrData = _extractor.Extract(File.ReadAllBytes(this.listViewSourceFiles.Items[_f].Text));
                    _fileLogs.Nodes.Add("Stream length: " + _extrData.ExtractionLog.StreamLength.ToString());
                    if (_extrData.ExtractionLog.AcquisitionStart != null)
                        _fileLogs.Nodes.Add("Date/time extraction starting: " + ((DateTime)_extrData.ExtractionLog.AcquisitionStart).ToString("dd/MM/yyyy") + " " + ((DateTime)_extrData.ExtractionLog.AcquisitionStart).ToString("HH:mm:ss"));
                    if (_extrData.ExtractionLog.AcquisitionEnd != null)
                        _fileLogs.Nodes.Add("Date/time extraction ending: " + ((DateTime)_extrData.ExtractionLog.AcquisitionEnd).ToString("dd/MM/yyyy") + " " + ((DateTime)_extrData.ExtractionLog.AcquisitionEnd).ToString("HH:mm:ss"));
                    _fileLogs.Nodes.Add("Extraction outcome: " + _extrData.ExtractionLog.Outcome.ToString());
                    if (_extrData.ExtractionLog.RecordsLog != null && _extrData.ExtractionLog.RecordsLog.Any())
                    {
                        List<TreeNode> _errorNodes = new List<TreeNode>();
                        foreach (IRecordExtractionLog _error in _extrData.ExtractionLog.RecordsLog)
                        {
                            _errorNodes.Add(new TreeNode("Error message: " + _error.FurtherMessage));
                            if (_error.LineNumber != null && _error.LineNumber > 0)
                                _errorNodes.Add(new TreeNode("    - Line number: " + _error.LineNumber.ToString()));
                            if (!string.IsNullOrEmpty(_error.RecordTypeName))
                                _errorNodes.Add(new TreeNode("    - Record type name: " + _error.RecordTypeName));
                            if (!string.IsNullOrEmpty(_error.RecordInnerContent))
                                _errorNodes.Add(new TreeNode("    - Record inner content: " + _error.RecordInnerContent));
                            if (_error.ExceptionOccurred != null)
                                _errorNodes.Add(new TreeNode("    - Has inner exception bound: yes (" + _error.ExceptionOccurred.Message + ")"));
                        }
                        _tmpNodeKey = Guid.NewGuid().ToString();
                        _fileLogs.Nodes.Add(_tmpNodeKey, "Extraction errors occurred: ");
                        _fileLogs.Nodes[_tmpNodeKey].Nodes.AddRange(_errorNodes.ToArray());
                    }
                    if (_extrData.ExtractionLog.Outcome == StreamAcquisitionOutcomes.Success || _extrData.ExtractionLog.Outcome == StreamAcquisitionOutcomes.WithErrors)
                    {
                        List<TreeNode> _builderNodes = new List<TreeNode>();
                        _tmpNodeKey = Guid.NewGuid().ToString();
                        _fileLogs.Nodes.Add(_tmpNodeKey, "Data building:");
                        this.textBoxJson.Text = "";
                        this.buttonCopy.Enabled = false;
                        IBuiltData _builtData = _builder.Build(_extrData.ExtractedItems, BuildFormats.Json);
                        if (_builtData.BuildingLog.BuildingStart != null)
                            _builderNodes.Add(new TreeNode("Date/time built start: " + ((DateTime)_builtData.BuildingLog.BuildingStart).ToString("dd/MM/yyyy") + " " + ((DateTime)_builtData.BuildingLog.BuildingStart).ToString("HH:mm:ss")));
                        if (_builtData.BuildingLog.BuildingEnd != null)
                            _builderNodes.Add(new TreeNode("Date/time built end: " + ((DateTime)_builtData.BuildingLog.BuildingEnd).ToString("dd/MM/yyyy") + " " + ((DateTime)_builtData.BuildingLog.BuildingEnd).ToString("HH:mm:ss")));
                        _builderNodes.Add(new TreeNode("Built outcome: " + _builtData.BuildingLog.Outcome.ToString()));
                        if (!string.IsNullOrEmpty(_builtData.BuildingLog.FurtherErrorMessage))
                            _builderNodes.Add(new TreeNode("Error message: " + _builtData.BuildingLog.FurtherErrorMessage));
                        if (_builtData.BuildingLog.ExceptionOccurred != null)
                            _builderNodes.Add(new TreeNode("Inner exception bound: " + _builtData.BuildingLog.ExceptionOccurred.Message));
                        _fileLogs.Nodes[_tmpNodeKey].Nodes.AddRange(_builderNodes.ToArray());
                        if (_builtData.BuildingLog.Outcome == BuildingOutcomes.Success)
                        {
                            this.tabControl.SelectedIndex = 1;
                            this.textBoxJson.Text = _builtData.OutputData;
                            this.buttonCopy.Enabled = true;
                            this.buttonCopy.Focus();
                        }
                        else
                            this.tabControl.SelectedIndex = 0;
                    }
                    this.treeViewLog.Nodes.Add(_fileLogs);
                    this.treeViewLog.Nodes[0].ExpandAll();
                }
            }
        }

        private void buttonCopy_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(this.textBoxJson.Text))
            {
                Clipboard.SetText(this.textBoxJson.Text);
                MessageBox.Show("The output data have been copied to clipboard.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

    }

}