using Antlr4.Runtime;
using SLangGrammar;
using System;
using System.IO;
using System.Linq;

namespace AntlrSyntaxTests
{
    static class GrammarTester
    {
        public static void Test(string filePath)
        {
            string text = string.Empty;
            using (var reader = new StreamReader(filePath))
            {
                text = reader.ReadToEnd();
            }

            var inputStream = new AntlrInputStream(text);
            var lexer = new SLangGrammarLexer(inputStream);
            var commonTokenStream = new CommonTokenStream(lexer);
            var parser = new SLangGrammarParser(commonTokenStream);

            var errorListener = new SlangParserErrorListener();
            var lexerErrorListener = new SlangLexerErrorListener();

            lexer.RemoveErrorListeners();
            lexer.AddErrorListener(lexerErrorListener);
            parser.RemoveErrorListeners();
            parser.AddErrorListener(errorListener);

            parser.start();

            var lexerErrors = lexerErrorListener.ErrorMessages;
            var parserErrors = errorListener.ErrorMessages;
            var commonErrors = lexerErrors.Concat(parserErrors);

            if (commonErrors.Any())
            {
                foreach (var error in commonErrors)
                {
                    Console.WriteLine(error);
                }
                throw new Exception("Some errors found!");
            }
        }

    }
}
