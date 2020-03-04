using Antlr4.Runtime;
using System.Collections.Generic;

namespace AntlrSyntaxTests
{
    class SlangLexerErrorListener : IAntlrErrorListener<int>
    {
        public List<EditorMessage> ErrorMessages { get; private set; } // Сообщения об ошибке
        public SlangLexerErrorListener()
        {
            ErrorMessages = new List<EditorMessage>();
        }

        public void SyntaxError(IRecognizer recognizer, int offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            // Перегрузка метода SyntaxError -- сохранение полей в отдельную структуру
            EditorMessage em = new EditorMessage()
            {
                Message = msg,
                Line = line - 1,
                Character = charPositionInLine,
                Symbol = "",
                Length = 1
            };

            if (offendingSymbol != 0) // Для нормального отображения токенов 
            {
                em.Symbol = recognizer.Vocabulary.GetDisplayName(offendingSymbol);
            }
            ErrorMessages.Add(em);
        }
    }
}
