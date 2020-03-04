using System;

namespace SLangCompiler.Exceptions
{
    /// <summary>
    /// Exception throws when module name contains in system modules
    /// </summary>
    class FileAlreadyExistsException : Exception
    {
        public FileAlreadyExistsException(string text) : base(text) { }
    }
}
