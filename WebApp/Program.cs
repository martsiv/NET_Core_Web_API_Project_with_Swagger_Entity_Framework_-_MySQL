using ApplicationCore;
using Infrastructure;

namespace WebApp
{
    public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Connection string from secrets
			builder.Configuration.AddJsonFile("secrets.json", optional: true);
			string connStr = builder.Configuration.GetConnectionString("DefaultConnection")!;

			builder.Services.AddControllers();

			// Add services to the container.
			builder.Services.AddAuthorization();

			builder.Services.AddDbContext(connStr);
            builder.Services.AddRepositories();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
            });

            // Add file logger
            //builder.Logging.AddFile(Path.Combine(Directory.GetCurrentDirectory(), "logger.txt"));

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			// Auto mapper configuration for Business logic
			builder.Services.AddAutoMapper();
			// Fluent validators configuration
			builder.Services.AddFluentValidator();

			// Add custom servies from Business logic
			builder.Services.AddCustomServices();

			var app = builder.Build();

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
            //	app.UseSwagger();
            //	app.UseSwaggerUI();
            //}
            app.UseSwagger();
			app.UseSwaggerUI();

			app.UseHttpsRedirection();

            app.UseCors(builder => builder.AllowAnyOrigin());

			// Exeption handler
			app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

            app.UseAuthorization();

			app.MapControllers();

			app.Run();
		}
	}
}
