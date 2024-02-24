using CommonCode.Interfaces;
using CommonCode.Services;
using CommonCode.Services.DataServices;
using DocumentChecker.JsConnectors;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Globalization;
using System.Text.RegularExpressions;
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
            builder.Services.AddSingleton<SpellingPageDataService>();
            builder.Services.AddSingleton<ISpellingApiService, SpellingApiService>();
            builder.Services.AddSingleton<ILanguageToolApiService, LanguageToolApiService>();

            builder.Services.AddLocalization();
            var culture = new CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
            await builder.Build().RunAsync();
        }
    }
}
