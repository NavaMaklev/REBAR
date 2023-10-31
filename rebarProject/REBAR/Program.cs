using Microsoft.Extensions.Options;
using MongoDB.Driver;
using REBAR.Configuration;
using REBAR.Models;
using REBAR.Services;

namespace REBAR
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Add configurations
            var configuration = builder.Configuration;
            builder.Services.Configure<MongoSettings>(configuration.GetSection("MongoSettings"));
            builder.Services.AddSingleton<IMongoDatabase>(sp =>
            {
                var settings = sp.GetRequiredService<IOptions<MongoSettings>>().Value;
                var client = new MongoClient(settings.ConnectionString);
                return client.GetDatabase(settings.DatabaseName);
            });

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddScoped<ShakeService>();
            builder.Services.AddScoped<OrderService>();
            builder.Services.AddScoped<PriceEntryService>();
            builder.Services.AddScoped<PriceTableService>();
            builder.Services.AddScoped<MenuService>();
            builder.Services.AddScoped<PaymentService>();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();
            app.UseStaticFiles();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}