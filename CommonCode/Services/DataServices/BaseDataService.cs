﻿using CommonCode.Interfaces;
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
        public virtual List<IListBoxItem> FoundErrors { get; set; } = new List<IListBoxItem>();
    }
}
