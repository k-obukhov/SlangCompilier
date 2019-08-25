using SLangCompiler.FrontEnd.Tables;
using System;
using System.Collections.Generic;
using System.Text;
using Antlr4.Runtime;
using System.Linq;
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

        private SLGrammarParser generateParser(string sourceCode)
        {
            AntlrInputStream inputStream = new AntlrInputStream(sourceCode);
            SLGrammarLexer lexer = new SLGrammarLexer(inputStream);
            CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
            return new SLGrammarParser(commonTokenStream);
        }

        public void CheckErrors(ProjectManager projectManager)
        {
            // TODO: make async
            string[] allModules = projectManager.FileModules.Values.Select(m => m.Name).ToArray();
            foreach (var code in projectManager.FileModules.Values)
            {
                SLGrammarParser parser = generateParser(code.Data);
                SLangErrorListener errorListener = new SLangErrorListener(code);
                parser.AddErrorListener(errorListener);

                StoreStepVisitor = new SlangStoreStepVisitor(SourceCode, code, allModules);
                // store data step
                StoreStepVisitor.Visit(parser.start());
            }

            // step #2

        }
    }
}
