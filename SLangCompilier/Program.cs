using SLangCompiler.Exceptions;
using SLangCompiler.FileServices;
using SLangCompiler.FrontEnd;
using SLangCompiler.FrontEnd.Tables;
using System;
using System.IO;

namespace SLangCompiler
{
    class Program
    {
        static void Main(string[] args)
        {
            var sourceCodeFolder = @"C:\projects\sldemo";
            ProjectManager p = new ProjectManager();
            p.LoadCode(new System.IO.DirectoryInfo(sourceCodeFolder));

            var frontend = new FrontendCompiler();

            frontend.CheckErrors(p);
            /*
            try
            {
                
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
                Console.WriteLine(e.Message);
            }
            */
        }
    }
}
