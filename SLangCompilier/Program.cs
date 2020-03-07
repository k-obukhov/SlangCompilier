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
            Compiler compiler = null;
            try
            {
                var sourceCodeFolder = args.Length == 0 ? throw new Exception("Input path is not set") : args[0];
                var destCodeFolder = args.Length == 1 ? throw new Exception("Output path is not set") : args[1];
                var lang = args.Length < 3 ? defaultLang : args[2];

                compiler = new CompilerBuilder().SetInputPath(sourceCodeFolder).SetCompiler(GetBackendById(lang)).Build();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Invalid cl parameters: Error {e}");
            }

            compiler.Translate();
        }
    }
}
