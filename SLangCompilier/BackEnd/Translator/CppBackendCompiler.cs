using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SLangCompiler.FrontEnd.Tables;
using SLangGrammar;
using SLangCompiler.FrontEnd;
using Antlr4.Runtime;

namespace SLangCompiler.BackEnd.Translator
{
    class CppBackendCompiler: BackendCompiler
    {
        public CppBackendCompiler(SourceCodeTable table): base(table)
        {
        }

        public CppBackendCompiler(): base(null)
        {

        }

        public override void Translate(DirectoryInfo pathToProject)
        {
            var genPath = pathToProject.CreateSubdirectory("/gen/cpp");
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
