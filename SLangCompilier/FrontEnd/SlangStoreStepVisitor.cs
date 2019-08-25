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
        private string[] allModuleNames;
        public SlangStoreStepVisitor(SourceCodeTable table, ModuleData moduleData, string[] modules) : base(table, moduleData)
        {
            allModuleNames = modules;
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
            
            foreach (var module in moduleNames)
            {
                // нет в папке проекта и папке Lib
                var moduleName = module.GetText();
                if (moduleTable.ImportedModules.Contains(moduleName))
                {
                    throw new CompilerException($"Repeating import of module ${moduleName}", ModuleData.File, module.Symbol);
                }
                if (!allModuleNames.Contains(moduleName))
                {
                    throw new CompilerException($"Module {moduleName} not found", ModuleData.File, module.Symbol);
                }
                if (moduleName == ModuleData.Name)
                {
                    throw new CompilerException($"Module {moduleName} imports itself", ModuleData.File, module.Symbol);
                }
                if (moduleName == CompilerConstants.MainModuleName)
                {
                    throw new CompilerException($"Unable to import main module from other!", ModuleData.File, module.Symbol);
                }
                moduleTable.ImportedModules.Add(moduleName);
            }
            
            return base.VisitModuleImportList(context);
        }

        public override object VisitModule([NotNull] SLGrammarParser.ModuleContext context)
        {
            var moduleName = context.Id().GetText();
            if (context.Id().GetText() != ModuleData.Name)
            {
                throw new CompilerException($"Module name \"{moduleName}\" doest not match \"{ModuleData.Name}\"", ModuleData.File, context.Id().Symbol);
            }

            return base.VisitModule(context);
        }

        public override object VisitClassDeclare([NotNull] SLGrammarParser.ClassDeclareContext context)
        {
            var className = context.Id().GetText();
            ThrowIfReservedWord(className, context.Id().Symbol);

            if (moduleTable.Classes.ContainsKey(className))
            {
                throw new CompilerException($"Redefinition of class \"{className}\"", ModuleData.File, context.Id().Symbol);
            }

            var isBase = context.base_head() == null;

            var classItem = new ClassNameTableItem { TypeIdent = new Types.SlangCustomType(className), CanBeBase = isBase };

            moduleTable.Classes[className] = classItem;
            return base.VisitClassDeclare(context);
        }
    }
}
