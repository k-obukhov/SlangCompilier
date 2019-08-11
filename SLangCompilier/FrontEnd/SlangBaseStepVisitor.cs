using Antlr4.Runtime.Misc;
using SLangCompiler.FileServices;
using SLangCompiler.FrontEnd.Tables;
using SLangGrammar;
using System;
using System.Collections.Generic;
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
            AccessModifier res;
            if (name == CompilerConstants.Public)
            {
                res = AccessModifier.Public;
            }
            else
            {
                res = AccessModifier.Private;
            }
            return res;
        }

        public ParamModifier GetParamModifierByName(string name)
        {
            ParamModifier res;
            if (name == CompilerConstants.Val)
            {
                res = ParamModifier.Val;
            }
            else
            {
                res = ParamModifier.Ref;
            }
            return res;
        }
    }
}
