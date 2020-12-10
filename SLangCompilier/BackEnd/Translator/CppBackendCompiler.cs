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

        public override void Translate(DirectoryInfo genPath)
        {
            Directory.CreateDirectory(genPath.FullName);
            // remove existing files
            foreach (var file in genPath.EnumerateFiles())
            {
                file.Delete();
            }
            // and dirs
            foreach (DirectoryInfo dir in genPath.GetDirectories())
            {
                dir.Delete(true);
            }
            foreach (var key in Table.Modules.Keys)
            {
                if (Table.Modules[key].IsEmpty)
                {
                    continue;
                }
                AntlrInputStream inputStream = new AntlrInputStream(Table.Modules[key].ModuleData.Data);
                SLangGrammarLexer lexer = new SLangGrammarLexer(inputStream);
                CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
                var parser = new SLangGrammarParser(commonTokenStream);
                parser.AddErrorListener(new SLangErrorListener(Table.Modules[key].ModuleData));

                var translatorVisitor = new CppTranslator(new StreamWriter($"{genPath}/{key}.h"), new StreamWriter($"{genPath}/{key}.cpp"), Table, Table.Modules[key], genPath);
                translatorVisitor.Visit(parser.start());
            }
        }
    }
}
