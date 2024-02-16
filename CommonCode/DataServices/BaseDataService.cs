using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonCode.DataServices
{
    public class BaseDataService
    {
        public virtual HashSet<string> IgnoredParagraphs { get; set; } = new HashSet<string>();
    }
}
