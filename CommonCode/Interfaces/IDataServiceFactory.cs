using CommonCode.Services.DataServices;

namespace CommonCode.Interfaces
{
    /// <summary>
    /// Represents a factory for creating data service instances.
    /// </summary>
    public interface IDataServiceFactory
    {
        /// <summary>
        /// Creates a new instance of the FormattingPageDataService.
        /// </summary>
        /// <returns>The created FormattingPageDataService instance.</returns>
        FormattingPageDataService GetFormattingDataService();

        /// <summary>
        /// Creates a new instance of the SpellingPageDataService.
        /// </summary>
        /// <returns>The created SpellingPageDataService instance.</returns>
        SpellingPageDataService GetSpellingDataService();

        /// <summary>
        /// Creates a new instance of the ConsistencyPageDataService.
        /// </summary>
        /// <returns>The created ConsistencyPageDataService instance.</returns>
        ConsistencyPageDataService GetConsistencyDataService();
    }
}
