using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using SLangCompiler.FileServices;
using SLangCompiler.FrontEnd.Tables;
using SLangCompiler.FrontEnd.Types;
using SLangGrammar;
using System;
using System.Collections.Generic;
using System.Linq;
using static SLangCompiler.Exceptions.CompilerErrors;

namespace SLangCompiler.FrontEnd
{
    public class SlangStoreRoutinesVisitor: SlangBaseVisitor
    {
        private readonly ModuleNameTable moduleItem;
        public SlangStoreRoutinesVisitor(SourceCodeTable table, ModuleData module) : base(table, module)
        {
            moduleItem = table.Modules[module.Name];
        }

        public override object VisitImportHead([NotNull] SLangGrammarParser.ImportHeadContext context) => new ImportHeader(context?.StringLiteral()[0].GetText(), context?.StringLiteral()[1].GetText());
        public override object VisitThisHeader([NotNull] SLangGrammarParser.ThisHeaderContext context) => Visit(context.customType());

        /// <summary>
        /// return custom type for pointer and custom types
        /// </summary>
        /// <param name="type">type</param>
        /// <returns>custom type (if exists), else null</returns>
        private SlangCustomType GetUserType(SlangType type)
        {
            SlangCustomType usedType = null;
            if (type is SlangCustomType t)
            {
                usedType = t;
            }
            else if (type is SlangPointerType pt)
            {
                usedType = pt.PtrType;
            }
            return usedType;
        }
        private void CheckLevelAccessForRoutines(RoutineNameTableItem routine, Antlr4.Runtime.Tree.ITerminalNode routineToken, string routineName)
        {
            foreach (var item in routine.Params)
            {
                var typeArg = item.TypeArg.Type;
                if (typeArg is SlangCustomType || typeArg is SlangPointerType)
                {
                    var customType = GetUserType(typeArg);
                    var classItem = Table.Modules[customType.ModuleName].Classes[customType.Name];
                    if (classItem.AccessModifier == AccessModifier.Private)
                    {
                        ThrowLevelAccessibilityException(routineToken, ModuleData.File, customType.ToString(), routineName);
                    }
                }
            }
            if (routine.ReturnType != null)
            {
                if (routine.ReturnType is SlangCustomType || routine.ReturnType is SlangPointerType)
                {
                    var type = GetUserType(routine.ReturnType);
                    var classItem = Table.Modules[type.ModuleName].Classes[type.Name];
                    if (classItem.AccessModifier == AccessModifier.Private)
                    {
                        ThrowLevelAccessibilityException(routineToken, ModuleData.File, type.ToString(), routineName);
                    }
                }
            }
        }

        private void CheckLevelAccessForMethods(MethodNameTableItem method, Antlr4.Runtime.Tree.ITerminalNode routineToken, SlangCustomType classIdent)
        {
            foreach (var item in method.Params)
            {
                var typeArg = item.TypeArg.Type;
                if (typeArg is SlangCustomType || typeArg is SlangPointerType)
                {
                    var customType = GetUserType(typeArg);
                    var classItem = Table.Modules[customType.ModuleName].Classes[customType.Name];
                    if (classItem.AccessModifier == AccessModifier.Private && customType != classIdent)
                    {
                        ThrowLevelAccessibilityException(routineToken, ModuleData.File, customType.ToString(), method.Name);
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
                        ThrowLevelAccessibilityException(routineToken, ModuleData.File, type.ToString(), method.Name);
                    }
                }
            }
        }

        public override object VisitFunctionDecl([NotNull] SLangGrammarParser.FunctionDeclContext context)
        {
            var isMethod = context.thisHeader() != null;
            var symbol = context.Id().Symbol;

            string nameOfThis = string.Empty;
            if (isMethod)
            {
                nameOfThis = context.thisHeader().Id().GetText();
                ThrowIfReservedWord(nameOfThis, ModuleData.File, context.thisHeader().Id().Symbol);
                if (context.importHead() != null)
                {
                    ThrowImportHeaderMethodsException(ModuleData.File, context.Id());
                }
            }
            var name = context.Id().GetText();
            ThrowIfReservedWord(name, ModuleData.File, context.thisHeader().Id().Symbol);
            var args = Visit(context.routineArgList()) as List<RoutineArgNameTableItem>;
            ImportHeader header = null;
            if (context.importHead() != null)
            {
                header = Visit(context.importHead()) as ImportHeader;
            }

            var returnType = Visit(context.typeName()) as SlangType;
            var modifier = GetModifierByName(context.AccessModifier().GetText());

            var isAbstract = context.Abstract() != null;
            var isOverride = context.Override() != null;

            if (!isMethod && (isAbstract || isOverride))
            {
                ThrowRoutinesAbstractOverrideException(ModuleData.File, context.Abstract() ?? context.Override());
            }

            if (header != null && context.statementSeq().statement().Length != 0)
            {
                ThrowImportHeaderException(ModuleData.File, context.Id());
            }

            if (isMethod)
            {
                var methodTypeIdent = Visit(context.thisHeader()) as SlangCustomType;
                if (methodTypeIdent.ModuleName != ModuleData.Name)
                {
                    ThrowModuleFromOtherClassModuleException(context.Id(), ModuleData.File);
                }
                if (isAbstract)
                {
                    ThrowIfAbstractMethodPrivate(modifier, ModuleData.File, context.Id());
                }
                if (args.Any(a => a.Name == nameOfThis))
                {
                    ThrowConfictsThisException(context.thisHeader().Id(), ModuleData.File);
                }
                var classData = Visit(context.thisHeader()) as SlangCustomType;
                var foundClass = Table.FindClass(classData);

                if (foundClass.Methods.Any(m => m.Name == name && m.Params.SequenceEqual(args)))
                {
                    ThrowMethodSignatureExistsException(classData, context.Id(), ModuleData.File);
                }

                var method = new MethodNameTableItem
                {
                    AccessModifier = modifier,
                    Column = symbol.Column,
                    Header = header,
                    IsAbstract = isAbstract,
                    IsOverride = isOverride,
                    Line = symbol.Line,
                    Name = name,
                    NameOfThis = nameOfThis,
                    Params = args,
                    ReturnType = returnType
                };
                if (modifier == AccessModifier.Public)
                {
                    CheckLevelAccessForMethods(method, context.Id(), classData);
                }
                
                foundClass.Methods.Add(method);
            }
            else
            {
                if (moduleItem.Routines.Any(r => r.Name == name && r.Params.SequenceEqual(args)))
                {
                    ThrowRoutineExistsException(context.Id(), ModuleData.File);
                }

                var routine = new RoutineNameTableItem
                {
                    AccessModifier = modifier,
                    Column = symbol.Column,
                    Line = symbol.Line,
                    Header = header,
                    Name = name,
                    Params = args,
                    ReturnType = returnType
                };

                if (modifier == AccessModifier.Public)
                {
                    CheckLevelAccessForRoutines(routine, context.Id(), name);
                }

                moduleItem.Routines.Add(routine);
            }

            return base.VisitFunctionDecl(context);
        }

        public override object VisitRoutineArgList([NotNull] SLangGrammarParser.RoutineArgListContext context)
        {
            IList<RoutineArgNameTableItem> res = new List<RoutineArgNameTableItem>(context.routineArg().Length);

            foreach (var arg in context.routineArg())
            {
                var routineArg = (RoutineArgNameTableItem)Visit(arg);
                if (res.Any(a => a.Name == routineArg.Name))
                {
                    ThrowException($"Parameter with name {routineArg.Name} already defined", ModuleData.File, arg.Id().Symbol);
                }
                res.Add(routineArg);
            }

            return res;
        }

        // same as functions only without return type (maybe i can optimize that later)
        public override object VisitProcedureDecl([NotNull] SLangGrammarParser.ProcedureDeclContext context)
        {
            var isMethod = context.thisHeader() != null;
            var symbol = context.Id().Symbol;

            string nameOfThis = string.Empty;
            if (isMethod)
            {
                nameOfThis = context.thisHeader().Id().GetText();
                ThrowIfReservedWord(nameOfThis, ModuleData.File, context.thisHeader().Id().Symbol);
                if (context.importHead() != null)
                {
                    ThrowImportHeaderMethodsException(ModuleData.File, context.Id());
                }
            }
            var name = context.Id().GetText();
            ThrowIfReservedWord(name, ModuleData.File, context.thisHeader().Id().Symbol);
            var args = Visit(context.routineArgList()) as List<RoutineArgNameTableItem>;
            ImportHeader header = null;
            if (context.importHead() != null)
            {
                header = Visit(context.importHead()) as ImportHeader;
            }

            var modifier = GetModifierByName(context.AccessModifier().GetText());

            var isAbstract = context.Abstract() != null;
            var isOverride = context.Override() != null;

            if (!isMethod && (isAbstract || isOverride))
            {
                ThrowRoutinesAbstractOverrideException(ModuleData.File, context.Abstract() ?? context.Override());
            }

            if (header != null && context.statementSeq().statement().Length != 0)
            {
                ThrowImportHeaderException(ModuleData.File, context.Id());
            }

            if (isMethod)
            {
                var methodTypeIdent = Visit(context.thisHeader()) as SlangCustomType;
                if (methodTypeIdent.ModuleName != ModuleData.Name)
                {
                    ThrowModuleFromOtherClassModuleException(context.Id(), ModuleData.File);
                }
                if (isAbstract)
                {
                    ThrowIfAbstractMethodPrivate(modifier, ModuleData.File, context.Id());
                }
                if (args.Any(a => a.Name == nameOfThis))
                {
                    ThrowConfictsThisException(context.thisHeader().Id(), ModuleData.File);
                }
                var classData = Visit(context.thisHeader()) as SlangCustomType;
                var foundClass = Table.FindClass(classData);

                if (foundClass.Methods.Any(m => m.Name == name && m.Params.SequenceEqual(args)))
                {
                    ThrowMethodSignatureExistsException(classData, context.Id(), ModuleData.File);
                }

                var method = new MethodNameTableItem
                {
                    AccessModifier = modifier,
                    Column = symbol.Column,
                    Header = header,
                    IsAbstract = isAbstract,
                    IsOverride = isOverride,
                    Line = symbol.Line,
                    Name = name,
                    NameOfThis = nameOfThis,
                    Params = args,
                    ReturnType = null
                };
                if (modifier == AccessModifier.Public)
                {
                    CheckLevelAccessForMethods(method, context.Id(), classData);
                }

                foundClass.Methods.Add(method);
            }
            else
            {
                if (moduleItem.Routines.Any(r => r.Name == name && r.Params.SequenceEqual(args)))
                {
                    ThrowRoutineExistsException(context.Id(), ModuleData.File);
                }

                var routine = new RoutineNameTableItem
                {
                    AccessModifier = modifier,
                    Column = symbol.Column,
                    Line = symbol.Line,
                    Header = header,
                    Name = name,
                    Params = args,
                    ReturnType = null
                };

                if (modifier == AccessModifier.Public)
                {
                    CheckLevelAccessForRoutines(routine, context.Id(), name);
                }

                moduleItem.Routines.Add(routine);
            }
            return base.VisitProcedureDecl(context);
        }
    }
}
