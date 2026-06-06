using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

// 1. Camada de Aplicação (Interfaces de Serviços, Casos de Uso e DTOs)
using WikiApi.Application.Interfaces;
using WikiApi.Domain.Interfaces.Repositories;
using WikiApi.Application.Interfaces.Services;
using WikiApi.Application.Services;

// 3. Camada de Infraestrutura (Implementações de banco de dados e segurança)
using WikiApi.Infrastructure.Data;
using WikiApi.Infrastructure.Repositories;
using WikiApi.Infrastructure.Security;

var builder = WebApplication.CreateBuilder(args);

// Configuração do CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });

    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // URL do seu Angular
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Configuração do Banco de Dados PostgreSQL
var connection = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? Environment.GetEnvironmentVariable("DATABASE_URL");
builder.Services.AddDbContext<WikiDbContext>(option => option.UseNpgsql(connection));

// --- INJEÇÃO DE DEPENDÊNCIA (DI) ---

// Repositórios (Infraestrutura implementando o Domínio)
builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>(); // Agrupado corretamente aqui

// Serviços de Segurança / Infraestrutura Técnica
builder.Services.AddScoped<ITokenService, TokenService>();

// Serviços de Aplicação (Casos de Uso)
builder.Services.AddScoped<IArticleService, ArticleService>(); // Corrigido: Interface + Classe Concreta
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// --- CONFIGURAÇÕES DO FRAMEWORK ASP.NET ---

builder.Services.AddControllers(); // Mantido apenas este registro

// Configuração do FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddEndpointsApiExplorer();

// Configuração do Swagger com suporte a Bearer Token
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Wiki API",
        Version = "v1",
        Description = "API para armazenar e gerenciar artigos, tutoriais e soluções técnicas."
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Configuração da Autenticação JWT
var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]!);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"]
    };
});

builder.Services.AddAuthorization();

// Permite acessar o HttpContext (e o usuário logado) de dentro dos serviços de aplicação
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// --- PIPELINE DE REQUISIÇÕES (MIDDLEWARES) ---

app.UseCors("AllowAngular");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();