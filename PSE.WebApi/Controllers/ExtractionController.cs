#define ALLOW_ANONYMOUS_NO

using Microsoft.AspNetCore.Mvc;
#if ALLOW_ANONYMOUS
using Microsoft.AspNetCore.Authorization;
#endif
using Newtonsoft.Json;
using PSE.Model.Common;
using PSE.WebApi.ApplicationLogic;
using PSE.Model.Exchange;
using System.Diagnostics;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;

namespace PSE.WebApi.Controllers
{

    [ApiController]
#if ALLOW_ANONYMOUS
    [AllowAnonymous]
#endif
    [Route("api/[controller]")]
    public class ExtractionController : Controller
    {

        public ExtractionController(AppSettings appSettings) 
        { 
            ExtractionManager.Initialize(appSettings);
        }

        [AllowAnonymous]
        [HttpGet("Version")]        
        public ActionResult<string> GetVersion()
        {
            Assembly _assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo _fvi = FileVersionInfo.GetVersionInfo(_assembly.Location);
            return Ok(_fvi.FileVersion);
        }

        [HttpPost("Build")]
        public ActionResult<OutputContent> Build([FromForm] List<IFormFile> files)
        {
            if (files != null && files.Any())
            {
                string outputJson = string.Empty;
                var fileContentList = files.Select(ExtractionManager.ReadFile).ToList();
                OutputContent _outCont = ExtractionManager.ExtractFiles(fileContentList);
                if (_outCont != null && _outCont.JsonGenerated != string.Empty)
                    return Ok(JsonConvert.SerializeObject(_outCont));
                else
                    return BadRequest();
            }
            else
                return NotFound();
        }

    }

}
