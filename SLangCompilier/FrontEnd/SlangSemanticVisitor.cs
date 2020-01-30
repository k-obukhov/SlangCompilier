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
        private Scope scope;
        private SlangCustomType currentType;
        private FileInfo file;
        public SlangSemanticVisitor(SourceCodeTable table, ModuleData module) : base(table, module)
        {
            moduleItem = table.Modules[module.Name];
            scope = new Scope(new Dictionary<string, VariableNameTableItem>()); // не включает в себя глобальную область видимости
            file = moduleItem.ModuleData.File;
        }

        public override object VisitFunctionDecl([NotNull] SLangGrammarParser.FunctionDeclContext context)
        {
            currentType = context.thisHeader() != null ? Visit(context.thisHeader().customType()) as SlangCustomType: null;
            if (currentType != null)
            {
                // TODO find method by line and column and assign to current
            }
            else
            {
                // TODO find method by line and column and assign to current
            }
            // some work...
            currentType = null;
            return base.VisitFunctionDecl(context);
        }

        public override object VisitProcedureDecl([NotNull] SLangGrammarParser.ProcedureDeclContext context)
        {
            currentType = context.thisHeader() != null ? Visit(context.thisHeader().customType()) as SlangCustomType : null;
            if (currentType != null)
            {
                // TODO find method by line and column and assign to current
            }
            else
            {
                // TODO find method by line and column and assign to current
            }
            // some work...
            currentType = null;
            return base.VisitProcedureDecl(context);
        }

        public override object VisitModuleFieldDecl([NotNull] SLangGrammarParser.ModuleFieldDeclContext context)
        {
            // toDo check expressions
            return base.VisitModuleFieldDecl(context);
        }

        public override object VisitModuleStatementsSeq([NotNull] SLangGrammarParser.ModuleStatementsSeqContext context)
        {
            // state -- work in module statements
            currentRoutine = null;
            currentType = null;
            return base.VisitModuleStatementsSeq(context);
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
