using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using SLangCompiler.FileServices;
using SLangCompiler.FrontEnd.Tables;
using SLangCompiler.FrontEnd.Types;
using SLangGrammar;
using System;
using System.Linq;
using static SLangCompiler.Exceptions.CompilerErrors;

namespace SLangCompiler.FrontEnd
{
    public class SlangBaseVisitor : SLangGrammarBaseVisitor<object>
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

        public override object VisitArrayDeclType([NotNull] SLangGrammarParser.ArrayDeclTypeContext context)
        {
            var type = Visit(context.scalarType()) as SlangType;
            var dimension = context.exp().Length;
            return new SlangArrayType(type, dimension);
        }

        public override object VisitPtrType([NotNull] SLangGrammarParser.PtrTypeContext context)
        {
            var customType = context.customType() != null ? Visit(context.customType()) as SlangCustomType : SlangCustomType.Object;
            return new SlangPointerType(customType);
        }

        public void CheckClassExists(string moduleName, string typeName, IToken errToken)
        {
            if (!Table.Modules.ContainsKey(moduleName)) // модуля нет
            {
                ThrowModuleNotFoundException(moduleName, ModuleData.File, errToken);
            }
            else if (!Table.Modules[ModuleData.Name].ImportedModules.Contains(moduleName) && moduleName != ModuleData.Name)
            {
                ThrowModuleNotImportedException(moduleName, ModuleData.File, errToken);
            }
            else if (!Table.Modules[moduleName].Classes.ContainsKey(typeName))
            {
                ThrowClassNotFoundException(moduleName, typeName, ModuleData.File, errToken);
            }
            else if (Table.Modules[moduleName].Classes[typeName].AccessModifier == AccessModifier.Private && moduleName != ModuleData.Name)
            {
                ThrowClassIsPrivateException(moduleName, typeName, ModuleData.File, errToken);
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
