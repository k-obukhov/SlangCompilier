using System;
using System.Collections.Generic;
using System.Text;

namespace SLangCompiler.FrontEnd.Tables
{
    // Base scope class logic
    public class Scope
    {
        public Scope Outer { get; set; } = null;

        public Dictionary<string, VariableNameTableItem> Names { get; set; } = new Dictionary<string, VariableNameTableItem>();

        public Scope(Dictionary<string, VariableNameTableItem> names)
        {
            Names = names;
        }

        public Scope(Dictionary<string, VariableNameTableItem> names, Scope outer)
        {
            Names = names;
            Outer = outer;
        }

        public bool VariableExists(string name) => FindVariable(name) != null;

        public VariableNameTableItem FindVariable(string name)
        {
            VariableNameTableItem res = null;
            // find in this scope
            if (Names.ContainsKey(name))
            {
                res = Names[name];
            }
            else
            {
                // search in outer scopes
                var outer = Outer;
                while (outer != null)
                {
                    if (outer.Names.ContainsKey(name))
                    {
                        res = Names[name];
                        break;
                    }
                    else
                    {
                        outer = outer.Outer;
                    }
                }
            }
            return res;
        }
    }
}
