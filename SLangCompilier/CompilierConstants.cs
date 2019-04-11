using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SLangCompilier.FileServices
{
    static class CompilierConstants
    {
        /// <summary>
        /// File extension for the source code (.sl)
        /// </summary>
        public static string SourceCodeFileExt => ".sl";
        /// <summary>
        /// Main module name
        /// </summary>
        public static string MainModuleName => $"Main{SourceCodeFileExt}";
        /// <summary>
        /// Path to SL standard modules!
        /// </summary>
        public static string LibPath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Lib");
        /// <summary>
        /// slang file mask
        /// </summary>
        public static string FileMask => $"*{SourceCodeFileExt}";
    }
}
