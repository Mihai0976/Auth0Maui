using Auth0Maui.UserServices.Data;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        ConfigureServices(builder);
        var app = builder.Build();
        ConfigurePipeline(app);
        app.Run();
    }

    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration; // Use builder.Configuration directly

        // API Controllers and JSON Serializer Options
        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });

        // Database Context
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // Swagger configuration
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Logging
        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();

        // HTTP Client and HttpContext Accessor
        builder.Services.AddHttpClient();
        builder.Services.AddHttpContextAccessor();

        // API Versioning
        builder.Services.AddApiVersioning(config =>
        {
            config.DefaultApiVersion = new ApiVersion(1, 0);
            config.AssumeDefaultVersionWhenUnspecified = true;
            config.ReportApiVersions = true;
        });

        // CORS
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("MyCorsPolicy", corsBuilder =>
            {
                corsBuilder.WithOrigins("http://example.com", "http://anotherdomain.com")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
        });

        // MediatR
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

        // JWT Authentication
        ConfigureJwtAuthentication(builder, configuration);
    }

    private static void ConfigureJwtAuthentication(WebApplicationBuilder builder, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("JwtSettings");
        var key = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"]);

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
                ValidateAudience = false
            };
        });
    }

    private static void ConfigurePipeline(WebApplication app)
    {
        // Enable HTTPS Redirection
        app.UseHttpsRedirection();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                options.RoutePrefix = string.Empty;
            });
        }

        // Enable CORS
        app.UseCors("MyCorsPolicy");

        // Enable Authentication and Authorization
        app.UseAuthentication();
        app.UseAuthorization();

        // Enable Routing
        app.UseRouting();

        // Map Controllers
        app.MapControllers();

        // Map the default route to index.html
        app.MapFallbackToFile("index.html");
    }
}
