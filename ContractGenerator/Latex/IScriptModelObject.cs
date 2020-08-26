using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractGenerator.Latex
{
    interface IScriptModelObject
    {
        GlobalScriptModel GlobalModel {get; set;}
        ScriptOptions Options { get; set; }
    }
}
