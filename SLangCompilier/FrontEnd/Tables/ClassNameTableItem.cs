using SLangCompiler.FrontEnd.Types;
using System.Collections.Generic;

namespace SLangCompiler.FrontEnd.Tables
{
    public class ClassNameTableItem
    {
        public SlangCustomType TypeIdent { get; set; }
        public SlangCustomType ParentClass { get; set; } = SlangCustomType.Object;

        public Dictionary<string, FieldNameTableItem> Fields { get; set; } = new Dictionary<string, FieldNameTableItem>();
        public IList<MethodNameTableItem> MethodProcs { get; set; } = new List<MethodNameTableItem>();

    }

    public class MethodNameTableItem
    {
        public SlangRoutineType FuncType { get; set; }
        public string Name { get; set; }
        public AccessModifier AccessModifier { get; set; }
        public bool IsAbstract { get; set; } 
        public bool IsOverride { get; set; }
        public string NameOfThis { get; set; }
    }
}