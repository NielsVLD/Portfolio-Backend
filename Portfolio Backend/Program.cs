using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Portfolio_Backend;
using Portfolio_Backend.Models;

var builder = WebApplication.CreateBuilder(args);

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


builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseInMemoryDatabase("AppDb"));

builder.Services.AddIdentityCore<MyUser>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
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
builder.Services.AddDbContext<ProjectsContext>(opt =>
    opt.UseInMemoryDatabase("Projects"));


var app = builder.Build();

app.MapIdentityApi<MyUser>();
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

app.Run();