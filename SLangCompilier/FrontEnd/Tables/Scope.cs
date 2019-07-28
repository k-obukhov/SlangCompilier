using System;
using System.Collections.Generic;
using System.Text;

namespace SLangCompiler.FrontEnd.Tables
{
    // Base scope class logic
    public class Scope
    {
        public Scope Outer { get; set; } = null;

        public Dictionary<string, VariableNameTable> Names { get; set; } = new Dictionary<string, VariableNameTable>();

        public Scope(Dictionary<string, VariableNameTable> names)
        {
            Names = names;
        }

        public Scope(Dictionary<string, VariableNameTable> names, Scope outer)
        {
            Names = names;
            Outer = outer;
        }
    }
}
