using SLangCompiler.Exceptions;
using SLangCompiler.FileServices;
using SLangCompiler.FrontEnd;
using System;
using System.IO;

namespace SLangCompiler
{
    class Program
    {
        static void Main(string[] args)
        {
            var defaultTestSourceFolder = @"C:\projects\sldemosem";
            var sourceCodeFolder = args.Length == 0 ? defaultTestSourceFolder : args[0];
            var p = new ProjectManager();
            p.LoadCode(new System.IO.DirectoryInfo(sourceCodeFolder));

            var frontend = new FrontendCompiler();
            
            try
            {
                frontend.CheckErrors(p);
                Console.WriteLine($"No errors found");
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
            }
        }
    }
}
