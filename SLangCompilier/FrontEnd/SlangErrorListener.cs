using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using SLangCompiler.Exceptions;
using SLangCompiler.FileServices;

namespace SLangCompiler.FrontEnd
{
    class SLangErrorListener: BaseErrorListener
    {
        public ModuleData Module { get; }

        public SLangErrorListener(ModuleData module)
        {
            Module = module;
        }

        public override void SyntaxError([NotNull] IRecognizer recognizer, [Nullable] IToken offendingSymbol, int line, int charPositionInLine, [NotNull] string msg, [Nullable] RecognitionException e)
        {
            throw new CompilerException(msg, Module.File, line, charPositionInLine);
        }
    }
}
