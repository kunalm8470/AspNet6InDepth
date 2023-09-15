using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route(template: "api/[controller]")]
[ApiController]
public class FilesController : ControllerBase
{
    private readonly BlobServiceClient _client;

    private readonly IConfiguration _configuration;

    public FilesController(
        BlobServiceClient client,
        IConfiguration configuration
    )
    {
        _client = client;

        _configuration = configuration;
    }

    [HttpPost(template: "single")]
    public async Task<ActionResult<string>> UploadFileAsync([FromForm(Name = "attachment")] IFormFile file)
    {
        string filename = file.FileName;

        string contentType = file.ContentType;

        Stream readStream = file.OpenReadStream();

        BlobContainerClient containerClient = _client.GetBlobContainerClient(
            blobContainerName: _configuration["AzureBlobStorage:ContainerName"]
        );

        BlobClient blobClient = containerClient.GetBlobClient(filename);

        await blobClient.UploadAsync(readStream, new BlobHttpHeaders
        {
            ContentType = contentType
        });

        return Ok(blobClient.Uri);
    }

    [HttpPost(template: "multiple")]
    public async Task<ActionResult<List<Uri>>> UploadFilesAsync([FromForm(Name = "attachments")] List<IFormFile> files)
    {
        List<Uri> uploadedUrl = new();

        foreach (IFormFile file in files)
        {
            string filename = file.FileName;

            string contentType = file.ContentType;

            Stream readStream = file.OpenReadStream();

            BlobContainerClient containerClient = _client.GetBlobContainerClient(
                blobContainerName: _configuration["AzureBlobStorage:ContainerName"]
            );

            BlobClient blobClient = containerClient.GetBlobClient(filename);

            await blobClient.UploadAsync(readStream, new BlobHttpHeaders
            {
                ContentType = contentType
            });

            uploadedUrl.Add(blobClient.Uri);
        }

        return Ok(uploadedUrl);
    }

    /// <summary>
    /// Use the URL obtained by this API and issue a HTTP PUT request with "x-ms-blob-type" header set to BlockBlob
    /// </summary>
    /// <param name="filename"></param>
    /// <returns></returns>
    [HttpGet("sas")]
    public ActionResult<string> GenerateSASUrl([FromQuery] string filename)
    {
        string containerName = _configuration["AzureBlobStorage:ContainerName"];

        string accountName = _configuration["AzureBlobStorage:AccountName"];

        string accountKey = _configuration["AzureBlobStorage:AccountKey"];

        // Set metadata
        BlobSasBuilder blobSasBuilder = new()
        {
            BlobContainerName = containerName,
            BlobName = filename,
            Resource = "b", // b = "blob", c = "container"
            StartsOn = DateTimeOffset.UtcNow,
            ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(60)
        };

        // Set the permissions
        blobSasBuilder.SetPermissions(BlobSasPermissions.Write | BlobSasPermissions.Create);

        StorageSharedKeyCredential credential = new(accountName, accountKey);

        string sasToken = blobSasBuilder.ToSasQueryParameters(credential).ToString();

        UriBuilder fullUri = new()
        {
            Scheme = "https",
            Host = string.Format("{0}.blob.core.windows.net", accountName),
            Path = string.Format("{0}/{1}", containerName, filename),
            Query = sasToken
        };

        return Ok(fullUri.Uri);
    }
}
