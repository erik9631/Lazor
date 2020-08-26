using ContractGenerator.Latex;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace ContractGenerator
{
    class LatexContractWriter : ILatexWriter, IContract
    {
        private string templatePath;
        public string TemplatePath 
        {
            get
            {
                return templatePath;
            }
            set
            {
                templatePath = value;
                latexCompiler.ExecutionPath = value;
            }
        }
        public string TemplateName { get; set; }
        public string CompanyName { get; set; }
        public string EmployeeName { get; set; }
        public string AddressLineOne { get; set; }
        public string AddressLineTwo { get; set; }
        public string EmployeeEmail { get; set; }
        public string HourlyRate { get; set; }

        private void CompileOutputer(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine(e.Data);
        }

        ILatex latexCompiler { get; }

        public LatexContractWriter()
        {
            LatexCompiler compiler = new LatexCompiler();
            compiler.TerminalProcess = "cmd.exe";
            compiler.DataRecievedEvent += CompileOutputer;
            latexCompiler = compiler;
        }

        private int FindStringIndex(string propertyName, StringBuilder Text)
        {
            Regex regex = new Regex(@"(?<=\\newcommand{\\" + propertyName + "}{)(.*)(?=})");//new Regex(@"(?<=\\newcommand{\\"+propertyName+@"}{\[)(.*)(?=]})");
            MatchCollection matches = regex.Matches(Text.ToString());
            foreach(Match i in matches)
            {
                Console.WriteLine(i.Groups[0].Value);
                Text.Remove(i.Groups[0].Index, i.Groups[0].Length);
                return i.Groups[0].Index;
            }
            return 0;
        }

        private bool SaveDocument(StringBuilder document)
        {
            System.IO.File.WriteAllText(TemplatePath + TemplateName, document.ToString());
            return true;

        }

        private bool SetHeaderProperty(string value, string propertyName, StringBuilder document)
        {
            int propertyIndex = FindStringIndex(propertyName, document);
            if (propertyIndex == 0)
                return false;
            document.Insert(propertyIndex, value);
            SaveDocument(document);
            return true;
        }

        private void UpdateDocument()
        {
            StringBuilder document = new StringBuilder(System.IO.File.ReadAllText(TemplatePath + TemplateName));
            SetHeaderProperty(EmployeeName, "EmployeeName", document);
            SetHeaderProperty(CompanyName, "CompanyName", document);
            SetHeaderProperty(AddressLineOne, "AddressLineOne", document);
            SetHeaderProperty(AddressLineTwo, "AddressLineTwo", document);
            SetHeaderProperty(EmployeeEmail, "EmployeeEmail", document);
            SetHeaderProperty(HourlyRate, "HourlyRate", document);
            File.WriteAllText(TemplatePath + TemplateName, document.ToString());
        }

        public void WriteData()
        {
            UpdateDocument();
            latexCompiler.Compile("contract.tex");
        }
    }
}
