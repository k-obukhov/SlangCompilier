using SLangCompiler.FrontEnd.Tables;
using System;
using System.Collections.Generic;
using System.Text;
using Antlr4.Runtime;
using SLangGrammar;
using Antlr4.Runtime.Tree;

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

        public void CheckErrors()
        {
            // TODO: make async
            foreach (var code in SourceCode.Modules)
            {
                // find errors
                AntlrInputStream inputStream = new AntlrInputStream(code.Value.ModuleData.Data);
                SLGrammarLexer lexer = new SLGrammarLexer(inputStream);
                CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
                SLGrammarParser parser = new SLGrammarParser(commonTokenStream);

                SLangErrorListener errorListener = new SLangErrorListener(code.Value.ModuleData);

                parser.RemoveErrorListeners();
                parser.AddErrorListener(errorListener);

                var pTree = parser.start();

                StoreStepVisitor = new SlangStoreStepVisitor(SourceCode, code.Key);
                SemanticVisitor = new SlangSemanticVisitor(SourceCode, code.Key);

                // store data step
                IParseTree tree = pTree;
                StoreStepVisitor.Visit(pTree);
                // last step
            }
        }
    }
}
