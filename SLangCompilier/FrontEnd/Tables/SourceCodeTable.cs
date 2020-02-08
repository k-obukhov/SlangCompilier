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
        /// <param name="item">result (array, because routine may have many overrides)</param>
        /// <returns>bool value -- true if something was found</returns>
        public bool TryFindModuleItemsByName(string name, string moduleCaller, string moduleDest, out BaseNameTableItem item)
        {
            bool result = true;
            item = null;
            // можно взять как public, так private
            if (moduleCaller == moduleDest)
            {
                var tableDest = Modules[moduleDest];
                if (tableDest.Routines.ContainsKey(name))
                {
                    item = tableDest.Routines[name];
                }
                else if (tableDest.Fields.ContainsKey(name))
                {
                    item = tableDest.Fields[name];
                }
                else
                {
                    result = false;
                }
            }
            else
            {
                var tableDest = Modules[moduleDest];
                if (tableDest.Routines.ContainsKey(name) && tableDest.Routines[name].AccessModifier == AccessModifier.Public)
                {
                    item = tableDest.Routines[name];
                }
                else if (tableDest.Fields.ContainsKey(name) && tableDest.Fields[name].AccessModifier == AccessModifier.Public)
                {
                    item = tableDest.Fields[name];
                }
                else
                {
                    result = false;
                }
            }
            return result;
        }

        public bool TryFoundClassItemsByName(string name, SlangCustomType classContext, SlangCustomType destType, out BaseNameTableItem item)
        {
            item = null;
            bool result = true;

            // находимся в контексте этого типа
            if (classContext != null && classContext.Equals(destType))
            {
                var classItem = FindClass(destType);
                if (classItem.Methods.ContainsKey(name))
                {
                    item = classItem.Methods[name];
                }
                else if (classItem.Fields.ContainsKey(name))
                {
                    item = classItem.Fields[name];
                }
                else
                {
                    result = false;
                }
            }
            else
            {
                var classItem = FindClass(destType);
                if (classItem.Methods.ContainsKey(name) && classItem.Methods[name].AccessModifier == AccessModifier.Public)
                {
                    item = classItem.Methods[name];
                }
                else if (classItem.Fields.ContainsKey(name) && classItem.Fields[name].AccessModifier == AccessModifier.Public)
                {
                    item = classItem.Fields[name];
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
