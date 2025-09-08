using AwsS3.Models;
using AwsS3.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors; 
using Microsoft.AspNetCore.Authorization;

namespace server.Controllers{

    [ApiController]
    [Route("[controller]")]
    [EnableCors("AllowAngularDev")]
    [RequestSizeLimit(100_000_000)] 
    [Authorize] 
    public class ImageController: ControllerBase{
        private readonly IStorageService _storageService;
        private readonly IConfiguration _config;

        public ImageController(IConfiguration config, IStorageService storageService){
            _config = config;
            _storageService = storageService;
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile([FromForm] IFormFile File, [FromForm] string UserId) {

        await using var memoryStream = new MemoryStream();
        await File.CopyToAsync(memoryStream);

        var fileExt = Path.GetExtension(File.FileName); 
        var docName = $"{Guid.NewGuid().ToString()}{fileExt}"; 
        var s3Key = $"{UserId}/{docName}";

        var s3Obj = new S3Object()
        {
            BucketName = "loop-social",
            InputStream = memoryStream,
            Name = s3Key
        };

        var accessKey = _config["AwsConfiguration:AWSAccessKey"];
        if (accessKey == null)
            throw new ArgumentNullException("AwsConfiguration:AWSAccessKey is not in configuration.");

        var secretKey = _config["AwsConfiguration:AWSSecretKey"];
        if (secretKey == null)
            throw new ArgumentNullException("AwsConfiguration:AWSSecretKey is not in configuration.");

        var cred = new AwsCredentials()
        {
            AccessKey = accessKey,
            SecretKey = secretKey
        };


        var result = await _storageService.UploadFileAsync(s3Obj, cred);
        return Ok(new { message = "Uploaded successfully", url = result.Url, key=docName });
        }

        [HttpDelete("{userId}/{*key}")]
        public async Task<IActionResult> DeleteFile(string userId, string key)
        {
                var s3Obj = new S3Object()
                {
                    BucketName = "loop-social",
                    Name = $"{userId}/{key}"  
                };

                var accessKey = _config["AwsConfiguration:AWSAccessKey"];
                if (accessKey == null)
                    throw new ArgumentNullException("AwsConfiguration:AWSAccessKey is not in configuration.");

                var secretKey = _config["AwsConfiguration:AWSSecretKey"];
                if (secretKey == null)
                    throw new ArgumentNullException("AwsConfiguration:AWSSecretKey is not in configuration.");

                var cred = new AwsCredentials()
                {
                    AccessKey = accessKey,
                    SecretKey = secretKey
                };

                var response = await _storageService.DeleteFileAsync(s3Obj, cred);
                return Ok(response);
        }
    }
}