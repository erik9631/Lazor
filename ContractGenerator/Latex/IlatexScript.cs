using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractGenerator.Latex
{
    interface IlatexScript : IScriptModelObject
    {
        public ScriptState CompileScript();

        /// <summary>
        /// Returns the starting index of the script in which the document was written in
        /// </summary>
        int InlineScriptStart { get; }

        /// <summary>
        /// Returns the ending index of the script in which the document was written in
        /// </summary>
        int InlineScriptEnd { get; }

        string Code { get;}
    }
}
