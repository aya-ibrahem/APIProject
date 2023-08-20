using APIProject.Extensions;
using APIProject.Helper;
using APIProject.MiddleWare;
using TalabatBLL.Interfaces;
using TalabatDAL;
using TalabatDAL.Data;
using TalabatDAL.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace APIProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddDbContext<StoreContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddDbContext<AppIdentityDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });

            builder.Services.AddApplicationServices();

            builder.Services.AddIdentityServices(builder.Configuration);

            builder.Services.AddSingleton<IConnectionMultiplexer>(config =>
            {
                var configuration = ConfigurationOptions.Parse(builder.Configuration.GetConnectionString("Redis"), true);
                return ConnectionMultiplexer.Connect(configuration);
            });

            builder.Services.AddCors( opt =>
            {
                opt.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200");
                });
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwagerDocumention();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();

                app.UseMiddleware<ExceptionMiddleware>();
            }
            Seed.SeedData(app);
            app.UseStaticFiles();

            app.UseHttpsRedirection();
            app.UseAuthentication();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();

            //async void SeedData()
            //{
            //    using (var scope = app.Services.CreateScope())
            //    {
            //        var services = scope.ServiceProvider;
            //        var loggerfactory=services.GetRequiredService<ILoggerFactory>();
            //        try
            //        {
            //            var context = services.GetRequiredService<StoreContext>();
            //            await context.Database.MigrateAsync();
            //            await StoreContextSeed.SeedAsync(context, loggerfactory);
            //        }
            //        catch (Exception ex)
            //        {
            //            var logger = loggerfactory.CreateLogger<Program>();
            //            logger.LogError(ex.Message);
            //        }
            //    }
            //}
        }
    }
}