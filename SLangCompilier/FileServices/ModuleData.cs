using System.IO;

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

        public ModuleData(string name, FileInfo file, string data, bool isLib)
        {
            Name = name;
            File = file;
            Data = data;
        }
    }
}
