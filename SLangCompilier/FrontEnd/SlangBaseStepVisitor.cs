using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using SLangCompiler.Exceptions;
using SLangCompiler.FileServices;
using SLangCompiler.FrontEnd.Tables;
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
            Enum.TryParse(typeof(AccessModifier), name, out object res);
            return (AccessModifier) res;
        }

        public ParamModifier GetParamModifierByName(string name)
        {
            Enum.TryParse(typeof(ParamModifier), name, out object res);
            return (ParamModifier) res;
        }

        public void ThrowIfReservedWord(string name, IToken token)
        {
            if (CompilerConstants.CppKeywords.Contains(name) || CompilerConstants.SlangKeywords.Contains(name))
            {
                throw new CompilerException($"Name {name} is reserved", ModuleData.File, token);
            }
        }
    }
}
