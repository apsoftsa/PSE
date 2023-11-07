using System.Reflection;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PSE.Model.Exchange;

namespace PSE.Executor
{

    public partial class FormMain : Form
    {

        private readonly IConfiguration _configuration;        
        private readonly string _webApiUrl;

        public FormMain(IConfiguration configuration)
        {
            InitializeComponent();
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(GlobalExceptionHandler);
            _configuration = configuration;
            Assembly _assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo _fvi = FileVersionInfo.GetVersionInfo(_assembly.Location);
            this.Text += " - Ver. " + _fvi.FileVersion;            
            _webApiUrl = _configuration["WebApiSettings:Url"];
            if(!_webApiUrl.EndsWith("/")) _webApiUrl += "/";
            _webApiUrl += "api/Extraction/";
        }

        private void UnexpectedExceptionMangement(Exception ex)
        {
            this.treeViewLog.Nodes.Clear();
            this.treeViewLog.Nodes.Add("exception", "UNEXPECTED EXCEPTION OCCURRED:");
            this.treeViewLog.Nodes[0].Nodes.Add("Error: " + ex.Message);
            this.treeViewLog.Nodes[0].Nodes.Add("Source: " + ex.Source);
            this.treeViewLog.Nodes[0].Nodes.Add("Stack trace: " + ex.StackTrace);
            if (ex.InnerException != null)
                this.treeViewLog.Nodes[0].Nodes.Add("Inner exception: " + ex.InnerException.Message);
            this.treeViewLog.ExpandAll();
            this.tabControl.SelectedIndex = 0;
            this.buttonExtraction.Enabled = true;
        }

        private void GlobalExceptionHandler(object sender, UnhandledExceptionEventArgs args)
        {
            Exception _ex = (Exception)args.ExceptionObject;
            UnexpectedExceptionMangement(_ex);
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

        private async void buttonExtraction_Click(object sender, EventArgs e)
        {
            string _oldCaption = this.Text;
            try
            {
                if (this.listViewSourceFiles.Items.Count > 0)
                {                    
                    this.Text = "Please wait, extraction in progress...";
                    this.buttonExtraction.Enabled = false;
                    this.treeViewLog.Nodes.Clear();
                    this.treeViewLog.Nodes.Add("extraction", "EXTRACTION");
                    this.treeViewLog.Nodes.Add("building", "BUILDING");
                    this.Refresh();
                    using (var _client = new HttpClient(new HttpClientHandler { UseDefaultCredentials = true }))
                    {
                        using (var _formContent = new MultipartFormDataContent())
                        {
                            FileInfo _fileInfo;
                            StreamContent _fileContent;
                            for (int _f = 0; _f < this.listViewSourceFiles.Items.Count; _f++)
                            {
                                _fileInfo = new FileInfo(this.listViewSourceFiles.Items[_f].Text);
                                _fileContent = new StreamContent(File.OpenRead(this.listViewSourceFiles.Items[_f].Text));
                                _formContent.Add(_fileContent, "files", _fileInfo.Name);
                            }
                            var _response = await _client.PostAsync(_webApiUrl + "build", _formContent);
                            if (_response.IsSuccessStatusCode)
                            {
                                OutputContent _outCont = JsonConvert.DeserializeObject<OutputContent>(await _response.Content.ReadAsStringAsync());
                                this.textBoxJson.Text = _outCont?.JsonGenerated;
                                if (_outCont.Logs != null && _outCont.Logs.Any())
                                {
                                    TreeNode _node;
                                    foreach (OutputLog _log in _outCont.Logs)
                                    {
                                        if (this.treeViewLog.Nodes.ContainsKey(_log.ActivityType))
                                        {
                                            _node = new TreeNode(_log.Content);
                                            if (_log.Childs != null && _log.Childs.Any())
                                            {
                                                foreach (OutputLog _subLog in _log.Childs)
                                                {
                                                    _node.Nodes.Add(_subLog.Content);
                                                }
                                            }
                                            this.treeViewLog.Nodes[_log.ActivityType].Nodes.Add(_node);
                                        }
                                    }
                                }
                                this.treeViewLog.Nodes["extraction"].ExpandAll();
                                this.treeViewLog.Nodes["building"].ExpandAll();
                                this.tabControl.SelectedIndex = 1;
                                this.buttonCopy.Enabled = true;
                                this.buttonCopy.Focus();
                            }
                            else
                            {
                                List<TreeNode> _errorNodes = new List<TreeNode>();
                                if (_response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                                {
                                    _errorNodes.Add(new TreeNode("Error occurred: BAD REQUEST [400] (Extraction operation failed!)"));
                                    this.tabControl.SelectedIndex = 0;
                                }
                                else if (_response.StatusCode == System.Net.HttpStatusCode.NotFound)
                                {
                                    _errorNodes.Add(new TreeNode("Error occurred: NOT FOUND [404] (No valid input files received!)"));
                                    this.tabControl.SelectedIndex = 0;
                                }
                                else if (_response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                                {
                                    _errorNodes.Add(new TreeNode("Error occurred: UNAUTHORIZED [401] (User without enough privileges to proceed!)"));
                                    this.tabControl.SelectedIndex = 0;
                                }
                                else if (_response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                                {
                                    _errorNodes.Add(new TreeNode("Error occurred: INTERNAL SERVER ERROR [500] (Internal server error occurred during request elaboration!)"));
                                    this.tabControl.SelectedIndex = 0;
                                }
                                this.treeViewLog.Nodes["extraction"].Nodes.AddRange(_errorNodes.ToArray());
                                this.treeViewLog.Nodes["extraction"].ExpandAll();
                            }
                        }
                    }
                    this.buttonExtraction.Enabled = true;                    
                }
            }
            catch(Exception _ex)
            {
                UnexpectedExceptionMangement(_ex);
            }
            finally
            {
                this.Text = _oldCaption;
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