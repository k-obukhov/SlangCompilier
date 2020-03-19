using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace SLangCompiler.BackEnd.Executor
{
    internal class GccCompilerExecutor : CompilerExecutor
    {
        public GccCompilerExecutor(DirectoryInfo src, DirectoryInfo dest): base(src, dest)
        {

        }
        public override void ExecuteCompilerCall()
        {
            // create outputDir
            Directory.CreateDirectory(Path.GetDirectoryName(pathToExecutable.FullName));
            // get all files
            var extensionsAllowed = new string[] { ".cc", ".cpp", ".cxx", ".c", ".c++", ".h", ".hpp", ".hh", ".hxx", ".h++" };
            var sourceCodeFiles = Directory.GetFiles(pathToGeneratedSource.FullName)
            .Where(file => {
                var extension = Path.GetExtension(file);
                if (extensionsAllowed.Contains(extension))
                {
                    return true;
                }
                return false;
            });

            sourceCodeFiles = sourceCodeFiles.Select(str => $"\"{str}\"");

            var process = new Process();
            process.StartInfo.FileName = "g++";
            process.StartInfo.Arguments = $" {string.Join(' ', sourceCodeFiles)} -o \"{pathToExecutable}\"";

            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.UseShellExecute = false;
            
            process.Start();
            process.WaitForExit();
        }
    }
}
