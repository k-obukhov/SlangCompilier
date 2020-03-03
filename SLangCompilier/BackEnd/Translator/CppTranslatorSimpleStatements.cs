using SLangGrammar;
using SLangCompiler.FrontEnd.Types;
using Antlr4.Runtime.Misc;

namespace SLangCompiler.BackEnd.Translator
{
    // return, call, input, output, let,
    public partial class CppTranslator : SLangGrammarBaseVisitor<object>
    {
        public override object VisitSimpleStatement([NotNull] SLangGrammarParser.SimpleStatementContext context)
        {
            base.VisitSimpleStatement(context);
            cppText.Write(';');
            return null;
        }
    }
}