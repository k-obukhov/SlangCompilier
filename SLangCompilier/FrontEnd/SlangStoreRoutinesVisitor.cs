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
    public class SlangStoreRoutinesVisitor: SlangBaseStepVisitor
    {
        /// <summary>
        /// Второй проход -- сбор данных о функциях и процедурах
        /// Обязанности:
        /// 1) Сбор данных о процедурах и функциях (без проверки тела)
        /// 2) Сбор данных о методах (без проверки тела)
        /// 3) Сбор данных о полях модулей (без проверки значений, им присвоенных)
        /// 4) Сбор данных о классах (проверка на существование базового класса и т.д)
        /// </summary>

        private ModuleNameTable moduleItem;
        public SlangStoreRoutinesVisitor(SourceCodeTable table, ModuleData module, string moduleName): base(table, module)
        {
            moduleItem = table.Modules[moduleName];
        }

        public override object VisitClassDeclare([NotNull] SLGrammarParser.ClassDeclareContext context)
        {
            var classItem = moduleItem.Classes[context.Id().GetText()];
            if (context.inherit_head().id() != null)
            {
                // Есть наследник
                classItem.Base = FindTypeByName(context.inherit_head().id(), moduleItem);
            }
            else
            {
                // Нету наследника, берем Object из System
                classItem.Base = Table.Modules[CompilerConstants.SystemModuleName].Classes[CompilerConstants.ObjectClassName].TypeIdent;
            }
            return null;
        }
    }
}
