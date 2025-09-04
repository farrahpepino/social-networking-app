using AwsS3.Models;
using AwsS3.Configurations;

namespace AwsS3.Services{

public interface IStorageService
{
    Task<S3ResponseDto> UploadFileAsync(S3Object obj, AwsCredentials awsCredentialsValues);
    Task<S3ResponseDto> DeleteFileAsync(S3Object obj, AwsCredentials awsCredentialsValues);
}
}