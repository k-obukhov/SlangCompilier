using Antlr4.Runtime;
using System;
using System.IO;

namespace SLangCompiler.Exceptions
{
    /// <summary>
    /// Exception for errors from compilier
    /// </summary>
    public class CompilerException : ApplicationException
    {
        public CompilerException(string message, FileInfo moduleFile, int line, int column) : base(message)
        {
            ModuleFile = moduleFile;
            Line = line;
            Column = column;
        }

        public CompilerException(string message, FileInfo moduleFile, IToken symbol)
            : base(message)
        {
            ModuleFile = moduleFile;
            Line = symbol.Line;
            Column = symbol.Column;
        }

        public FileInfo ModuleFile { get; }
        public int Line { get; }
        public int Column { get; }
    }
}
