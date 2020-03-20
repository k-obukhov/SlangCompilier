using SLangCompiler.FrontEnd.Tables;

namespace SLangCompiler.BackEnd
{
    public abstract class BackendCompiler
    {
        public SourceCodeTable table;
        public void SetTable(SourceCodeTable table)
        {
            this.table = table;
        }

        public BackendCompiler(SourceCodeTable table)
        {
            this.table = table;
        }

        public abstract void Translate(System.IO.DirectoryInfo pathGen);
    }
}
