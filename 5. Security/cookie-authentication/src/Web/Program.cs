using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.CookiePolicy;
using Web.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IUserService, UserService>();

// Set Authentication cookie options
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
.AddCookie(options =>
{
    options.Cookie.Name = "DemoApplication.AuthCookieAspNet6";

    options.LoginPath = "/Account/Login";

    options.LogoutPath = "/Account/Logout";

    // Javascript document.cookie cannot mutate the auth cookie in Browser
    options.Cookie.HttpOnly = true;

    options.Cookie.SecurePolicy = builder.Environment.IsDevelopment()
    ? CookieSecurePolicy.None 
    : CookieSecurePolicy.Always; // For production (HTTPS) set secure cookie to true

    options.Cookie.SameSite = SameSiteMode.Lax;

    options.SlidingExpiration = true;
});

// For global cookie configuration
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.MinimumSameSitePolicy = SameSiteMode.Lax;

    options.HttpOnly = HttpOnlyPolicy.Always;

    options.Secure = builder.Environment.IsDevelopment()
    ? CookieSecurePolicy.None
    : CookieSecurePolicy.Always; // For production (HTTPS) set secure cookie to true
});

// This will add AuthorizationFilter
builder.Services.AddAuthorization();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
