namespace SLangCompiler.FrontEnd.Tables
{
    /// <summary>
    /// Необходим для логики импорта извне 
    /// На данный момент его можно использовать только из функций и процедур модуля
    /// </summary>
    interface IImportable
    {
        ImportHeader Header { get; }
    }
}
