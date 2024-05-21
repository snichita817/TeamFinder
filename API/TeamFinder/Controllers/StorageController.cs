using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Mvc;
using TeamFinder.Models.Domain;

namespace TeamFinder.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class StorageController : Controller
    {
        public StorageController()
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetFile(string fileName)
        {
            var client = StorageClient.Create();
            var stream = new MemoryStream();
            var obj = await client.DownloadObjectAsync("profile-picture-uploads", fileName, stream);
            stream.Position = 0;

            return File(stream, obj.ContentType, obj.Name);
        }

        [HttpPost]
        [Route("upload")]
        public async Task<IActionResult> AddFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var client = StorageClient.Create();
            var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            stream.Position = 0;

            var obj = await client.UploadObjectAsync(
                "profile-picture-uploads",
                file.FileName,
                file.ContentType,
                stream);

            return Ok(obj.Name);
        }
    }
}