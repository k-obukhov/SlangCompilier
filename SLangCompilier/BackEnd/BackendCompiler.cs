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
        private readonly BaseBackendTranslator translator;
        public BackendCompiler(SourceCodeTable table, BaseBackendTranslator translator)
        {
            this.table = table;
            this.translator = translator;
        }

        public void Translate()
        {

        }
    }
}
