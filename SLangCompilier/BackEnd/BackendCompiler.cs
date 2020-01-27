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
        private readonly BaseBackEndTranslator translator;
        public BackendCompiler(SourceCodeTable table, BaseBackEndTranslator translator)
        {
            this.table = table;
            this.translator = translator;
        }

        public void Translate()
        {

        }
    }
}
