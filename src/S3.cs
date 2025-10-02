using System.Text.Json;
using Amazon.S3;
using Amazon.S3.Model;

public class S3 {

    public static async Task<bool> UploadStringToS3(AmazonS3Client client, string bucketName, string key, string content){
        using (var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(content))){
            var putRequest = new PutObjectRequest{
                BucketName = bucketName,
                Key = key,
                InputStream = stream,
                ContentType = "text/plain",
            };

        var response = await client.PutObjectAsync(putRequest);
        return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
    }
}

}