using BusinessLayer.Interface.Notes;
using BusinessLayer.Interface.Users;
using BusinessLayer.Service.MessagingServices;
using BusinessLayer.Service.Notes;
using BusinessLayer.Service.Users;
using Fundoo.ExceptionHandler;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Middleware.Services.Email;
using Middleware.Services.JWT;
using NLog;
using NLog.Web;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface.Notes;
using RepositoryLayer.Interface.User;
using RepositoryLayer.Service.Notes;
using ServiceLayer;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;

namespace Fundoo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var nlogLogger = LogManager
    .Setup()
    .LoadConfigurationFromFile("NLog.config")
    .GetCurrentClassLogger();
            var builder = WebApplication.CreateBuilder(args);
            builder.Logging.ClearProviders();
            builder.Host.UseNLog();

            // Add services to the container.
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });
            var jwtKey = builder.Configuration["Jwt:Key"];
            var jwtIssuer = builder.Configuration["Jwt:Issuer"];
            var jwtAudience = builder.Configuration["Jwt:Audience"];

            if (string.IsNullOrWhiteSpace(jwtKey) ||
                string.IsNullOrWhiteSpace(jwtIssuer) ||
                string.IsNullOrWhiteSpace(jwtAudience))
            {
                throw new InvalidOperationException(
                    "JWT configuration is missing. Check Jwt:Key, Jwt:Issuer, and Jwt:Audience.");
            }
            builder.Services.AddAuthorization();
            builder.Services.AddControllers();
            builder.Services.AddScoped<IPasswordHasher<UserEntity>, PasswordHasher<UserEntity>>();
            builder.Services.AddScoped<IUserBl, UserBl>();// jis jis class ka obj automatically bne use yha pass krna hai
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddScoped<Middleware.Services.Email.IEmailSender, SmtpEmailSender>();

            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter your JWT token"
                });
                options.OperationFilter<InlineAuthorizeCheckOperationFilter>();
            });
            builder.Services.AddScoped<IUserRL, UserRl>();
            var connectionString =
    builder.Configuration.GetConnectionString("FundooConnection");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(
                    "Database connection string 'FundooConnection' is missing.");
            }

            builder.Services.AddDbContext<FundooContext>(options =>
                options.UseNpgsql(connectionString));
            builder.Services.AddDbContext<FundooContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("FundooConnection")
    ));
            builder.Services.AddScoped<INotesBL, NotesBL>();
            builder.Services.AddScoped<INotesRL, NotesRL>();
            builder.Services.AddScoped<IJwtTokenServices, JwtService>();
            builder.Services.AddHttpClient();
            builder.Services.AddHttpClient<MessagingService>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFundoo", policy =>
                {
                    policy
                        .WithOrigins("https://localhost:53351")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
            builder.Services.AddTransient<SmtpEmailSender>();

            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddProblemDetails();




            var app = builder.Build();

            // Handle unhandled exceptions first
            app.UseExceptionHandler();

            app.UseHttpsRedirection();

            app.UseCors("AllowFundoo");

            app.UseAuthentication();
            app.UseAuthorization();
        

            app.MapControllers();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.MapGet("/instance", () =>
            {
                return Results.Ok(new
                {
                    Instance = Environment.ProcessId
                });
            });

            app.Run();

        }
    }
    public class InlineAuthorizeCheckOperationFilter : IOperationFilter
    {
        public void Apply(
            OpenApiOperation operation,
            OperationFilterContext context)
        {
            var hasAuthorize =
                context.MethodInfo.GetCustomAttributes(true)
                    .OfType<AuthorizeAttribute>().Any()
                || context.MethodInfo.DeclaringType?
                    .GetCustomAttributes(true)
                    .OfType<AuthorizeAttribute>().Any() == true;

            var hasAllowAnonymous =
                context.MethodInfo.GetCustomAttributes(true)
                    .OfType<AllowAnonymousAttribute>().Any()
                || context.MethodInfo.DeclaringType?
                    .GetCustomAttributes(true)
                    .OfType<AllowAnonymousAttribute>().Any() == true;

            if (!hasAuthorize || hasAllowAnonymous)
                return;

            operation.Security ??= new List<OpenApiSecurityRequirement>();

            operation.Security.Add(
                new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference(
                        "Bearer", context.Document)] = new List<string>()
                });
        }
    }
}
