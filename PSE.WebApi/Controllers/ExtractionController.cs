using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PSE.WebApi.Controllers
{

    [ApiController]
    //[AllowAnonymous]
    [Route("api/[controller]")]
    public class ExtractionController : Controller
    {

        private FileContent ReadFile(IFormFile file) 
        {
            return new FileContent
            {
                Content = ReadContent(file),
                ContentDisposition = file.ContentDisposition,
                ContentType = file.ContentType,
                FileName = file.FileName,
                Length = file.Length,
                Name = file.Name,
            };
        }

        private string ReadContent(IFormFile file)
        {
            if (file.Length <= 0) return string.Empty;
            
            using var stream = file.OpenReadStream();
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Build([FromForm] List<IFormFile> files)
        {
            var fileContentList = files.Select(ReadFile).ToList();

            // Processo di elaborazione

            return Ok(fileContentList);
        }

    }

    public class FileContent
    {

        public string ContentType { get; init; }

        public string ContentDisposition { get; init; }

        public long Length { get; init; }

        public string Name { get; init; }        

        public string FileName { get; init; }

        public string Content { get; init; }
    }

}
