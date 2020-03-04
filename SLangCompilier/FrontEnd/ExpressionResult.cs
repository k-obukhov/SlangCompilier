using SLangCompiler.FrontEnd.Types;

namespace SLangCompiler.FrontEnd
{
    class ExpressionResult
    {
        public SlangType Type { get; }
        public ExpressionValueType ExpressionType { get; }
        public ExpressionResult(SlangType type, ExpressionValueType exprType)
        {
            Type = type;
            ExpressionType = exprType;
        }
    }
}
