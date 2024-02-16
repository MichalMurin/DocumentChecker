﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonCode.DataServices
{
    public class ConsistencyPageDataService
    {
        public bool TitleConsistency { get; set; } = true;
        public bool DoubleSpaces { get; set; } = true;
        public bool EmptyLines { get; set; } = true;
        public bool CrossReferenceFunctionality { get; set; } = true;
        public bool DocumentAlignment { get; set; } = true;
        public bool DescriptionValidation { get; set; } = true;
        public bool ListValidation { get; set; } = true;
        public bool ParenthesesValidation { get; set; } = true;
        public bool DotsComasColonsValidation { get; set; } = true;
    }
}
