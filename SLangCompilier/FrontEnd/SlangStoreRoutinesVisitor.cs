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
        public SlangStoreRoutinesVisitor(SourceCodeTable table, ModuleData module): base(table, module)
        {
            moduleItem = table.Modules[module.Name];
        }

        public override object VisitClassDeclare([NotNull] SLGrammarParser.ClassDeclareContext context)
        {
            var classItem = moduleItem.Classes[context.Id().GetText()];
            if (context.inheritHead().customType() != null)
            {
                // Есть наследник
                classItem.Base = Visit(context.inheritHead().customType()) as SlangCustomType;
            }
            else
            {
                // Нету наследника, берем Object из System
                classItem.Base = Table.Modules[CompilerConstants.SystemModuleName].Classes[CompilerConstants.ObjectClassName].TypeIdent;
            }

            var moduleName = classItem.Base.ModuleName;
            var typeName = classItem.Base.Name;
            var errToken = context.Id().Symbol;

            CheckClassExists(moduleName, typeName, errToken);

            if (!Table.Modules[classItem.Base.ModuleName].Classes[classItem.Base.Name].CanBeBase)
            {
                ThrowException($"Class {classItem.Base} is not marked as base", errToken);
            }

            return null;
        }

        // store routines data here
    }
}
