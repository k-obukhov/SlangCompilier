using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
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
        private readonly FileInfo file;
        public SlangSemanticVisitor(SourceCodeTable table, ModuleData module) : base(table, module)
        {
            moduleItem = table.Modules[module.Name];
            scope = new Scope(); // не включает в себя глобальную область видимости
            file = moduleItem.ModuleData.File;
        }

        public override object VisitFunctionDecl([NotNull] SLangGrammarParser.FunctionDeclContext context)
        {
            var symbol = context.Id().Symbol;
            InitializeRoutineStates(context.thisHeader(), symbol);

            var result = Visit(context.statementSeq()) as StatementResult;
            // функция, которая не импортируется извне и не является абстрактным методом?
            if (!result.Returning && currentRoutine.Header == null && !(currentRoutine is MethodNameTableItem method && method.IsAbstract))
            {
                ThrowException("not all code paths returns value", file, context.statementSeq().Start);
            }

            currentType = null;
            currentRoutine = null;
            return base.VisitFunctionDecl(context);
        }

        public override object VisitProcedureDecl([NotNull] SLangGrammarParser.ProcedureDeclContext context)
        {
            var symbol = context.Id().Symbol;
            InitializeRoutineStates(context.thisHeader(), symbol);
            // some work... need to call visit()?
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

        private VariableNameTableItem FindVariable(string name)
        {
            // в функции?
            if (currentRoutine is RoutineNameTableItem routine)
            {
                if (currentRoutine is MethodNameTableItem method)
                {
                    if (method.NameOfThis == name)
                    {
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
            // если сейчас в контексте типа - может в полях типа?
            // в методах обращение будет идти через this.name, а мы ищем по одному имени
            if (currentType != null && currentRoutine == null)
            {
                var classData = Table.FindClass(currentType);
                if (classData.Fields.ContainsKey(name))
                {
                    return classData.Fields[name];
                }
            }

            var result = scope.FindVariable(name);
            if (result == null)
            {
                // ну тогда может в контексте полей нашего модуля?
                if (moduleItem.Fields.ContainsKey(name))
                {
                    return moduleItem.Fields[name];
                }
            }
            return result;
        }

        public override object VisitStatementSeq([NotNull] SLangGrammarParser.StatementSeqContext context)
        {
            var newScope = new Scope(scope);
            scope = newScope;

            var result = new StatementResult(false);

            foreach (var statement in context.statement())
            {
                var res = Visit(statement);
                if (res != null && res is StatementResult stRes && stRes.Returning)
                {
                    result.Returning = true;
                }
            }

            scope = scope?.Outer;
            return result;
        }

        public override object VisitExp([NotNull] SLangGrammarParser.ExpContext context)
        {
            // что можно сравнивать в языке?
            // int и float и char и bool только между собой -- все простые типы кроме строк
            // указатели любого типа только между собой 

            if (context.exp() != null)
            {
                var leftResultType = (Visit(context.simpleExpr()) as ExpressionResult).Type;
                var rightResultType = (Visit(context.exp()) as ExpressionResult).Type;
                var allowedTypes = new[] { SlangSimpleType.Int, SlangSimpleType.Real, SlangSimpleType.Boolean, SlangSimpleType.Character };

                if ((leftResultType is SlangSimpleType lt && rightResultType is SlangSimpleType rt 
                    && lt.Equals(rt) && allowedTypes.Contains(lt))
                    || (leftResultType is SlangPointerType && rightResultType is SlangPointerType))
                {
                    return new ExpressionResult(SlangSimpleType.Boolean, ExpressionValueType.Value);
                }
                else
                {
                    ThrowInvalidTypesForBinaryOperationException(context.Relation().Symbol, file, leftResultType, rightResultType);
                }
                
            }
            return Visit(context.simpleExpr()) as ExpressionResult;
        }

        public override object VisitSimpleExpr([NotNull] SLangGrammarParser.SimpleExprContext context)
        {
            if (context.simpleExpr() != null)
            {
                var leftResultType = (Visit(context.term()) as ExpressionResult).Type;
                var rightResultType = (Visit(context.simpleExpr()) as ExpressionResult).Type;
                var allowedTypes = new[] { SlangSimpleType.Int, SlangSimpleType.Real };
                if (leftResultType is SlangSimpleType lt && rightResultType is SlangSimpleType rt && lt.Equals(rt) && allowedTypes.Contains(lt))
                {
                    return new ExpressionResult(leftResultType, ExpressionValueType.Value);
                }
                else
                {
                    ThrowInvalidTypesForBinaryOperationException(context.AddictiveOp().Symbol, file, leftResultType, rightResultType);
                }
            }
            return Visit(context.term()) as ExpressionResult;
        }

        public override object VisitTerm([NotNull] SLangGrammarParser.TermContext context)
        {
            if (context.term() != null)
            {
                var leftResultType = (Visit(context.signedFactor()) as ExpressionResult).Type;
                var rightResultType = (Visit(context.term()) as ExpressionResult).Type;
                var allowedTypes = new[] { SlangSimpleType.Int, SlangSimpleType.Real };
                if (leftResultType is SlangSimpleType lt && rightResultType is SlangSimpleType rt && lt.Equals(rt) && allowedTypes.Contains(lt))
                {
                    return new ExpressionResult(leftResultType, ExpressionValueType.Value);
                }
                else
                {
                    ThrowInvalidTypesForBinaryOperationException(context.MultiplicativeOp().Symbol, file, leftResultType, rightResultType);
                }
            }
            return Visit(context.signedFactor()) as ExpressionResult;
        }

        public override object VisitSignedFactor([NotNull] SLangGrammarParser.SignedFactorContext context)
        {
            var type = (Visit(context.factor()) as ExpressionResult).Type;

            if (type is SlangSimpleType t && (t.Equals(SlangSimpleType.Real) || t.Equals(SlangSimpleType.Int)))
            {
                return new ExpressionResult(type, ExpressionValueType.Value);
            }
            ThrowInvalidTypesForUnaryOperationException(context.AddOp() ?? context.SubOp(), file, type);
            return null;
        }

        public override object VisitFactor([NotNull] SLangGrammarParser.FactorContext context)
        {
            if (context.IntValue() != null)
            {
                return new ExpressionResult(SlangSimpleType.Int, ExpressionValueType.Value);
            }
            else if (context.RealValue() != null)
            {
                return new ExpressionResult(SlangSimpleType.Real, ExpressionValueType.Value);
            }
            else if (context.BoolValue() != null)
            {
                return new ExpressionResult(SlangSimpleType.Boolean, ExpressionValueType.Value);
            }
            else if (context.SingleCharacter() != null)
            {
                return new ExpressionResult(SlangSimpleType.Character, ExpressionValueType.Value);
            }
            else if (context.StringLiteral() != null)
            {
                return new ExpressionResult(SlangSimpleType.String, ExpressionValueType.Value);
            }
            else if (context.BoolNot() != null)
            {
                var type = (Visit(context.factor()) as ExpressionResult).Type;
                if (type is SlangSimpleType && type.Equals(SlangSimpleType.Boolean))
                {
                    return new ExpressionResult(SlangSimpleType.Boolean, ExpressionValueType.Value);
                }
                else
                {
                    ThrowInvalidTypesForUnaryOperationException(context.BoolNot(), file, type);
                }
            }
            else if (context.exp() != null)
            {
                return Visit(context.exp()) as ExpressionResult;
            }
            else if (context.designator() != null)
            {
                return Visit(context.designator()) as ExpressionResult;
            }
            else if (context.newC() != null)
            {
                var type = Visit(context.newC().customType()) as SlangCustomType;
                return new ExpressionResult(new SlangPointerType(type), ExpressionValueType.Value); // value or var? hmm
            }

            return null;
        }

        public override object VisitDesignator([NotNull] SLangGrammarParser.DesignatorContext context)
        {
            // TODO designator logic -- call funcs, indexers (array, string), fields etc
            // first step -- find in context or in imported modules
            // maybe use states

            SlangType resultType = null;
            ExpressionValueType valueType = ExpressionValueType.Variable;

            return new ExpressionResult(resultType, valueType);
        }

        public override object VisitCustomType([NotNull] SLangGrammarParser.CustomTypeContext context)
        {
            var classItem = base.VisitCustomType(context) as SlangCustomType;
            CheckClassExists(classItem.ModuleName, classItem.Name, context.qualident().Id().First().Symbol);
            return classItem;
        }
    }
}
