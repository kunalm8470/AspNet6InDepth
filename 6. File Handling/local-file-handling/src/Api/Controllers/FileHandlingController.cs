using Api.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FileHandlingController : ControllerBase
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    private readonly IHttpContextAccessor _httpContextAccessor;

    private readonly List<FileUpload> _fileUploadMetadata;

    public FileHandlingController(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor, List<FileUpload> fileUploadMetadata)
    {
        _webHostEnvironment = webHostEnvironment;

        _httpContextAccessor = httpContextAccessor;

        _fileUploadMetadata = fileUploadMetadata;
    }

    [HttpPost("upload")]
    //[RequestSizeLimit(512 * 1024 * 1024)]
    public async Task<ActionResult<FileUpload>> UploadFileAsync([FromForm(Name = "upload")] IFormFile file)
    {
        string uploadDirectoryPath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads");

        string fileName = string.Join("_", Guid.NewGuid().ToString(), file.FileName);

        // Resolve absolute path for the uploaded file in wwwroot folder
        string filePath = Path.Combine(uploadDirectoryPath, fileName);

        CancellationToken cancellationToken = _httpContextAccessor.HttpContext.RequestAborted;

        using FileStream fileStream = System.IO.File.Create(filePath);

        await file.CopyToAsync(fileStream, cancellationToken);

        FileUpload fileUpload = new()
        {
            FileName = fileName,
            Size = file.Length,
            ContentType = file.ContentType
        };

        _fileUploadMetadata.Add(fileUpload);

        return StatusCode(StatusCodes.Status201Created, fileUpload);
    }

    [HttpPost("uploadmultiple")]
    public async Task<ActionResult<List<FileUpload>>> UploadFileAsync([FromForm(Name = "upload")] List<IFormFile> files)
    {
        string uploadDirectoryPath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads");

        List<FileUpload> multipleFileUploads = new();

        foreach (IFormFile file in files)
        {
            string fileName = string.Join("_", Guid.NewGuid().ToString(), file.FileName);

            // Resolve absolute path for the uploaded file in wwwroot folder
            string filePath = Path.Combine(uploadDirectoryPath, fileName);

            CancellationToken cancellationToken = _httpContextAccessor.HttpContext.RequestAborted;

            using FileStream fileStream = System.IO.File.Create(filePath);

            await file.CopyToAsync(fileStream, cancellationToken);

            FileUpload fileUpload = new()
            {
                FileName = fileName,
                Size = file.Length,
                ContentType = file.ContentType
            };

            multipleFileUploads.Add(fileUpload);
        }

        _fileUploadMetadata.AddRange(multipleFileUploads);

        return StatusCode(StatusCodes.Status201Created, multipleFileUploads);
    }

    [HttpGet("download")]
    public IActionResult DownloadFileAsync([FromQuery] string fileName)
    {
        FileUpload found = _fileUploadMetadata.FirstOrDefault(x => x.FileName == fileName);

        string uploadDirectoryPath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads");

        string filePath = Path.Combine(uploadDirectoryPath, fileName);

        if (found is null || !System.IO.File.Exists(filePath))
        {
            return NotFound(new
            {
                Message = $"File with name: {fileName} not found."
            });
        }

        FileStream fileStream = System.IO.File.OpenRead(filePath);

        _httpContextAccessor.HttpContext.Response.ContentType = found.ContentType;

        ContentDisposition contentDisposition = new()
        {
            FileName = fileName,
            Size = fileStream.Length,
            DispositionType = DispositionTypeNames.Attachment
        };

        _httpContextAccessor.HttpContext.Response.Headers.Add("Content-Disposition", contentDisposition.ToString());

        return new FileStreamResult(fileStream, found.ContentType);
    }
}
