using CommonCode.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonCode.Models
{
    public class FoundErrorModel : IListBoxItem
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool CanBeDeleted { get; set; } = true;
        public string ErrorType { get; set; } = string.Empty;
    }
}
