using Antlr4.Runtime.Misc;
using SLangCompiler.Exceptions;
using SLangCompiler.FileServices;
using SLangCompiler.FrontEnd.Tables;
using SLangGrammar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLangCompiler.FrontEnd
{
    public class SlangSemanticVisitor: SlangBaseStepVisitor
    {
        public SlangSemanticVisitor(SourceCodeTable table, ModuleData moduleData) : base(table, moduleData)
        {
            
        }
    }
}
