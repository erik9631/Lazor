using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ContractGenerator.Latex
{
    class LatexCompiler : ILatex
    {
        private string terminalProcess;
        public string TerminalProcess 
        {
            get 
            {
                return terminalProcess;
            } 
            set
            {
                this.terminalProcess = value;
                consoleProcess.StartInfo.FileName = value;
            }
        }
        public string ExecutionPath { get; set; }
        public event DataReceivedEventHandler DataRecievedEvent
        {
            add
            {
                consoleProcess.OutputDataReceived += value;
            }
            remove
            {
                consoleProcess.OutputDataReceived += value;
            }
        }
        private Process consoleProcess;

        public LatexCompiler()
        {
            consoleProcess = new Process();
            consoleProcess.StartInfo.FileName = "cmd.exe";
            consoleProcess.StartInfo.RedirectStandardError = true;
            consoleProcess.StartInfo.RedirectStandardOutput = true;
            consoleProcess.StartInfo.RedirectStandardInput = true;
            consoleProcess.StartInfo.CreateNoWindow = true;
            consoleProcess.StartInfo.UseShellExecute = false;
        }

        public void Compile(string fileName)
        {
            consoleProcess.Start();
            consoleProcess.BeginOutputReadLine();
            consoleProcess.StandardInput.WriteLine("cd " + ExecutionPath);
            consoleProcess.StandardInput.WriteLine("pdflatex " + fileName);
        }
    }
}
