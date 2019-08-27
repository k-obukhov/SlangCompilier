using System;
using System.Collections.Generic;
using System.Text;

namespace SLangCompiler.FrontEnd.Tables
{
    public class MethodFieldNameTableItem: FieldNameTableItem
    {
        public bool IsReadonly { get; set; }
    }
}
