using System;
using System.Collections.Generic;
using System.Text;

namespace SLangCompiler.FrontEnd.Tables
{
    /// <summary>
    /// Name table for single module
    /// </summary>
    public class ModuleNameTable
    {
        public IList<string> ImportedModules { get; set; } = new List<string>();
        public IList<RoutineNameTableItem> Routines { get; set; } = new List<RoutineNameTableItem>();
        public Dictionary<string, FieldNameTableItem> Fields { get; set; }

        public IList<ClassNameTableItem> Classes { get; set; } = new List<ClassNameTableItem>();
    }
}
