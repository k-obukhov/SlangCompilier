using SLangCompiler.FrontEnd.Types;

namespace SLangCompiler.FrontEnd.Tables
{
    public abstract class BaseNameTableItem
    {
        public int Line { get; set; }
        public int Column { get; set; }
        public string Name { get; set; }
        public abstract SlangType ToSlangType();
    }
}
