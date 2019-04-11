using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using SLangCompilier.FileServices;

namespace SLangCompilier
{
    public class Compilier
    {
        public Compilier(string path)
        {
            DirectoryInfo directory = new DirectoryInfo(path);
            ProjectManager manager = new ProjectManager();
            manager.LoadCode(directory);
        }

    }
}
