using SLangCompiler.FileServices;
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
        public ModuleData ModuleData { get; set; }
        public bool TranslatedRecently { get; set; } = false;
        public IList<string> ImportedModules { get; set; } = new List<string>();
        public IList<RoutineNameTableItem> Routines { get; set; } = new List<RoutineNameTableItem>();
        public Dictionary<string, MethodFieldNameTableItem> Fields { get; set; }
        public Dictionary<string, ClassNameTableItem> Classes { get; set; } = new Dictionary<string, ClassNameTableItem>();
    }
}
