using Antlr4.Runtime.Misc;
using SLangCompiler.FrontEnd.Tables;
using SLangGrammar;
using System;
using System.Collections.Generic;
using System.Text;

namespace SLangCompiler.FrontEnd
{
    public class SlangBaseStepVisitor: SLGrammarBaseVisitor<object>
    {
        // base visitor login there (basic type returning, etc)
        public SourceCodeTable Table { get; }
        private string ModuleName { get; }
        public SlangBaseStepVisitor(SourceCodeTable table, string moduleName)
        {
            Table = table;
            ModuleName = moduleName;
        }

    }
}
