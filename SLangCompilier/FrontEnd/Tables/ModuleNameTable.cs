using SLangCompiler.FileServices;
using SLangCompiler.FrontEnd.Types;
using System.Collections.Generic;
using static SLangCompiler.Exceptions.CompilerErrors;

namespace SLangCompiler.FrontEnd.Tables
{
    /// <summary>
    /// Name table for single module
    /// </summary>
    public class ModuleNameTable : BaseNameTableItem
    {
        public new string Name => ModuleData.Name;
        public ModuleData ModuleData { get; set; }
        public bool TranslatedRecently { get; set; } = false; // toDo in future
        public IList<string> ImportedModules { get; set; } = new List<string>();
        public Dictionary<string, RoutineNameTableItem> Routines { get; set; } = new Dictionary<string, RoutineNameTableItem>();
        public Dictionary<string, ModuleFieldNameTableItem> Fields { get; set; } = new Dictionary<string, ModuleFieldNameTableItem>();
        public Dictionary<string, ClassNameTableItem> Classes { get; set; } = new Dictionary<string, ClassNameTableItem>();
        public bool IsEmpty { get; set; } = false;

        public override SlangType ToSlangType() => null;

        // basic checks (only for checking name conflicts)

        public void CheckCommonNamesConflicts(string name, int line, int column)
        {
            if (Classes.ContainsKey(name) || Routines.ContainsKey(name) || Fields.ContainsKey(name) || ImportedModules.Contains(name) || ModuleData.Name == name)
            {
                ThrowConflictNameException(ModuleData.File, line, column);
            }
        }
    }
}
