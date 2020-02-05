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
    public class SlangStoreFieldsVisitor: SlangBaseVisitor
    {
        private readonly ModuleNameTable moduleItem;
        public SlangStoreFieldsVisitor(SourceCodeTable table, ModuleData module) : base(table, module)
        {
            moduleItem = table.Modules[module.Name];
        }

        public override object VisitTypeDecl([NotNull] SLangGrammarParser.TypeDeclContext context)
        {
            var classItem = moduleItem.Classes[context.Id().GetText()];
            if (context.typeInherit() != null)
            {
                // Есть наследник
                classItem.Base = Visit(context.typeInherit().customType()) as SlangCustomType;
            }
            else
            {
                // Нету наследника, берем Object из System
                classItem.Base = Table.Modules[CompilerConstants.SystemModuleName].Classes[CompilerConstants.ObjectClassName].TypeIdent;
            }

            var errToken = context.Id().Symbol;

            if (!Table.Modules[classItem.Base.ModuleName].Classes[classItem.Base.Name].CanBeBase)
            {
                ThrowException($"Class {classItem.Base} is not marked as base", ModuleData.File, errToken);
            }

            // store fields
            foreach (var fieldContext in context.typeFieldDecl())
            {
                var item = Visit(fieldContext) as FieldNameTableItem;
                if (fieldContext.variableDecl().exp() != null)
                {
                    ThrowException("Expressions not allowed for fields in types", ModuleData.File, fieldContext.variableDecl().exp().Start);
                }
                ThrowIfReservedWord(item.Name, ModuleData.File, fieldContext.variableDecl().Start);
                if (classItem.Fields.ContainsKey(item.Name))
                {
                    ThrowException($"Field {item.Name} already defined in class {context.Id().GetText()}", ModuleData.File, fieldContext.variableDecl().Start);
                }

                // check level of access
                if (item.Type is SlangCustomType || item.Type is SlangPointerType)
                {
                    ClassNameTableItem usedType = null;
                    if (item.Type is SlangCustomType t)
                    {
                        usedType = Table.FindClass(t);
                    }
                    else if (item.Type is SlangPointerType pt)
                    {
                        usedType = Table.FindClass(pt.PtrType);
                    }

                    // если поле класса публично, а тип поля приватный, но при этом тип поля не тип класса
                    if (item.AccessModifier == AccessModifier.Public && usedType.AccessModifier == AccessModifier.Private && (usedType.TypeIdent != classItem.TypeIdent))
                    {
                        ThrowException($"Level of accessibility of field {item.Name} more than type {usedType.TypeIdent}", ModuleData.File, fieldContext.variableDecl().Start);
                    }
                }
                classItem.CheckFieldConflicts(ModuleData, item);
                classItem.Fields.Add(item.Name, item);
            }

            return base.VisitTypeDecl(context);
        }

        public override object VisitModuleFieldDecl([NotNull] SLangGrammarParser.ModuleFieldDeclContext context)
        {
            var data = Visit(context.variableDecl()) as VariableNameTableItem;
            var isReadonly = context.Readonly() == null ? true : false;

            ThrowIfReservedWord(data.Name, ModuleData.File, context.variableDecl().Start);
            if (moduleItem.Fields.ContainsKey(data.Name))
            {
                ThrowIfVariableExistsException(data.Name, ModuleData.File, data.Line, data.Column);
            }

            var item = new ModuleFieldNameTableItem
            {
                AccessModifier = GetModifierByName(context.AccessModifier().GetText()),
                IsConstant = false,
                Column = data.Column,
                Line = data.Line,
                IsReadonly = isReadonly,
                Name = data.Name,
                Type = data.Type
            };

            if (item.Type is SlangCustomType t)
            {
                var typeOfItem = Table.FindClass(t);
                // если поле класса публично, а тип поля приватный, но при этом тип поля не тип класса
                if (item.AccessModifier == AccessModifier.Public && typeOfItem.AccessModifier == AccessModifier.Private)
                {
                    ThrowException($"Level of accessibility of field {item.Name} more than type {t}", ModuleData.File, data.Line, data.Column);
                }
            }

            moduleItem.CheckFieldConflicts(item);
            moduleItem.Fields[data.Name] = item;

            return base.VisitModuleFieldDecl(context);
        }

        public override object VisitVariableDecl([NotNull] SLangGrammarParser.VariableDeclContext context)
        {
            if (context.arrayDecl() != null)
            {
                return Visit(context.arrayDecl());
            }
            else if (context.simpleDecl() != null)
            {
                return Visit(context.simpleDecl());
            }
            else
            {
                return Visit(context.ptrDecl());
            }
        }
        // var -> scalar
        public override object VisitSimpleDecl([NotNull] SLangGrammarParser.SimpleDeclContext context)
        {
            SlangType type;
            if (context.simpleType() != null)
            {
                type = Visit(context.simpleType()) as SlangSimpleType;
            }
            else
            {
                type = Visit(context.customType()) as SlangCustomType;
            }
            var name = context.Id();
            ThrowIfReservedWord(name.GetText(), ModuleData.File, name.Symbol);
            return new VariableNameTableItem { IsConstant = false, Type = type, Name = name.GetText(), Column = name.Symbol.Column, Line = name.Symbol.Line };
        }
        // var -> arr
        public override object VisitArrayDecl([NotNull] SLangGrammarParser.ArrayDeclContext context)
        {
            var type = Visit(context.arrayDeclType()) as SlangArrayType;
            var name = context.Id();
            ThrowIfReservedWord(name.GetText(), ModuleData.File, name.Symbol);
            return new VariableNameTableItem { IsConstant = false, Type = type, Line = name.Symbol.Line, Column = name.Symbol.Column, Name = name.GetText() };
        }

        // var -> ptr
        public override object VisitPtrDecl([NotNull] SLangGrammarParser.PtrDeclContext context)
        {
            var type = Visit(context.ptrType()) as SlangType;
            var name = context.Id();
            ThrowIfReservedWord(name.GetText(), ModuleData.File, name.Symbol);
            return new VariableNameTableItem { IsConstant = false, Type = type, Column = name.Symbol.Column, Line = name.Symbol.Line, Name = name.GetText() };
        }

        public override object VisitCustomType([NotNull] SLangGrammarParser.CustomTypeContext context)
        {
            var classItem = base.VisitCustomType(context) as SlangCustomType;
            CheckClassExists(classItem.ModuleName, classItem.Name, context.qualident().Id().First().Symbol);
            return classItem;
        }
    }
}
