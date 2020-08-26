using ContractGenerator.Latex;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractGenerator
{
    public class GlobalScriptModel
    {
        string modelOutput;

        public string ModelOutput
        {
            get
            {
                return modelOutput;
            }
        }

        public void ClearModelOutput()
        {
            modelOutput = "";
        }

        public virtual void WriteLiteral(string literal)
        {
            modelOutput += literal;
        }
        public void Write(object obj)
        {
            modelOutput += obj.ToString();
        }
    }
}
