using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLangCompiler.FrontEnd.Types
{
    public class SlangFunctionType: SlangRoutineType
    {
        public SlangType ReturnType { get; set; }
        public SlangFunctionType(IList<SlangRoutineTypeArg> args, SlangType returnType): base(args) 
        {
            ReturnType = returnType;
        }

        public override bool Equals(SlangType other) => other is SlangFunctionType t && (t.ReturnType.Equals(ReturnType)) && (t.Args.SequenceEqual(Args));

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
