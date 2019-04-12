using System;
using System.IO;

namespace SLangCompiler.Exceptions
{
    public class CompilerException: ApplicationException
    {
        public CompilerException(string message, FileInfo moduleFile, int line, int column)
            : base(message)
        {
            ModuleFile = moduleFile;
            Line = line;
            Column = column;
        }

        public FileInfo ModuleFile { get; }
        public int Line { get; }
        public int Column { get; }
    }
}
