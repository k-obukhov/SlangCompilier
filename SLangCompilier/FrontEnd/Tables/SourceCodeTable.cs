using SLangCompiler.FrontEnd.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace SLangCompiler.FrontEnd.Tables
{
    public class SourceCodeTable
    {
        public Dictionary<string, ModuleNameTable> Modules { get; set; } = new Dictionary<string, ModuleNameTable>();
        public ClassNameTableItem FindClass(string moduleName, string className)
        {
            ClassNameTableItem res = null;

            if (Modules.ContainsKey(moduleName) && Modules[moduleName].Classes.ContainsKey(className))
            {
                res = Modules[moduleName].Classes[moduleName];
            }

            return res;
        }

        public ClassNameTableItem FindClass(SlangCustomType type)
        {
            return FindClass(type.ModuleName, type.Name);
        }
    }
}
