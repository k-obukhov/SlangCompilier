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
    public class SlangStoreStepVisitor: SlangBaseStepVisitor
    {
        private ModuleNameTable moduleTable = new ModuleNameTable();
        public SlangStoreStepVisitor(SourceCodeTable table, ModuleData moduleData) : base(table, moduleData)
        {
        }

        public override object VisitStart([NotNull] SLGrammarParser.StartContext context)
        {
            Visit(context.moduleImportList());
            Visit(context.module());

            Table.Modules[ModuleData.Name] = moduleTable;

            return base.VisitStart(context);
        }

        public override object VisitModuleImportList([NotNull] SLGrammarParser.ModuleImportListContext context)
        {
            var moduleNames = context.moduleImport().Select(i => i.Id());
            var modules = context.moduleImport(); 

            // todo перенос в semantic visitor
            /*
            foreach (var module in moduleNames)
            {
                // нет в папке проекта и папке Lib
                var moduleName = module.GetText();
                if (!Table.Modules.Keys.Contains(moduleName))
                {
                    throw new CompilerException($"Module {moduleName} not found", ModuleData.File, module.Symbol.Line, module.Symbol.Column);
                }
            }
            */

            moduleTable.ImportedModules = moduleNames.Select(i => i.GetText()).ToList();
            // Add basic modules if not exists (System, etc)
            return base.VisitModuleImportList(context);
        }

        public override object VisitModule([NotNull] SLGrammarParser.ModuleContext context)
        {
            return base.VisitModule(context);
        }
    }
}
