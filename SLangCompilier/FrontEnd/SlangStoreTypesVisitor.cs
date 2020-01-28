using Antlr4.Runtime.Misc;
using SLangCompiler.FileServices;
using SLangCompiler.FrontEnd.Tables;
using SLangGrammar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static SLangCompiler.Exceptions.CompilerErrors;

namespace SLangCompiler.FrontEnd
{
    /// <summary>
    /// Store Visitor — стартовый проход семантического анализатора
    /// Обязанности: 
    /// 1) Проверки имен модулей
    /// 2) Резервирование типов без их дополнительной информации (Этап объявления, все что касается определений полей и методов, а также наследования -- пропускается)
    /// Мотивация:
    /// 1) Иметь базовую информацию по типам к следующему проходу, который использует эту информацию при определении функций и методов
    /// </summary>
    public class SlangStoreTypesVisitor: SlangBaseVisitor
    {
        private readonly ModuleNameTable moduleTable = new ModuleNameTable();
        private readonly string[] allModuleNames;
        public SlangStoreTypesVisitor(SourceCodeTable table, ModuleData moduleData, string[] modules) : base(table, moduleData)
        {
            allModuleNames = modules;
            moduleTable.ModuleData = moduleData;

            Table.Modules[ModuleData.Name] = moduleTable;
        }

        public override object VisitStart([NotNull] SLangGrammarParser.StartContext context)
        {
            CheckModuleNames(context);
            Visit(context.module());

            return null;
        }

        public void CheckModuleNames([NotNull] SLangGrammarParser.StartContext context)
        {
            var moduleNames = context.moduleImport().Select(i => i.Id());
            var modules = context.moduleImport();

            foreach (var module in moduleNames)
            {
                // нет в папке проекта и папке Lib
                var moduleName = module.GetText();
                if (moduleTable.ImportedModules.Contains(moduleName))
                {
                    ThrowException($"Repeating import of module ${moduleName}", ModuleData.File, module.Symbol);
                }
                if (!allModuleNames.Contains(moduleName))
                {
                    ThrowException($"Module {moduleName} not found", ModuleData.File, module.Symbol);
                }
                if (moduleName == ModuleData.Name)
                {
                    ThrowException($"Module {moduleName} imports itself", ModuleData.File, module.Symbol);
                }
                if (moduleName == CompilerConstants.MainModuleName)
                {
                    ThrowException($"Unable to import main module from other!", ModuleData.File, module.Symbol);
                }
                moduleTable.ImportedModules.Add(moduleName);
            }
        }

        public override object VisitModule([NotNull] SLangGrammarParser.ModuleContext context)
        {
            var moduleToken = context.Id().Symbol;
            var moduleName = context.Id().GetText();
            ThrowIfReservedWord(moduleName, ModuleData.File, moduleToken.Line, moduleToken.Column);

            if (moduleName != ModuleData.Name)
            {
                ThrowException($"Module name \"{moduleName}\" doest not match \"{ModuleData.Name}\"", ModuleData.File, moduleToken);
            }

            if (moduleName != CompilerConstants.MainModuleName && context.moduleStatementsSeq() != null)
            {
                ThrowException($"Module {moduleName} is not main module but have an entry point", ModuleData.File, context.moduleStatementsSeq().Start().Symbol);
            }

            return base.VisitModule(context);
        }

        public override object VisitTypeDecl([NotNull] SLangGrammarParser.TypeDeclContext context)
        {
            var className = context.Id().GetText();
            ThrowIfReservedWord(className, ModuleData.File, context.Id().Symbol);

            if (moduleTable.Classes.ContainsKey(className))
            {
                ThrowException($"Redefinition of class \"{className}\"", ModuleData.File, context.Id().Symbol);
            }

            var isBase = context.Base() != null;
            var modifier = GetModifierByName(context.AccessModifier().GetText());

            var classItem = new ClassNameTableItem { TypeIdent = new Types.SlangCustomType(ModuleData.Name, className), CanBeBase = isBase, Column = context.Id().Symbol.Column, Line = context.Id().Symbol.Line, AccessModifier = modifier };

            moduleTable.Classes[className] = classItem;
            return base.VisitTypeDecl(context);
        }
    }
}
