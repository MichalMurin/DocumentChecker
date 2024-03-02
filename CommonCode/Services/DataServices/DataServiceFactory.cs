using CommonCode.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CommonCode.Services.DataServices
{
    public class DataServiceFactory : IDataServiceFactory
    {
        private readonly  IServiceProvider _serviceProvider;

        public DataServiceFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ConsistencyPageDataService GetConsistencyDataService()
        {
            return _serviceProvider.GetRequiredService<ConsistencyPageDataService>();
        }

        public FormattingPageDataService GetFormattingDataService()
        {
            return _serviceProvider.GetRequiredService<FormattingPageDataService>();
        }

        public SpellingPageDataService GetSpellingDataService()
        {
            return _serviceProvider.GetRequiredService<SpellingPageDataService>();
        }
    }
}
