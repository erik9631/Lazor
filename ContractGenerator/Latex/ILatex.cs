using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ContractGenerator.Latex
{
    interface ILatex
    {
        ///<summary>
        ///Path in which the file you want to compile is located
        ///</summary>
        string ExecutionPath { get; set; }


        ///<summary>
        /// Name of the file to compile in the ExecutionPath directory
        ///</summary>
        void Compile(string fileName);

        event DataReceivedEventHandler DataRecievedEvent;
    }
}
