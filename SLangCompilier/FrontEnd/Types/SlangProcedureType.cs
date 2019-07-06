using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLangCompiler.FrontEnd.Types
{
    public class SlangProcedureType: SlangRoutineType
    {
        public SlangProcedureType(SlangBaseType returnType, IList<SlangRoutineArgType> args) : base(args)
        {
        }

        public override bool IsAssignable(SlangBaseType other)
        {
            if (!(other is SlangProcedureType functionType))
            {
                return false;
            }
            return Args.SequenceEqual(functionType.Args);
        }

        public override bool Equals(SlangBaseType other)
        {
            return IsAssignable(other);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("procedure (");
            sb.Append(string.Join(", ", (from arg in Args select arg.ToString()).ToArray()));
            sb.Append($")");
            return sb.ToString();
        }
    }
}
