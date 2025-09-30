#define ALLOW_ANONYMOUS_NO

using System.Diagnostics;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using PSE.Dictionary;
using PSE.Model.Common;
using PSE.Model.Exchange;
using PSE.WebApi.ApplicationLogic;

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

        [AllowAnonymous]
        [HttpGet("Version")]        
        public ActionResult<string> GetVersion()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            return Ok(fvi.FileVersion);
        }

        [HttpPost("Build")]
        public ActionResult<OutputContent> Build([FromForm] List<IFormFile> files)
        {
            if (files != null && files.Any())
            {
                string outputJson = string.Empty;
                var fileContentList = files.Select(ExtractionManager.ReadFile).ToList();
                OutputContent outCont = ExtractionManager.ExtractFiles(_dictionaryService, fileContentList);
                if (outCont != null && outCont.JsonGenerated != string.Empty)
                    return Ok(JsonConvert.SerializeObject(outCont));
                else
                    return BadRequest();
            }
            else
                return NotFound();
        }

    }

}
