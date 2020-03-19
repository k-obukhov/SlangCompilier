using SLangCompiler.BackEnd;
using SLangCompiler.BackEnd.Executor;
using SLangCompiler.BackEnd.Translator;
using System;
using System.Collections.Generic;
using System.IO;

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

        static CompilerExecutor GetExecutorById(string id, DirectoryInfo sourcePath, DirectoryInfo execPath)
        {
            switch (id)
            {
                case cppLang:
                    return new GccCompilerExecutor(sourcePath, execPath);
                default:
                    throw new Exception($"Invalid language tag: {id}");
            }
        }

        static void Main(string[] args)
        {
            var defaultLang = cppLang;
            try
            {
                var sourceCodeFolder = args.Length == 0 ? throw new Exception("Input path is not set") : args[0];
                var destCodeFolder = args.Length == 1 ? throw new Exception("Output path is not set") : args[1];
                var lang = args.Length < 3 ? defaultLang : args[2];
                var executableFolder = args.Length < 4 ? null : args[3];

                Compiler compiler = new CompilerBuilder()
                    .SetInputPath(sourceCodeFolder)
                    .SetOutputPath(destCodeFolder)
                    .SetCompiler(GetBackendById(lang))
                    .SetCompilerExecutor(executableFolder != null 
                        ? GetExecutorById(lang, new DirectoryInfo(destCodeFolder), new DirectoryInfo(executableFolder)) 
                        : null)
                    .Build();

                compiler.Translate();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Invalid cl parameters: Error {e}");
            }
        }
    }
}
