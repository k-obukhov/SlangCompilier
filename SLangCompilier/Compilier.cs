using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using SLangCompiler.FileServices;

namespace SLangCompiler
{
    public class Compiler
    {
        public Compiler(string path)
        {
            DirectoryInfo directory = new DirectoryInfo(path);
            ProjectManager manager = new ProjectManager();
            manager.LoadCode(directory);
        }

    }
}
