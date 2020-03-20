using Antlr4.Runtime.Misc;
using SLangGrammar;

namespace SLangCompiler.BackEnd.Translator
{
    // return, call, input, output, let,
    public partial class CppTranslator : SLangGrammarBaseVisitor<object>
    {

        public override object VisitStatement([NotNull] SLangGrammarParser.StatementContext context)
        {
            VisitChildren(context);
            cppText.WriteLine();
            return null;
        }
        public override object VisitSimpleStatement([NotNull] SLangGrammarParser.SimpleStatementContext context)
        {
            VisitChildren(context);
            cppText.Write(';');
            return null;
        }

        public override object VisitReturnC([NotNull] SLangGrammarParser.ReturnCContext context)
        {
            cppText.Write("return ");
            Visit(context.exp());
            return null;
        }

        public override object VisitCall([NotNull] SLangGrammarParser.CallContext context)
        {
            Visit(context.designator());
            return null;
        }

        public override object VisitInput([NotNull] SLangGrammarParser.InputContext context)
        {
            cppText.Write("std::cin");
            for (int i = 0; i < context.designator().Length; ++i)
            {
                cppText.Write(" >> ");
                Visit(context.designator()[i]);
            }
            return null;
        }

        public override object VisitOutput([NotNull] SLangGrammarParser.OutputContext context)
        {
            cppText.Write("std::cout");
            for (int i = 0; i < context.exp().Length; ++i)
            {
                cppText.Write(" << (");
                Visit(context.exp()[i]);
                cppText.Write(")");
            }
            return null;
        }

        public override object VisitLet([NotNull] SLangGrammarParser.LetContext context)
        {
            Visit(context.designator());
            cppText.Write(" = ");
            Visit(context.exp());
            return null;
        }
    }
}