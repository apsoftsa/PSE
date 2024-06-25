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
                OutputContent outCont = ExtractionManager.ExtractFiles(fileContentList);
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
