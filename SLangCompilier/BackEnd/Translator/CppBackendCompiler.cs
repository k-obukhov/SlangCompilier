using Antlr4.Runtime;
using SLangCompiler.FrontEnd;
using SLangCompiler.FrontEnd.Tables;
using SLangGrammar;
using System.IO;

namespace SLangCompiler.BackEnd.Translator
{
    class CppBackendCompiler : BackendCompiler
    {
        public CppBackendCompiler(SourceCodeTable table) : base(table)
        {
        }

        public CppBackendCompiler() : base(null)
        {

        }

        public override void Translate(DirectoryInfo pathToProject)
        {
            var genPath = pathToProject.CreateSubdirectory("gen");
            // remove existing files
            foreach (var file in genPath.EnumerateFiles())
            {
                file.Delete();
            }
            foreach (var key in table.Modules.Keys)
            {
                AntlrInputStream inputStream = new AntlrInputStream(table.Modules[key].ModuleData.Data);
                SLangGrammarLexer lexer = new SLangGrammarLexer(inputStream);
                CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
                var parser = new SLangGrammarParser(commonTokenStream);
                parser.AddErrorListener(new SLangErrorListener(table.Modules[key].ModuleData));

                var translatorVisitor = new CppTranslator(new StreamWriter($"{genPath}/{key}.h"), new StreamWriter($"{genPath}/{key}.cpp"), table, table.Modules[key]);
                translatorVisitor.Visit(parser.start());
            }
        }
    }
}
