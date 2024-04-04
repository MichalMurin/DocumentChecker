using Microsoft.Extensions.ML;
using Microsoft.OpenApi.Models;
using ML_DigramsDatabase;
using SpelingCheckAPI.Interfaces;
using SpelingCheckAPI.Services;

namespace SpelingCheckAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var AllowAllOrigins = "AllowAllOrigins";
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: AllowAllOrigins,
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Spelling Check API", Version = "v1" });
            });

            builder.Services.AddPredictionEnginePool<MLModel_DigramDb.ModelInput, MLModel_DigramDb.ModelOutput>()
                .FromFile("MLPrepositionChecker\\MLModel_DigramDb_10h.mlnet");
            builder.Services.AddSingleton<IPrepositionCheckService, PrepositionCheckService>();
            builder.Services.AddSingleton<ILanguageToolService, LanguageToolService>();
            builder.Services.AddControllers();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Spelling Check API v1");
            });
            app.MapControllers();

            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.UseCors(AllowAllOrigins);
            app.Run();
        }
    }
}
