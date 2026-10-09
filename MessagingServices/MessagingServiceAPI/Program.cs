using MessagingServiceAPI.Services.Interface;
using MessagingServiceAPI.Services.Service;
using MessagingServiceRL.Interface;
using MessagingServiceRL.Services;
using MessagingServiceBL.Interface;
using MessagingServiceBL.Services;

namespace MessagingServiceAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<IEmailBL, EmailBL>();
            builder.Services.AddScoped<IEmailRL, EmailRL>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI();
            app.MapControllers();

            app.Run();
        }
    }
}
