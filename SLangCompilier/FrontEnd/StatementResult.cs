using System;
using System.Collections.Generic;
using System.Text;

namespace SLangCompiler.FrontEnd
{
    /// <summary>
    /// Класс для возврата данных о выражениях
    /// Нужен только для одной вещи -- проверки, в заданной ветке есть возврат значения
    /// </summary>
    class StatementResult
    {
        public bool Returning { get; set; }
        public StatementResult(bool returning)
        {
            Returning = returning;
        }
    }
}
