namespace SLangCompiler.FrontEnd.Tables
{
    /// <summary>
    /// Fields used in classes 
    /// </summary>
    public class FieldNameTableItem: VariableNameTableItem
    {
        public bool IsDerived { get; set; } = false;
        public AccessModifier AccessModifier { get; set; }
    }
}