using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using SLangCompiler.FileServices;
using SLangCompiler.FrontEnd.Tables;
using SLangCompiler.FrontEnd.Types;
using SLangGrammar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using static SLangCompiler.Exceptions.CompilerErrors;

namespace SLangCompiler.FrontEnd
{
    public class SlangSemanticVisitor: SlangBaseVisitor
    {
        private readonly ModuleNameTable moduleItem;
        private RoutineNameTableItem currentRoutine;
        private readonly Scope scope;
        private SlangCustomType currentType;
        private readonly FileInfo file;
        public SlangSemanticVisitor(SourceCodeTable table, ModuleData module) : base(table, module)
        {
            moduleItem = table.Modules[module.Name];
            scope = new Scope(new Dictionary<string, VariableNameTableItem>()); // не включает в себя глобальную область видимости
            file = moduleItem.ModuleData.File;
        }

        public override object VisitFunctionDecl([NotNull] SLangGrammarParser.FunctionDeclContext context)
        {
            var symbol = context.Id().Symbol;
            InitializeRoutineStates(context.thisHeader(), symbol);
            // some work...
            currentType = null;
            currentRoutine = null;
            return base.VisitFunctionDecl(context);
        }

        public override object VisitProcedureDecl([NotNull] SLangGrammarParser.ProcedureDeclContext context)
        {
            var symbol = context.Id().Symbol;
            InitializeRoutineStates(context.thisHeader(), symbol);
            // some work...
            currentType = null;
            currentRoutine = null;
            return base.VisitProcedureDecl(context);
        }

        private void InitializeRoutineStates(SLangGrammarParser.ThisHeaderContext context, IToken symbol)
        {
            currentType = context != null ? Visit(context.customType()) as SlangCustomType : null;
            if (currentType != null)
            {
                currentRoutine = Table.FindClass(currentType).Methods.First(m => m.Line == symbol.Line && m.Column == symbol.Column);
            }
            else
            {
                currentRoutine = moduleItem.Routines.First(r => r.Line == symbol.Line && r.Column == symbol.Column);
            }
        }

        public override object VisitTypeDecl([NotNull] SLangGrammarParser.TypeDeclContext context)
        {
            currentType = new SlangCustomType(ModuleData.Name, context.Id().GetText());
            // ToDo checks expressions in fields
            currentType = null;
            return base.VisitTypeDecl(context);
        }

        public override object VisitModuleStatementsSeq([NotNull] SLangGrammarParser.ModuleStatementsSeqContext context)
        {
            // state -- work in module statements
            currentRoutine = null;
            currentType = null;
            //ToDo checks expressions in fields
            return base.VisitModuleStatementsSeq(context);
        }

        public override object VisitModuleFieldDecl([NotNull] SLangGrammarParser.ModuleFieldDeclContext context)
        {
            // toDo check expressions
            return base.VisitModuleFieldDecl(context);
        }

        private VariableNameTableItem FindVariable(string name, IToken symbol)
        {
            // в функции?
            if (currentRoutine is RoutineNameTableItem routine)
            {
                if (currentRoutine is MethodNameTableItem method)
                {
                    if (method.NameOfThis == name)
                    {
                        var typeIdent = currentType;
                        return new VariableNameTableItem { IsConstant = false, Type = currentType };
                    }
                    foreach (var param in routine.Params)
                    {
                        if (param.Name == name)
                        {
                            return new VariableNameTableItem { IsConstant = false, Type = param.TypeArg.Type };
                        }
                    }
                }
            }
            var result = scope.FindVariable(name);
            if (result == null)
            {
                ThrowException($"Variable or constant with name {name} not found in current context", file, symbol);
            }
            return result;
        }
        

        public override object VisitCustomType([NotNull] SLangGrammarParser.CustomTypeContext context)
        {
            var classItem = base.VisitCustomType(context) as SlangCustomType;
            CheckClassExists(classItem.ModuleName, classItem.Name, context.qualident().Id().First().Symbol);
            return classItem;
        }
    }
}
