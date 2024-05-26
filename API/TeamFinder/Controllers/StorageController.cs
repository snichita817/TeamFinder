using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TeamFinder.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StorageController : Controller
    {
        private readonly string teamSubmissionsBucket = "team-submissions";
        private readonly string profilePhotosBucket = "profile-picture-uploads";

        public StorageController()
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetFile(string fileName, string bucket)
        {
            var client = StorageClient.Create();
            var stream = new MemoryStream();
            var obj = await client.DownloadObjectAsync(bucket, fileName, stream);
            stream.Position = 0;

            return File(stream, obj.ContentType, obj.Name);
        }

        [HttpPost]
        [Route("upload")]
        public async Task<IActionResult> AddFile(IFormFile file, string bucket)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            string? targetBucket = null;
            if (bucket == "team-submissions")
                targetBucket = teamSubmissionsBucket;
            else if (bucket == "profile-picture-uploads")
                targetBucket = profilePhotosBucket;

            if (targetBucket == null)
                return BadRequest("Invalid bucket specified.");

            var sanitizedFileName = SanitizeFileName(file.FileName);

            var client = StorageClient.Create();
            var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            stream.Position = 0;

            var obj = await client.UploadObjectAsync(
                targetBucket,
                sanitizedFileName,
                file.ContentType,
                stream);

            return Ok(obj.Name);
        }

        private string SanitizeFileName(string fileName)
        {
            return Regex.Replace(fileName, @"[:/\\*?""<>|]", "-").Replace(".", "-");
        }
    }
}
