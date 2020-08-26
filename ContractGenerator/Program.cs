using System;
using System.Diagnostics;
using System.Reflection;
using Microsoft.CSharp;
using Microsoft.CodeDom.Providers.DotNetCompilerPlatform;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using System.Threading.Tasks;
using ContractGenerator.Latex;
using System.Text;

namespace ContractGenerator
{
    public class Program
    {

        public class ScriptModel : GlobalScriptModel

        {
            public string Name { get; set; }
        }


        static void Main(string[] args)
        {
            LatexScriptContractWriter writer = new LatexScriptContractWriter(@"latextest\", "testdata.tex");
            ScriptModel model = new ScriptModel();
            model.Name = "Erik";

            writer.GlobalModel = model;
            writer.WriteData();

            Console.WriteLine("Finished");
            Console.ReadKey();
        }
    }
}
