namespace Api.Models;

public class FileUpload
{
    public string FileName { get; set; }

    public long Size { get; set; }

    public string ContentType { get; set; }
}
