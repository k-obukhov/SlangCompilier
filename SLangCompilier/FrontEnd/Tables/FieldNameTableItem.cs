using System;

namespace SLangCompiler.FrontEnd.Tables
{
    /// <summary>
    /// Fields used in classes 
    /// </summary>
    public class FieldNameTableItem : VariableNameTableItem, ICloneable
    {
        public bool IsDerived { get; set; } = false;
        public AccessModifier AccessModifier { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}