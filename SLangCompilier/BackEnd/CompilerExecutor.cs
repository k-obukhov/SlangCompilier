using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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
