using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLangCompiler.FrontEnd.Types
{
    public class SlangFunctionType : SlangRoutineType
    {
        public SlangBaseType ReturnType { get; }
        public SlangFunctionType(SlangBaseType returnType, IList<SlangRoutineArgType> args) : base(args)
        {
            ReturnType = returnType;
        }

        public override bool IsAssignable(SlangBaseType other)
        {
            if (!(other is SlangFunctionType functionType))
            {
                return false;
            }
            return ReturnType.Equals(functionType) && Args.SequenceEqual(functionType.Args);
        }

        public override bool Equals(SlangBaseType other)
        {
            return IsAssignable(other);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("function (");
            sb.Append(string.Join(", ", (from arg in Args select arg.ToString()).ToArray()));
            sb.Append($"): {ReturnType}");
            return sb.ToString();
        }
    }
}
