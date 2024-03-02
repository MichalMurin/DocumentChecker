using CommonCode.Services.DataServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonCode.Interfaces
{
    public interface IDataServiceFactory
    {
        FormattingPageDataService GetFormattingDataService();
        SpellingPageDataService GetSpellingDataService();
        ConsistencyPageDataService GetConsistencyDataService();
    }
}
