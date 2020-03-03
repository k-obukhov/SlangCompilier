using SLangGrammar;
using SLangCompiler.FrontEnd.Types;
using Antlr4.Runtime.Misc;

namespace SLangCompiler.BackEnd.Translator
{
    public partial class CppTranslator : SLangGrammarBaseVisitor<object>
    {
        public override object VisitExprList([NotNull] SLangGrammarParser.ExprListContext context)
        {
            var expList = context.exp();
            int n = expList.Length;
            for (int i = 0; i < n; ++i)
            {
                Visit(expList[i]);
                if (i != n - 1)
                {
                    cppText.Write(", ");
                }
            }
            return null;
        }

        public override object VisitExp([NotNull] SLangGrammarParser.ExpContext context)
        {
            Visit(context.simpleExpr());
            if (context.exp() != null)
            {
                var op = context.BoolEq() ?? context.BoolG() ?? context.BoolGeq() ?? context.BoolL() ?? context.BoolLeq() ?? context.BoolNeq();
                cppText.Write($" {op.GetText()} ");
                Visit(context.exp());
            }
            return null;
        }

        public override object VisitSimpleExpr([NotNull] SLangGrammarParser.SimpleExprContext context)
        {
            Visit(context.term());
            if (context.simpleExpr() != null)
            {
                var op = context.AddOp() ?? context.SubOp() ?? context.BoolOr();
                cppText.Write($" {op.GetText()} ");
                Visit(context.simpleExpr());
            }
            return null;
        }

        public override object VisitTerm([NotNull] SLangGrammarParser.TermContext context)
        {
            Visit(context.signedFactor());
            if (context.term() != null)
            {
                var op = context.MulOp() ?? context.DivOp() ?? context.BoolAnd();
                cppText.Write($" {op.GetText()} ");
                Visit(context.term());
            }
            return null;
        }
        public override object VisitSignedFactor([NotNull] SLangGrammarParser.SignedFactorContext context)
        {
            var op = context.AddOp() ?? context.SubOp();
            if (op != null)
            {
                cppText.Write(op.GetText());
            }
            Visit(context.factor());
            return null;
        }

        public override object VisitFactor([NotNull] SLangGrammarParser.FactorContext context)
        {
            if (context.IntValue() != null)
            {
                cppText.Write(context.IntValue().GetText());
            }
            else if (context.RealValue() != null)
            {
                cppText.Write(context.RealValue().GetText());
            }
            else if (context.BoolValue() != null)
            {
                cppText.Write(context.BoolValue().GetText());
            }
            else if (context.SingleCharacter() != null)
            {
                cppText.Write(context.SingleCharacter().GetText());
            }
            else if (context.StringLiteral() != null)
            {
                cppText.Write($"std::string({context.StringLiteral().GetText()})");
            }
            else if (context.BoolNot() != null)
            {
                cppText.Write(context.BoolNot());
                Visit(context.factor());
            }
            else if (context.exp() != null)
            {
                cppText.Write('(');
                Visit(context.exp());
                cppText.Write(')');
            }
            else if (context.designator() != null)
            {
                Visit(context.designator());
            }
            else if (context.newC() != null)
            {
                cppText.Write($"new({GetStringFromType(Visit(context.newC().customType()) as SlangCustomType)})");
            }
            else if (context.Nil() != null)
            {
                cppText.Write("nullptr");
            }
            return null;
        }

        public override object VisitDesignator([NotNull] SLangGrammarParser.DesignatorContext context)
        {
            //todo...
            return null;
        }
    }
}