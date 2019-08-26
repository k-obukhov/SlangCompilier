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
        private SlangStoreRoutinesVisitor StoreStepRoutinesVisitor { get; set; }

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
            var modules = projectManager.FileModules;

            foreach (var key in modules.Keys)
            {
                SLGrammarParser parser = generateParser(modules[key].Data);
                SLangErrorListener errorListener = new SLangErrorListener(modules[key]);
                parser.AddErrorListener(errorListener);

                StoreStepVisitor = new SlangStoreStepVisitor(SourceCode, modules[key], allModules);
                // store data step
                StoreStepVisitor.Visit(parser.start());
            }

            // step #2
            foreach (var key in modules.Keys)
            {
                SLGrammarParser parser = generateParser(modules[key].Data);
                SLangErrorListener errorListener = new SLangErrorListener(modules[key]);
                parser.AddErrorListener(errorListener);

                StoreStepRoutinesVisitor = new SlangStoreRoutinesVisitor(SourceCode, modules[key]);
                // store data step
                StoreStepRoutinesVisitor.Visit(parser.start());
            }
        }
    }
}
