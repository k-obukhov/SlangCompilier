using SLangCompiler.BackEnd;
using SLangCompiler.BackEnd.Executor;
using SLangCompiler.BackEnd.Translator;
using System;
using System.IO;

namespace SLangCompiler
{
    class Program
    {
        const string cppLang = "cpp";

        static (BackendCompiler b, CompilerExecutor e) GetBackendById(string id, string sourcePath, string execPath)
        {
            return id switch
            {
                cppLang => (new CppBackendCompiler(), execPath != null ? new GccCompilerExecutor(sourcePath, execPath) : null),
                _ => throw new Exception($"Invalid language tag: {id}"),
            };
        }

        static void Main(string[] args)
        {
            var defaultLang = cppLang;
            try
            {
                //var folder = @"";
                //args = new string[] { folder, System.IO.Path.Combine(folder, "gen"), defaultLang, System.IO.Path.Combine(folder, "bin", "program.out") };
                var sourceCodeFolder = args.Length == 0 ? throw new Exception("Input path is not set") : args[0];
                var destCodeFolder = args.Length == 1 ? throw new Exception("Output path is not set") : args[1];
                var lang = args.Length < 3 ? defaultLang : args[2];
                var executableFolder = args.Length < 4 ? null : args[3];
                var (b, e) = GetBackendById(lang, destCodeFolder, executableFolder);
                Compiler compiler = new CompilerBuilder()
                    .SetInputPath(sourceCodeFolder)
                    .SetOutputPath(destCodeFolder)
                    .SetBackend(b)
                    .SetCompilerExecutor(executableFolder != null
                        ? e
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
