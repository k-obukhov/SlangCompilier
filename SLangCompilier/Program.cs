using CommandLine;
using SLangCompiler.BackEnd;
using SLangCompiler.BackEnd.Executor;
using SLangCompiler.BackEnd.Translator;
using System;
using System.Collections.Generic;

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

        class Options
        {
            [Option('i', "input", Required = true, HelpText = "Path to SLang project")]
            public string PathToProject { get; set; }

            [Option('o', "output", Required = true, HelpText = "Path to generated source")]
            public string PathToGenerated { get; set; }

            [Option('l', "lang", Default = cppLang, HelpText = "Language id (default value - cpp)")]
            public string LanguageId { get; set; }

            [Option('p', "program", Required = false, HelpText = "Path to program (including filename!)")]
            public string PathToExecutable { get; set; }
        }

        static void RunOptions(Options opts)
        {
            try
            {
                var (b, e) = GetBackendById(opts.LanguageId, opts.PathToGenerated, opts.PathToExecutable);
                Compiler compiler = new CompilerBuilder()
                    .SetInputPath(opts.PathToProject)
                    .SetOutputPath(opts.PathToGenerated)
                    .SetBackend(b)
                    .SetCompilerExecutor(!string.IsNullOrEmpty(opts.PathToExecutable)
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

        static void HandleParseError(IEnumerable<Error> errs)
        {
        }

        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(RunOptions)
                .WithNotParsed(HandleParseError);
        }
    }
}
