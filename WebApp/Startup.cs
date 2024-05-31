using ApplicationCore;
using Infrastructure;

namespace WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Sessions will be saved in a browser like a session identifier. But the full session's information will be saved on the server
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // The time after which the session will be terminated if it is not in use
                options.Cookie.HttpOnly = true; // Make session cookies available only over HTTP
                options.Cookie.IsEssential = true; // Necessary for the operation of the application
            });
            // Registering a distributed cache in memory
            services.AddDistributedMemoryCache();

            services.AddControllers();

            services.AddAuthentication();

            services.AddAuthorization();

            string connStr = Configuration.GetConnectionString("DefaultConnection")!;
            services.AddDbContext(connStr);

            services.AddRepositories();

            services.AddCors(options =>
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
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            // Auto mapper configuration for Business logic
            services.AddAutoMapper();
            // Fluent validators configuration
            services.AddFluentValidator();

            // Add custom servies from Business logic
            services.AddCustomServices();

            // Add costom logic for saving tokens
            services.AddTokenService();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (!env.IsDevelopment())
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            
            // Exeption handler
            app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

            app.UseRouting();

            app.UseCors(builder =>
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseSession();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
