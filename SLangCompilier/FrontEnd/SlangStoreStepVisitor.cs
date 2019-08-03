using Antlr4.Runtime.Misc;
using SLangCompiler.FrontEnd.Tables;
using SLangGrammar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLangCompiler.FrontEnd
{
    public class SlangStoreStepVisitor: SlangBaseStepVisitor
    {
        private ModuleNameTable moduleTable;
        public SlangStoreStepVisitor(SourceCodeTable table, string moduleName) : base(table, moduleName)
        {
        }

        public override object VisitStart([NotNull] SLGrammarParser.StartContext context)
        {
            Visit(context.moduleImportList());
            Visit(context.module());

            return base.VisitStart(context);
        }

        public override object VisitModuleImportList([NotNull] SLGrammarParser.ModuleImportListContext context)
        {
            return base.VisitModuleImportList(context);
        }

        public override object VisitModule([NotNull] SLGrammarParser.ModuleContext context)
        {
            return base.VisitModule(context);
        }
    }
}
