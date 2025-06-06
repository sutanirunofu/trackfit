using System.IO.Compression;
using System.Security.Claims;
using System.Text;
using Fitness;
using Fitness.Notification;
using Fitness.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(8080);    // HTTP
    serverOptions.ListenAnyIP(8081);    // HTTPS
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHostedService<NotificationBackgroundService>();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Fitness App API", Version = "v1" });

    // Добавьте JWT-авторизацию
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
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

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthorization();
builder.Services.AddMvc(options => options.EnableEndpointRouting = false);

// Добавляем службу сжатия
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.MimeTypes =
    [
        "text/plain",
        "text/css",
        "application/javascript",
        "text/html",
        "application/xml",
        "text/xml",
        "application/json",
        "text/json",
        "image/svg+xml"
    ];
    options.Providers.Add<GzipCompressionProvider>();
});

builder.Services.Configure<GzipCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.SmallestSize;
});

// Добавление зависимостей в контекст
builder.Services.AddScoped<JwtUtil>();

// Политики
// builder.Services.AddAuthorizationBuilder()
//     .AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));

// Конфигурация JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // Обязательные параметры
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"])),

            // Дополнительные рекомендации
            NameClaimType = ClaimTypes.Name,
            RoleClaimType = ClaimTypes.Role
        };
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        builder => builder.WithOrigins("http://localhost", "https://localhost", "https://localhost:4200", "http://localhost:4200",
                "https://127.0.0.1:4200",
                "http://127.0.0.1:4200")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());
});

builder.Services.AddControllers()
    .AddNewtonsoftJson();

var app = builder.Build();

app.UseResponseCompression();
app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger()
        .UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "TrackFit V1");
            // Добавьте конфигурацию OAuth
            c.OAuthClientId("swagger-ui");
            c.OAuthAppName("Swagger UI");
            c.OAuthUsePkce();
        });

    app.UseMvc(
        routes =>
        {
            routes.MapRoute(
                name: "default",
                template: "/{controller=Home}/{action=Index}/{id?}");
        }
    );
}

app.UseHttpsRedirection();
app.UseCors("AllowAngular");

app.Run();