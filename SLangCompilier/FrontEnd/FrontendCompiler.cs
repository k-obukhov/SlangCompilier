using System.Linq;
using Antlr4.Runtime;
using SLangCompiler.FileServices;
using SLangCompiler.FrontEnd.Tables;
using SLangGrammar;

namespace SLangCompiler.FrontEnd
{
    public class FrontendCompiler
    {
        public SourceCodeTable SourceCode { get; set; }

        public FrontendCompiler()
        {
            SourceCode = new SourceCodeTable();
        }

        private SLangGrammarParser GenerateParser(string sourceCode)
        {
            AntlrInputStream inputStream = new AntlrInputStream(sourceCode);
            SLangGrammarLexer lexer = new SLangGrammarLexer(inputStream);
            CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
            return new SLangGrammarParser(commonTokenStream);
        }

        public void CheckErrors(ProjectManager projectManager)
        {
            // TODO: make async
            string[] allModules = projectManager.FileModules.Keys.ToArray();
            var modules = projectManager.FileModules;


            modules.Keys.ToList().ForEach(key =>
            {
                SLangGrammarParser parser = GenerateParser(modules[key].Data);
                SLangErrorListener errorListener = new SLangErrorListener(modules[key]);
                parser.AddErrorListener(errorListener);

                var storeStepVisitor = new SlangStoreTypesVisitor(SourceCode, modules[key], allModules);
                // store data step
                storeStepVisitor.Visit(parser.start());
            });

            modules.Keys.ToList().ForEach(key =>
            {
                SLangGrammarParser parser = GenerateParser(modules[key].Data);
                SLangErrorListener errorListener = new SLangErrorListener(modules[key]);
                parser.AddErrorListener(errorListener);

                var storeStepFieldsVisitor = new SlangStoreFieldsVisitor(SourceCode, modules[key]);
                // store data step
                storeStepFieldsVisitor.Visit(parser.start());
            });

            modules.Keys.ToList().ForEach(key =>
            {
                SLangGrammarParser parser = GenerateParser(modules[key].Data);
                SLangErrorListener errorListener = new SLangErrorListener(modules[key]);
                parser.AddErrorListener(errorListener);

                var storeStepRoutinesVisitor = new SlangStoreRoutinesVisitor(SourceCode, modules[key]);
                // store data step
                storeStepRoutinesVisitor.Visit(parser.start());
            });

            var classChecker = new ClassesValidator(SourceCode);
            classChecker.Check();

            modules.Keys.ToList().ForEach(key =>
            {
                SLangGrammarParser parser = GenerateParser(modules[key].Data);
                SLangErrorListener errorListener = new SLangErrorListener(modules[key]);
                parser.AddErrorListener(errorListener);

                var semanticVisitor = new SlangSemanticVisitor(SourceCode, modules[key]);
                // store data step
                semanticVisitor.Visit(parser.start());
            });
        }
    }
}
