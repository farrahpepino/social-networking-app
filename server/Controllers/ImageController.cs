using AwsS3.Models;
using AwsS3.Services;
using Microsoft.AspNetCore.Mvc;

namespace server.Controllers;

[ApiController]
[Route("[controller]")]
public class ImageController : ControllerBase
{

    private readonly IStorageService _storageService;
    private readonly IConfiguration _config;
    private readonly ILogger<ImageController> _logger;

    public ImageController(
        ILogger<ImageController> logger,
        IConfiguration config,
        IStorageService storageService)
    {
        _logger = logger;
        _config = config;
        _storageService = storageService;
    }

    [HttpPost(Name = "UploadFile")]

    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        // Process file
        await using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);

        var fileExt = Path.GetExtension(file.FileName);
        var docName = $"{Guid.NewGuid}.{fileExt}";
        // call server

        var s3Obj = new S3Object() {
            BucketName = "live-demo-bucket821",
            InputStream = memoryStream,
            Name = docName
        };

        var cred = new AwsCredentials() {
            AccessKey = _config["AwsConfiguration:AWSAccessKey"],
            SecretKey = _config["AwsConfiguration:AWSSecretKey"]
        };

        var result = await _storageService.UploadFileAsync(s3Obj, cred);
        // 
       return Ok(result);

    }
}