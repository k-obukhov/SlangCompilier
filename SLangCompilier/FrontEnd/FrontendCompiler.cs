using SLangCompiler.FrontEnd.Tables;
using System;
using System.Collections.Generic;
using System.Text;
using Antlr4.Runtime;
using SLangGrammar;
using Antlr4.Runtime.Tree;
using SLangCompiler.FileServices;

namespace SLangCompiler.FrontEnd
{
    public class FrontendCompiler
    {
        public SourceCodeTable SourceCode { get; set; }
        private SlangStoreStepVisitor StoreStepVisitor { get; set; }
        private SlangSemanticVisitor SemanticVisitor { get; set; }

        public FrontendCompiler()
        {
            SourceCode = new SourceCodeTable();
        }

        public void CheckErrors(ProjectManager projectManager)
        {
            // TODO: make async
            foreach (var code in projectManager.FileModules.Values)
            {
                // find errors
                AntlrInputStream inputStream = new AntlrInputStream(code.Data);
                SLGrammarLexer lexer = new SLGrammarLexer(inputStream);
                CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
                SLGrammarParser parser = new SLGrammarParser(commonTokenStream);

                SLangErrorListener errorListener = new SLangErrorListener(code);

                parser.RemoveErrorListeners();
                parser.AddErrorListener(errorListener);

                StoreStepVisitor = new SlangStoreStepVisitor(SourceCode, code);
                SemanticVisitor = new SlangSemanticVisitor(SourceCode, code);

                // store data step
                StoreStepVisitor.Visit(parser.start());
                // last step
            }
        }
    }
}
