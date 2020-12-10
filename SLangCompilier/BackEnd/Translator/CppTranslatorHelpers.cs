using SLangCompiler.FrontEnd.Types;
using SLangGrammar;
using System.Collections.Generic;
using System.Linq;

namespace SLangCompiler.BackEnd.Translator
{
    public partial class CppTranslator
    {
        private SlangCustomType GetCustomTypeContext(SLangGrammarParser.CustomTypeContext context)
        {
            string nameOfModule = "", typeName = "";
            var ids = context.qualident().Id().Select(x => x.GetText()).ToArray();

            if (ids.Count() == 1)
            {
                nameOfModule = moduleName;
                typeName = ids[0];
            }
            else if (ids.Count() == 2)
            {
                nameOfModule = ids[0];
                typeName = ids[1];

            }
            return new SlangCustomType(nameOfModule, typeName);
        }

        // helpers
        /// <summary>
        /// uses only for routines/fields declaration, another declares should use Visit() for types
        /// </summary>
        /// <param name="returnType"></param>
        /// <returns></returns>
        private string GetStringFromType(SlangType returnType)
        {
            var res = "";
            var simpleTypes = new Dictionary<string, string>
            {
                { CompilerConstants.IntegerType, "int64_t" },
                { CompilerConstants.RealType, "double" },
                { CompilerConstants.CharacterType, "char" },
                { CompilerConstants.BooleanType, "bool" },
                { CompilerConstants.StringType, "std::string" }
            };
            if (returnType is SlangSimpleType st)
            {
                res = simpleTypes[st.Name];
            }
            else if (returnType is SlangArrayType at)
            {
                res += GetVectorTypeStart(at.Dimension);
                res += GetStringFromType(at.Type);
                res += GetVectorTypeEnd(at.Dimension);
            }
            else if (returnType is SlangCustomType ct)
            {
                var item = source.FindClass(ct);
                if (item.Header != null)
                {
                    res = item.Header.Ident.Replace("\"", "");
                }
                else
                {
                    res = ct.ModuleName == moduleName ? ct.Name : $"{ct.ModuleName}::{ct.Name}";
                }
            }
            else if (returnType is SlangPointerType pt)
            {
                res = $"std::shared_ptr<{GetStringFromType(pt.PtrType)}>";
            }
            else if (returnType == null)
            {
                res = "void";
            }
            return res;
        }

        private void WriteLineAll(string text)
        {
            headerText.WriteLine(text);
            cppText.WriteLine(text);
        }

        private string GetVectorTypeStart(int dimension)
        {
            string res = "";
            for (int i = 0; i < dimension; ++i)
            {
                res += "std::vector<";
            }
            return res;
        }

        private string GetVectorTypeEnd(int dimension)
        {
            string res = "";
            for (int i = 0; i < dimension; ++i)
            {
                res += ">";
            }
            return res;
        }
    }
}