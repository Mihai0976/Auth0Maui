using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
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
        var configuration = builder.Configuration;

        // API Controllers and JSON Serializer Options
        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                //options.JsonSerializerOptions.TypeInfoResolver = System.Text.Json.JsonSerializerOptions.DefaultTypeInfoResolver;
            });

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
        // MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));


        // Swagger
        builder.Services.AddSwaggerGen();

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
                options.RoutePrefix = string.Empty; // Swagger at root URL
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
