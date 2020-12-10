using System.IO;

namespace SLangCompiler.BackEnd
{
    public abstract class CompilerExecutor
    {
        protected DirectoryInfo PathToGeneratedSource;
        protected DirectoryInfo PathToExecutable;

        protected CompilerExecutor(string pathToSrc, string pathToExec)
        {
            PathToGeneratedSource = new DirectoryInfo(pathToSrc);
            PathToExecutable = new DirectoryInfo(pathToExec);
        }

        protected CompilerExecutor(DirectoryInfo pathToSrc, DirectoryInfo pathToExec)
        {
            PathToGeneratedSource = pathToSrc;
            PathToExecutable = pathToExec;
        }
        public abstract void ExecuteCompilerCall();
    }
}
