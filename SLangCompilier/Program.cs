using SLangCompiler.BackEnd;
using SLangCompiler.BackEnd.Translator;
using System;

namespace SLangCompiler
{
    class Program
    {
        const string cppLang = "cpp";
        static BackendCompiler GetBackendById(string id)
        {
            switch (id)
            {
                case cppLang:
                    return new CppBackendCompiler();
                default:
                    throw new Exception($"Invalid language tag: {id}");
            }
        }
        static void Main(string[] args)
        {
            var defaultLang = cppLang;
            var defaultTestSourceFolder = @"C:\projects\sldemosem";
            Compiler compiler = null;
            try
            {
                var sourceCodeFolder = args.Length == 0 ? defaultTestSourceFolder : args[0];
                var lang = args.Length < 2 ? defaultLang : args[1];

                compiler = new CompilerBuilder().SetPath(sourceCodeFolder).SetCompiler(GetBackendById(lang)).Build();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Invalid cl parameters: Error {e}");
            }

            compiler.Translate();
        }
    }
}
