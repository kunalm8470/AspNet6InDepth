# Learning ASP.NET 6 in Depth

1. **ASP.NET 6 overview**
    - History and Backstory
    - Versioning Strategy
    - Installation
    - Project structure
    - .NET SDK

2. **ASP.NET 6 Fundamentals**
    - Scaffold ASP.NET 6 API application using .NET SDK
    - Analyzing `Program.cs` file
    - Dependency injection
        - Register object as singleton lifetime
        - Register object as scoped lifetime
        - Register object as transient lifetime
    - Middlewares
        - Overview
        - Make custom middlewares
            - By implementing `IMiddleware` interface
            - By injecting `RequestDelegate` object
            - Write inline middlewares using `app.Use`, `app.Map`, `app.MapWhen`, `app.UseWhen` and `app.Run`
        - Static files middleware `app.UseStaticFiles();`
        - CORS middleware `app.UseCors();`
        - Add Correlationid middleware
        - Add global unhandled exception middleware and return standard `ProblemDetails` response
    - Setup host
    - Configurations
        - Read configurations from `appsettings.{environment}.json` and the `IConfiguration` interface
        - Options pattern
            - Bind configuration to strongly typed C# POCO using `IOptions<T>` interface
            - Bind configuration which can reload at runtime to strongly typed C# POCO using `IOptionsSnapshot<T>` or `IOptionsMonitor<T>` interface
    - Understanding `HttpContext`, `HttpContext.Request` and `HttpContext.Response`
    - Attribute routing
        - Bind multiple routes to a controller using `Route` attribute
        - Pass parameters to action methods using Route parameters
        - Action verbs `HttpGetAttribute`, `HttpPostAttribute`, `HttpPutAttribute`, `HttpPatchAttribute` and `HttpDeleteAttribute`
    - Model Binding
        - Model bind from the following sources
            - `FromQuery` :  Gets values from the query string
            - `FromRoute` : Gets values from route data
            - `FromForm` : Gets values from posted form fields
            - `FromBody` : Gets values from the request body
            - `FromHeader` : Gets values from HTTP headers
    - Handle responses using `IActionResult<T>` or `ActionResult<T>`
    - Handle content negotiation using `Accept` header
        - Send XML response if `Accept` header is set to `application/xml`
        - Send JSON response if `Accept` header is set to `application/json`
    - Versioning in .NET6 API(s)
    - Handle `HTTP Patch` type requests
    - Model validation
        - Using Data Annotation Attributes
            - `Compare` : Validate if two properties in a model match.
            - `EmailAddress` : Validate if a property has an email format.
            - `Phone` : Validate if a property has a telephone number format.
            - `Range` : Validate if a property value falls within a specified range.
            - `RegularExpression` : Validate if a property value matches a specified regular expression.
            - `Required` : Validate if a field isn't null.
            - `StringLength` : Validate if a string property value doesn't exceed a specified length limit.
            - `Url` : Validate if a property has a URL format.
        - FluentValidation validation using Fluent methods
    - HTTP Client
        - Creating basic IHttpClientFactory using `IHttpClientFactory.CreateClient();`
        - Named HttpClient
        - Typed HttpClient
        - Add DelegatingHandler to HTTP Clients for executing common logic
        - Handle HTTP exceptions using Polly library
    - Filters
    - Compress response body
        - Add response compression using `gzip` algorithm
    - Globalization and localization
    - Rate Limiting
        - Rate Limiting using `AspNetCoreRateLimit` nuget package with memory provider.
        - Rate Limiting using `AspNetCoreRateLimit` and `AspNetCoreRateLimit.Redis` nuget package with Azure Cache for Redis provider.
    - Logging
        - Understanding log levels
        - Log using `ILogger<T>` extension methods
            - `logger.LogTrace` log trace level messages
            - `logger.LogDebug` log debug level messages
            - `logger.LogInformation` log information level messages
            - `logger.Warning` log warning level messages
            - `logger.Error` log error level messages
            - `logger.Critical` log critical level messages
        - Log using `Azure AppInsights` SDK
    - Hosted Services
        - Implement hosted services using `IHostedService` interface
        - Implement background services by overriding the `BackgroundService` class
    - Mapping objects from one type to another using Automapper nuget package
    - Mediator pattern
        - Implement loosely coupled services using MediatR nuget package
    - Swagger
        - Install `Swashbuckle.AspNetCore` nuget for adding Swagger OpenAPI support
        - Add API Info and description
        - Add summary tags for API level metadata
        - Add OperationIds to group API(s)
        - Enrich Operation Metadata
        - Enrich Response Metadata
        - Enrich Parameter Metadata
        - Enrich RequestBody Metadata
        - Enrich Schema Metadata
        - Add Tag Metadata
        - Change swagger route prefix to serve `swagger.json` from root level
        - Omit API(s) to be visible in Swagger
        - Generate Multiple Swagger Documents
    - Health checks

3. **Data Access in ASP.NET 6 API Application**
    - Data Access using Micro ORM Dapper and PostgreSQL
        - Install Npgsql
        - Install Dapper Micro ORM
        - Install Azure Data Studio
        - Configure Azure Database for PostgreSQL
        - Expose CRUD API(s) using Dapper MicroORM exposing the following endpoints
            - **Offset pagination** `/api/Persons?page=1&limit=10`
            - **Keyset pagination** `/api/Persons?searchAfter=4499b79a-c710-45e4-ba87-083d22c4d6ad_2023-04-17T12:00:25&limit=10`
            - **Get single by id** `/api/Persons/b1333cad-9d7c-4a64-8823-db8c9aa55646`
            - **Create** `/api/Persons`
            - **Update** `/api/Persons/b1333cad-9d7c-4a64-8823-db8c9aa55646`
            - **Deleting** `/api/Persons/b1333cad-9d7c-4a64-8823-db8c9aa55646`

    - Data Access using EF Core 6 ORM and PostgreSQL
        - Install EF Core 6
        - Install Azure Data Studio
        - Configure Azure Database for PostgreSQL
        - Add `DbSet<T>` to model tables in database
        - Analyze `DbContext`
        - Make repository pattern with DbContext extension methods
        - Add configurations to add constraints to database tables
        - Add seed data
        - Add migrations using EF Core migration scripts and `idempotent` option
        - Expose CRUD API(s) using EF Core ORM exposing the following endpoints
            - **Offset pagination** `/api/Employees?page=1&limit=10`
            - **Keyset pagination** `/api/Employees?searchAfter=4499b79a-c710-45e4-ba87-083d22c4d6ad_2023-04-17T12:00:25&limit=10`
            - **Get single by id** `/api/Employees/b1333cad-9d7c-4a64-8823-db8c9aa55646`
            - **Create** `/api/Employees`
            - **Update** `/api/Employees/b1333cad-9d7c-4a64-8823-db8c9aa55646`
            - **Deleting** `/api/Employees/b1333cad-9d7c-4a64-8823-db8c9aa55646`

4. **Response Caching in ASP.NET 6 API Application**
    - Add In Memory Caching using `IMemoryCache`
    - Add Distributed Caching using Azure Cache for Redis and `StackExchange.Redis`

5. **Security in ASP.NET 6 API Application**
    - Add Authentication
        - Understand `Claim`, `ClaimsIdentity` and `ClaimsPrincipal`
        - Implement Basic Authentication
        - Implement Cookie Authentication
        - Implement JWT Authentication (Custom)
            - Hash plain text passwords using BCrypt algorithm and `BCrypt.Net-Next` nuget
            - Generate JWTs using `System.IdentityModel.Tokens.Jwt`
            - Add the following API(s)
                - `/api/Account/Register` to register the user
                - `/api/Account/Login` to login the user and generate the initial set of access token and refresh token
                - `/api/Account/Token` to refresh the access token using the refresh token sent
                - `/api/Account/Revoke` to revoke the refresh token

    - Add Authorization
        - Implement Role based Authorization
        - Implement Policy based Authorization
            - Understand policy "requirements" and implement the `IAuthorizationRequirement` interface
            - Override the `HandleRequirementAsync` to make custom policy requirement
            - Register the custom requirement in `Program.cs`

    - Case Study: Add JWT Authentication and Role Authorization using Auth0
        - Understand OAuth2
        - Understand OAuth2 terminologies
        - Understand OAuth flows
            - Implicit flow
            - Client credentials flow
            - Auth Code flow
        - Create Application in Auth0
        - Set Redirects URLs in Auth0
        - Secure all API endpoints using Auth0
        - Define permissions on the API and do policy authorization

6. **Handling file uploads and file downloads**
    - File uploads
        - Handle upload single or multiple files
        - Store uploaded file locally in `wwwroot folder`
        - Store uploaded file in Azure Blob Storage Containers
    - File downloads
        - Stream CSV file to response
        - Stream Excel `.xlsx` file to response

7. **Testing**
    - Unit Testing
        - Add Unit testing using `xUnit.net`
        - Understanding `[Fact]` and `[Theory]`
        - Intialize test data using constructor
        - Cleanup test data using `Dispose` method
        - Create parameterized tests
            - Pass primitive parameters using `[InlineData]`
            - Pass primitive and complex parameters using `[MemberData]`
            - Pass primitive and complex parameters using `[ClassData]`
        - Share test context in same class using `IClassFixture`
        - Share test context between multiple classes using `ICollectionFixture`
        - Mock a dependency service using `Moq`
        - Verify if mocked service was called using `Verify()` or `VerifyAll()`

    - Integration Testing
        - Create custom web application factory by inheriting `WebApplicationFactory<TStartup>`
        - Use EF Core In Memory Database for Integration Testing
        - Write Integration tests for API(s) using `xUnit.net`

8. **Synchronous Inter-Service communication using gRPC and HTTP2**
    - Build a gateway API and internal API communicating with each other synchronously using gRPC

9. **Asynchronous Inter-Service communication using Message Queues**
    - Build a gateway API and internal API(s) (Azure Function) communicating with each other asynchronously using Azure Service Bus.

10. **Hosting ASP.NET 6 API application**
    - Host ASP.NET 6 API application using IIS and folder publish
    - Host ASP.NET 6 API application using Azure App Service and manual publish
