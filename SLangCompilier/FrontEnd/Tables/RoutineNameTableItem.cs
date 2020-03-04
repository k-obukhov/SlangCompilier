using SLangCompiler.FrontEnd.Types;
using System.Collections.Generic;
using System.Linq;

namespace SLangCompiler.FrontEnd.Tables
{
    public class RoutineNameTableItem : BaseNameTableItem, IImportable
    {
        public ImportHeader Header { get; set; }
        public SlangType ReturnType { get; set; }
        public IList<RoutineArgNameTableItem> Params { get; set; }
        public override SlangType ToSlangType() => IsFunction() ? new SlangFunctionType(ToSlangArgs(), ReturnType) as SlangRoutineType : new SlangProcedureType(ToSlangArgs()) as SlangRoutineType;
        private IList<SlangRoutineTypeArg> ToSlangArgs() => Params.Select(p => p.TypeArg).ToList();
        public AccessModifier AccessModifier { get; set; }

        public bool IsFunction() => ReturnType != null;
        public bool IsProcedure() => ReturnType == null;
    }

    public class RoutineArgNameTableItem : BaseNameTableItem
    {
        public SlangRoutineTypeArg TypeArg { get; set; }

        public override SlangType ToSlangType() => TypeArg.Type;
    }
}