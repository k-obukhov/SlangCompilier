using SLangCompiler.FrontEnd.Types;
using System.Collections.Generic;

namespace SLangCompiler.FrontEnd.Tables
{
    public class RoutineNameTableItem: BaseNameTableItem
    {
        public string Name { get; set; }
        public SlangRoutineType Type { get; set; }
        public IList<string> NameParams { get; set; }

        public AccessModifier AccessModifier { get; set; }

        public bool IsFunction() => Type is SlangFunctionType;
        public bool IsProcedure() => Type is SlangProcedureType;
    }
}