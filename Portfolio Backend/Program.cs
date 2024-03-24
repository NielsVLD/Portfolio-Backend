using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Portfolio_Backend.Models;

var builder = WebApplication.CreateBuilder(args);

// Token Bucket limiter
var tokenPolicy = "token";
var myOptions = new MyRateLimitOptions();
builder.Configuration.GetSection(MyRateLimitOptions.MyRateLimit).Bind(myOptions);

builder.Services.AddRateLimiter(_ => _
    .AddTokenBucketLimiter(policyName: tokenPolicy, options =>
    {
        options.TokenLimit = myOptions.TokenLimit;
        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        options.QueueLimit = myOptions.QueueLimit;
        options.ReplenishmentPeriod = TimeSpan.FromSeconds(myOptions.ReplenishmentPeriod);
        options.TokensPerPeriod = myOptions.TokensPerPeriod;
        options.AutoReplenishment = myOptions.AutoReplenishment;
    }));


// Angular client auth 
builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme)
    .AddIdentityCookies()
    .ApplicationCookie!.Configure(opt => opt.Events = new CookieAuthenticationEvents()
    {
        OnRedirectToLogin = ctx =>
        {
            ctx.Response.StatusCode = 401;
            return Task.CompletedTask;
        }
    }); 

builder.Services.AddAuthorizationBuilder();

builder.Services.AddDbContext<ProjectsContext>(
    options => options.UseInMemoryDatabase("AppDb"));

builder.Services.AddIdentityCore<MyUser>()
    .AddEntityFrameworkStores<ProjectsContext>()
    .AddApiEndpoints();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "V1",
        Title = "Portfolio API with angular client",
        Description = "An ASP.NET Core Web API for managing my portfolio Angular website.",
        Contact = new OpenApiContact
        {
            Name = "My portfolio url",
            Url = new Uri("https://nielsvld.github.io/Portfolio/")
        },
    });
} );

// Custom projects entities
builder.Services.AddControllers();

// APP
var app = builder.Build();

app.MapIdentityApi<MyUser>().RequireRateLimiting(tokenPolicy);
app.UseDefaultFiles();
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.MapPost("/logout", async (
    SignInManager<MyUser> signInManager,
    [FromBody]object empty) =>
{
    await signInManager.SignOutAsync();
    return Results.Ok();
}).RequireAuthorization();

app.MapFallbackToFile("/index.html");

// Rate limiter
app.UseRateLimiter();

app.Run();