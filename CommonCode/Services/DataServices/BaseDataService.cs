using CommonCode.Interfaces;
using CommonCode.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonCode.Services.DataServices
{
    public class BaseDataService
    {
        public virtual HashSet<string> IgnoredParagraphs { get; set; } = new HashSet<string>();
        public virtual List<FoundErrorModel> FoundErrors { get; set; } = new List<FoundErrorModel>();
        public bool IsError { get; set; } = false;
        public string ErrorMessage { get; set; } = string.Empty;

        public void CopyFrom(BaseDataService source)
        {
            var properties = source.GetType().GetProperties();
            foreach (var property in properties)
            {
                if (property.CanWrite)
                {
                    property.SetValue(this, property.GetValue(source));
                }
            }
        }
    }
}
