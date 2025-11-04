using System.Diagnostics;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PSE.Model.Exchange;

namespace PSE.Executor
{

    public partial class FormMain : Form {

        private readonly IConfiguration? _configuration;
        private readonly string _webApiUrl;

        public FormMain(IConfiguration? configuration) {
            InitializeComponent();
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(GlobalExceptionHandler);
            _configuration = configuration;
            _webApiUrl = _configuration != null ? (_configuration["WebApiSettings:Url"] ?? string.Empty) : string.Empty;
            if (!_webApiUrl.EndsWith("/"))
                _webApiUrl += "/";
            _webApiUrl += "api/Extraction/";
        }

        private static string? GetFileNameFromContentDisposition(ContentDispositionHeaderValue? contentDisposition) {
            string? fileName = null;
            if (contentDisposition != null) {
                fileName = contentDisposition.FileNameStar ?? contentDisposition.FileName;
                if (!string.IsNullOrWhiteSpace(fileName))
                    fileName = fileName.Trim('"');
            }
            return fileName;
        }       

        private static async Task OpenFileManager(string fileName, byte[] fileContent) {
            string tempDir = Path.GetTempPath();
            string fullPath = Path.Combine(tempDir, fileName);
            await File.WriteAllBytesAsync(fullPath, fileContent);
            var psi = new ProcessStartInfo {
                FileName = fullPath,
                UseShellExecute = true
            };
            Process.Start(psi);
        }

        private void ShowLogs(List<OutputLog>? logs) {
            if (logs != null && logs.Any()) {
                TreeNode node;
                foreach (OutputLog log in logs) {
                    if (this.treeViewLog.Nodes.ContainsKey(log.ActivityType)) {
                        node = new TreeNode(log.Content);
                        if (log.Childs != null && log.Childs.Any()) {
                            foreach (OutputLog subLog in log.Childs) {
                                node.Nodes.Add(subLog.Content);
                            }
                        }
                        this.treeViewLog.Nodes[log.ActivityType].Nodes.Add(node);
                    }
                }
            }
        }

        private async void FormMain_Load(object sender, EventArgs e) {
            using (var client = new HttpClient(new HttpClientHandler { UseDefaultCredentials = false })) {
                var response = await client.GetAsync(_webApiUrl + "version");
                if (response.IsSuccessStatusCode)
                    this.Text += " - Ver. " + await response.Content.ReadAsStringAsync();
                else
                    this.Text += " - Ver. [UNKNOWN]";
            }
        }

        private void UnexpectedExceptionMangement(Exception ex) {
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

        private void GlobalExceptionHandler(object sender, UnhandledExceptionEventArgs args) {
            Exception ex = (Exception)args.ExceptionObject;
            UnexpectedExceptionMangement(ex);
        }

        private void browseFiles_Click(object sender, EventArgs e) {
            DialogResult drFiles = this.openFileDialog.ShowDialog();
            if (drFiles == DialogResult.OK) {
                this.listViewSourceFiles.Items.Clear();
                foreach (string fileName in this.openFileDialog.FileNames) {
                    this.listViewSourceFiles.Items.Add(fileName);
                }
            }
            this.buttonExtraction.Enabled = this.listViewSourceFiles.Items.Count > 0;
        }

        private async void buttonExtraction_Click(object sender, EventArgs e) {
            string oldCaption = this.Text;
            try {
                if (this.listViewSourceFiles.Items.Count > 0) {
                    string apiToCall = "buildOnlyJson";
                    if (this.checkBoxGenerateFile.Checked) {
                        if (this.checkBoxGenerateJson.Checked)
                            apiToCall = this.radioButtonPdf.Checked ? "buildJsonAndPdf" : "buildJsonAndDocx";
                        else
                            apiToCall = this.radioButtonPdf.Checked ? "buildPdf" : "buildDocx";
                    }
                    this.Text = "Please wait, extraction in progress...";
                    this.buttonExtraction.Enabled = false;
                    this.textBoxJson.Text = "";
                    this.treeViewLog.Nodes.Clear();
                    this.treeViewLog.Nodes.Add("extraction", "EXTRACTION");
                    this.treeViewLog.Nodes.Add("building", "BUILDING");
                    this.Refresh();
                    using (var client = new HttpClient(new HttpClientHandler { UseDefaultCredentials = true })) {
                        using (var formContent = new MultipartFormDataContent()) {
                            FileInfo fileInfo;
                            StreamContent fileContent;
                            for (int f = 0; f < this.listViewSourceFiles.Items.Count; f++) {
                                fileInfo = new FileInfo(this.listViewSourceFiles.Items[f].Text);
                                fileContent = new StreamContent(File.OpenRead(this.listViewSourceFiles.Items[f].Text));
                                formContent.Add(fileContent, "files", fileInfo.Name);
                            }
                            var response = await client.PostAsync(_webApiUrl + apiToCall, formContent);
                            if (response.IsSuccessStatusCode) {
                                if (this.checkBoxGenerateFile.Checked) {
                                    if (this.checkBoxGenerateJson.Checked) {
                                        OutputContentWithFile outCont = JsonConvert.DeserializeObject<OutputContentWithFile>(await response.Content.ReadAsStringAsync());
                                        this.textBoxJson.Text = outCont?.JsonGenerated;
                                        this.ShowLogs(outCont?.Logs);
                                        await OpenFileManager(outCont.FileName, outCont.FileContent);
                                    } else {
                                        byte[] fileBytes = await response.Content.ReadAsByteArrayAsync();
                                        string fileName = GetFileNameFromContentDisposition(response.Content.Headers.ContentDisposition)
                                            ?? $"Report_{DateTime.UtcNow:yyyyMMdd_HHmmss}" + (this.radioButtonPdf.Checked ? this.radioButtonPdf.Text : this.radioButtonDocx.Text);
                                        await OpenFileManager(fileName, fileBytes);
                                    }
                                } else {
                                    OutputContent outCont = JsonConvert.DeserializeObject<OutputContent>(await response.Content.ReadAsStringAsync());
                                    this.textBoxJson.Text = outCont?.JsonGenerated;
                                    this.ShowLogs(outCont?.Logs);
                                }
                                this.treeViewLog.Nodes["extraction"].ExpandAll();
                                this.treeViewLog.Nodes["building"].ExpandAll();
                                this.tabControl.SelectedIndex = 1;
                                this.buttonCopy.Enabled = true;
                                this.buttonCopy.Focus();
                            } else {
                                List<TreeNode> errorNodes = new List<TreeNode>();
                                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest) {
                                    errorNodes.Add(new TreeNode("Error occurred: BAD REQUEST [400] (Extraction operation failed!)"));
                                    this.tabControl.SelectedIndex = 0;
                                } else if (response.StatusCode == System.Net.HttpStatusCode.NotFound) {
                                    errorNodes.Add(new TreeNode("Error occurred: NOT FOUND [404] (No valid input files received!)"));
                                    this.tabControl.SelectedIndex = 0;
                                } else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized) {
                                    errorNodes.Add(new TreeNode("Error occurred: UNAUTHORIZED [401] (User without enough privileges to proceed!)"));
                                    this.tabControl.SelectedIndex = 0;
                                } else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError) {
                                    errorNodes.Add(new TreeNode("Error occurred: INTERNAL SERVER ERROR [500] (Internal server error occurred during request elaboration!)"));
                                    this.tabControl.SelectedIndex = 0;
                                }
                                this.treeViewLog.Nodes["extraction"].Nodes.AddRange(errorNodes.ToArray());
                                this.treeViewLog.Nodes["extraction"].ExpandAll();
                            }
                        }
                    }
                    this.buttonExtraction.Enabled = true;
                }
            } catch (Exception ex) {
                UnexpectedExceptionMangement(ex);
            } finally {
                this.Text = oldCaption;
            }
        }

        private void buttonCopy_Click(object sender, EventArgs e) {
            if (!string.IsNullOrWhiteSpace(this.textBoxJson.Text)) {
                Clipboard.SetText(this.textBoxJson.Text);
                MessageBox.Show("The output data have been copied to clipboard.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void checkBoxGenerateJson_CheckedChanged(object sender, EventArgs e) {
            if (this.checkBoxGenerateJson.Checked == false && this.checkBoxGenerateFile.Checked == false)
                this.checkBoxGenerateFile.Checked = true;
        }

        private void checkBoxGenerateFile_CheckedChanged(object sender, EventArgs e) {
            this.radioButtonPdf.Enabled = this.checkBoxGenerateFile.Checked;
            this.radioButtonDocx.Enabled = this.checkBoxGenerateFile.Checked;
            if (!this.checkBoxGenerateFile.Checked)
                this.checkBoxGenerateJson.Checked = true;
        }

    }

}