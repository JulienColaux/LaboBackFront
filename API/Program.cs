using BLL.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using DAL.Repositories;
using Microsoft.AspNetCore.Authentication;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Ajout des repositories
            builder.Services.AddScoped<ProductRepository>();  // Exemple de Repository
            builder.Services.AddScoped<UserRepository>();     // Repository utilisateur
            builder.Services.AddScoped<PannierRepository>();  // Repository panier ajouté

            // Ajout des services
            builder.Services.AddScoped<ProductService>();     // Exemple de service
            builder.Services.AddScoped<UserService>();        // Service utilisateur
            builder.Services.AddScoped<AuthenticationService>(); // Service d'authentification
            builder.Services.AddScoped<PannierService>();

            // Configuration de la sécurité et de l'authentification JWT
            var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:SecretKey"]); // Clé secrète pour JWT
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
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    };
                });

            // Ajout de la configuration CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAngularApp", policy =>
                {
                    policy.WithOrigins("http://localhost:4200") // URL de ton frontend Angular
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

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

            // Utilisation de CORS
            app.UseCors("AllowAngularApp");

            // Utilisation de l'authentification
            app.UseAuthentication(); // Activer l'authentification
            app.UseAuthorization();  // Activer l'autorisation

            app.MapControllers();

            app.Run();
        }
    }
}
