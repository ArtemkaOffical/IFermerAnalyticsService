
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using IFermerAnalyticsService.Data;
using IFermerAnalyticsService.RabbitMqService;

namespace IFermerAnalyticsService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            //builder.Services.AddHostedService<RabbitService>();
            //builder.Services.AddScoped<RabbitService>();
            builder.Services.AddHttpClient();
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddCors();
            builder.Services.AddHttpClient();
            builder.Services.AddControllers().AddNewtonsoftJson(options => {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.DateFormatString = "yyyy-MM-ddTHH:mm:ss.fffZ";
            }
           );
            builder.Services.AddDbContext<AnalyticsDbContext>(options => options
                .UseLazyLoadingProxies()
                .UseNpgsql(builder.Configuration.GetConnectionString("DbContext")));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseCors(builder => builder.AllowAnyOrigin());
            app.MapControllers();
            app.Run();
        }
    }
}