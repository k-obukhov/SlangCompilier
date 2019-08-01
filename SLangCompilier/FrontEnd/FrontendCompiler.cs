using SLangCompiler.FrontEnd.Tables;
using System;
using System.Collections.Generic;
using System.Text;

namespace SLangCompiler.FrontEnd
{
    public class FrontendCompiler
    {
        public SourceCodeTable SourceCode { get; set; }
        private SlangStoreStepVisitor StoreStepVisitor { get; } = new SlangStoreStepVisitor();
        private SlangSemanticVisitor SemanticVisitor { get; } = new SlangSemanticVisitor();

        public void CheckErrors()
        {
            // TODO: make async
        }
    }
}
