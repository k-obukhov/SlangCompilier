using System.IO;

namespace SLangCompiler.BackEnd
{
    public abstract class CompilerExecutor
    {
        protected DirectoryInfo pathToGeneratedSource;
        protected DirectoryInfo pathToExecutable;

        public CompilerExecutor(DirectoryInfo pathToSrc, DirectoryInfo pathToExec)
        {
            pathToGeneratedSource = pathToSrc;
            pathToExecutable = pathToExec;
        }
        public abstract void ExecuteCompilerCall();
    }
}
