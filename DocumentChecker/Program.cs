using CommonCode.DataServices;
using DocumentChecker.JsConnectors;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using LanguageToolHandler.Interfaces;
using LanguageToolHandler;
namespace DocumentChecker
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddSingleton<CommonJsConnectorService>();
            builder.Services.AddSingleton<FormattingPageDataService>();
            builder.Services.AddSingleton<FormattingPageConnectorService>();
            builder.Services.AddSingleton<ConsistencyPageDataService>();
            builder.Services.AddSingleton<ConsistencyPageConnectorService>();
            builder.Services.AddSingleton<SpellingPageConnectorService>();
            builder.Services.AddSingleton<ILanguageToolService, LanguageToolService>();
            await builder.Build().RunAsync();
        }
    }
}
