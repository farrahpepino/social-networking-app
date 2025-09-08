using AwsS3.Models;
using Amazon.S3;
using Amazon.S3.Transfer;
using Amazon.S3.Model;
using AwsS3.Configurations;
using Amazon.Runtime;
using Microsoft.Extensions.Configuration;

using S3Object = AwsS3.Models.S3Object;

namespace AwsS3.Services
{
    public class StorageService : IStorageService
    {
        private readonly IConfiguration _config;

        public StorageService(IConfiguration config)
        {
            _config = config;
        }

        public async Task<S3ResponseDto> UploadFileAsync(S3Object obj, AwsCredentials awsCredentialsValues)
        {
            var credentials = new BasicAWSCredentials(awsCredentialsValues.AccessKey, awsCredentialsValues.SecretKey);
            var config = new AmazonS3Config
            {
                RegionEndpoint = Amazon.RegionEndpoint.USEast1
            };

            var response = new S3ResponseDto();
            try
            {
                var uploadRequest = new TransferUtilityUploadRequest
                {
                    InputStream = obj.InputStream,
                    Key = obj.Name,
                    BucketName = obj.BucketName,
                    CannedACL = S3CannedACL.NoACL
                };

                using var client = new AmazonS3Client(credentials, config);
                var transferUtility = new TransferUtility(client);

                await transferUtility.UploadAsync(uploadRequest);

                response.StatusCode = 201;
                response.Message = $"{obj.Name} has been uploaded successfully";
                response.Url = $"https://{obj.BucketName}.s3.us-east-1.amazonaws.com/{obj.Name}";
            }
            catch (AmazonS3Exception s3Ex)
            {
                response.StatusCode = (int)s3Ex.StatusCode;
                response.Message = s3Ex.Message;
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<S3ResponseDto> DeleteFileAsync(S3Object obj, AwsCredentials awsCredentialsValues)
        {
            var credentials = new BasicAWSCredentials(awsCredentialsValues.AccessKey, awsCredentialsValues.SecretKey);
            var config = new AmazonS3Config
            {
                RegionEndpoint = Amazon.RegionEndpoint.USEast1
            };

            var response = new S3ResponseDto();
            try{

                var deleteRequest = new DeleteObjectRequest
                {
                    BucketName = obj.BucketName,
                    Key = obj.Name,
                };

                using var client = new AmazonS3Client(credentials, config);
                
                await client.DeleteObjectAsync(deleteRequest);
                response.StatusCode = 200;
                response.Message = $"{obj.Name} has been deleted successfully";
            }
            catch (AmazonS3Exception s3Ex)
            {
                response.StatusCode = (int)s3Ex.StatusCode;
                response.Message = s3Ex.Message;
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
