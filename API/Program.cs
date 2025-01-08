using BLL.Services;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Ajout des repositories
            builder.Services.AddScoped<DAL.Repositories.ProductRepository>();

            // Ajout des services
            builder.Services.AddScoped<ProductService>();

            // Configuration de base
            builder.Services.AddControllers();
            builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

            // Configuration de Swagger/OpenAPI
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configurer le pipeline HTTP
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}