using System;
using System.Collections.Generic;
using System.Text;

namespace ContractGenerator.Latex
{
    interface ILatexWriter : ITemplate
    {
        public void WriteData();
    }
}
