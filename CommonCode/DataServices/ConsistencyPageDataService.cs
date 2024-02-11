using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonCode.DataServices
{
    public class ConsistencyPageDataService
    {
        public bool TitleConsistency { get; set; }
        public bool CrossReferenceFunctionality { get; set; }
        public bool DocumentAlignment { get; set; }
        public bool DescriptionValidation { get; set; }
        public bool ListValidation { get; set; }
        public bool InconsistentWordValidation { get; set; }
        public bool NumericalValidation { get; set; }
        public bool ParenthesesValidation { get; set; }
        public bool SentenceEndingValidation { get; set; }
    }
}
