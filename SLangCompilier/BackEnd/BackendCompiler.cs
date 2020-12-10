using SLangCompiler.FrontEnd.Tables;
using System.IO;

namespace SLangCompiler.BackEnd
{
    public abstract class BackendCompiler
    {
        public SourceCodeTable Table;
        public void SetTable(SourceCodeTable table)
        {
            Table = table;
        }

        protected BackendCompiler(SourceCodeTable table)
        {
            Table = table;
        }

        public abstract void Translate(DirectoryInfo pathGen);
    }
}
