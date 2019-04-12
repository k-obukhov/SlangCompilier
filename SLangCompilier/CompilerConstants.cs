using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SLangCompiler.FileServices
{
    static class CompilerConstants
    {
        /// <summary>
        /// File extension for the source code (.sl)
        /// </summary>
        public static string SourceCodeFileExt => ".sl";
        /// <summary>
        /// Main module name (no ext)
        /// </summary>
        public static string MainModuleName => "Main";
        /// <summary>
        /// Main module name with ext
        /// </summary>
        public static string MainModuleNameWithExt => $"{MainModuleName}{SourceCodeFileExt}";
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
