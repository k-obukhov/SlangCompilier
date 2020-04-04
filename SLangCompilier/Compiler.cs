using SLangCompiler.BackEnd;
using SLangCompiler.Exceptions;
using SLangCompiler.FileServices;
using SLangCompiler.FrontEnd;
using System;
using System.IO;

namespace SLangCompiler
{
    public class CompilerBuilder
    {
        readonly Compiler compiler = new Compiler();
        public CompilerBuilder SetInputPath(string path)
        {
            compiler.SetInputPath(path);
            return this;
        }

        public CompilerBuilder SetOutputPath(string path)
        {
            compiler.SetOutputPath(path);
            return this;
        }

        public CompilerBuilder SetCompilerExecutor(CompilerExecutor exec)
        {
            compiler.SetCompilerExecutor(exec);
            return this;
        }

        public CompilerBuilder SetCompiler(BackendCompiler backend)
        {
            compiler.SetBackend(backend);
            return this;
        }
        public Compiler Build()
        {
            return compiler;
        }
    }

    public class Compiler
    {
        /// <summary>
        /// Input path (SL code)
        /// </summary>
        private string inputPath;
        private string outputPath;
        BackendCompiler backendCompiler;
        CompilerExecutor compilerExecutor;

        public void SetInputPath(string path)
        {
            inputPath = path;
        }

        public void SetOutputPath(string path)
        {
            outputPath = path;
        }

        public void SetBackend(BackendCompiler compiler)
        {
            backendCompiler = compiler;
        }

        public void SetCompilerExecutor(CompilerExecutor executor)
        {
            compilerExecutor = executor;
        }

        public Compiler()
        {

        }

        public void Translate()
        {
            var p = new ProjectManager();
            p.LoadCode(new System.IO.DirectoryInfo(inputPath));

            var frontend = new FrontendCompiler();
            
            try
            {
                frontend.CheckErrors(p);
                Console.WriteLine($"No syntax errors found, backend compiler starts");
                backendCompiler.SetTable(frontend.SourceCode);
                backendCompiler.Translate(new DirectoryInfo(outputPath));
                if (compilerExecutor != null)
                {
                    Console.WriteLine("Target build starts...");
                    compilerExecutor.ExecuteCompilerCall();
                    Console.WriteLine("Target build successful");
                }
            }
            catch (CompilerException e)
            {
                Console.Error.WriteLine($"{e.ModuleFile.Name}, [{e.Line}, {e.Column}]: {e.Message}");
            }
            catch (IOException e)
            {
                Console.Error.WriteLine($"IOError: {e.Message}");
            }
            catch (Exception e)
            {
                // all others
                Console.Error.WriteLine($"Compiler error: {e.Message}");
            }
        }
    }
}
