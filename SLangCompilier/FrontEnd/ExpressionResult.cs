using SLangCompiler.FrontEnd.Types;
using System;
using System.Collections.Generic;
using System.Text;

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
