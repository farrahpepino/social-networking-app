namespace AwsS3.Models
{
    public class S3Object
    {
        public MemoryStream InputStream { get; set; } = null!;
        public string BucketName { get; set; } = null!;
        public string Name { get; set; } = null!;

    }
}