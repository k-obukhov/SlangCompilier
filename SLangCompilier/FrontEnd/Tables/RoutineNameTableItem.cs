using SLangCompiler.FrontEnd.Types;
using System.Collections.Generic;

namespace SLangCompiler.FrontEnd.Tables
{
    public class RoutineNameTableItem: BaseNameTableItem
    {
        public ImportHeader Header { get; set; }
        public string Name { get; set; }
        public SlangType ReturnType { get; set; }
        public IList<RoutineArgNameTableItem> Params { get; set; }

        public AccessModifier AccessModifier { get; set; }

        public bool IsFunction() => ReturnType == null;
        public bool IsProcedure() => ReturnType == null;
    }

    public class RoutineArgNameTableItem: BaseNameTableItem
    {
        public string Name { get; set; }
        public SlangRoutineTypeArg Type { get; set; }
    }
}