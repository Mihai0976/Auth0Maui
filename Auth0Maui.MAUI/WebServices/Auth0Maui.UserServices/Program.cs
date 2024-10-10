using Auth0Maui.UserServices.Data;
using Auth0Maui.UserServices.Models;
using Auth0Maui.UserServices.Services;
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

        // Configure services
        ConfigureServices(builder);

        var app = builder.Build();

        // Configure the request pipeline
        ConfigurePipeline(app);

        app.Run();
    }

    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;

        // Add Controllers and JSON Serializer Options
        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });

        // Add Database Context with SQL Server
        ConfigureDatabase(builder, configuration);

        // Add Authentication
        ConfigureJwtAuthentication(builder, configuration);

        // Add Swagger for API documentation
        ConfigureSwagger(builder);

        // Add MediatR and Scoped Services
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

        // Add HttpClient and HttpContextAccessor
        builder.Services.AddHttpClient();
        builder.Services.AddHttpContextAccessor();

        // Add CORS Policy
        ConfigureCors(builder);

        // Add API Versioning
        builder.Services.AddApiVersioning(config =>
        {
            config.DefaultApiVersion = new ApiVersion(1, 0);
            config.AssumeDefaultVersionWhenUnspecified = true;
            config.ReportApiVersions = true;
        });
    }

    private static void ConfigureDatabase(WebApplicationBuilder builder, IConfiguration configuration)
    {
        // Add Database Context with SQL Server
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
    }

    private static void ConfigureJwtAuthentication(WebApplicationBuilder builder, IConfiguration configuration)
    {
        // JWT Configuration
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

    private static void ConfigureSwagger(WebApplicationBuilder builder)
    {
        // Add Swagger configuration with JWT support
        builder.Services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'",
                Name = "Authorization",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
            {
                {
                    new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                    {
                        Reference = new Microsoft.OpenApi.Models.OpenApiReference
                        {
                            Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            });
        });
    }

    private static void ConfigureCors(WebApplicationBuilder builder)
    {
        // Add CORS policy
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("MyCorsPolicy", corsBuilder =>
            {
                corsBuilder.WithOrigins("http://10.0.2.2", "http://127.0.0.1", "http://localhost")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
        });
    }

    private static void ConfigurePipeline(WebApplication app)
    {
        // Use HTTPS Redirection
        app.UseHttpsRedirection();

        // Use Routing (comes before CORS, Authentication, and Authorization)
        app.UseRouting();

        // Use CORS
        app.UseCors("MyCorsPolicy");

        // Enable developer exception page and Swagger in development
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

        

        // Enable Authentication and Authorization
        app.UseAuthentication();
        app.UseAuthorization();

        // Map API Controllers
        app.MapControllers();

        // Fallback to index.html for SPA support (if applicable)
        app.MapFallbackToFile("index.html");
    }
}
