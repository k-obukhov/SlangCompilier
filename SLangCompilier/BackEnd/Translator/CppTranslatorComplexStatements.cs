using Antlr4.Runtime.Misc;
using SLangGrammar;
using System.Linq;

namespace SLangCompiler.BackEnd.Translator
{
    // if, while, do-while
    public partial class CppTranslator
    {
        public override object VisitIfC([NotNull] SLangGrammarParser.IfCContext context)
        {
            var exps = context.exp();
            var stmts = context.statementSeq();

            for (int i = 0; i < exps.Length; ++i)
            {
                if (i == 0)
                {
                    cppText.Write("if (");
                    Visit(exps[i]);
                    cppText.Write(')');
                }
                else
                {
                    cppText.Write("else if(");
                    Visit(exps[i]);
                    cppText.Write(')');
                }
                Visit(stmts[i]);
            }

            if (context.Else() != null)
            {
                cppText.Write("else");
                Visit(stmts.Last());
            }
            return null;
        }

        public override object VisitWhileC([NotNull] SLangGrammarParser.WhileCContext context)
        {
            cppText.Write("while (");
            Visit(context.exp());
            cppText.Write(")");
            Visit(context.statementSeq());
            return null;
        }

        public override object VisitRepeatC([NotNull] SLangGrammarParser.RepeatCContext context)
        {
            cppText.Write("do ");
            Visit(context.statementSeq());
            cppText.Write("while (");
            Visit(context.exp());
            cppText.Write(");");

            return null;
        }
    }
}