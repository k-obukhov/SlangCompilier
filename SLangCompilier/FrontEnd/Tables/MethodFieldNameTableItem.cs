using System;
using System.Collections.Generic;
using System.Text;

namespace SLangCompiler.FrontEnd.Tables
{
    public class ModuleFieldNameTableItem: FieldNameTableItem
    {
        public bool IsReadonly { get; set; }
    }
}
