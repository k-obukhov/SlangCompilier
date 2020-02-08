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
        public static string StringType => "string";
        public static string ObjectClassName => "Object";

        public static string Public => "public";
        public static string Private => "private";
        public static string Val => "val";
        public static string Ref => "ref";

        public static string[] CppKeywords => new string[] { "alignas", "alignof", "and", "and_eq", "asm", "auto", "bitand", "bitor", "bool", "break", "case", "catch", "char", "char16_t", "char32_t", "class", "compl", "const", "constexpr", "const_cast", "continue", "decltype", "default", "delete", "do", "double", "dynamic_cast", "else", "enum", "explicit", "export", "extern", "false", "float", "for", "friend", "goto", "if", "inline", "int", "long", "mutable", "namespace", "new", "noexcept", "not", "not_eq", "nullptr", "operator", "or", "or_eq", "private", "protected", "public", "register", "reinterpret_cast", "return", "short", "signed", "sizeof", "static", "static_assert", "static_cast", "struct", "switch", "template", "this", "thread_local", "throw", "true", "try", "typedef", "typeid", "typename", "union", "unsigned", "using", "virtual", "void", "volatile", "wchar_t", "while", "xor", "xor_eq", "override", "final" };
        public static string[] SlangKeywords => new string[] { "variable", "const", "let", "return", "input", "output", "call", "if", "then", "else", "while", "repeat", "elseif", "do", "raw", "module", "import", "start", "end", "function", "procedure", "integer", "real", "character", "boolean", "string", "new", "nil", "pointer", "true", "false", "array", "val", "ref", "public", "private", "class", "inherit", "base", "abstract", "override", "readonly" };

        public static string SystemModuleName => "System";

        public static string NullType => "Nil";
    }
}
