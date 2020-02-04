using SLangCompiler.FileServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using static SLangCompiler.Exceptions.CompilerErrors;

namespace SLangCompiler.FrontEnd.Tables
{
    /// <summary>
    /// Name table for single module
    /// </summary>
    public class ModuleNameTable: BaseNameTableItem
    {
        public ModuleData ModuleData { get; set; }
        public bool TranslatedRecently { get; set; } = false; // toDo in future
        public IList<string> ImportedModules { get; set; } = new List<string>();
        public IList<RoutineNameTableItem> Routines { get; set; } = new List<RoutineNameTableItem>();
        public Dictionary<string, ModuleFieldNameTableItem> Fields { get; set; } = new Dictionary<string, ModuleFieldNameTableItem>();
        public Dictionary<string, ClassNameTableItem> Classes { get; set; } = new Dictionary<string, ClassNameTableItem>();

        // basic checks (only for checking name conflicts)
        public void CheckRoutineConflicts(RoutineNameTableItem routineItem)
        {
            if (Fields.ContainsKey(routineItem.Name) || Classes.ContainsKey(routineItem.Name) || ImportedModules.Contains(routineItem.Name) || module.Name == routineItem.Name)
            {
                ThrowConflictNameException(ModuleData.File, routineItem.Line, routineItem.Column);
            }
        }

        public void CheckFieldConflicts(ModuleFieldNameTableItem fieldItem)
        {
            if (Routines.Any(i => i.Name == fieldItem.Name) || Classes.ContainsKey(fieldItem.Name) || ImportedModules.Contains(fieldItem.Name) || module.Name == fieldItem.Name)
            {
                ThrowConflictNameException(ModuleData.File, fieldItem.Line, fieldItem.Column);
            }
        }

        public void CheckClassConflicts(ClassNameTableItem classItem)
        {
            if (Routines.Any(i => i.Name == classItem.TypeIdent.Name) || Fields.ContainsKey(classItem.TypeIdent.Name) || ImportedModules.Contains(classItem.TypeIdent.Name) || module.Name == classItem.TypeIdent.Name)
            {
                ThrowConflictNameException(ModuleData.File, classItem.Line, classItem.Column);
            }
        }

    }
}
