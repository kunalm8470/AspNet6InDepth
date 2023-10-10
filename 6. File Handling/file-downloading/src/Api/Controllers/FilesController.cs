using Api.Models;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System.Globalization;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FilesController : ControllerBase
{
    private static readonly List<Person> _people = new()
    {
        new Person { Id = 1, FirstName = "John", LastName = "Doe", Email = "JohnDoe@email.com" },
        new Person { Id = 2, FirstName = "Jane", LastName = "Doe", Email = "JaneDoe@email.com" },
        new Person { Id = 3, FirstName = "John", LastName = "Smith", Email = "JohnSmith@email.com" }
    };

    private readonly IHttpContextAccessor _httpContextAccessor;

    public FilesController(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpGet("csv")]
    public async Task<FileResult> GenerateCsvAsync()
    {
        MemoryStream stream = new();

        StreamWriter streamWriter = new(stream);

        CsvWriter csv = new CsvWriter(streamWriter, new CsvConfiguration(CultureInfo.InvariantCulture));

        await csv.WriteRecordsAsync<Person>(_people);

        await streamWriter.FlushAsync();

        // Client will come to know how to treat the attachment, to download or attach it
        _httpContextAccessor.HttpContext.Response.Headers.Add("Content-Disposition", "attachment; filename=people.csv");

        // Client will come to know what type of file it is
        _httpContextAccessor.HttpContext.Response.ContentType = "text/csv";

        byte[] response = stream.ToArray();

        return File(response, "text/csv", "people.csv");
    }

    [HttpGet("excel")]
    public async Task<FileResult> GenerateExcel()
    {
        // Set the use to be non-commercial
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using ExcelPackage package = new();

        ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("People Summary");

        // Add headers
        worksheet.Cells[1, 1].Value = "Id";
        worksheet.Cells[1, 2].Value = "First Name";
        worksheet.Cells[1, 3].Value = "Last Name";
        worksheet.Cells[1, 4].Value = "Email";

        // Add body
        for (int i = 0; i < _people.Count; i++)
        {
            worksheet.Cells[i + 2, 1].Value = _people[i].Id;
            worksheet.Cells[i + 2, 2].Value = _people[i].FirstName;
            worksheet.Cells[i + 2, 3].Value = _people[i].LastName;
            worksheet.Cells[i + 2, 4].Value = _people[i].Email;
        }

        byte[] excelData = await package.GetAsByteArrayAsync();

        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        // Client will come to know how to treat the attachment, to download or attach it
        _httpContextAccessor.HttpContext.Response.Headers.Add("Content-Disposition", "attachment; filename=people.xlsx");

        // Client will come to know what type of file it is
        _httpContextAccessor.HttpContext.Response.ContentType = contentType;

        return File(excelData, contentType, "people.xlsx");
    }
}
