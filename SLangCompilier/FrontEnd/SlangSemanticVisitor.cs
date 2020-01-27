using Antlr4.Runtime.Misc;
using SLangCompiler.FileServices;
using SLangCompiler.FrontEnd.Tables;
using SLangCompiler.FrontEnd.Types;
using SLangGrammar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLangCompiler.FrontEnd
{
    public class SlangSemanticVisitor: SlangBaseVisitor
    {
        private readonly ModuleNameTable moduleItem;
        public SlangSemanticVisitor(SourceCodeTable table, ModuleData module) : base(table, module)
        {
            moduleItem = table.Modules[module.Name];
        }

        public override object VisitCustomType([NotNull] SLangGrammarParser.CustomTypeContext context)
        {
            var classItem = base.VisitCustomType(context) as SlangCustomType;
            CheckClassExists(classItem.ModuleName, classItem.Name, context.qualident().Id().First().Symbol);
            return classItem;
        }
    }
}
