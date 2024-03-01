using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonCode.Services.DataServices
{
    public class ConsistencyPageDataService : BaseDataService
    {
        public bool TitleContinutity { get; set; } = true;
        public bool TitleConsistency { get; set; } = true;
        public bool DoubleSpaces { get; set; } = true;
        public bool EmptyLines { get; set; } = true;
        public bool CrossReferenceFunctionality { get; set; } = true;
        public bool DocumentAlignment { get; set; } = true;
        public bool CaptionValidation { get; set; } = true;
        public bool ListValidation { get; set; } = true;
        public bool ParenthesesValidation { get; set; } = true;
        public bool DotsComasColonsValidation { get; set; } = true;
    }
}
