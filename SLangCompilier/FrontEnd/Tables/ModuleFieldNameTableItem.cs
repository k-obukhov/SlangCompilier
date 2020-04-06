namespace SLangCompiler.FrontEnd.Tables
{
    public class ModuleFieldNameTableItem : FieldNameTableItem, IImportable
    {
        public bool IsReadonly { get; set; }

        public ImportHeader Header { get; set; }
}
}
