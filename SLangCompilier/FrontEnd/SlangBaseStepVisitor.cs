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

        /// <summary>
        /// base check of class that found in inherit head
        /// 1) is should be exists
        /// 2) it should be public (in another module)
        /// 3) it should have base modifier (anyway)
        /// </summary>
        /// <param name="item"></param>
        /// <param name="moduleName"></param>
        private void checkInheritanceClass(ClassNameTableItem item, string moduleName, bool inSameModule, IToken tokenToError)
        {
            if (item == null)
            {
                throw new CompilerException($"Class {item.TypeIdent} does not exists in module {moduleName}", ModuleData.File, tokenToError);
            }
            else if (item.AccessModifier == AccessModifier.Private && !inSameModule)
            {
                throw new CompilerException($"Class {item.TypeIdent} is private", ModuleData.File, tokenToError);
            }
            else if (item.CanBeBase == false)
            {
                throw new CompilerException($"Class {item.TypeIdent} is not marked as base", ModuleData.File, tokenToError);
            }
        }

        /// <summary>
        /// Поиск идентификатора типа по его названию
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SlangCustomType FindTypeByName(SLGrammarParser.IdContext id, ModuleNameTable module)
        {
            SlangCustomType res = null;
            var tokenToError = id.Id().First().Symbol;
            var nameArray = id.Id().Select(t => t.GetText()).ToArray();

            if (nameArray.Count() == 1)
            {
                var className = nameArray[0];
                var classFound = module.Classes[className];

                checkInheritanceClass(classFound, module.ModuleData.Name, true, tokenToError);
                res = classFound.TypeIdent;
            }
            else if (nameArray.Count() == 2)
            {
                var moduleName = nameArray[0];
                var className = nameArray[1];

                // check module name in imported modules
                if (module.ImportedModules.Contains(moduleName))
                {
                    throw new CompilerException($"Module {moduleName} is not imported", module.ModuleData.File, tokenToError);
                }
                else
                {
                    // check class in module
                    var moduleFound = Table.Modules[moduleName];
                    var classFound = moduleFound.Classes[className];

                    checkInheritanceClass(classFound, moduleName, false, tokenToError);
                    res = classFound.TypeIdent;
                }
            }
            else
            {
                throw new CompilerException("Invalid name format", module.ModuleData.File, tokenToError);
            }

            return res;
        }
    }
}
