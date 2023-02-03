using Api.ResponseProvider;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Options;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHttpContextAccessor();

/*
 * This extension method will add versioning to our application
*/
builder.Services.AddApiVersioning(options =>
{
    // Default version
    options.DefaultApiVersion = new ApiVersion(2, 0);

    /* 
     * Specify the default version, .NET 6 will attempt to
     * redirect all request to this version, if no
     * version is specified.
    */
    options.AssumeDefaultVersionWhenUnspecified = true;

    /*
     * Broadcast supported version in "api-supported-versions" response header, and
     * broadcast deprecated version in "api-deprecated-versions" response header.
    */
    options.ReportApiVersions = true;

    /* Versioning Strategy
     * 
     * API now supports Query Parameter versioning, Custom header api versioning
     * Accept Header versioning and Path based versioning
    */
    options.ApiVersionReader = ApiVersionReader.Combine(
        new QueryStringApiVersionReader("v"),
        new HeaderApiVersionReader("x-version"),
        new MediaTypeApiVersionReader("version"),
        new UrlSegmentApiVersionReader()
    );

    options.ErrorResponses = new ApiVersioningErrorResponseProvider();
});

WebApplication app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
