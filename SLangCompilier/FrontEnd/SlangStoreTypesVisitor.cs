using Antlr4.Runtime.Misc;
using SLangCompiler.FileServices;
using SLangCompiler.FrontEnd.Tables;
using SLangCompiler.FrontEnd.Types;
using SLangGrammar;
using System;
using System.Linq;
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
    public class SlangStoreTypesVisitor : SlangBaseVisitor
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
                    ThrowRepeatingModuleException(moduleName, ModuleData.File, module.Symbol);
                }
                if (!allModuleNames.Contains(moduleName))
                {
                    ThrowModuleNotFoundException(moduleName, ModuleData.File, module.Symbol);
                }
                if (moduleName == ModuleData.Name)
                {
                    ThrowModuleImportsItselfException(moduleName, ModuleData.File, module.Symbol);
                }
                if (moduleName == CompilerConstants.MainModuleName)
                {
                    ThrowUnableImportMainException(ModuleData.File, module.Symbol);
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
                ThrowModuleNameConflictFileNameException(moduleName, ModuleData.Name, ModuleData.File, moduleToken);
            }

            if (moduleName != CompilerConstants.MainModuleName && context.moduleStatementsSeq() != null)
            {
                ThrowEntryPointException(moduleName, ModuleData.File, context.moduleStatementsSeq().Start().Symbol);
            }
            if (moduleName == CompilerConstants.MainModuleName && context.moduleStatementsSeq() == null)
            {
                ThrowEntryPointMainException(ModuleData.File, context.Id().Symbol);
            }

            return base.VisitModule(context);
        }

        public override object VisitTypeDecl([NotNull] SLangGrammarParser.TypeDeclContext context)
        {
            var className = context.Id().GetText();
            ThrowIfReservedWord(className, ModuleData.File, context.Id().Symbol);

            if (moduleTable.Classes.ContainsKey(className))
            {
                ThrowClassRedefinitionException(className, ModuleData.File, context.Id().Symbol);
            }

            var isBase = context.Base() != null;
            var modifier = GetModifierByName(context.AccessModifier().GetText());

            var classItem = new ClassNameTableItem { TypeIdent = new SlangCustomType(ModuleData.Name, className), CanBeBase = isBase, Column = context.Id().Symbol.Column, Line = context.Id().Symbol.Line, AccessModifier = modifier };
            moduleTable.CheckCommonNamesConflicts(classItem.TypeIdent.Name, classItem.Line, classItem.Column);
            moduleTable.Classes[className] = classItem;
            return base.VisitTypeDecl(context);
        }

        public override object VisitEmptyTypeDecl([NotNull] SLangGrammarParser.EmptyTypeDeclContext context)
        {
            var classItem = new ClassNameTableItem
            {
                TypeIdent = new SlangCustomType(ModuleData.Name, context.Id().GetText()),
                AccessModifier = GetModifierByName(context.AccessModifier().GetText()),
                Base = null,
                CanBeBase = false,
                Column = context.Id().Symbol.Column,
                Line = context.Id().Symbol.Line,
                Header = Visit(context.importHead()) as ImportHeader
            };
            moduleTable.CheckCommonNamesConflicts(classItem.Name, classItem.Line, classItem.Column);
            moduleTable.Classes[classItem.Name] = classItem;
            return base.VisitEmptyTypeDecl(context);
        }
    }
}
