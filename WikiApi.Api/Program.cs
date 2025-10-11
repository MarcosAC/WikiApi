using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using WikiApi.Application.Interfaces;
using WikiApi.Application.Services;
using WikiApi.Infrastructure.Data;
using WikiApi.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

var connection = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? Environment.GetEnvironmentVariable("DATABASE_URL");

builder.Services.AddDbContext<WikiDbContext>(option => option.UseNpgsql(connection));

builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
builder.Services.AddScoped<ArticleService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Wiki API",
        Version = "v1",
        Description = "API para armazenar e gerenciar artigos, tutoriais e soluções técnicas."
    });
});

var app = builder.Build();

app.UseCors();

if (app.Environment.IsDevelopment()) { app.UseSwagger(); app.UseSwaggerUI(); }

app.UseHttpsRedirection();
app.MapControllers();
app.Run();