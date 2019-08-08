using SLangCompiler.FrontEnd.Types;
using System.Collections.Generic;

namespace SLangCompiler.FrontEnd.Tables
{
    public class ClassNameTableItem
    {
        public SlangCustomType TypeIdent { get; set; }
        public SlangCustomType Base { get; set; } = SlangCustomType.Object;
        public bool CanBeBase { get; set; } = false; // от класса можно отнаследоваться 

        public Dictionary<string, FieldNameTableItem> Fields { get; set; } = new Dictionary<string, FieldNameTableItem>();
        public IList<MethodNameTableItem> Methods { get; set; } = new List<MethodNameTableItem>();

    }

    public class MethodNameTableItem: RoutineNameTableItem
    {
        public bool IsAbstract { get; set; }
        public bool IsOverride { get; set; }
        public string NameOfThis { get; set; }
    }

}