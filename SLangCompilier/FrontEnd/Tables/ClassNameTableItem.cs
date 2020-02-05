using SLangCompiler.FileServices;
using SLangCompiler.FrontEnd.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using static SLangCompiler.Exceptions.CompilerErrors;

namespace SLangCompiler.FrontEnd.Tables
{
    public class ClassNameTableItem: BaseNameTableItem
    {
        public AccessModifier AccessModifier { get; set; }
        public SlangCustomType TypeIdent { get; set; }
        public SlangCustomType Base { get; set; } // may be null (if TypeIdent == System.Object)
        public bool CanBeBase { get; set; } = false; // от класса можно отнаследоваться 

        public Dictionary<string, FieldNameTableItem> Fields { get; set; } = new Dictionary<string, FieldNameTableItem>();
        public IList<MethodNameTableItem> Methods { get; set; } = new List<MethodNameTableItem>();

        public bool IsAbstract() => Methods.Any(m => m.IsAbstract);

        public override SlangType ToSlangType() => TypeIdent;

        public void CheckRoutineConflicts(ModuleData module, MethodNameTableItem routineItem)
        {
            if (Fields.ContainsKey(routineItem.Name))
            {
                ThrowConflictNameException(module.File, routineItem.Line, routineItem.Column);
            }
        }

        public void CheckFieldConflicts(ModuleData module, FieldNameTableItem fieldItem)
        {
            if (Methods.Any(i => i.Name == fieldItem.Name))
            {
                ThrowConflictNameException(module.File, fieldItem.Line, fieldItem.Column);
            }
        }
    }

    public class MethodNameTableItem: RoutineNameTableItem, ICloneable
    {
        public bool IsDerived { get; set; } = false;
        public bool IsAbstract { get; set; }
        public bool IsOverride { get; set; }
        public string NameOfThis { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

}