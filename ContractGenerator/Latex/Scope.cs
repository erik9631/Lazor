using System;
using System.Collections.Generic;
using System.Text;

namespace ContractGenerator.Latex
{
    class Scope
    {
        public Scope()
        {
            ScopeHeader = new StringBuilder("");
            ScopeData = new StringBuilder("");
            ScopeEndIndex = 0;
            ScopeStartIndex = 0;
        }

        public override string ToString()
        {
            return ScopeData.ToString();
        }

        public Scope(StringBuilder scopeData, StringBuilder scopeHeader)
        {
            ScopeHeader = scopeHeader;
            ScopeData = scopeData;
            ScopeEndIndex = 0;
            ScopeStartIndex = 0;
        }
        public StringBuilder ScopeHeader { get; set; }
        public StringBuilder ScopeData { get; set; }
        public int ScopeEndIndex { get; set; }
        public int ScopeStartIndex { get; set; }
    }
}
