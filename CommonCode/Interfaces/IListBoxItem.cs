using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonCode.Interfaces
{
    public interface IListBoxItem
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string WarningMessage { get; set; }
        public bool CanBeDeleted { get; set; }
    }
}
