using Antlr4.Runtime.Misc;
using SLangCompiler.FileServices;
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
        public ModuleData ModuleData { get; }
        public SlangBaseStepVisitor(SourceCodeTable table, ModuleData moduleData)
        {
            Table = table;
            ModuleData = moduleData;
        }

    }
}
