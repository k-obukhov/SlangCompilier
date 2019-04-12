using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using SLangCompiler.FileServices;
using System.Threading.Tasks;

namespace SLangCompiler
{
    public class Compiler
    {
        /// <summary>
        /// Input path (SL code)
        /// </summary>
        public string InputPath { get; set; }
        /// <summary>
        /// Output path (C++ code)
        /// </summary>
        public string OutputPath { get; set; }

        public Compiler(string inputPath, string outputPath)
        {
            InputPath = inputPath;
            OutputPath = outputPath;
        }

        public async Task Translate()
        {
            DirectoryInfo directory = new DirectoryInfo(InputPath);
            ProjectManager manager = new ProjectManager();
            await Task.Run(() => manager.LoadCode(directory));
            // TODO Front-End
        }

    }
}
