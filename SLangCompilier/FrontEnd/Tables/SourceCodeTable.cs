using SLangCompiler.FrontEnd.Types;
using System;
using System.Collections.Generic;
using System.Linq;
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
                res = Modules[moduleName].Classes[className];
            }

            return res;
        }

        public ClassNameTableItem FindClass(SlangCustomType type)
        {
            return FindClass(type.ModuleName, type.Name);
        }

        /// <summary>
        /// Logic for finding data for modules
        /// </summary>
        /// <param name="name">name identifier</param>
        /// <param name="moduleCaller">module where need to call</param>
        /// <param name="moduleDest">module to find</param>
        /// <param name="items">result (array, because routine may have many overrides)</param>
        /// <returns>bool value -- true if something was found</returns>
        public bool TryFindModuleItemsByName(string name, string moduleCaller, string moduleDest, out BaseNameTableItem[] items)
        {
            bool result = true;
            items = null;
            // можно взять как public, так private
            if (moduleCaller == moduleDest)
            {
                var tableDest = Modules[moduleDest];
                if (tableDest.Routines.Any(r => r.Name == name))
                {
                    items = tableDest.Routines.Where(r => r.Name == name).ToArray();
                }
                else if (tableDest.Fields.ContainsKey(name))
                {
                    items = new BaseNameTableItem[] { tableDest.Fields[name] };
                }
                else
                {
                    result = false;
                }
            }
            else
            {
                var tableDest = Modules[moduleDest];
                if (tableDest.Routines.Any(r => r.Name == name && r.AccessModifier == AccessModifier.Public))
                {
                    items = tableDest.Routines.Where(r => r.Name == name).ToArray();
                }
                else if (tableDest.Fields.ContainsKey(name) && tableDest.Fields[name].AccessModifier == AccessModifier.Public)
                {
                    items = new BaseNameTableItem[] { tableDest.Fields[name] };
                }
                else
                {
                    result = false;
                }
            }
            return result;
        }

        public bool TryFoundClassItemsByName(string name, SlangCustomType classContext, SlangCustomType destType, out BaseNameTableItem[] items)
        {
            items = null;
            bool result = true;

            // находимся в контексте этого типа
            if (classContext.Equals(destType))
            {
                var classItem = FindClass(destType);
                if (classItem.Methods.Any(m => m.Name == name))
                {
                    items = classItem.Methods.Where(m => m.Name == name).ToArray();
                }
                else if (classItem.Fields.ContainsKey(name))
                {
                    items = new BaseNameTableItem[] { classItem.Fields[name] };
                }
                else
                {
                    result = false;
                }
            }
            else
            {
                var classItem = FindClass(destType);
                if (classItem.Methods.Any(m => m.Name == name && m.AccessModifier == AccessModifier.Public))
                {
                    items = classItem.Methods.Where(m => m.Name == name).ToArray();
                }
                else if (classItem.Fields.ContainsKey(name) && classItem.Fields[name].AccessModifier == AccessModifier.Public)
                {
                    items = new BaseNameTableItem[] { classItem.Fields[name] };
                }
                else
                {
                    result = false;
                }
            }

            return result;
        }
    }
}
