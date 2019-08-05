using SLangCompiler.FileServices;
using SLangCompiler.FrontEnd.Tables;
using System;
using System.Collections.Generic;
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
