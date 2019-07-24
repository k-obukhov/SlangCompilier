using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLangCompiler.FrontEnd.Types
{
    class SlangProcedureType: SlangRoutineType
    {
        public SlangProcedureType(IList<SlangRoutineTypeArg> args): base(args)
        {
        }

        public override bool Equals(SlangType other) => other is SlangProcedureType t && Args.SequenceEqual(t.Args);

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
