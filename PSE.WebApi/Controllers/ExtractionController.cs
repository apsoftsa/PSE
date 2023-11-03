using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using PSE.Model.Common;
using PSE.WebApi.ApplicationLogic;
using PSE.Model.Exchange;

namespace PSE.WebApi.Controllers
{

    [ApiController]
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class ExtractionController : Controller
    {

        public ExtractionController(AppSettings appSettings) 
        { 
            ExtractionManager.Initialize(appSettings);
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
