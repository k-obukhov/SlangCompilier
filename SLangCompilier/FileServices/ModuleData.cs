using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SLangCompiler.FileServices
{
    public class ModuleData
    {
        /// <summary>
        /// Module name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// File Info
        /// </summary>
        public FileInfo File { get; set; }
        /// <summary>
        /// File data
        /// </summary>
        public string Data { get; set; }
        /// <summary>
        /// Check module is in standard library
        /// </summary>
        bool IsLib { get; set; }

        public ModuleData(string name, FileInfo file, string data, bool isLib)
        {
            Name = name;
            File = file;
            Data = data;
            IsLib = isLib;
        }
        /// <summary>
        /// Check module name equals to main module name
        /// </summary>
        /// <returns></returns>
        bool IsMain() => CompilerConstants.MainModuleNameWithExt == File.Name;
    }
}
