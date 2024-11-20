
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Task2API.Data;
using Task2API.DTOs.Product;
using Task2API.Errors;

namespace Task2API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddScoped<IValidator<CreateProductDtos>, CreateProductDtoValidation>();

            builder.Services.AddControllers();

            

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            

            builder.Host.UseSerilog((context, Configuration) =>
            {
                Configuration.ReadFrom.Configuration(context.Configuration);
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();


            app.UseAuthorization();


            app.MapControllers();

            app.UseExceptionHandler(opt => { });

            app.Run();
        }
    }
}
