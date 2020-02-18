﻿using Antlr4.Runtime;
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
        private static void ThrowException(string message, FileInfo file, IToken symbol) => ThrowException(message, file, symbol.Line, symbol.Column);
        private static void ThrowException(string message, FileInfo file, int line, int column) => throw new CompilerException(message, file, line, column);

        public static void ThrowIfReservedWord(string name, FileInfo file, IToken token) => ThrowIfReservedWord(name, file, token.Line, token.Column);
        public static void ThrowIfReservedWord(string name, FileInfo file, int line, int column)
        {
            if (CompilerConstants.CppKeywords.Contains(name) || CompilerConstants.SlangKeywords.Contains(name))
            {
                ThrowException($"Name {name} is reserved", file, line, column);
            }
        }

        public static void ThrowIfVariableExistsException(string name, FileInfo file, int line, int column) => ThrowException($"variable or constant with name {name} already exists", file, line, column);
        public static void ThrowLevelAccessibilityForRoutineException(Antlr4.Runtime.Tree.ITerminalNode token, FileInfo file, string className, string routineName) => ThrowException($"Level of accessibility of type {className} less than access to routine {routineName}", file, token.Symbol);
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

        public static void ThrowCannotInitializeAbstractClassException(SlangType type, FileInfo file, int line, int column) => ThrowException($"Cannot create variable or allocate memory for abstract class {type}", file, line, column);
    
        public static void ThrowNotAllCodePathException(FileInfo file, IToken symbol) => ThrowException("Not all code paths returns value", file, symbol);
        public static void ThrowUsingModuleAsVariableException(FileInfo file, IToken symbol) => ThrowException("Using an imported module without field access is not supported", file, symbol);
        public static void ThrowArrayElementException(FileInfo file, IToken symbol) => ThrowException($"Array length expression must have integer type", file, symbol);

        public static void ThrowCallException(FileInfo file, IToken symbol) => ThrowException($"Call instruction is only for procedures and method-procedures", file, symbol);
        public static void ThrowReturnException(FileInfo file, IToken symbol) => ThrowException($"Return statement allowed only for routines", file, symbol);
        public static void ThrowFunctionReturnException(FileInfo file, IToken symbol) => ThrowException($"Function must have an expression for return", file, symbol);
        public static void ThrowInputTypeException(FileInfo file, IToken symbol) => ThrowException($"Input is allowed only for non-constant simple types", file, symbol);
        public static void ThrowOutputTypeException(FileInfo file, IToken symbol) => ThrowException($"Output is allowed only for simple types", file, symbol);
        public static void ThrowLetForValueException(FileInfo file, IToken symbol) => ThrowException($"Cannot use assign for right - side expression", file, symbol);
        public static void ThrowInvalidUseIncompleteTypeException(SlangCustomType classItem, FileInfo file, IToken symbol) => ThrowException($"Invalid use of incomplete type {classItem}", file, symbol);
        public static void ThrowRepeatingModuleException(string moduleName, FileInfo file, IToken symbol) => ThrowException($"Repeating import of module ${moduleName}", file, symbol);
        public static void ThrowModuleNotFoundException(string moduleName, FileInfo file, IToken symbol) => ThrowException($"Module {moduleName} not found", file, symbol);
        public static void ThrowModuleImportsItselfException(string moduleName, FileInfo file, IToken symbol) => ThrowException($"Module {moduleName} imports itself", file, symbol);
        public static void ThrowUnableImportMainException(FileInfo file, IToken symbol) => ThrowException($"Unable to import main module from other!", file, symbol);
        public static void ThrowModuleNameConflictFileNameException(string moduleName, string fileName, FileInfo file, IToken symbol) => ThrowException($"Module name \"{moduleName}\" doest not match \"{fileName}\"", file, symbol);
        public static void ThrowEntryPointException(string moduleName, FileInfo file, IToken symbol) => ThrowException($"Module {moduleName} is not main module but have an entry point", file, symbol);
        public static void ThrowClassRedefinitionException(string className, FileInfo file, IToken symbol) => ThrowException($"Redefinition of class \"{className}\"", file, symbol);
        public static void ThrowClassNotMarkedAsBaseException(SlangCustomType classItem, FileInfo file, IToken symbol) => ThrowException($"Class {classItem} is not marked as base", file, symbol);
        public static void ThrowClassFieldExprException(FileInfo file, IToken symbol) => ThrowException("Expressions not allowed for fields in types", file, symbol);
        public static void ThrowClassFieldAlreadyDefinedException(string fieldName, string className, FileInfo file, IToken symbol) => ThrowException($"Field {fieldName} already defined in class {className}", file, symbol);
        public static void ThrowLevelAccessibilityForFieldsException(IToken token, FileInfo file, string className, string fieldName) => ThrowException($"Level of accessibility of type {className} less than access to field {fieldName}", file, token);
        public static void ThrowParameterAlreadyDefinedException(string paramName, FileInfo file, IToken symbol) => ThrowException($"Parameter with name {paramName} already defined", file, symbol);
        public static void ThrowModuleNotImportedException(string moduleName, FileInfo file, IToken symbol) => ThrowException($"Module {moduleName} is not imported", file, symbol);
        public static void ThrowClassNotFoundException(string moduleName, string className, FileInfo file, IToken symbol) => ThrowException($"Class {className} not found in module {moduleName}", file, symbol);
        public static void ThrowClassIsPrivateException(string moduleName, string className, FileInfo file, IToken symbol) => ThrowException($"Class {className} from module {moduleName} is private", file, symbol);
    }
}
