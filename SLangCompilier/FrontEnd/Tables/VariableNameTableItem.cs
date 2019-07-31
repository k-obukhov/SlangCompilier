using SLangCompiler.FrontEnd.Types;

namespace SLangCompiler.FrontEnd.Tables
{
    public class VariableNameTableItem
    {
        public bool IsConstant { get; set; }
        public string Name { get; set; }
        public SlangType Type { get; set; }
    }
}