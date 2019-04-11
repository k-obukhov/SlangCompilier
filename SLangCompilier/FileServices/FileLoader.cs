using System.Threading.Tasks;
using System.IO;

namespace SLangCompilier.FileServices
{
    public class FileLoader
    {
        public async Task<ModuleData> LoadModuleFile(FileInfo file, bool isLib)
        {
            string nameOfModule = Path.GetFileNameWithoutExtension(file.FullName);
            string code;

            using (TextReader reader = file.OpenText())
            {
                code = await reader.ReadToEndAsync();
            }
            return new ModuleData(nameOfModule, file, code, isLib);
        }
    }
}
