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

using static SLangCompiler.Exceptions.CompilerErrors;

namespace SLangCompiler.FrontEnd
{
    public class SlangSemanticVisitor : SlangBaseVisitor
    {
        private readonly ModuleNameTable moduleItem;
        private RoutineNameTableItem currentRoutine;
        private bool inProgramBlock = false;
        private Scope scope;
        private SlangCustomType currentType;
        private readonly FileInfo file;
        // проверка определений для классов, функций и переменных модуля -- нельзя использовать их до их объявления, как в С++!
        private Dictionary<string, bool> checkDefinitions = new Dictionary<string, bool>(); 
        public SlangSemanticVisitor(SourceCodeTable table, ModuleData module) : base(table, module)
        {
            moduleItem = table.Modules[module.Name];
            scope = new Scope(); // не включает в себя глобальную область видимости
            file = moduleItem.ModuleData.File;

            foreach (var field in moduleItem.Fields)
            {
                checkDefinitions.Add(field.Key, false);
            }

            foreach (var routine in moduleItem.Routines)
            {
                checkDefinitions.Add(routine.Key, false);
            }

            foreach (var classItem in moduleItem.Classes)
            {
                checkDefinitions.Add(classItem.Key, false);
            }
        }

        public override object VisitTypeDecl([NotNull] SLangGrammarParser.TypeDeclContext context)
        {
            checkDefinitions[context.Id().GetText()] = true;
            return base.VisitTypeDecl(context);
        }

        public override object VisitFunctionDecl([NotNull] SLangGrammarParser.FunctionDeclContext context)
        {
            var symbol = context.Id().Symbol;
            InitializeRoutineStates(context.thisHeader(), symbol);
            CheckParamsNameConflicts(context.thisHeader(), context.routineArgList());

            var result = Visit(context.statementSeq()) as StatementResult;
            // функция, которая не импортируется извне и не является абстрактным методом?
            if (!result.Returning && currentRoutine.Header == null && !(currentRoutine is MethodNameTableItem method && method.IsAbstract))
            {
                ThrowException("Not all code paths returns value", file, context.statementSeq().Start);
            }

            currentType = null;
            currentRoutine = null;
            return null;
        }

        public override object VisitProcedureDecl([NotNull] SLangGrammarParser.ProcedureDeclContext context)
        {
            var symbol = context.Id().Symbol;
            InitializeRoutineStates(context.thisHeader(), symbol);
            CheckParamsNameConflicts(context.thisHeader(), context.routineArgList());
            // some work... need to call visit()?
            Visit(context.statementSeq());
            currentType = null;
            currentRoutine = null;
            return null;
        }

        private void InitializeRoutineStates(SLangGrammarParser.ThisHeaderContext context, IToken symbol)
        {
            currentType = context != null ? Visit(context.customType()) as SlangCustomType : null;
            if (currentType != null)
            {
                currentRoutine = Table.FindClass(currentType).Methods[symbol.Text];
            }
            else
            {
                currentRoutine = moduleItem.Routines[symbol.Text];
                checkDefinitions[symbol.Text] = true;
            }
        }

        public override object VisitModuleStatementsSeq([NotNull] SLangGrammarParser.ModuleStatementsSeqContext context)
        {
            // state -- work in module statements
            currentRoutine = null;
            inProgramBlock = true;
            currentType = null;
            //ToDo checks expressions in fields
            return base.VisitModuleStatementsSeq(context);
        }

        private void CheckParamsNameConflicts(SLangGrammarParser.ThisHeaderContext thisHeader, SLangGrammarParser.RoutineArgListContext argList)
        {
            if (thisHeader != null)
            {
                var token = thisHeader.Id().Symbol;
                moduleItem.CheckCommonNamesConflicts(token.Text, token.Line, token.Column);
            }
            foreach (var arg in argList.routineArg())
            {
                var token = arg.Id().Symbol;
                moduleItem.CheckCommonNamesConflicts(token.Text, token.Line, token.Column);
            }
        }

        private BaseNameTableItem FindItemByName(string name)
        {
            var result = scope.FindVariable(name); // заходим во внешние блоки
            if (result == null)
            {
                // мы сейчас находимся в функции?
                if (currentRoutine != null && currentRoutine is RoutineNameTableItem routine)
                {
                    // в методе?
                    if (currentRoutine is MethodNameTableItem method)
                    {
                        // может это имя this метода?
                        if (method.NameOfThis == name)
                        {
                            return new VariableNameTableItem { IsConstant = false, Type = currentType };
                        }
                    }
                    // неважно, в функции или методе -- проверяем его параметры
                    foreach (var param in routine.Params)
                    {
                        if (param.Name == name)
                        {
                            return new VariableNameTableItem { IsConstant = false, Type = param.TypeArg.Type };
                        }
                    }
                }

                if (moduleItem.Fields.ContainsKey(name) && checkDefinitions[name])
                {
                    return moduleItem.Fields[name];
                }
                // либо это процедура-функция (взять все сигнатуры) ?
                if (moduleItem.Routines.ContainsKey(name) && checkDefinitions[name])
                {
                    return moduleItem.Routines[name];
                }
                // ну либо это другой модуль?
                if (moduleItem.ImportedModules.Contains(name))
                {
                    return Table.Modules[name];
                }

                return null;
                // иначе ничего не остается кроме как отдать пустоту
            }
            else
            {
                return result;
            }
        }

        public override object VisitStatementSeq([NotNull] SLangGrammarParser.StatementSeqContext context)
        {
            var newScope = new Scope(scope);
            scope = newScope;

            var returning = false;

            foreach (var statement in context.statement())
            {
                var res = Visit(statement) as StatementResult;
                if (res?.Returning == true || statement?.simpleStatement()?.returnC() != null)
                {
                    returning = true;
                }
            }

            scope = scope?.Outer;
            return new StatementResult(returning);
        }

        public override object VisitExprList([NotNull] SLangGrammarParser.ExprListContext context)
        {
            var expressionResults = new List<ExpressionResult>();
            foreach (var expr in context.exp())
            {
                expressionResults.Add(Visit(expr) as ExpressionResult);
            }
            return expressionResults;
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
                var relOp = context.BoolEq() ?? context.BoolNeq() ?? context.BoolG() ?? context.BoolGeq() ?? context.BoolL() ?? context.BoolLeq();

                if ((leftResultType is SlangSimpleType lt && rightResultType is SlangSimpleType rt
                    && lt.Equals(rt) && allowedTypes.Contains(lt))
                    || ((leftResultType is SlangPointerType || leftResultType is SlangNullType) && (rightResultType is SlangPointerType || rightResultType is SlangNullType)))
                {
                    return new ExpressionResult(SlangSimpleType.Boolean, ExpressionValueType.Value);
                }
                else
                {
                    ThrowInvalidTypesForBinaryOperationException(relOp.Symbol, file, leftResultType, rightResultType);
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
                var opToken = context.AddOp() ?? context.SubOp() ?? context.BoolOr();


                var allowedTypes = new[] { SlangSimpleType.Int, SlangSimpleType.Real };
                if (context.BoolOr() == null && (leftResultType is SlangSimpleType lt && rightResultType is SlangSimpleType rt && lt.Equals(rt) && allowedTypes.Contains(lt)))
                {
                    return new ExpressionResult(leftResultType, ExpressionValueType.Value);
                }
                else if (context.BoolOr() != null && (leftResultType is SlangSimpleType ltB && rightResultType is SlangSimpleType rtB && ltB.Equals(rtB) && ltB.Equals(SlangSimpleType.Boolean)))
                {
                    return new ExpressionResult(leftResultType, ExpressionValueType.Value);
                }
                else
                {
                    ThrowInvalidTypesForBinaryOperationException(opToken.Symbol, file, leftResultType, rightResultType);
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
                var opToken = context.MulOp() ?? context.DivOp() ?? context.BoolAnd();
                var allowedTypes = new[] { SlangSimpleType.Int, SlangSimpleType.Real };
                if (leftResultType is SlangSimpleType lt && rightResultType is SlangSimpleType rt && lt.Equals(rt) && allowedTypes.Contains(lt))
                {
                    return new ExpressionResult(leftResultType, ExpressionValueType.Value);
                }
                else if (context.BoolAnd() != null && (leftResultType is SlangSimpleType ltB && rightResultType is SlangSimpleType rtB && ltB.Equals(rtB) && ltB.Equals(SlangSimpleType.Boolean)))
                {
                    return new ExpressionResult(leftResultType, ExpressionValueType.Value);
                }
                else
                {
                    ThrowInvalidTypesForBinaryOperationException(opToken.Symbol, file, leftResultType, rightResultType);
                }
            }
            return Visit(context.signedFactor()) as ExpressionResult;
        }

        public override object VisitSignedFactor([NotNull] SLangGrammarParser.SignedFactorContext context)
        {
            var exprRes = (Visit(context.factor()) as ExpressionResult);
            var type = exprRes.Type;
            var signToken = context.AddOp() ?? context.SubOp();

            if (signToken != null)
            {
                if (type is SlangSimpleType t && (t.Equals(SlangSimpleType.Real) || t.Equals(SlangSimpleType.Int)))
                {
                    return new ExpressionResult(type, ExpressionValueType.Value);
                }
                ThrowInvalidTypesForUnaryOperationException(signToken, file, type);
            }
            else
            {
                return new ExpressionResult(type, exprRes.ExpressionType);
            }
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
                if (Table.FindClass(type).IsAbstract())
                {
                    ThrowCannotInitializeAbstractClassException(type, file, context.newC().Start.Line, context.newC().Start.Column);
                }
                return new ExpressionResult(new SlangPointerType(type), ExpressionValueType.Value); // value or var? hmm
            }
            else if (context.Nil() != null)
            {
                return new ExpressionResult(new SlangNullType(), ExpressionValueType.Value);
            }

            return null;
        }

        public override object VisitDesignator([NotNull] SLangGrammarParser.DesignatorContext context)
        {
            // first step -- find in context or in imported modules

            SlangType resultType;
            ExpressionValueType valueType = ExpressionValueType.Variable;
            var item = FindItemByName(context.Id().GetText());
            bool fromCurrentModule = true;

            if (item != null)
            {
                if (item is ModuleNameTable module && module.ModuleData.Name != moduleItem.ModuleData.Name)
                {
                    fromCurrentModule = false;
                }
                else if (item is VariableNameTableItem var)
                {
                    if (var.IsConstant) // константа
                    {
                        valueType = ExpressionValueType.Value;
                    }
                }
            }
            else
            {
                ThrowItemNotFoundException(context.Id().GetText(), file, context.Id().Symbol.Line, context.Id().Symbol.Column);
            }

            foreach (var statement in context.designatorStatement())
            {
                if (valueType == ExpressionValueType.Nothing)
                {
                    ThrowProcedureReturnException(file, statement.Start.Line, statement.Start.Column);
                }

                if (statement.Id() != null)
                {
                    var node = statement.Id();

                    if (item is VariableNameTableItem varItem)
                    {
                        // переменная.поле -- для указателей и кастомных типов
                        // если попали в этот кейс -- значит это самое первое выражение
                        // либо это поле класса или модуля 
                        SlangCustomType typeIdent = null;
                        if (item.ToSlangType() is SlangCustomType t)
                        {
                            typeIdent = t;
                        }
                        else if (item.ToSlangType() is SlangPointerType pt)
                        {
                            typeIdent = pt.PtrType;
                        }
                        else
                        {
                            ThrowInvalidUseOfTypeException(varItem.ToSlangType(), file, node.Symbol.Line, node.Symbol.Column);
                        }

                        if (Table.TryFoundClassItemsByName(node.GetText(), currentType, typeIdent, out BaseNameTableItem foundItem))
                        {
                            item = foundItem;
                        }
                    }
                    else if (item is ModuleNameTable module)
                    {
                        if (Table.TryFindModuleItemsByName(node.GetText(), ModuleData.Name, module.ModuleData.Name, out BaseNameTableItem foundItem))
                        {
                            item = foundItem;
                            // если вытянули из модуля readonly-переменную и она константа или же readonly из другого модуля
                            if (item is ModuleFieldNameTableItem moduleField
                                && (moduleField.IsConstant || (moduleField.IsReadonly && !fromCurrentModule)))
                            {
                                valueType = ExpressionValueType.Value; // такие вещи иожно передать только по значению
                            }
                        }
                        else
                        {
                            ThrowModuleItemNotFoundException(node.GetText(), module.ModuleData.Name, file, node.Symbol.Line, node.Symbol.Column);
                        }
                    }
                    else
                    {
                        ThrowInvalidUseOfTypeException(item.ToSlangType(), file, node.Symbol.Line, node.Symbol.Column);
                    }
                }
                else if (statement.exp() != null)
                {
                    var errToken = statement.LSBrace().Symbol;
                    if (item is VariableNameTableItem varItem)
                    {
                        if (varItem.ToSlangType() is SlangArrayType arrayType)
                        {
                            item = new VariableNameTableItem { Type = arrayType.ArrayElementType(),
                                Column = errToken.Column,
                                Line = errToken.Line,
                                Name = $"{varItem.Name}[]",
                                IsConstant = false
                            };
                        }
                        else if (varItem.ToSlangType() is SlangSimpleType st && st.Equals(SlangSimpleType.String))
                        {
                            item = new VariableNameTableItem { Type = SlangSimpleType.Character,
                                Column = errToken.Column,
                                Line = errToken.Line,
                                Name = $"{varItem.Name}[]",
                                IsConstant = false
                            };
                        }
                        else
                        {
                            ThrowInvalidUseOfTypeException(varItem.ToSlangType(), file, errToken.Line, errToken.Column);
                        }
                    }
                    else
                    {
                        ThrowInvalidUseOfTypeException(item.ToSlangType(), file, errToken.Line, errToken.Column);
                    }
                }
                else if (statement.exprList() != null)
                {
                    var errToken = statement.exprList().Start;

                    if (item is RoutineNameTableItem routine)
                    {
                        var exprTypes = Visit(statement.exprList()) as List<ExpressionResult>;
                        // найти функции-процедуры, у которых столько же параметров и типами, которые можно кастануть неявно 

                        if (routine.Params.Count == exprTypes.Count)
                        {
                            for (int i = 0; i < exprTypes.Count; ++i)
                            {
                                if (!CanAssignToType(routine.Params[i].TypeArg.Type, exprTypes[i].Type))
                                {
                                    ThrowNoSuchOverrloadingException(file, routine.Name, routine.Line, routine.Column);
                                }
                            }
                        }
                        else
                        {
                            ThrowNoSuchOverrloadingException(file, routine.Name, routine.Line, routine.Column);
                        }
                        // item для результата вызова
                        item = new VariableNameTableItem { Name = string.Empty, Column = errToken.Column, Line = errToken.Line, IsConstant = true, Type = routine.ReturnType ?? new SlangVoidType() };

                        if (routine.IsFunction())
                        {
                            valueType = ExpressionValueType.Value;
                        }
                        else
                        {
                            valueType = ExpressionValueType.Nothing;
                        }

                    }
                    else
                    {
                        ThrowInvalidUseOfTypeException(item.ToSlangType(), file, errToken.Line, errToken.Column);
                    }
                }
            }
            if (item is ModuleNameTable)
            {
                ThrowException("Using an imported module without field access is not supported", file, context.Id().Symbol);
            }
            resultType = item.ToSlangType();
            return new ExpressionResult(resultType, valueType);
        }
        private bool CanAssignToType(SlangType type, SlangType expressionType)
        {
            bool result;
            if (type is SlangCustomType t && expressionType is SlangCustomType et)
            {
                result = CanAssignCustomType(t, et);
            }
            else if (type is SlangPointerType pt && expressionType is SlangPointerType pet)
            {
                result = CanAssignCustomType(pt.PtrType, pet.PtrType);
            }
            else if (type is SlangPointerType && expressionType is SlangNullType)
            {
                result = true;
            }
            else
            {
                result = type.Equals(expressionType);
            }
            return result;
        }

        private bool CanAssignCustomType(SlangCustomType type, SlangCustomType expressionType)
        {
            // true, если type является базовым или совпадает с expressionType
            var result = false;
            if (type.Equals(expressionType) || type.Equals(SlangCustomType.Object))
            {
                result = true;
            }
            else
            {
                var classItem = Table.FindClass(expressionType);
                while (!classItem.Base.Equals(SlangCustomType.Object))
                {
                    classItem = Table.FindClass(classItem.Base);
                    if (classItem.TypeIdent.Equals(type))
                    {
                        result = true;
                    }
                }
            }
            return result;
        }

        // declares -- check context -- current routine or start-block (need check)
        public override object VisitVariableDecl([NotNull] SLangGrammarParser.VariableDeclContext context)
        {
            // maybe need checks for context
            VariableNameTableItem variable;
            if (context.simpleDecl() != null)
            {
                variable = Visit(context.simpleDecl()) as VariableNameTableItem;
            }
            else if (context.arrayDecl() != null)
            {
                variable = Visit(context.arrayDecl()) as VariableNameTableItem;
            }
            else
            {
                variable = Visit(context.ptrDecl()) as VariableNameTableItem;
            }

            CheckDeclareContext(context.exp(), variable);

            return null;
        }

        private void CheckDeclareContext(SLangGrammarParser.ExpContext context, VariableNameTableItem variable)
        {
            // проверяем какой-то контекст?
            if (inProgramBlock || currentRoutine != null)
            {
                if (FindItemByName(variable.Name) != null)
                {
                    ThrowNameAlreadyDefinedException(variable.Name, file, variable.Line, variable.Column);
                    CheckExpressionContext(context, variable);
                }
                else
                {
                    scope.PutVariable(variable);
                }
            }
            else // проверяем переменные модуля?
            {
                checkDefinitions[variable.Name] = true;
                CheckExpressionContext(context, variable);
            }

            if (variable.Type is SlangCustomType ct && Table.FindClass(ct).IsAbstract())
            {
                ThrowCannotInitializeAbstractClassException(variable.Type, file, variable.Line, variable.Column);
            }
        }

        private void CheckExpressionContext(SLangGrammarParser.ExpContext context, VariableNameTableItem variable)
        {
            if (context != null)
            {
                var exprRes = Visit(context) as ExpressionResult;
                if (!CanAssignToType(variable.Type, exprRes.Type))
                {
                    ThrowCannotAssignException(variable.Type, exprRes.Type, file, variable.Line, variable.Column);
                }
            }
        }

        public override object VisitConstDecl([NotNull] SLangGrammarParser.ConstDeclContext context)
        {
            var name = context.Id();
            var type = Visit(context.typeName()) as SlangType;
            var constant = new VariableNameTableItem { Name = name.GetText(), Column = name.Symbol.Column, Line = name.Symbol.Line, IsConstant = true, Type = type };

            CheckDeclareContext(context.exp(), constant);

            return null;
        }

        public override object VisitSimpleDecl([NotNull] SLangGrammarParser.SimpleDeclContext context)
        {
            var name = context.Id();
            var type = context.simpleType() != null ? Visit(context.simpleType()) as SlangType : Visit(context.customType()) as SlangType;

            return new VariableNameTableItem { IsConstant = false, Name = name.GetText(), Column = name.Symbol.Column, Line = name.Symbol.Line, Type = type };
        }

        public override object VisitArrayDecl([NotNull] SLangGrammarParser.ArrayDeclContext context)
        {
            var name = context.Id();
            var type = Visit(context.arrayDeclType()) as SlangType;
            foreach (var exp in context.arrayDeclType().exp())
            {
                var res = Visit(exp) as ExpressionResult;
                if (!res.ExpressionType.Equals(SlangSimpleType.Int))
                {
                    ThrowException($"Array length expression must have integer type", file, exp.Start);
                }
            }

            return new VariableNameTableItem { IsConstant = false, Name = name.GetText(), Column = name.Symbol.Column, Line = name.Symbol.Line, Type = type };
        }

        public override object VisitPtrDecl([NotNull] SLangGrammarParser.PtrDeclContext context)
        {
            var name = context.Id();
            var type = Visit(context.ptrType()) as SlangType;

            return new VariableNameTableItem { IsConstant = false, Name = name.GetText(), Column = name.Symbol.Column, Line = name.Symbol.Line, Type = type };
        }

        public override object VisitIfC([NotNull] SLangGrammarParser.IfCContext context)
        {
            foreach (var exp in context.exp())
            {
                CheckExpIsBoolean(exp);
            }
            bool result = true;
            foreach (var statementSeq in context.statementSeq())
            {
                var res = Visit(statementSeq) as StatementResult;
                if (!res.Returning)
                {
                    result = false;
                }
            }
            return new StatementResult(result);
        }

        private void CheckExpIsBoolean(SLangGrammarParser.ExpContext exp)
        {
            var res = Visit(exp) as ExpressionResult;
            if (!res.Type.Equals(SlangSimpleType.Boolean))
            {
                ThrowCannotAssignException(SlangSimpleType.Boolean, res.Type, file, exp.Start.Line, exp.Start.Column);
            }
        }

        public override object VisitWhileC([NotNull] SLangGrammarParser.WhileCContext context)
        {
            CheckExpIsBoolean(context.exp());
            return Visit(context.statementSeq()) as StatementResult;
        }

        public override object VisitRepeatC([NotNull] SLangGrammarParser.RepeatCContext context)
        {
            CheckExpIsBoolean(context.exp());
            return Visit(context.statementSeq()) as StatementResult;
        }

        public override object VisitCall([NotNull] SLangGrammarParser.CallContext context)
        {
            var exprRes = Visit(context.designator()) as ExpressionResult;
            if (exprRes.ExpressionType != ExpressionValueType.Nothing)
            {
                ThrowException($"Call instruction is only for procedures and method-procedures", file, context.designator().Start);
            }
            return null;
        }

        public override object VisitReturnC([NotNull] SLangGrammarParser.ReturnCContext context)
        {
            if (currentRoutine == null)
            {
                ThrowException($"Return statement allowed only for routines", file, context.Start);
            }
            else if (currentRoutine.IsFunction() && context.exp() == null)
            {
                ThrowException($"Function must have an expression for return", file, context.Start);
            }
            else if (currentRoutine.IsProcedure() && context.exp() != null)
            {
                ThrowException($"Procedures must not have an expression for return", file, context.Start);
            }

            if (context.exp() != null)
            {
                var res = Visit(context.exp()) as ExpressionResult;
                if (!CanAssignToType(currentRoutine.ReturnType, res.Type))
                {
                    ThrowCannotAssignException(currentRoutine.ReturnType, res.Type, file, context.exp().Start.Line, context.exp().Start.Column);
                }
            }
            return new StatementResult(true);
        }

        public override object VisitInput([NotNull] SLangGrammarParser.InputContext context)
        {
            foreach (var exp in context.designator())
            {
                var res = Visit(exp) as ExpressionResult;
                if (!(res.ExpressionType == ExpressionValueType.Variable && res.Type is SlangSimpleType))
                {
                    ThrowException($"Input is allowed only for non-constant simple types", file, exp.Start);
                }
            }
            return null;
        }

        public override object VisitOutput([NotNull] SLangGrammarParser.OutputContext context)
        {
            foreach (var exp in context.exp())
            {
                var res = Visit(exp) as ExpressionResult;
                if (!(res.Type is SlangSimpleType))
                {
                    ThrowException($"Output is allowed only for simple types", file, exp.Start);
                }
            }
            return null;
        }

        public override object VisitLet([NotNull] SLangGrammarParser.LetContext context)
        {
            var itemType = Visit(context.designator()) as ExpressionResult;
            var exprType = Visit(context.exp()) as ExpressionResult;

            if (itemType.ExpressionType != ExpressionValueType.Variable)
            {
                ThrowException("Cannot use assign for left-side expression", file, context.designator().Start);
            }
            if (!CanAssignToType(itemType.Type, exprType.Type))
            {
                ThrowCannotAssignException(itemType.Type, exprType.Type, file, context.exp().Start.Line, context.exp().Start.Column);
            }

            return null;
        }

        public override object VisitCustomType([NotNull] SLangGrammarParser.CustomTypeContext context)
        {
            var classItem = base.VisitCustomType(context) as SlangCustomType;
            CheckClassExists(classItem.ModuleName, classItem.Name, context.qualident().Id().First().Symbol);
            if (classItem.ModuleName == ModuleData.Name)
            {
                if (checkDefinitions[classItem.Name] == false)
                {
                    ThrowException($"Invalid use of incomplete type {classItem}", file, context.Start);
                }
            }
            return classItem;
        }
    }
}
