using SLangCompiler.BackEnd.Translator;
using SLangCompiler.FrontEnd.Tables;
using System;
using System.Collections.Generic;
using System.Text;

namespace SLangCompiler.BackEnd
{
    public class BackendCompiler
    {
        private readonly SourceCodeTable table;
        public BackendCompiler(SourceCodeTable table)
        {
            this.table = table;
        }

        public void Translate()
        {

        }
    }
}
