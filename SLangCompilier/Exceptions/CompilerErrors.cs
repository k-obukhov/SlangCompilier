using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using SLangCompiler.FrontEnd.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SLangCompiler.Exceptions
{
    public static class CompilerErrors
    {
        public static void ThrowException(string message, FileInfo file, IToken symbol) => ThrowException(message, file, symbol.Line, symbol.Column);
        public static void ThrowException(string message, FileInfo file, int line, int column) => throw new CompilerException(message, file, line, column);

        public static void ThrowIfReservedWord(string name, FileInfo file, IToken token) => ThrowIfReservedWord(name, file, token.Line, token.Column);
        public static void ThrowIfReservedWord(string name, FileInfo file, int line, int column)
        {
            if (CompilerConstants.CppKeywords.Contains(name) || CompilerConstants.SlangKeywords.Contains(name))
            {
                ThrowException($"Name {name} is reserved", file, line, column);
            }
        }

        public static void ThrowIfVariableExistsException(string name, FileInfo file, int line, int column) => ThrowException($"variable or constant with name {name} already exists", file, line, column);
        public static void ThrowLevelAccessibilityException(Antlr4.Runtime.Tree.ITerminalNode token, FileInfo file, string className, string routineName) => ThrowException($"Level of accessibility of type {className} less than access to routine {routineName}", file, token.Symbol);
        public static void ThrowModuleFromOtherClassModuleException(Antlr4.Runtime.Tree.ITerminalNode token, FileInfo file) => ThrowException($"Method with name {token.GetText()} refers to a class in another module", file, token.Symbol);
        public static void ThrowConfictsThisException(Antlr4.Runtime.Tree.ITerminalNode thisName, FileInfo file) => ThrowException($"Name {thisName.GetText()} conflicts with one of the parameter's name", file, thisName.Symbol);
        public static void ThrowMethodSignatureExistsException(SlangCustomType classData, Antlr4.Runtime.Tree.ITerminalNode name, FileInfo file) => ThrowException($"Method with same signature already exists in class {classData}", file, name.Symbol);
        public static void ThrowRoutineExistsException(Antlr4.Runtime.Tree.ITerminalNode token, FileInfo file) => ThrowException($"Procedure or function {token.GetText()} with same signature already exists", file, token.Symbol);
        public static void ThrowIfAbstractMethodPrivate(AccessModifier modifier, FileInfo file, Antlr4.Runtime.Tree.ITerminalNode token)
        {
            if (modifier == AccessModifier.Private)
            {
                ThrowException("Abstract methods cannot be private", file, token.Symbol);
            }
        }

        public static void ThrowImportHeaderMethodsException(FileInfo file, Antlr4.Runtime.Tree.ITerminalNode token) => ThrowException("Methods are not allowed for import in this version", file, token.Symbol);
        public static void ThrowImportHeaderException(FileInfo file, Antlr4.Runtime.Tree.ITerminalNode token) => ThrowImportHeaderException(file, token.Symbol.Line, token.Symbol.Column);

        public static void ThrowImportHeaderException(FileInfo file, int line, int column) => ThrowException("Routines and module fields marked imported must't contain logic", file, line, column);

        public static void ThrowRoutinesAbstractOverrideException(FileInfo file, Antlr4.Runtime.Tree.ITerminalNode token) => ThrowException("Routines cannot have abstract or override modifiers", file, token.Symbol);

        public static void ThrowAbstractEmptyException(Antlr4.Runtime.Tree.ITerminalNode terminalNode, FileInfo file) => ThrowException("Abstract methods cannot have a body", file, terminalNode.Symbol);

        public static void ThrowConflictNameException(FileInfo file, int line, int column) => ThrowException("name conflicts with another name in current context or with current module name or included modules", file, line, column);
        public static void ThrowConflictNameException(FileInfo file, Antlr4.Runtime.Tree.ITerminalNode terminalNode) => ThrowConflictNameException(file, terminalNode.Symbol.Line, terminalNode.Symbol.Column);

        public static void ThrowNotFoundInContextException(FileInfo file, IToken symbol) => ThrowException($"Variable or constant with name {symbol.Text} not found in the current context", file, symbol);

        public static void ThrowInvalidTypesForBinaryOperationException(IToken symbol, FileInfo file, SlangType leftType, SlangType rightType) => ThrowException($"Operation {symbol.Text} not allowed for types {leftType} and {rightType}", file, symbol);

        public static void ThrowInvalidTypesForUnaryOperationException(ITerminalNode terminalNode, FileInfo file, SlangType type) => ThrowException($"Operation {terminalNode.GetText()} is not allowed for type {type}", file, terminalNode.Symbol);

        public static void ThrowConflictMethodException(FileInfo fileInfo, string name, int line, int column) => ThrowException($"Method {name} - name conflict with class methods", fileInfo, line, column);


        public static void ThrowNoSuchOverrloadingException(FileInfo file, string name, int line, int column) => ThrowException($"No such overrload for routine {name}", file, line, column);

        public static void ThrowItemNotFoundException(string name, FileInfo file, int line, int column) => ThrowException($"Item {name} not found in module context", file, line, column);


        public static void ThrowModuleItemNotFoundException(string itemName, string moduleName, FileInfo file, int line, int column) => ThrowException($"routine or item with name {itemName} in module {moduleName} not found or marked private", file, line, column);

        public static void ThrowInvalidUseOfTypeException(SlangType slangType, FileInfo file, int line, int column) => ThrowException($"Invalid use of type {slangType}", file, line, column);

        public static void ThrowProcedureReturnException(FileInfo file, int line, int column) => ThrowException("Procedure return value must be empty", file, line, column);

        public static void ThrowCannotAssignException(SlangType type, SlangType exprType, FileInfo file, int line, int column) => ThrowException($"Cannot assign value of type {exprType} to variable or constant with type {type}", file, line, column);

        public static void ThrowNameAlreadyDefinedException(string name, FileInfo file, int line, int column) => ThrowException($"Name {name} already defined in current contexts", file, line, column);
    }
}
