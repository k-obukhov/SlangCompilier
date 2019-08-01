using System;
using System.Collections.Generic;
using System.Text;

namespace SLangCompiler.FrontEnd.Tables
{
    public class SourceCodeTable
    {
        public Dictionary<string, ModuleNameTable> Modules { get; set; }
    }
}
