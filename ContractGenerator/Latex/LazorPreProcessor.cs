using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace ContractGenerator.Latex
{
    class LazorPreProcessor
    {
        string currentScript;
        private List<IlatexScript> scriptList;
        private int escapeCharCount = 2;

        public LazorPreProcessor()
        {
            scriptList = new List<IlatexScript>();
        }

        private MatchCollection FindScript(StringBuilder inputString)
        {
            RegexOptions options = RegexOptions.Multiline | RegexOptions.Singleline;
            Regex regex = new Regex(@"((?<!_)@!(for|if|foreach).*?).*?{|(?<!_)@!.*?(?=[<|\n}{\[\] \\])", options);
            return regex.Matches(inputString.ToString());
        }

        private string EraseNewLines(string inputString)
        {
            StringBuilder builder = new StringBuilder(inputString);
            builder.Replace("\r", "");
            builder.Replace("\n", "");
            return builder.ToString();
        }

        private void AddExpressionToScript(string expression)
        {
            currentScript += EraseNewLines(expression);
        }

        private void AddTokenToScript(string expression)
        {
            currentScript += EraseNewLines("Write(" + expression + ")")+";";
        }

        private void AddLiteralToScript(string literal)
        {
            StringBuilder literalEditor = new StringBuilder(literal);
            literalEditor.Replace(" ", "");
            literalEditor.Replace("\t", "");
            literalEditor.Replace("\\", "\\\\");

            if (literalEditor.Length == 0)
                return;

            string[] literals = literalEditor.ToString().Split("\r\n");
            foreach(string currentLiteral in literals)
            {
                if (currentLiteral.Length == 0)
                    continue;
                string finalizedLiteral = EraseNewLines(currentLiteral);
               // Regex.Unescape(finalizedLiteral);
                currentScript += "WriteLiteral(\"" + EraseNewLines(finalizedLiteral) + "\");";
            }

        }

        private void AddClosingBracket()
        {
            currentScript += "}";
        }

        private bool IsSubScope(int mainScopeEndIndex, int index)
        {
            if (mainScopeEndIndex > index)
                return true;
            return false;
        }

        private void ProcessScript(Scope currentScope)
        {
            //Detect all declarators
            MatchCollection declarators = FindScript(currentScope.ScopeData);

            //Add current declarator to script
            //If the declarator has scope add as expression, enter recursively and repeat, else write the token
            if (currentScope.ScopeData.Length != 0)
                AddExpressionToScript(currentScope.ScopeHeader.ToString());
            else
            {
                AddTokenToScript(currentScope.ScopeHeader.ToString());
                return;
            }


            //Iterate through all the declarators
            int lastDeclaratorIndex = 0;
            foreach(Match declaratorMatch in declarators)
            {
                //Check if within the scope, if not then continue
                if (IsSubScope(lastDeclaratorIndex, declaratorMatch.Index))
                    continue;

                //Try to get the next declarators
                Scope declarator = CreateScope(currentScope.ScopeData, declaratorMatch);

                //Write the literals till the declarator
                int literalsLenght = declarator.ScopeStartIndex - lastDeclaratorIndex;
                string literals = currentScope.ScopeData.ToString().Substring(lastDeclaratorIndex, literalsLenght);
                AddLiteralToScript(literals);


                //If the declarator has a scope, enter recursively and repeat, else write the token
                if (declarator.ScopeData.Length != 0)
                    ProcessScript(declarator);
                else
                    AddTokenToScript(declarator.ScopeHeader.ToString());
                lastDeclaratorIndex = declarator.ScopeEndIndex+1;
            }
            //Write remaining data
            int LastliteralsLenght = currentScope.ScopeData.Length - lastDeclaratorIndex;
            string remainingLiteral = currentScope.ScopeData.ToString().Substring(lastDeclaratorIndex, LastliteralsLenght);
            AddLiteralToScript(remainingLiteral);

            AddClosingBracket();
        }

        private int GetScopeEndIndex(StringBuilder scope, Match scopeDeclarator)
        {
            int braces = -1;
            //Check if it is a scoped expression
            RegexOptions options = RegexOptions.Multiline | RegexOptions.Singleline;
            Regex regex = new Regex(@"((?<!_)@!(for|if|foreach).*?).*?{", options);
            if (regex.Matches(scopeDeclarator.Value).Count == 0)
                return scopeDeclarator.Index + scopeDeclarator.Length - 1;

            for(int i = scopeDeclarator.Index + scopeDeclarator.Length - 1; i < scope.Length; i++)
            {
                if (scope[i] == '{')
                {
                    if (braces == -1)
                        braces = 1;
                    else
                        braces++;
                }
                if (scope[i] == '}')
                    braces--;
                if (braces == 0)
                    return i;
            }
            return -1;
        }

        private Scope CreateScope(StringBuilder sourceCode, Match scopeDeclarator)
        {
            //Add scope start to script
            Scope scope = new Scope();
            scope.ScopeEndIndex = GetScopeEndIndex(sourceCode, scopeDeclarator);
            scope.ScopeStartIndex = scopeDeclarator.Index;

            string declaratorExpression = scopeDeclarator.Value.Substring(escapeCharCount, scopeDeclarator.Length - escapeCharCount);
            scope.ScopeHeader.Append(declaratorExpression);

            int scopeStartIndex = scopeDeclarator.Index + scopeDeclarator.Length;
            if (scopeStartIndex > scope.ScopeEndIndex)
                return scope;
            string scopeData = sourceCode.ToString().Substring(scopeStartIndex, scope.ScopeEndIndex - scopeStartIndex);
            scope.ScopeData.Append(scopeData);
            return scope;
        }

        private IlatexScript CreateLatexScript(int startIndex, int endIndex)
        {
            IlatexScript latexScript = new LatexScript(currentScript, startIndex, endIndex);
            return latexScript;
        }

        public IlatexScript Process(StringBuilder stringToProcess)
        {
            scriptList.Clear();
            MatchCollection scripts = FindScript(stringToProcess);
            Match script = null;
            //create a loop and interate over scripts
            if (scripts.Count > 0)
                script = scripts[0];
            if (script == null)
                return null;

            //Strip the currentScope, start recursion
            Scope scope = CreateScope(stringToProcess, script);
            ProcessScript(scope);


            //Create script
            int startIndex = script.Index;
            int endIndex = scope.ScopeEndIndex;
            IlatexScript latexScript = new LatexScript(currentScript, startIndex, endIndex);
            //scriptList.Add(latexScript);
            //Clear the currentScript;
            currentScript = "";
            return latexScript;
        }
    }
}
