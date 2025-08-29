using AwsS3.Models;
using AwsS3.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors; 
namespace server.Controllers{

[ApiController]
[Route("[controller]")]
[EnableCors("AllowAngularDev")]
[RequestSizeLimit(100_000_000)] 
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

    [HttpPost]
    public async Task<IActionResult> UploadFile([FromForm] IFormFile File, [FromForm] string UserId) {

    await using var memoryStream = new MemoryStream();
    await File.CopyToAsync(memoryStream);

    var fileExt = Path.GetExtension(File.FileName); var docName = $"{Guid.NewGuid()}{fileExt}"; 
    var s3Key = $"{UserId}/{docName}";

    var s3Obj = new S3Object()
    {
        BucketName = "loop-social",
        InputStream = memoryStream,
        Name = s3Key
    };

    var cred = new AwsCredentials(){
        AccessKey = _config["AwsConfiguration:AWSAccessKey"],
        SecretKey = _config["AwsConfiguration:AWSSecretKey"]
    };

    var result = await _storageService.UploadFileAsync(s3Obj, cred);
    return Ok(new { message = "Uploaded successfully", url = result.Url });


    }
 
}
}