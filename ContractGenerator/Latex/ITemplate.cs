using System;
using System.Collections.Generic;
using System.Text;

namespace ContractGenerator
{
    interface ITemplate
    {
        public string TemplatePath { get; set; }
        public string TemplateName { get; set; }
    }
}
