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

// ToDo приватные классы не могут быть в публичных полях?
// ToDo приватные классы не могут быть возвращаемым значением и параметром публичных методов и функций?

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
        public override object VisitImportHeader([NotNull] SLGrammarParser.ImportHeaderContext context)
        {
            return new ImportHeader(context?.StringLiteral()[0].GetText(), context?.StringLiteral()[1].GetText());
        }

        /// <summary>
        /// checks access level for public routines -- they should not have a private class in args or return type (for funcs)
        /// </summary>
        /// <param name="items">routine args</param>
        /// <param name="returnType">return type (may be null for procedures)</param>
        /// <param name="routineToken">token for exception data</param>
        /// <param name="routineName">name of routine</param>
        private void CheckLevelAccessForRoutines(IList<RoutineArgNameTableItem> items, SlangType returnType, Antlr4.Runtime.Tree.ITerminalNode routineToken, string routineName)
        {
            foreach (var item in items)
            {
                var typeArg = item.TypeArg.Type;
                if (typeArg is SlangCustomType customType)
                {
                    var classItem = Table.Modules[customType.ModuleName].Classes[customType.Name];
                    if (classItem.AccessModifier == AccessModifier.Private)
                    {
                        ThrowLevelAccessibilityException(routineToken, customType.ToString(), routineName);
                    }
                }
            }
            if (returnType != null)
            {
                if (returnType is SlangCustomType type)
                {
                    var classItem = Table.Modules[type.ModuleName].Classes[type.Name];
                    if (classItem.AccessModifier == AccessModifier.Private)
                    {
                        ThrowLevelAccessibilityException(routineToken, type.ToString(), routineName);
                    }
                }
            }
        }

        private void CheckLevelAccessForMethods(MethodNameTableItem method, Antlr4.Runtime.Tree.ITerminalNode routineToken, SlangCustomType classIdent)
        {
            foreach (var item in method.Params)
            {
                var typeArg = item.TypeArg.Type;
                if (typeArg is SlangCustomType customType)
                {
                    var classItem = Table.Modules[customType.ModuleName].Classes[customType.Name];
                    if (classItem.AccessModifier == AccessModifier.Private && customType != classIdent)
                    {
                        ThrowLevelAccessibilityException(routineToken, customType.ToString(), method.Name);
                    }
                }
            }
            if (method.ReturnType != null)
            {
                if (method.ReturnType is SlangCustomType type)
                {
                    var classItem = Table.Modules[type.ModuleName].Classes[type.Name];
                    if (classItem.AccessModifier == AccessModifier.Private)
                    {
                        ThrowLevelAccessibilityException(routineToken, type.ToString(), method.Name);
                    }
                }
            }
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

            // store fields
            foreach(var fieldContext in context.classStatements().fieldDeclare())
            {
                var item = Visit(fieldContext) as FieldNameTableItem;
                if (classItem.Fields.ContainsKey(item.Name))
                {
                    ThrowException($"Field {item.Name} already defined in class {context.Id().GetText()}", fieldContext.varDeclare().Start);
                }
                // check level of access
                if (item.Type is SlangCustomType t)
                {
                    var typeOfItem = Table.FindClass(t);
                    // если поле класса публично, а тип поля приватный, но при этом тип поля не тип класса
                    if (item.AccessModifier == AccessModifier.Public && typeOfItem.AccessModifier == AccessModifier.Private && (t != classItem.TypeIdent))
                    {
                        ThrowException($"Level of accessibility of field {item.Name} more than type {t}", fieldContext.varDeclare().Start);
                    }
                }
                classItem.Fields.Add(item.Name, item);
                //classItem.Fields.Add(item.Name, new FieldNameTableItem { AccessModifier = GetModifierByName(fieldContext.AccessModifier().GetText()), Name = item.Name, IsConstant = item.IsConstant, Column = item.Column, Line = item.Line, Type = item.Type });
            }

            return base.VisitClassDeclare(context);
        }

        private void ThrowLevelAccessibilityException(Antlr4.Runtime.Tree.ITerminalNode token, string className, string routineName)
        {
            ThrowException($"Level of accessibility of type {className} less than access to routine {routineName}", token.Symbol);
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

        private void ThrowIfAbstractMethodPrivate(AccessModifier modifier, IToken symbol)
        {
            if (modifier == AccessModifier.Private)
            {
                ThrowException("Abstract methods cannot be private", symbol);
            }
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

                var method = new MethodNameTableItem
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
                };

                if (modifier == AccessModifier.Public)
                {
                    CheckLevelAccessForMethods(method, context.Id(), classData);
                }

                foundClass.Methods.Add(method);
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
                ThrowIfAbstractMethodPrivate(modifier, name.Symbol);
                var args = Visit(context.functionalDeclareArgList()) as IList<RoutineArgNameTableItem>;
                if (args.Any(a => a.Name == thisName.GetText()))
                {
                    ThrowConfictsThisException(thisName);
                }
                if (foundClass.Methods.Any(m => m.Name == name.GetText() && m.Params.SequenceEqual(args)))
                {
                    ThrowMethodSignatureExistsException(classData, name);
                }

                var method = new MethodNameTableItem
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
                };

                if (modifier == AccessModifier.Public)
                {
                    CheckLevelAccessForMethods(method, context.Id(), classData);
                }

                foundClass.Methods.Add(method);
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
                ThrowIfAbstractMethodPrivate(modifier, name.Symbol);
                var args = Visit(context.functionalDeclareArgList()) as IList<RoutineArgNameTableItem>;
                if (args.Any(a => a.Name == thisName.GetText()))
                {
                    ThrowConfictsThisException(thisName);
                }
                if (foundClass.Methods.Any(m => m.Name == name.GetText() && m.Params.SequenceEqual(args)))
                {
                    ThrowMethodSignatureExistsException(classData, name);
                }

                var method = new MethodNameTableItem
                {
                    AccessModifier = modifier,
                    IsAbstract = true,
                    IsOverride = false,
                    NameOfThis = thisName.GetText(),
                    Params = args,
                    Name = name.GetText(),
                    Column = name.Symbol.Column,
                    Line = name.Symbol.Line
                };

                if (modifier == AccessModifier.Public)
                {
                    CheckLevelAccessForMethods(method, context.Id(), classData);
                }

                foundClass.Methods.Add(method);
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

                var method = new MethodNameTableItem
                {
                    AccessModifier = modifier,
                    IsAbstract = false,
                    IsOverride = isOverride,
                    NameOfThis = thisName.GetText(),
                    Params = args,
                    Name = name.GetText(),
                    Column = name.Symbol.Column,
                    Line = name.Symbol.Line
                };

                if (modifier == AccessModifier.Public)
                {
                    CheckLevelAccessForMethods(method, context.Id(), classData);
                }

                foundClass.Methods.Add(method);
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
            var importHeader = context?.importHeader() != null ? Visit(context?.importHeader()) as ImportHeader : null;

            if (moduleItem.Routines.Any(r => r.Name == name && r.Params.SequenceEqual(args)))
            {
                ThrowRoutineExistsException(token);
            }

            if (modifier == AccessModifier.Public)
            {
                CheckLevelAccessForRoutines(args, returnType, context.Id(), name);
            }

            moduleItem.Routines.Add(new RoutineNameTableItem { AccessModifier = modifier, Name = name, Params = args, Column = token.Symbol.Column, Line = token.Symbol.Line, ReturnType = returnType, Header = importHeader });
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
            var importHeader = context?.importHeader() != null ? Visit(context?.importHeader()) as ImportHeader : null;

            if (moduleItem.Routines.Any(r => r.Name == name && r.Params.SequenceEqual(args)))
            {
                ThrowRoutineExistsException(token);
            }

            if (modifier == AccessModifier.Public)
            {
                CheckLevelAccessForRoutines(args, null, context.Id(), name);
            }

            moduleItem.Routines.Add(new RoutineNameTableItem { AccessModifier = modifier, Name = name, Params = args, Column = token.Symbol.Column, Line = token.Symbol.Line, Header = importHeader });
            return null;
        }
        // store arguments
        public override object VisitFunctionalDeclareArg([NotNull] SLGrammarParser.FunctionalDeclareArgContext context)
        {
            var modifier = GetParamModifierByName(context.FunctionArgModifier().GetText());
            var type = Visit(context.typeName()) as SlangType;
            var name = context.Id().GetText();
            ThrowIfReservedWord(name, context.Id().Symbol);

            return new RoutineArgNameTableItem { Name = name, TypeArg = new SlangRoutineTypeArg(modifier, type), Column = context.Id().Symbol.Column, Line = context.Id().Symbol.Line};
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
        // fieldDeclare -> varDeclare
        // varModuleDeclare -> declare -> (varDeclare | constDeclare)
        // constDeclare -> anyType
        // varDeclare -> ptrDeclare (ptrType) | arrDeclare (arrType) | scalarDeclare (anyType - array - ptr)
        public override object VisitFieldDeclare([NotNull] SLGrammarParser.FieldDeclareContext context)
        {
            var data = Visit(context.varDeclare()) as VariableNameTableItem;
            var modifier = GetModifierByName(context.AccessModifier().GetText());
            return new FieldNameTableItem { AccessModifier = modifier, Column = data.Column, IsConstant = data.IsConstant, Line = data.Line, Name = data.Name, Type = data.Type };

        }

        public override object VisitVarModuleDeclare([NotNull] SLGrammarParser.VarModuleDeclareContext context)
        {
            var data = Visit(context.declare()) as VariableNameTableItem;
            var isReadonly = context.Readonly() == null ? true : false;
            var importHeader = context?.importHeader() != null ? Visit(context?.importHeader()) as ImportHeader : null;

            if (moduleItem.Fields.ContainsKey(data.Name))
            {
                ThrowIfVariableExistsException(data.Name, data.Line, data.Column);
            }
            
            var item = new ModuleFieldNameTableItem { AccessModifier = GetModifierByName(context.AccessModifier().GetText()),
                IsConstant = data.IsConstant,
                Column = data.Column,
                Line = data.Line,
                IsReadonly = isReadonly,
                Name = data.Name,
                Type = data.Type,
                Header = importHeader
            };

            if (item.Type is SlangCustomType t)
            {
                var typeOfItem = Table.FindClass(t);
                // если поле класса публично, а тип поля приватный, но при этом тип поля не тип класса
                if (item.AccessModifier == AccessModifier.Public && typeOfItem.AccessModifier == AccessModifier.Private)
                {
                    ThrowException($"Level of accessibility of field {item.Name} more than type {t}", data.Line, data.Column);
                }
            }

            moduleItem.Fields[data.Name] = item;
            return base.VisitVarModuleDeclare(context);
        }
        // var & const declare
        public override object VisitDeclare([NotNull] SLGrammarParser.DeclareContext context)
        {
            if (context.varDeclare() != null)
            {
                return Visit(context.varDeclare());
            }
            else
            {
                return Visit(context.constDeclare());
            }
        }
        public override object VisitVarDeclare([NotNull] SLGrammarParser.VarDeclareContext context)
        {
            if (context.arrayDeclare() != null)
            {
                return Visit(context.arrayDeclare());
            }
            else if (context.scalarDeclare() != null)
            {
                return Visit(context.scalarDeclare());
            }
            else
            {
                return Visit(context.ptrDeclare());
            }
        }
        public override object VisitConstDeclare([NotNull] SLGrammarParser.ConstDeclareContext context)
        {
            var type = Visit(context.typeName()) as SlangType;
            var name = context.Id();
            ThrowIfReservedWord(name.GetText(), name.Symbol);
            return new VariableNameTableItem { IsConstant = true, Name = name.GetText(), Type = type, Column = name.Symbol.Column, Line = name.Symbol.Line };
        }
        // var -> scalar
        public override object VisitScalarDeclare([NotNull] SLGrammarParser.ScalarDeclareContext context)
        {
            var type = Visit(context.scalarType()) as SlangType;
            var name = context.Id();
            ThrowIfReservedWord(name.GetText(), name.Symbol);
            return new VariableNameTableItem { IsConstant = false, Type = type, Name = name.GetText(), Column = name.Symbol.Column, Line = name.Symbol.Line };
        }
        // var -> arr
        public override object VisitArrayDeclare([NotNull] SLGrammarParser.ArrayDeclareContext context)
        {
            var type = Visit(context.arrayDeclareType().scalarType()) as SlangType;
            var dimensiton = context.arrayDeclareType().arrayDeclareDimention().Length;
            var name = context.Id();
            ThrowIfReservedWord(name.GetText(), name.Symbol);
            return new VariableNameTableItem { IsConstant = false, Type = new SlangArrayType(type, dimensiton), Line = name.Symbol.Line, Column = name.Symbol.Column, Name = name.GetText() };
        }
        // var -> ptr
        public override object VisitPtrDeclare([NotNull] SLGrammarParser.PtrDeclareContext context)
        {
            var type = Visit(context.ptrType()) as SlangType;
            var name = context.Id();
            ThrowIfReservedWord(name.GetText(), name.Symbol);
            return new VariableNameTableItem { IsConstant = false, Type = type, Column = name.Symbol.Column, Line = name.Symbol.Line, Name = name.GetText() };
        }
    }
}
