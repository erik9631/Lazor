using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ContractGenerator.Latex
{
    class LatexScript : IlatexScript
    {
        public int InlineScriptStart { get; }

        public int InlineScriptEnd { get; }

        public GlobalScriptModel GlobalModel { get; set; }

        public string Code { get; }
        public ScriptOptions Options { get; set; }

        public LatexScript(string code, int startIndex, int endIndex)
        {
            Options = ScriptOptions.Default;
            Code = code;
            InlineScriptEnd = endIndex;
            InlineScriptStart = startIndex;
        }

        public ScriptState CompileScript()
        {
            Task<ScriptState<object>> scriptTask = CSharpScript.RunAsync(Code, Options, GlobalModel, GlobalModel.GetType());
            return scriptTask.Result;
        }
    }
}
