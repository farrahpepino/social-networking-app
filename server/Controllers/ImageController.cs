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
    public async Task<IActionResult> UploadFile(IFormFile file, [FromForm] string UserId)
    {  
    if (file == null || file.Length == 0)
        return BadRequest("No file uploaded");

    await using var memoryStream = new MemoryStream();
    await file.CopyToAsync(memoryStream);
    memoryStream.Position = 0;

    var fileExt = Path.GetExtension(file.FileName);
    var docName = $"{Guid.NewGuid()}{fileExt}";

    var s3Key = $"{UserId}/{docName}";

    var s3Obj = new S3Object()
    {
        BucketName = "loop-social",
        InputStream = memoryStream,
        Name = s3Key
    };

    var cred = new AwsCredentials()
    {
        AccessKey = _config["AwsConfiguration:AWSAccessKey"],
        SecretKey = _config["AwsConfiguration:AWSSecretKey"]
    };

    var result = await _storageService.UploadFileAsync(s3Obj, cred);

    var fileUrl = $"https://{s3Obj.BucketName}.s3.amazonaws.com/{s3Key}";

    return Ok(new { result.StatusCode, result.Message, Url = fileUrl });
    }

}