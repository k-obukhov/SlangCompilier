using Antlr4.Runtime;
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

        private readonly ModuleNameTable moduleItem;
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

            var errToken = context.Id().Symbol;

            if (!Table.Modules[classItem.Base.ModuleName].Classes[classItem.Base.Name].CanBeBase)
            {
                ThrowException($"Class {classItem.Base} is not marked as base", errToken);
            }

            return null;
        }

        private void ThrowModuleFromOtherClassModuleException(Antlr4.Runtime.Tree.ITerminalNode token)
        {
            ThrowException($"Method with name {token.GetText()} refers to a class in another module", token.Symbol);
        }

        private void ThrowConfictsThisException(Antlr4.Runtime.Tree.ITerminalNode thisName)
        {
            ThrowException($"Name {thisName.GetText()} conflicts with one of the parameter's name", thisName.Symbol);
        }

        private void ThrowMethodSignatureExistsException(SlangCustomType classData, Antlr4.Runtime.Tree.ITerminalNode name)
        {
            ThrowException($"Method with same signature already exists in class {classData}", name.Symbol);
        }

        private void ThrowRoutineExistsException(Antlr4.Runtime.Tree.ITerminalNode token)
        {
            ThrowException($"Procedure or function {token.GetText()} with same signature already exists", token.Symbol);
        }
        // store methods
        public override object VisitMethodFuncDeclare([NotNull] SLGrammarParser.MethodFuncDeclareContext context)
        {
            // find class
            var classData = Visit(context.thisHeader().customType()) as SlangCustomType;
            var foundClass = Table.FindClass(classData);
            
            if (classData.ModuleName != ModuleData.Name)
            {
                ThrowModuleFromOtherClassModuleException(context.Id());
            }
            else
            {
                // add class item
                // check thisName != method args
                // check that no method with same signature
                // check name not a keyword
                var thisName = context.thisHeader().Id();
                var nameMethod = context.Id().GetText();
                var name = context.Id();
                var modifier = GetModifierByName(context.AccessModifier().GetText());
                var isOverride = context.Override() == null ? true : false;
                var returnType = Visit(context.typeName()) as SlangType;

                ThrowIfReservedWord(name.GetText(), context.Id().Symbol);
                ThrowIfReservedWord(thisName.GetText(), thisName.Symbol);
                var args = Visit(context.functionalDeclareArgList()) as IList<RoutineArgNameTableItem>;
                if (args.Any(a => a.Name == thisName.GetText()))
                {
                    ThrowConfictsThisException(thisName);
                }
                if (foundClass.Methods.Any(m => m.Name == name.GetText() && m.Params.SequenceEqual(args)))
                {
                    ThrowMethodSignatureExistsException(classData, name);
                }

                foundClass.Methods.Add(new MethodNameTableItem
                {
                    AccessModifier = modifier,
                    IsAbstract = false,
                    IsOverride = isOverride,
                    NameOfThis = thisName.GetText(),
                    Params = args,
                    Name = name.GetText(),
                    ReturnType = returnType,
                    Column = name.Symbol.Column,
                    Line = name.Symbol.Line
                });
            }
            return null;
        }
        public override object VisitMethodFuncAbstract([NotNull] SLGrammarParser.MethodFuncAbstractContext context)
        {
            // find class
            var classData = Visit(context.thisHeader().customType()) as SlangCustomType;
            var foundClass = Table.FindClass(classData);

            if (classData.ModuleName != ModuleData.Name)
            {
                ThrowModuleFromOtherClassModuleException(context.Id());
            }
            else
            {
                // add class item
                // check thisName != method args
                // check that no method with same signature
                // check name not a keyword
                var thisName = context.thisHeader().Id();
                var nameMethod = context.Id().GetText();
                var name = context.Id();
                var modifier = GetModifierByName(context.AccessModifier().GetText());
                var returnType = Visit(context.typeName()) as SlangType;

                ThrowIfReservedWord(name.GetText(), context.Id().Symbol);
                ThrowIfReservedWord(thisName.GetText(), thisName.Symbol);
                var args = Visit(context.functionalDeclareArgList()) as IList<RoutineArgNameTableItem>;
                if (args.Any(a => a.Name == thisName.GetText()))
                {
                    ThrowConfictsThisException(thisName);
                }
                if (foundClass.Methods.Any(m => m.Name == name.GetText() && m.Params.SequenceEqual(args)))
                {
                    ThrowMethodSignatureExistsException(classData, name);
                }

                foundClass.Methods.Add(new MethodNameTableItem
                {
                    AccessModifier = modifier,
                    IsAbstract = true,
                    IsOverride = false,
                    NameOfThis = thisName.GetText(),
                    Params = args,
                    Name = name.GetText(),
                    ReturnType = returnType,
                    Column = name.Symbol.Column,
                    Line = name.Symbol.Line
                });
            }
            return null;
        }
        public override object VisitMethodProcAbstract([NotNull] SLGrammarParser.MethodProcAbstractContext context)
        {
            // find class
            var classData = Visit(context.thisHeader().customType()) as SlangCustomType;
            var foundClass = Table.FindClass(classData);

            if (classData.ModuleName != ModuleData.Name)
            {
                ThrowModuleFromOtherClassModuleException(context.Id());
            }
            else
            {
                // add class item
                // check thisName != method args
                // check that no method with same signature
                // check name not a keyword
                var thisName = context.thisHeader().Id();
                var nameMethod = context.Id().GetText();
                var name = context.Id();
                var modifier = GetModifierByName(context.AccessModifier().GetText());

                ThrowIfReservedWord(name.GetText(), context.Id().Symbol);
                ThrowIfReservedWord(thisName.GetText(), thisName.Symbol);
                var args = Visit(context.functionalDeclareArgList()) as IList<RoutineArgNameTableItem>;
                if (args.Any(a => a.Name == thisName.GetText()))
                {
                    ThrowConfictsThisException(thisName);
                }
                if (foundClass.Methods.Any(m => m.Name == name.GetText() && m.Params.SequenceEqual(args)))
                {
                    ThrowMethodSignatureExistsException(classData, name);
                }

                foundClass.Methods.Add(new MethodNameTableItem
                {
                    AccessModifier = modifier,
                    IsAbstract = true,
                    IsOverride = false,
                    NameOfThis = thisName.GetText(),
                    Params = args,
                    Name = name.GetText(),
                    Column = name.Symbol.Column,
                    Line = name.Symbol.Line
                });
            }
            return null;
        }
        public override object VisitMethodProcDeclare([NotNull] SLGrammarParser.MethodProcDeclareContext context)
        {
            // find class
            var classData = Visit(context.thisHeader().customType()) as SlangCustomType;
            var foundClass = Table.FindClass(classData);

            if (classData.ModuleName != ModuleData.Name)
            {
                ThrowModuleFromOtherClassModuleException(context.Id());
            }
            else
            {
                // add class item
                // check thisName != method args
                // check that no method with same signature
                // check name not a keyword
                var thisName = context.thisHeader().Id();
                var nameMethod = context.Id().GetText();
                var name = context.Id();
                var modifier = GetModifierByName(context.AccessModifier().GetText());
                var isOverride = context.Override() == null ? true : false;

                ThrowIfReservedWord(name.GetText(), context.Id().Symbol);
                ThrowIfReservedWord(thisName.GetText(), thisName.Symbol);
                var args = Visit(context.functionalDeclareArgList()) as IList<RoutineArgNameTableItem>;
                if (args.Any(a => a.Name == thisName.GetText()))
                {
                    ThrowConfictsThisException(thisName);
                }
                if (foundClass.Methods.Any(m => m.Name == name.GetText() && m.Params.SequenceEqual(args)))
                {
                    ThrowMethodSignatureExistsException(classData, name);
                }

                foundClass.Methods.Add(new MethodNameTableItem
                {
                    AccessModifier = modifier,
                    IsAbstract = false,
                    IsOverride = isOverride,
                    NameOfThis = thisName.GetText(),
                    Params = args,
                    Name = name.GetText(),
                    Column = name.Symbol.Column,
                    Line = name.Symbol.Line
                });
            }
            return null;
        }
        // store function
        public override object VisitFunctionDeclare([NotNull] SLGrammarParser.FunctionDeclareContext context)
        {
            var name = context.Id().GetText();
            ThrowIfReservedWord(name, context.Id().Symbol);
            var modifier = GetModifierByName(context.AccessModifier().GetText());
            var args = Visit(context.functionalDeclareArgList()) as IList<RoutineArgNameTableItem>;
            var token = context.Id();
            var returnType = Visit(context.typeName()) as SlangType;

            if (moduleItem.Routines.Any(r => r.Name == name && r.Params.SequenceEqual(args)))
            {
                ThrowRoutineExistsException(token);
            }

            moduleItem.Routines.Add(new RoutineNameTableItem { AccessModifier = modifier, Name = name, Params = args, Column = token.Symbol.Column, Line = token.Symbol.Line, ReturnType = returnType });
            return null;
        }
        // store procedure
        public override object VisitProcedureDeclare([NotNull] SLGrammarParser.ProcedureDeclareContext context)
        {
            var name = context.Id().GetText();
            ThrowIfReservedWord(name, context.Id().Symbol);
            var modifier = GetModifierByName(context.AccessModifier().GetText());
            var args = Visit(context.functionalDeclareArgList()) as IList<RoutineArgNameTableItem>;
            var token = context.Id();

            if (moduleItem.Routines.Any(r => r.Name == name && r.Params.SequenceEqual(args)))
            {
                ThrowRoutineExistsException(token);
            }

            moduleItem.Routines.Add(new RoutineNameTableItem { AccessModifier = modifier, Name = name, Params = args, Column = token.Symbol.Column, Line = token.Symbol.Line });
            return null;
        }
        // store arguments
        public override object VisitFunctionalDeclareArg([NotNull] SLGrammarParser.FunctionalDeclareArgContext context)
        {
            var modifier = GetParamModifierByName(context.FunctionArgModifier().GetText());
            var type = Visit(context.typeName()) as SlangType;
            var name = context.Id().GetText();
            ThrowIfReservedWord(name, context.Id().Symbol);

            return new RoutineArgNameTableItem { Name = name, Type = new SlangRoutineTypeArg(modifier, type), Column = context.Id().Symbol.Column, Line = context.Id().Symbol.Line};
        }

        public override object VisitCustomType([NotNull] SLGrammarParser.CustomTypeContext context)
        {
            var classItem = base.VisitCustomType(context) as SlangCustomType;
            CheckClassExists(classItem.ModuleName, classItem.Name, context.id().Id().First().Symbol);
            return classItem;
        }

        public override object VisitFunctionalDeclareArgList([NotNull] SLGrammarParser.FunctionalDeclareArgListContext context)
        {
            IList<RoutineArgNameTableItem> res = new List<RoutineArgNameTableItem>(context.functionalDeclareArg().Length);

            foreach (var arg in context.functionalDeclareArg())
            {
                var routineArg = (RoutineArgNameTableItem)Visit(arg);
                if (res.Any(a => a.Name == routineArg.Name))
                {
                    ThrowException($"Parameter with name {routineArg.Name} already defined", arg.Id().Symbol);
                }
                res.Add(routineArg);
            }

            return res;
        }
        // store fields of class / module
        public override object VisitFieldDeclare([NotNull] SLGrammarParser.FieldDeclareContext context)
        {
            return base.VisitFieldDeclare(context);
        }

        public override object VisitVarModuleDeclare([NotNull] SLGrammarParser.VarModuleDeclareContext context)
        {
            return base.VisitVarModuleDeclare(context);
        }
    }
}
