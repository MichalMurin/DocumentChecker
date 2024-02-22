using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonCode.DataServices
{
    public class SpellingPageDataService: BaseDataService
    {
        public List<string> Rules { get; set; } = new List<string>();
    }
}
