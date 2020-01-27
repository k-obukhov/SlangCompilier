using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using SLangCompiler.FileServices;
using SLangCompiler.FrontEnd.Tables;
using SLangCompiler.FrontEnd.Types;
using SLangGrammar;

using static SLangCompiler.Exceptions.CompilerErrors;

namespace SLangCompiler.FrontEnd
{
    public class SlangBaseVisitor: SLangGrammarBaseVisitor<object>
    {
        public SourceCodeTable Table { get; }
        public ModuleData ModuleData { get; }
        public SlangBaseVisitor(SourceCodeTable table, ModuleData moduleData)
        {
            Table = table;
            ModuleData = moduleData;
        }

        public AccessModifier GetModifierByName(string name)
        {
            return (AccessModifier)Enum.Parse(typeof(AccessModifier), name, true);
        }

        public ParamModifier GetParamModifierByName(string name)
        {
            return (ParamModifier)Enum.Parse(typeof(ParamModifier), name, true);
        }

        // Type Visit
        public override object VisitSimpleType([NotNull] SLangGrammarParser.SimpleTypeContext context)
        {
            return new SlangSimpleType(context.SimpleType().GetText());
        }

        public override object VisitArrayType([NotNull] SLangGrammarParser.ArrayTypeContext context)
        {
            var type = Visit(context.scalarType()) as SlangType;
            var dimension = context.arrayDimention().Length;
            return new SlangArrayType(type, dimension);
        }

        public override object VisitPtrType([NotNull] SLangGrammarParser.PtrTypeContext context)
        {
            return new SlangPointerType(Visit(context.customType()) as SlangCustomType);
        }

        public void CheckClassExists(string moduleName, string typeName, IToken errToken)
        {
            if (!Table.Modules.ContainsKey(moduleName)) // модуля нет
            {
                ThrowException($"Module {moduleName} not found", ModuleData.File, errToken);
            }
            else if (!Table.Modules[ModuleData.Name].ImportedModules.Contains(moduleName) && moduleName != ModuleData.Name)
            {
                ThrowException($"Module {moduleName} not imported", ModuleData.File, errToken);
            }
            else if (!Table.Modules[moduleName].Classes.ContainsKey(typeName))
            {
                ThrowException($"Class {typeName} not found in module {moduleName}", ModuleData.File, errToken);
            }
            else if (Table.Modules[moduleName].Classes[typeName].AccessModifier == AccessModifier.Private && moduleName != ModuleData.Name)
            {
                ThrowException($"Class {typeName} from module {moduleName} is private", ModuleData.File, errToken);
            }
        }

        public override object VisitCustomType([NotNull] SLangGrammarParser.CustomTypeContext context)
        {
            string moduleName = "", typeName = "";
            var ids = context.qualident().Id().Select(x => x.GetText()).ToArray();
            var errToken = context.qualident().Id().First().Symbol;

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

            ThrowIfReservedWord(typeName, ModuleData.File, errToken.Line, errToken.Column);
            return new SlangCustomType(moduleName, typeName);
        }
    }
}
