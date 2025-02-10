using System.Text;
using FamilyAccountant.Application;
using FamilyAccountant.Endpoints;
using FamilyAccountant.Handlers;
using FamilyAccountant.Infrastructure;
using FamilyAccountant.Infrastructure.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog;
using NLog.Web;
using Swashbuckle.AspNetCore.SwaggerGen;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);
    
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(ConfigureSwaggerPage);
    builder.Services.AddApplication();
    builder.Services.AddInfrastructure(builder.Configuration);
    ConfigureAuthentication(builder);
    builder.Services.AddAuthorization();
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddExceptionHandler<ExceptionHandler>();
    builder.Services.AddProblemDetails();

    var app = builder.Build();
    
    app.UseExceptionHandler();
    app.UseSwagger();
    app.UseSwaggerUI();
    
    app.UseAuthentication();
    app.UseAuthorization();

    var group = app.MapGroup("/api/v1");
    group.MapAccountEndpoints();
    group.MapFamilyEndpoints();

    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex, ex.Message);
}
finally
{
    LogManager.Shutdown();
}

void ConfigureAuthentication(WebApplicationBuilder builder)
{
    var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>()
        ?? throw new ApplicationException("Missing JwtSettings");

    var key = Encoding.UTF8.GetBytes(jwtSettings.SecurityKey);

    builder.Services.AddAuthentication(opt =>
    {
        opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });
}

void ConfigureSwaggerPage(SwaggerGenOptions options)
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Family Accountant",
        Version = "v1",
        Description = """
                        This is a **sample Family Accountant API** for demonstrating my abilities.

                        ## Features
                        - Create users
                        - Login users (manipulating tokens)
                        - Create families and add/remove members

                        ## Contact
                        For more information, contact [Daniil Zh](mailto:daniil.zhukovets21@gmail.com).
                        """,
        License = new OpenApiLicense
        {
            Name = "MIT License",
            Url = new Uri("https://opensource.org/licenses/MIT")
        }
    });
    
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = """JWT Authorization header using the Bearer scheme. Example: "Authorization: Bearer {token}" """,
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            []
        }
    });
}