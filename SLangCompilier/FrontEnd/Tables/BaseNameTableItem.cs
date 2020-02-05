using SLangCompiler.FrontEnd.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace SLangCompiler.FrontEnd.Tables
{
    public abstract class BaseNameTableItem
    {
        public int Line { get; set; }
        public int Column { get; set; }

        public abstract SlangType ToSlangType();
    }
}
