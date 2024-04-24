using CommonCode.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CommonCode.Services.DataServices
{
    /// <summary>
    /// Represents a factory for creating data services.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="DataServiceFactory"/> class.
    /// </remarks>
    /// <param name="serviceProvider">The service provider.</param>
    public class DataServiceFactory(IServiceProvider serviceProvider) : IDataServiceFactory
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        /// <summary>
        /// Gets the consistency data service.
        /// </summary>
        /// <returns>The consistency data service.</returns>
        public ConsistencyPageDataService GetConsistencyDataService()
        {
            return _serviceProvider.GetRequiredService<ConsistencyPageDataService>();
        }

        /// <summary>
        /// Gets the formatting data service.
        /// </summary>
        /// <returns>The formatting data service.</returns>
        public FormattingPageDataService GetFormattingDataService()
        {
            return _serviceProvider.GetRequiredService<FormattingPageDataService>();
        }

        /// <summary>
        /// Gets the spelling data service.
        /// </summary>
        /// <returns>The spelling data service.</returns>
        public SpellingPageDataService GetSpellingDataService()
        {
            return _serviceProvider.GetRequiredService<SpellingPageDataService>();
        }
    }
}
