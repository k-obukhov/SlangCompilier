namespace SLangCompiler.FrontEnd.Tables
{
    public class ModuleFieldNameTableItem : FieldNameTableItem, IImportable
    {
        public ImportHeader Header { get; set; }
    }
}
