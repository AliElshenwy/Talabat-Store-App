
using Microsoft.EntityFrameworkCore;
using Store.Repository.Data.Context;

namespace Talabat_Store
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<StoreDbContext>(option =>
           option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
            );
            // Applay Update Datebse 

           //StoreDbContext context = new StoreDbContext();

           var app = builder.Build();

           using var scope = app.Services.CreateScope();

           var services = scope.ServiceProvider;
           var context = services.GetRequiredService<StoreDbContext>();

            //handling Erorr 
           var loggerFactory= services.GetRequiredService<ILoggerFactory>();
            try
            {
                await context.Database.MigrateAsync(); //Update DateBase 
            }
            catch (Exception Ex)
            {
                //  Error Display for Console 
               var logger =loggerFactory.CreateLogger<Program>();
                logger.LogError(Ex, "There Are Problems During  Apply Migrations..! ");
            }
            

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
