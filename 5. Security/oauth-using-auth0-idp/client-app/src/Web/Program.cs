using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using System.Net.Http.Headers;
using Web.Services;
using Web.Services.HttpHandler;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddNewtonsoftJson();

builder.Services.AddHttpContextAccessor();

builder.Services.AddAuth0WebAppAuthentication(options =>
{
    options.Domain = builder.Configuration["Auth0:Domain"];
    options.ClientId = builder.Configuration["Auth0:ClientId"];
    options.ClientSecret = builder.Configuration["Auth0:ClientSecret"];

    options.Scope = builder.Configuration["Auth0:Scope"];
})
.WithAccessToken(options =>
{
    options.Audience = builder.Configuration["Auth0:Audience"];

    options.UseRefreshTokens = true;
});

builder.Services.AddHttpClient("notes-api", (client) =>
{
    client.BaseAddress = new Uri(builder.Configuration["NotesApi:BaseUrl"]);

    client.DefaultRequestHeaders.Clear();

    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
})
.AddHttpMessageHandler<AccessTokenHttpHandler>();

builder.Services.AddScoped<AccessTokenHttpHandler>();

builder.Services.AddScoped<INotesService, NotesService>();

builder.Services.Configure<CookieAuthenticationOptions>(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
    // Redirect to login path if not authenticated
    options.LoginPath = "/Login";

    options.LogoutPath = "/Logout";

    // Cookie name where current session, access_token and id_token will be stored in browser
    options.Cookie.Name = "Auth0.SecurityCookie";
});

WebApplication app = builder.Build();

app.UseExceptionHandler("/Error");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Use cookie middleware
app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.None,

    Secure = CookieSecurePolicy.Always
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();
