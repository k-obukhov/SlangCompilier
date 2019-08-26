using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using SLangCompiler.Exceptions;
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
    public class SlangBaseStepVisitor: SLGrammarBaseVisitor<object>
    {
        // base visitor login there (basic type returning, etc)
        public SourceCodeTable Table { get; }
        public ModuleData ModuleData { get; }
        public SlangBaseStepVisitor(SourceCodeTable table, ModuleData moduleData)
        {
            Table = table;
            ModuleData = moduleData;
        }

        public AccessModifier GetModifierByName(string name)
        {
            return (AccessModifier) Enum.Parse(typeof(AccessModifier), name, true);
        }

        public ParamModifier GetParamModifierByName(string name)
        {
            return (ParamModifier) Enum.Parse(typeof(ParamModifier), name, true);
        }

        public void ThrowIfReservedWord(string name, IToken token)
        {
            if (CompilerConstants.CppKeywords.Contains(name) || CompilerConstants.SlangKeywords.Contains(name))
            {
                throw new CompilerException($"Name {name} is reserved", ModuleData.File, token);
            }
        }

        // Type Visit
        public override object VisitSimpleType([NotNull] SLGrammarParser.SimpleTypeContext context)
        {
            return new SlangSimpleType(context.SimpleType().GetText());
        }

        public override object VisitFunctionType([NotNull] SLGrammarParser.FunctionTypeContext context)
        {
            var argTypes = Visit(context.functionalArgList()) as IList<SlangRoutineTypeArg>;
            var returnType = Visit(context.typeName()) as SlangType;
            return new SlangFunctionType(argTypes, returnType);
        }

        public override object VisitProcedureType([NotNull] SLGrammarParser.ProcedureTypeContext context)
        {
            var argTypes = Visit(context.functionalArgList()) as IList<SlangRoutineTypeArg>;
            return new SlangProcedureType(argTypes);
        }

        public override object VisitFunctionalArgList([NotNull] SLGrammarParser.FunctionalArgListContext context)
        {
            IList<SlangRoutineTypeArg> args = new List<SlangRoutineTypeArg>(context.functionalArg().Length);

            foreach (var arg in context.functionalArg())
            {
                args.Add(Visit(arg) as SlangRoutineTypeArg);
            }

            return args;
        }

        public override object VisitFunctionalArg([NotNull] SLGrammarParser.FunctionalArgContext context)
        {
            var modifier = GetParamModifierByName(context.FunctionArgModifier().GetText());
            var type = Visit(context.typeName()) as SlangType;
            return new SlangRoutineTypeArg(modifier, type);
        }

        public override object VisitArrayType([NotNull] SLGrammarParser.ArrayTypeContext context)
        {
            var type = Visit(context.scalarType()) as SlangType;
            var dimension = context.arrayDimention().Length;
            return new SlangArrayType(type, dimension);
        }

        public override object VisitPtrType([NotNull] SLGrammarParser.PtrTypeContext context)
        {
            return new SlangPointerType(Visit(context.customType()) as SlangCustomType);
        }

        public void ThrowException(string message, IToken symbol)
        {
            throw new CompilerException(message, ModuleData.File, symbol);
        }

        public void CheckClassExists(string moduleName, string typeName, IToken errToken)
        {
            if (!Table.Modules.ContainsKey(moduleName)) // модуля нет
            {
                ThrowException($"Module {moduleName} not found", errToken);
            }
            else if (!Table.Modules[ModuleData.Name].ImportedModules.Contains(moduleName) && moduleName != ModuleData.Name)
            {
                ThrowException($"Module {moduleName} not imported", errToken);
            }
            else if (!Table.Modules[moduleName].Classes.ContainsKey(typeName))
            {
                ThrowException($"Class {typeName} not found in module {moduleName}", errToken);
            }
            else if (Table.Modules[moduleName].Classes[typeName].AccessModifier == AccessModifier.Private && moduleName != ModuleData.Name)
            {
                ThrowException($"Class {typeName} from module {moduleName} is private", errToken);
            }
        }

        public override object VisitCustomType([NotNull] SLGrammarParser.CustomTypeContext context)
        {
            string moduleName = "", typeName = "";
            var ids = context.id().Id().Select(x => x.GetText()).ToArray();
            var errToken = context.id().Id().First().Symbol;

            if (ids.Count() == 1)
            {
                moduleName = ModuleData.Name;
                typeName = ids[0];
            }
            else if (ids.Count() == 2)
            {
                moduleName = ids[0];
                typeName = ids[1];
            }
            else
            {
                ThrowException($"Invalid name: {context.id().GetText()}", errToken);
            }
            ThrowIfReservedWord(typeName, context.id().Id().First().Symbol);
            return new SlangCustomType(moduleName, typeName);
        }
    }
}
