using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Portfolio_Backend.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddDbContext<ProjectsContext>(opt =>
    opt.UseInMemoryDatabase("Projects"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "V1",
        Title = "Portfolio API",
        Description = "An ASP.NET Core Web API for managing my portfolio Angular website.",
        Contact = new OpenApiContact
        {
            Name = "My portfolio url",
            Url = new Uri("https://nielsvld.github.io/Portfolio/")
        },
    });
} );

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();