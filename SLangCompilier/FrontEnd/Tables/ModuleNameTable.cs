using System;
using System.Collections.Generic;
using System.Text;

namespace SLangCompiler.FrontEnd.Tables
{
    /// <summary>
    /// Name table for single module
    /// </summary>
    public class ModuleNameTable
    {
        public IList<string> ImportedModules { get; set; } = new List<string>();
        public IList<FunctionNameTableItem> Functions { get; set; } = new List<FunctionNameTableItem>();
        public IList<ProcedureNameTableItem> Procedures { get; set; } = new List<ProcedureNameTableItem>();
        public Dictionary<string, FieldNameTableItem> Variables { get; set; } = new Dictionary<string, FieldNameTableItem>();

        public IList<ClassNameTableItem> Classes { get; set; } = new List<ClassNameTableItem>();
    }
}
