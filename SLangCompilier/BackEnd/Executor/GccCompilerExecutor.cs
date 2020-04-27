using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace SLangCompiler.BackEnd.Executor
{
    internal class GccCompilerExecutor : CompilerExecutor
    {
        public GccCompilerExecutor(string src, string dest) : base(src, dest)
        {

        }
        public GccCompilerExecutor(DirectoryInfo src, DirectoryInfo dest) : base(src, dest)
        {

        }
        public override void ExecuteCompilerCall()
        {
            // create outputDir
            Directory.CreateDirectory(Path.GetDirectoryName(pathToExecutable.FullName));
            // get all files
            var extensionsAllowed = new string[] { ".cc", ".cpp", ".cxx", ".c", ".c++", ".h", ".hpp", ".hh", ".hxx", ".h++" };
            var sourceCodeFiles = Directory.GetFiles(pathToGeneratedSource.FullName)
            .Where(file =>
            {
                var extension = Path.GetExtension(file);
                return extensionsAllowed.Contains(extension);
            });

            sourceCodeFiles = sourceCodeFiles.Select(str => $"\"{str}\"");

            var process = new Process();
            process.StartInfo.FileName = "g++";
            process.StartInfo.Arguments = $" {string.Join(' ', sourceCodeFiles)} -o \"{pathToExecutable}\"";
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.UseShellExecute = false;
            process.ErrorDataReceived += (s, e) => throw new Exception($"Target compiler returned error = {e.Data}");
            
            process.Start();
            process.BeginErrorReadLine();
            process.WaitForExit();
        }
    }
}
