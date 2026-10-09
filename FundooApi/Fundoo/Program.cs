using BusinessLayer.Interface;
using BusinessLayer.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using ServiceLayer;
using ServiceLayer.Interface;
using System.Text;

namespace Fundoo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

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

            builder.Services.AddAuthorization();
            builder.Services.AddControllers();
            builder.Services.AddScoped<IPasswordHasher<UserEntity>, PasswordHasher<UserEntity>>();
            builder.Services.AddScoped<IUserBl ,UserBl>();// jis jis class ka obj automatically bne use yha pass krna hai
            builder.Services.AddEndpointsApiExplorer();
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

                options.AddSecurityRequirement(document =>
                    new OpenApiSecurityRequirement
                    {
                        [new OpenApiSecuritySchemeReference("Bearer", document)] = []
                    });
           
        });
            builder.Services.AddScoped<IUserRL, UserRl>();
            builder.Services.AddDbContext<FundooContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("FundooConnection")
    ));
            builder.Services.AddScoped<JwtService>();
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



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseHttpsRedirection();

            app.UseCors("AllowFundoo");
            app.UseAuthentication();
           app.UseAuthorization();


            app.MapControllers();
            app.UseSwagger();
            app.UseSwaggerUI();

            app.Run();
        }
    }
}
