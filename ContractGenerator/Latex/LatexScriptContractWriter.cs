using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractGenerator.Latex
{
    class LatexScriptContractWriter : ILatexWriter, IScriptModelObject
    {
        public string templatePath;
        public string TemplatePath
        {
            get
            {
                return templatePath;
            }

            set
            {
                templatePath = value;
                if(compiler != null)
                    compiler.ExecutionPath = value;
            }
        }
        public string TemplateName { get; set; }
        public GlobalScriptModel GlobalModel { get; set; }
        public ScriptOptions Options { get; set; }
        ILatex Compiler
        {
            get
            {
                return compiler;
            }
        }


        private ILatex compiler;

        private LazorPreProcessor scriptPreProcessor;

        public LatexScriptContractWriter(string templatePath, string templateName)
        {
            this.TemplatePath = templatePath;
            this.TemplateName = templateName;
            Options = ScriptOptions.Default;
            compiler = new LatexCompiler();
            compiler.ExecutionPath = templatePath;
            scriptPreProcessor = new LazorPreProcessor();
        }

        private void SubstituteScripts(StringBuilder document, IlatexScript script)
        {
            if (script == null)
                return;
            script.GlobalModel = this.GlobalModel;
            script.Options = Options;
            script.CompileScript();

            //Susbstituion process
            document.Remove(script.InlineScriptStart, script.InlineScriptEnd - script.InlineScriptStart + 1);
            document.Insert(script.InlineScriptStart, GlobalModel.ModelOutput);
            GlobalModel.ClearModelOutput();
        }

        public void WriteData()
        {
            string documentPath = TemplatePath + TemplateName;
            StringBuilder document = new StringBuilder (System.IO.File.ReadAllText(documentPath));
            IlatexScript latexScript = null;
            while (true)
            {
                latexScript = scriptPreProcessor.Process(document);
                if (latexScript == null)
                    break;
                SubstituteScripts(document, latexScript);
            }

            System.IO.File.WriteAllText(templatePath + "out.tex", document.ToString());
            compiler.Compile("out.tex");
            //System.IO.File.Delete(templatePath + "out.tex");
        }
    }
}
