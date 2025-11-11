#define ALLOW_ANONYMOUS_NO

using System.Reflection;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using PSE.Dictionary;
using PSE.Model.Common;
using PSE.Model.Exchange;
using PSE.Reporting.Reports;
using PSE.WebApi.ApplicationLogic;
using PSE.WebApi.ApplicationSettings;

namespace PSE.WebApi.Controllers
{

    [ApiController]
#if ALLOW_ANONYMOUS
    [AllowAnonymous]
#endif
    [Route("api/[controller]")]
    public class ExtractionController : Controller
    {

        private readonly IPSEDictionaryService _dictionaryService;

        public ExtractionController(AppSettings appSettings, IPSEDictionaryService dictionaryService) 
        { 
            ExtractionManager.Initialize(appSettings);
            _dictionaryService = dictionaryService; 
        }

        private static string GenerateFileName(string baseFileName, string fileType) {
            string dateTimeFile = DateTime.Today.Year.ToString()
                   + "_" + DateTime.Today.Month.ToString().PadLeft(2, '0')
                   + "_" + DateTime.Today.Day.ToString().PadLeft(2, '0')
                   + "_" + DateTime.Now.Hour.ToString().PadLeft(2, '0')
                   + "_" + DateTime.Now.Minute.ToString().PadLeft(2, '0')
                   + "_" + DateTime.Now.Second.ToString().PadLeft(2, '0')
                   + "_";
            return dateTimeFile + baseFileName.Split(".").First() + "." + fileType;
        }

        private static async Task<MemoryStream?> GenerateReport(IOutputContent outCont, string outputFileName, string fileType) {
            MemoryStream? ms = null;
            if (outCont != null && outCont.JsonGenerated != string.Empty) {
                ReportPSE report = new();
                ReportConfigurator.FixJsonConnections(report, outCont.JsonGenerated);
                ms = new MemoryStream();
                if (fileType == "docx")
                    await report.ExportToDocxAsync(ms);
                else
                    await report.ExportToPdfAsync(ms);
            }
            return ms;   
        }

        private async Task<ActionResult<OutputContent>> BuildJsonAndFile([FromForm] List<IFormFile> files, string fileType) {
            try {
                if (files != null && files.Any()) {
                    var fileContentList = files.Select(ExtractionManager.ReadFile).ToList();
                    IOutputContent outCont = ExtractionManager.ExtractFiles(_dictionaryService, fileContentList);
                    if (outCont != null && outCont.JsonGenerated != string.Empty) {
                        string outputFileName = GenerateFileName(fileContentList.First().FileName, fileType);
                        using var ms = await GenerateReport(outCont, outputFileName, fileType);
                        OutputContentWithFile outContWithFile = new(outCont) {
                            FileName = outputFileName,
                            FileContent = ms.ToArray()
                        };
                        return Ok(JsonConvert.SerializeObject(outContWithFile));
                    } else
                        return BadRequest();
                } else
                    return NotFound();
            } catch (Exception ex) {
                return BadRequest(ex.Message);  
            }
        }

        private async Task<IActionResult> BuildOnlyFile([FromForm] List<IFormFile> files, string fileType) {
            try {
                if (files != null && files.Any()) {
                    var fileContentList = files.Select(ExtractionManager.ReadFile).ToList();
                    IOutputContent outCont = ExtractionManager.ExtractFiles(_dictionaryService, fileContentList);
                    if (outCont != null && outCont.JsonGenerated != string.Empty) {
                        string outputFileName = GenerateFileName(fileContentList.First().FileName, fileType);
                        using var ms = await GenerateReport(outCont, outputFileName, fileType);
                        string fileTypeMime = fileType == "pdf" ? "application/pdf" : "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                        return File(ms.ToArray(), fileTypeMime, outputFileName);
                    } else
                        return BadRequest();
                } else
                    return NotFound();
            } catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("Version")]        
        public ActionResult<string> GetVersion()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            return Ok(fvi.FileVersion);
        }

        // Old method, only for compatibility with the previous versions of the client .exe
        [HttpPost("Build")]
        public ActionResult<OutputContent> Build([FromForm] List<IFormFile> files) {
            if (files != null && files.Any()) {
                string outputJson = string.Empty;
                var fileContentList = files.Select(ExtractionManager.ReadFile).ToList();
                IOutputContent outCont = ExtractionManager.ExtractFiles(_dictionaryService, fileContentList);
                if (outCont != null && outCont.JsonGenerated != string.Empty)
                    return Ok(JsonConvert.SerializeObject(outCont));
                else
                    return BadRequest();
            } else
                return NotFound();
        }

        [HttpPost("BuildOnlyJson")]
        public ActionResult<OutputContent> BuildOnlyJson([FromForm] List<IFormFile> files)
        {
            if (files != null && files.Any())
            {
                var fileContentList = files.Select(ExtractionManager.ReadFile).ToList();
                IOutputContent outCont = ExtractionManager.ExtractFiles(_dictionaryService, fileContentList);
                if (outCont != null && outCont.JsonGenerated != string.Empty)
                    return Ok(JsonConvert.SerializeObject(outCont));
                else
                    return BadRequest();
            }
            else
                return NotFound();
        }

        [HttpPost("BuildJsonAndPdf")]
        public async Task<ActionResult<OutputContent>> BuildJsonWithPdf([FromForm] List<IFormFile> files) {
            return await BuildJsonAndFile(files, "pdf");
        }

        [HttpPost("BuildJsonAndDocx")]
        public async Task<ActionResult<OutputContent>> BuildJsonWithDocx([FromForm] List<IFormFile> files) {
            return await BuildJsonAndFile(files, "docx");
        }

        [HttpPost("BuildPdf")]
        public async Task<IActionResult> BuildPdf([FromForm] List<IFormFile> files) {
            return await BuildOnlyFile(files, "pdf");
        }

        [HttpPost("BuildDocx")]
        public async Task<IActionResult> BuildDocx([FromForm] List<IFormFile> files) {
            return await BuildOnlyFile(files, "docx");
        }

    }

}
