namespace SLangCompiler.FrontEnd.Tables
{
    /// <summary>
    /// Fields used in classes & in modules declaration place
    /// </summary>
    public class FieldNameTableItem: VariableNameTable
    {
        public AccessModifier AccessModifier { get; set; }
    }
}