using Microsoft.Extensions.ML;
using PrepositionChecker;
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
            builder.Services.AddSwaggerGen();

            builder.Services.AddPredictionEnginePool<PrepositionChecker_ML.ModelInput, PrepositionChecker_ML.ModelOutput>()
                .FromFile("PrepositionChecker_ML.mlnet");
            builder.Services.AddSingleton<IPrepositionCheckService, PrepositionCheckService>();
            builder.Services.AddSingleton<ILanguageToolService, LanguageToolService>();
            builder.Services.AddControllers();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.MapControllers();

            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.UseCors(AllowAllOrigins);
            app.Run();
        }
    }
}
