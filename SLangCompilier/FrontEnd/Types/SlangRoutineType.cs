using System;
using System.Collections.Generic;
using System.Text;

namespace SLangCompiler.FrontEnd.Types
{
    public abstract class SlangRoutineType: SlangType
    {
        public IList<SlangRoutineTypeArg> Args { get; set; }

        public SlangRoutineType(IList<SlangRoutineTypeArg> args)
        {
            Args = args;
        }
    }

    public class SlangRoutineTypeArg
    {
        public ParamModifier Modifier { get; set; } // val or ref
        public SlangType Type { get; set; }

        public override bool Equals(object obj) => obj is SlangRoutineTypeArg arg && (arg.Type.Equals(Type)) && (arg.Modifier == Modifier);
        public override int GetHashCode() => base.GetHashCode();

        public override string ToString() => $"{Modifier} {Type}";
    }
}
