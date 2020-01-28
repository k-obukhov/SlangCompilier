namespace AntlrSyntaxTests
{
    public class EditorMessage
    {
        public string Message { get; set; } // Текст сообщения
        public string Symbol { get; set; } // С каким токеном связан
        public int Line { get; set; } // Номер строки
        public int Character { get; set; } // Номер символа в строке

        public int Length { get; set; } // Размер

        public override string ToString() => $"[{Line}:{Character}]: {Message}";
    }
}