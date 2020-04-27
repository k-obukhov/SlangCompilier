using System.IO;

namespace SLangCompiler.BackEnd
{
    public abstract class CompilerExecutor
    {
        protected DirectoryInfo pathToGeneratedSource;
        protected DirectoryInfo pathToExecutable;

        public CompilerExecutor(string pathToSrc, string pathToExec)
        {
            pathToGeneratedSource = new DirectoryInfo(pathToSrc);
            pathToExecutable = new DirectoryInfo(pathToExec);
        }
        public CompilerExecutor(DirectoryInfo pathToSrc, DirectoryInfo pathToExec)
        {
            pathToGeneratedSource = pathToSrc;
            pathToExecutable = pathToExec;
        }
        public abstract void ExecuteCompilerCall();
    }
}
