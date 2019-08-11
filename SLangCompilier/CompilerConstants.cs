using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SLangCompiler
{
    public enum AccessModifier { Public, Private }
    public enum ParamModifier { Val, Ref }

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
        public static string LibPath => Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName, "Lib");
        /// <summary>
        /// slang file mask
        /// </summary>
        public static string FileMask => $"*{SourceCodeFileExt}";

        public static string RealType => "real";
        public static string IntegerType => "integer";
        public static string CharacterType => "character";
        public static string BooleanType => "boolean";

        public static string ObjectClassName => "Object";

        public static string Public => "public";
        public static string Private => "private";
        public static string Val => "val";
        public static string Ref => "ref";
    }
}
