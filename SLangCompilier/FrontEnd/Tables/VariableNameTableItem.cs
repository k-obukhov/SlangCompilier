using SLangCompiler.FrontEnd.Types;

namespace SLangCompiler.FrontEnd.Tables
{
    public class VariableNameTableItem: BaseNameTableItem
    {
        public bool IsConstant { get; set; }
        public SlangType Type { get; set; }

        public override SlangType ToSlangType() => Type;
    }
}