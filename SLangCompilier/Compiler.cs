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
        public CompilerBuilder SetPath(string path)
        {
            compiler.SetPath(path);
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
        BackendCompiler backendCompiler;

        public void SetPath(string path)
        {
            inputPath = path;
        }

        public void SetBackend(BackendCompiler compiler)
        {
            backendCompiler = compiler;
        }

        public Compiler()
        {

        }

        public Compiler(string inputPath)
        {
            this.inputPath = inputPath;
        }


        public void Translate()
        {
            var p = new ProjectManager();
            p.LoadCode(new System.IO.DirectoryInfo(inputPath));

            var frontend = new FrontendCompiler();

            frontend.CheckErrors(p);
            Console.WriteLine($"No errors found");
            backendCompiler.SetTable(frontend.SourceCode);
            backendCompiler.Translate(new DirectoryInfo(inputPath));
            /*
            try
            {
                frontend.CheckErrors(p);
                Console.WriteLine($"No errors found");
                backendCompiler.Translate(new DirectoryInfo(inputPath));
            }
            catch (CompilerException e)
            {
                Console.WriteLine($"{e.ModuleFile.Name}, [{e.Line}, {e.Column}]: {e.Message}");
            }
            catch (IOException e)
            {
                Console.WriteLine($"IOError: {e.Message}");
            }
            catch (Exception e)
            {
                // all others
                Console.WriteLine($"Compiler error: {e.Message}");
                throw;
            } */
        }

    }
}
